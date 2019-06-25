using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Laser_beams_pew_pew.Game_objects.Bosses;
using Laser_beams_pew_pew.Game_objects.Bosses.Enums;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Scenes
{
    public class Menu : IScene
    {
        private SpriteFont _font;
        private readonly Texture2D _laserboss;
        private readonly Vector2 _laserBossPosition = new Vector2(550, 500);
        private Color _laserBossColor = Color.White;

        private Texture2D _bombBoss;
        private readonly Vector2 _bombBossPosition;
        private Color _bombBossColor = Color.White;
        private Texture2D _header;

        private Color _quitColor;
        private Vector2 _quitPosition = new Vector2(50, Main.Self.WindowHeight - 100);

        private Color _configColor;
        private Vector2 _configPosition = new Vector2(Main.Self.WindowWidth - 550, Main.Self.WindowHeight - 100);
        private readonly Rectangle _quitRect;
        private readonly Rectangle _configRect;
        private bool _showConfig;
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyboardState;

        private KeyValuePair<string, Keys?>? changeKey = null;

        //private bool changeUpKey;
        //private bool changeDownKey;
        //private bool changeLeftKey;
        //private bool changeRightKey;
        //private bool changeFireKey;

        public Menu()
        {
            Main.Self.IsMouseVisible = true;

            _font = Main.Self.Content.Load<SpriteFont>("fonts/Space Age");
            _header = Main.Self.Content.Load<Texture2D>("images/header");
            _laserboss = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");
            _bombBoss = Main.Self.Content.Load<Texture2D>("images/bomb-boss");


            _bombBossPosition = new Vector2(_laserBossPosition.X + _laserboss.Width, _laserBossPosition.Y);
            _quitRect = new Rectangle(_quitPosition.ToPoint(), new Point(150, 50));
            _configRect = new Rectangle(_configPosition.ToPoint(), new Point(550, 50));
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            var mouseRect = new Rectangle
            {
                Width = 1,
                Height = 1,
                X = mouseState.Position.X,
                Y = mouseState.Position.Y
            };

            if (_showConfig)
            {
                ConfigMenu(mouseRect, mouseState, keyboardState);
            }
            else
            {
                MainMenu(mouseRect, mouseState);
            }

            _oldMouseState = mouseState;
            _oldKeyboardState = keyboardState;
        }

        private void ConfigMenu(Rectangle mouseRect, MouseState mouseState, KeyboardState keyboardState)
        {
            if (changeKey != null)
            {
                var pressedKeys = keyboardState.GetPressedKeys();
                
                if (pressedKeys.Any())
                {
                    var pressedKey = pressedKeys.First();

                    if (pressedKey != Keys.Escape)
                    {
                        switch (changeKey.Value.Key)
                        {
                            case "up":
                                Settings.Up = pressedKey;
                                break;
                            case "down":
                                Settings.Down = pressedKey;
                                break;
                            case "left":
                                Settings.Left = pressedKey;
                                break;
                            case "right":
                                Settings.Right = pressedKey;
                                break;
                            case "fire":
                                Settings.Fire = pressedKey;
                                break;
                        }
                    }

                    changeKey = null;
                }

                return;
            }

            _quitColor = mouseRect.Intersects(_quitRect) ? Color.Gray : Color.White;
            _configColor = mouseRect.Intersects(_configRect) ? Color.Gray : Color.White;

            if (keyboardState.IsKeyDown(Keys.Escape) && _oldKeyboardState.IsKeyUp(Keys.Escape))
            {
                _showConfig = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseRect.Intersects(_quitRect))
                {
                    _showConfig = false;
                }
                else if (mouseRect.Intersects(_configRect))
                {
                    _showConfig = true;
                }

                else if (mouseRect.Intersects(new Rectangle(new Point(700, 250), new Point(400, 50))))
                {
                    // Change Up key
                    
                    changeKey = new KeyValuePair<string, Keys?>("up", null);
                }
                else if (mouseRect.Intersects(new Rectangle(new Point(700, 350), new Point(400, 50))))
                {
                    // Change Down key
                    changeKey = new KeyValuePair<string, Keys?>("down", null);
                }
                else if (mouseRect.Intersects(new Rectangle(new Point(700, 450), new Point(400, 50))))
                {
                    // Change Left key
                    changeKey = new KeyValuePair<string, Keys?>("left", null);
                }
                else if (mouseRect.Intersects(new Rectangle(new Point(700, 550), new Point(400, 50))))
                {
                    // Change Right key
                    changeKey = new KeyValuePair<string, Keys?>("right", null);
                }
                else if (mouseRect.Intersects(new Rectangle(new Point(700, 650), new Point(400, 50))))
                {
                    // Change Fire key
                    changeKey = new KeyValuePair<string, Keys?>("fire", null);
                }
            }
        }

        private void MainMenu(Rectangle mouseRect, MouseState mouseState)
        {
            var laserBossRect = new Rectangle
            {
                Width = _laserboss.Width / 2,
                Height = _laserboss.Height / 2,
                X = (int)_laserBossPosition.X,
                Y = (int)_laserBossPosition.Y,
            };

            var bombBossRect = new Rectangle
            {
                Width = _bombBoss.Width / 2,
                Height = _bombBoss.Height / 2,
                X = (int)_bombBossPosition.X,
                Y = (int)_bombBossPosition.Y,
            };

            _laserBossColor = mouseRect.Intersects(laserBossRect) ? Color.White : Color.Gray;
            _bombBossColor = mouseRect.Intersects(bombBossRect) ? Color.White : Color.Gray;

            _quitColor = mouseRect.Intersects(_quitRect) ? Color.Gray : Color.White;
            _configColor = mouseRect.Intersects(_configRect) ? Color.Gray : Color.White;

            if (mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseRect.Intersects(laserBossRect))
                {
                    Main.Self.CurrentScene = new GamePlay<LaserBoss>();
                }
                else if (mouseRect.Intersects(bombBossRect))
                {
                    Main.Self.CurrentScene = new GamePlay<BombBoss>();
                }
                else if (mouseRect.Intersects(_quitRect))
                {
                    Main.Self.Exit();
                }
                else if (mouseRect.Intersects(_configRect))
                {
                    _showConfig = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(_header, Vector2.One);

            spriteBatch.DrawString(_font, "Choose your enemy", new Vector2(600, 400), Color.White);
            spriteBatch.DrawString(_font, "Quit", _quitPosition, _quitColor);
            spriteBatch.DrawString(_font, "Configurations", _configPosition, _configColor);

            spriteBatch.Draw(
                _laserboss,
                _laserBossPosition,
                null,
                _laserBossColor,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                1f);

            spriteBatch.Draw(
                _bombBoss,
                _bombBossPosition,
                null,
                _bombBossColor,
                0f,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                1f);

            if (_showConfig)
            {
                var overlayTexture = new Texture2D(Main.Self.GraphicsDevice, 1, 1);
                overlayTexture.SetData(new[] { Color.Black });

                spriteBatch.Draw(
                    overlayTexture,
                    Vector2.Zero,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    new Vector2(1920, 1080),
                    SpriteEffects.None,
                    1f);

                spriteBatch.DrawString(_font, "Keyboard layout", new Vector2(700, 75), Color.White);

                string upText = changeKey != null && changeKey.Value.Key == "up" ? "Up : hit the new key" : $"Up : {Settings.Up.ToString()}";
                string downText = changeKey != null && changeKey.Value.Key == "down" ? "Down : hit the new key" : $"Down : {Settings.Down.ToString()}";
                string leftText = changeKey != null && changeKey.Value.Key == "left" ? "Left : hit the new key" : $"Left : {Settings.Left.ToString()}";
                string rightText = changeKey != null && changeKey.Value.Key == "right" ? "Right : hit the new key" : $"Right : {Settings.Right.ToString()}";
                string fireText = changeKey != null && changeKey.Value.Key == "fire" ? "Fire : hit the new key" : $"Fire : {Settings.Fire.ToString()}";

                spriteBatch.DrawString(_font, upText, new Vector2(700, 250), Color.White);
                spriteBatch.DrawString(_font, downText, new Vector2(700, 350), Color.White);
                spriteBatch.DrawString(_font, leftText, new Vector2(700, 450), Color.White);
                spriteBatch.DrawString(_font, rightText, new Vector2(700, 550), Color.White);
                spriteBatch.DrawString(_font, fireText, new Vector2(700, 650), Color.White);

                spriteBatch.DrawString(_font, "Back", _quitPosition, _quitColor);
                //spriteBatch.DrawString(_font, "Save", _configPosition + new Vector2(300, 0), _configColor);
            }

            spriteBatch.End();
        }
    }
}
