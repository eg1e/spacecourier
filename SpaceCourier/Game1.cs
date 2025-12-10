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

	private GameRenderer _renderer;
	private SpriteBatch _spriteBatch;

	public Game1() : base("Space Courier", 1280, 720, false)
    {
    }

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

		var ks = Keyboard.GetState();
		var routes = _engine.GetRoutesFrom(_player.Current).ToList();

		var routeIndex = GetSelectedRouteIndex(ks, routes.Count);
		if (routeIndex is int idx)
			HandleRouteSelection(routes[idx]);

		if (ks.IsKeyDown(Keys.Escape)) Exit();

		_previousKeyboardState = ks;
		base.Update(gameTime);
	}

	private void HandleRouteSelection(Route route)
	{
		TravelResult result = _engine.TryTravel(_player, route);

		_message = result.Message;

		if (result.IsGameOver)
        {
			_gameOver = true;
        }
	}


	protected override void Draw(GameTime gameTime)
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		_renderer = new GameRenderer(
			spriteBatch: _spriteBatch,
			lineTexture: _lineTexture,
			font: _font,
			planetTextures: _planetTextures
		);

		_renderer.Draw(
			_player,
			_engine.GetRoutesFrom(_player.Current),
			_message,
			GraphicsDevice
		);
		base.Draw(gameTime);
	}


	private int? GetSelectedRouteIndex(KeyboardState ks, int max)
	{
		for (int i = 0; i < max; i++)
			if (ks.IsKeyDown(Keys.D1 + i) && _previousKeyboardState.IsKeyUp(Keys.D1 + i))
				return i;

		return null;
	}

}