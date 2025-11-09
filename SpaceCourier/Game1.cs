using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace SpaceCourier;

public class Game1 : Core
{
	private SpriteFont _font;
	private Texture2D _planet1;
	private Texture2D _planet2;
	private Texture2D _planet3;
	private Texture2D _planet4;
	private Texture2D _planet5;
	private Texture2D _planet6;
	private Texture2D _lineTexture;
	private Dictionary<string, Texture2D> _planetTextures;

	private KeyboardState _previousKeyboardState;

	private GameEngine _engine;
	private Player _player;
	private string _message = "Choose your path using keys 1-4";
	private bool _gameOver;

	public Game1() : base("Space Courier", 1280, 720, false) { }

	protected override void Initialize()
	{
		base.Initialize();
		_engine = new GameEngine();
		_player = new Player(_engine.Planets.First(p => p.Name == "Earth"), 100);
		_lineTexture = new Texture2D(GraphicsDevice, 1, 1);
    	_lineTexture.SetData(new[] { Color.White });
	}

	protected override void LoadContent()
	{
		_planet1 = Content.Load<Texture2D>("images/planet1");
		_planet2 = Content.Load<Texture2D>("images/planet2");
		_planet3 = Content.Load<Texture2D>("images/planet3");
		_planet4 = Content.Load<Texture2D>("images/sun (1)");
		_planet5 = Content.Load<Texture2D>("images/sun (2)");
		_planet6 = Content.Load<Texture2D>("images/sun (3)");
		_font = Content.Load<SpriteFont>("DefaultFont");

		_planetTextures = new Dictionary<string, Texture2D>
		{
			{ "Earth", _planet1 },
			{ "Alpha", _planet2 },
			{ "Beta", _planet3 },
			{"Destination", _planet1 },
			{"Solarion Belt", _planet4},
			{"Zenova Obscura", _planet5},
			{"Relicos Delta", _planet6},
		};
	}

