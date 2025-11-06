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
	private Texture2D _lineTexture;
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
	}

	protected override void LoadContent()
	{
		_planet1 = Content.Load<Texture2D>("images/planet1");
		_planet2 = Content.Load<Texture2D>("images/planet2");
		_planet3 = Content.Load<Texture2D>("images/planet3");
		_font = Content.Load<SpriteFont>("DefaultFont");
		//_lineTexture = new Texture2D(GraphicsDevice, 1, 1);
		//_lineTexture.SetData(new[] { Color.White });
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
					_message = "Space storm!";
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

		GraphicsDevice.Clear(Color.DarkBlue);
		SpriteBatch.DrawString(_font, $"Fuel: {_player.Fuel}", new Vector2(10, 10), Color.White);
		SpriteBatch.DrawString(_font, $"Location: {_player.Current.Name}", new Vector2(10, 40), Color.White);
		SpriteBatch.DrawString(_font, _message, new Vector2(10, 70), Color.Yellow);
		SpriteBatch.Draw(_planet1, Vector2.Zero, Color.White);
	
		SpriteBatch.End();

		base.Draw(gameTime);
	}
}