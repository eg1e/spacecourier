using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameRenderer
{
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _lineTexture;
    private readonly SpriteFont _font;
    private readonly Dictionary<string, Texture2D> _planetTextures;

    private readonly float _horizontalSpacing;
    private readonly float _verticalOffsetAbovePlayer;
    private readonly float _verticalOffsetToNextPlanets;
    private readonly float _dottedLineSegment;
    private readonly float _dottedLineSpacing;
    private readonly float _dottedLineThickness;

    public GameRenderer(
        SpriteBatch spriteBatch,
        Texture2D lineTexture,
        SpriteFont font,
        Dictionary<string, Texture2D> planetTextures
    )
    {
        _spriteBatch = spriteBatch;
        _lineTexture = lineTexture;
        _font = font;
        _planetTextures = planetTextures;

        _horizontalSpacing = RenderConstants.HorizontalPlanetSpacing;
        _verticalOffsetAbovePlayer = RenderConstants.VerticalOffsetAbovePlayer;
        _verticalOffsetToNextPlanets = RenderConstants.VerticalOffsetToNextPlanets;
        _dottedLineSegment = RenderConstants.DottedSegmentLength;
        _dottedLineSpacing = RenderConstants.DottedSegmentSpacing;
        _dottedLineThickness = RenderConstants.DottedLineThickness;
    }

    public void Draw(Player player, IEnumerable<Route> routes, string message, GraphicsDevice graphicsDevice)
    {
        _spriteBatch.Begin();

        graphicsDevice.Clear(Color.MidnightBlue);

        DrawHUD(player, message);

        if (!_planetTextures.TryGetValue(player.Current.Name, out var currentPlanetTexture)) return;

        Vector2 currentPos = GetPlanetPosition(graphicsDevice, currentPlanetTexture);
        DrawPlanet(player.Current.Name, currentPlanetTexture, currentPos, Color.Yellow);

        DrawConnectedPlanets(player, routes.ToList(), currentPos);

        _spriteBatch.End();
    }

    private void DrawHUD(Player player, string message)
    {
        _spriteBatch.DrawString(_font, $"Fuel: {player.Fuel}", new Vector2(RenderConstants.HudFuelX, RenderConstants.HudFuelY), Color.White);
        _spriteBatch.DrawString(_font, $"Location: {player.Current.Name}", new Vector2(RenderConstants.HudLocationX, RenderConstants.HudLocationY), Color.White);
        _spriteBatch.DrawString(_font, message, new Vector2(RenderConstants.HudMessageX, RenderConstants.HudMessageY), Color.Yellow);
    }

    private Vector2 GetPlanetPosition(GraphicsDevice graphicsDevice, Texture2D planetTexture)
    {
        return new Vector2(
            (graphicsDevice.Viewport.Width - planetTexture.Width) / RenderConstants.CenterDivisor,
            graphicsDevice.Viewport.Height - planetTexture.Height - _verticalOffsetAbovePlayer
        );
    }

    private void DrawPlanet(string name, Texture2D texture, Vector2 position, Color nameColor)
    {
        _spriteBatch.Draw(texture, position, Color.White);

        Vector2 nameSize = _font.MeasureString(name);
        Vector2 namePos = new Vector2(
            position.X + (texture.Width - nameSize.X) / RenderConstants.CenterDivisor,
            position.Y - nameSize.Y - RenderConstants.PlanetNameVerticalMargin
        );

        _spriteBatch.DrawString(_font, name, namePos, nameColor);
    }

    private void DrawConnectedPlanets(Player player, List<Route> nextRoutes, Vector2 currentPos)
    {
        if (nextRoutes.Count == 0) return;

        var currentPlanetTexture = _planetTextures[player.Current.Name];
        float totalWidth = nextRoutes.Count * _horizontalSpacing;
        float startX = (_spriteBatch.GraphicsDevice.Viewport.Width - totalWidth) / RenderConstants.CenterDivisor;

        for (int i = 0; i < nextRoutes.Count; i++)
        {
            var route = nextRoutes[i];
            if (!_planetTextures.TryGetValue(route.DestinationPlanet.Name, out var nextTexture)) continue;

            Vector2 nextPos = new Vector2(
                startX + i * _horizontalSpacing + ( _horizontalSpacing - nextTexture.Width) / RenderConstants.CenterDivisor + nextTexture.Width / RenderConstants.CenterDivisor,
                currentPos.Y - nextTexture.Height - _verticalOffsetToNextPlanets + nextTexture.Height/RenderConstants.CenterDivisor
            );

            Vector2 currentCenter = currentPos + new Vector2(
                currentPlanetTexture.Width / RenderConstants.CenterDivisor,
                currentPlanetTexture.Height / RenderConstants.CenterDivisor
            );

            DrawDottedLine(currentCenter, nextPos, Color.White, route);

            Vector2 planetDrawPos = nextPos - new Vector2(nextTexture.Width / RenderConstants.CenterDivisor, nextTexture.Height / RenderConstants.CenterDivisor);
            _spriteBatch.Draw(nextTexture, planetDrawPos, Color.White);

            Vector2 nextNameSize = _font.MeasureString(route.DestinationPlanet.Name);
            Vector2 nextNamePos = new Vector2(
                planetDrawPos.X + (nextTexture.Width - nextNameSize.X) / RenderConstants.CenterDivisor,
                planetDrawPos.Y - nextNameSize.Y - RenderConstants.PlanetNameVerticalMargin
            );
            _spriteBatch.DrawString(_font, route.DestinationPlanet.Name, nextNamePos, Color.LightGreen);

            Vector2 routeNumberSize = _font.MeasureString("[" + (i + 1) + "]");
            Vector2 routeNumberPos = new Vector2(
                planetDrawPos.X + (nextTexture.Width - routeNumberSize.X) / RenderConstants.CenterDivisor,
                planetDrawPos.Y - routeNumberSize.Y + RenderConstants.RouteNumberYOffset
            );
            _spriteBatch.DrawString(_font, "[" + (i + 1) + "]", routeNumberPos, Color.Yellow);
        }
    }

    private void DrawDottedLine(Vector2 start, Vector2 end, Color color, Route route)
    {
        Vector2 delta = end - start;
        float totalLength = delta.Length();
        Vector2 direction = Vector2.Normalize(delta);

        float drawnLength = 0f;

        while (drawnLength < totalLength)
        {
            float currentSegmentLength = Math.Min(_dottedLineSegment, totalLength - drawnLength);
            Vector2 segmentStart = start + direction * drawnLength;
            float angle = (float)Math.Atan2(delta.Y, delta.X);

            _spriteBatch.Draw(_lineTexture, segmentStart, null, color, angle,
                              Vector2.Zero, new Vector2(currentSegmentLength, _dottedLineThickness),
                              SpriteEffects.None, 0f);

            drawnLength += _dottedLineSegment + _dottedLineSpacing;
        }

        DrawRouteFuelCost(route, start, direction, totalLength);
    }

    private void DrawRouteFuelCost(Route route, Vector2 start, Vector2 direction, float totalLength)
    {
        Vector2 middle = start + direction * (totalLength / RenderConstants.CenterDivisor);
        string planetType = PlanetType(route);
        string costText = $"Cost: {route.FuelCost}" + ", \n" + planetType;
        Vector2 textSize = _font.MeasureString(costText);
        Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
        Vector2 textPosition = middle + perpendicular * RenderConstants.RouteFuelPerpendicularOffset;

        _spriteBatch.DrawString(_font, costText, textPosition - textSize / RenderConstants.CenterDivisor, Color.Tomato);
    }

    private string PlanetType(Route route)
    {
        return route.DestinationPlanet.GetPlanetType();
    }
}