	protected override void Update(GameTime gameTime)
	{
		if (_gameOver) return;

		KeyboardState keyboardState = Keyboard.GetState();
		var routes = _engine.GetRoutesFrom(_player.Current).ToList();
		for (int i = 0; i < routes.Count; i++)
		{
			var key = Keys.D1 + i;
			if (keyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
			{
				
				var route = routes[i];
				_player.Fuel -= route.FuelCost;
				var eventTriggered = EventSystem.TriggerEvent(_player, route.Risk);

				if (eventTriggered == 0)
				{
					_message = "Pirates attacked!";
					_player.Fuel -= 20;
				}
				else if (eventTriggered == 1)
				{
					_message = "Fuel leak!";
					_player.Fuel -= 15;
				}
				else if (eventTriggered == 2)
				{
					_message = "Navigation error!";
					_player.Fuel -= 10;
				}
				else if (eventTriggered == 3)
				{
					_message = "Solar storm!";
					_player.Fuel -= 12;
				}
				else
                {
					_message = "Trip was smooth";
                }
				

				if (_player.Fuel <= 0)
				{
					_message = "You ran out of fuel!";
					_gameOver = true;
					break;
				}

				_player.Current = route.DestinationPlanet;

				if (_player.Current.Name == "Destination")
				{
					_message = "You delivered the cargo successfully!";
					_gameOver = true;
				}

			}
		}
		if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// TODO: Add your update logic here
		_previousKeyboardState = keyboardState;
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		SpriteBatch.Begin();

		GraphicsDevice.Clear(Color.MidnightBlue);
		SpriteBatch.DrawString(_font, $"Fuel: {_player.Fuel}", new Vector2(10, 10), Color.White);
		SpriteBatch.DrawString(_font, $"Location: {_player.Current.Name}", new Vector2(10, 40), Color.White);
		SpriteBatch.DrawString(_font, _message, new Vector2(10, 70), Color.Yellow);

		if (_planetTextures.TryGetValue(_player.Current.Name, out var currentPlanetTexture))
		{
			Vector2 currentPos = new Vector2(
				(GraphicsDevice.Viewport.Width - currentPlanetTexture.Width) / 2,
				GraphicsDevice.Viewport.Height - currentPlanetTexture.Height - 20
			);

			SpriteBatch.Draw(currentPlanetTexture, currentPos, Color.White);

			// Planet name above current planet
			Vector2 nameSize = _font.MeasureString(_player.Current.Name);
			Vector2 namePos = new Vector2(
				(GraphicsDevice.Viewport.Width - nameSize.X) / 2,
				currentPos.Y - nameSize.Y - 5
			);
			SpriteBatch.DrawString(_font, _player.Current.Name, namePos, Color.Yellow);

			// --- Draw connected planets above current planet ---
			var nextRoutes = _engine.GetRoutesFrom(_player.Current).ToList();
			if (nextRoutes.Count > 0)
			{
				float spacing = 250; // space between planets
				float totalWidth = nextRoutes.Count * spacing;
				float startX = (GraphicsDevice.Viewport.Width - totalWidth) / 2;

				for (int i = 0; i < nextRoutes.Count; i++)
				{
					var route = nextRoutes[i];
					if (_planetTextures.TryGetValue(route.DestinationPlanet.Name, out var nextPlanetTexture))
					{
						Vector2 nextPos = new Vector2(
							startX + i * spacing + (spacing - nextPlanetTexture.Width) / 2 + nextPlanetTexture.Width / 2,
							currentPos.Y - nextPlanetTexture.Height - 160 + nextPlanetTexture.Height / 2
						);

						Vector2 currentCenter = currentPos + new Vector2(currentPlanetTexture.Width / 2, currentPlanetTexture.Height / 2);
						DrawDottedLine(currentCenter, nextPos, Color.White, route, _font, segmentLength: 8, spacing: 5, thickness: 3f);

						Vector2 planetDrawPos = nextPos - new Vector2(nextPlanetTexture.Width / 2, nextPlanetTexture.Height / 2);
						SpriteBatch.Draw(nextPlanetTexture, planetDrawPos, Color.White);

						// Draw planet name above texture
						Vector2 nextNameSize = _font.MeasureString(route.DestinationPlanet.Name);
						Vector2 nextNamePos = new Vector2(
							planetDrawPos.X + (nextPlanetTexture.Width - nextNameSize.X) / 2,
							planetDrawPos.Y - nextNameSize.Y - 5
						);
						SpriteBatch.DrawString(_font, route.DestinationPlanet.Name, nextNamePos, Color.LightGreen);
						Vector2 routeNumberSize = _font.MeasureString("[" + (i + 1).ToString() + "]");
						Vector2 routeNumberPos = new Vector2(
							planetDrawPos.X + (nextPlanetTexture.Width - routeNumberSize.X) / 2,
							planetDrawPos.Y - routeNumberSize.Y + 100
						);

						SpriteBatch.DrawString(_font, "[" + (i + 1) + "]".ToString(), routeNumberPos, Color.Yellow);
					}
				}
			}
		}

		SpriteBatch.End();

		base.Draw(gameTime);
	}

	private void DrawDottedLine(Vector2 start, Vector2 end, Color color, Route route, SpriteFont font, float segmentLength = 10f, float spacing = 5f, float thickness = 2f)
	{
		Vector2 delta = end - start;
		float totalLength = delta.Length();
		Vector2 direction = Vector2.Normalize(delta);

		float drawnLength = 0f;

		while (drawnLength < totalLength)
		{
			float currentSegmentLength = Math.Min(segmentLength, totalLength - drawnLength);
			Vector2 segmentStart = start + direction * drawnLength;

			// Calculate angle
			float angle = (float)Math.Atan2(delta.Y, delta.X);

			// Draw segment
			SpriteBatch.Draw(_lineTexture, segmentStart, null, color, angle,
				Vector2.Zero, new Vector2(currentSegmentLength, thickness),
				SpriteEffects.None, 0f);

			drawnLength += segmentLength + spacing; // Move forward with spacing
		}

		DrawRouteFuelCost(route, start, direction, totalLength, font);
	}
	
	private void DrawRouteFuelCost(Route route, Vector2 start, Vector2 direction, float totalLength, SpriteFont font)
	{
		// Find the midpoint between planets
		Vector2 middle = start + direction * (totalLength / 2);

		// Display route cost text
		string costText = $"Cost: {route.FuelCost}";
		Vector2 textSize = font.MeasureString(costText);

		// Offset text slightly above the line
		Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
		Vector2 textPosition = middle + perpendicular * (-15); // move upward 15px

		SpriteBatch.DrawString(font, costText, textPosition - textSize / 2, Color.Tomato);
	}
}