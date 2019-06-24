using System;
using System.Collections.Generic;
using System.Linq;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Game_objects
{
    public sealed class Player : GameObject
    {
        public override int HitPoints { get; set; }
        public bool IsExploding { get; set; }
        public bool ExplosionFinished { get; set; }

        private readonly List<Bullet> _lasers;
        private double _lastShotTimer;
        private int i;
        private double _oldGameTime;
        private Texture2D[] _explosionTextures;

        public Player(List<Bullet> lasers)
        {
            LoadTextures();

            Scale = 0.1f;

            HitPoints = 1;

            Position = new Vector2(20, Main.Self.WindowHeight / 2 - HitBox.Height / 2);

            Speed = 8;
            _lasers = lasers;   
        }

        private void LoadTextures()
        {
            Texture = Main.Self.Content.Load<Texture2D>("images/ship");
            _explosionTextures = new[]
            {
                Main.Self.Content.Load<Texture2D>("images/explosion0"),
                Main.Self.Content.Load<Texture2D>("images/explosion1"),
                Main.Self.Content.Load<Texture2D>("images/explosion2"),
                Main.Self.Content.Load<Texture2D>("images/explosion3"),
                Main.Self.Content.Load<Texture2D>("images/explosion4"),
                Main.Self.Content.Load<Texture2D>("images/explosion5")
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (IsExploding) return;

            var keyboardState = Keyboard.GetState();

            Move(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Space) &&
                gameTime.TotalGameTime.TotalMilliseconds - _lastShotTimer > 500)
            {
                Vector2 position = new Vector2
                {
                    X = Position.X + HitBox.Width - HitBox.Width/8f,
                    Y = Position.Y + HitBox.Height / 2f,
                };

                _lasers.Add(new Bullet(position));
                _lastShotTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        private void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up) && Position.Y >= 0)
            {
                Position -= new Vector2(0, Speed);
            }

            if (keyboardState.IsKeyDown(Keys.Down) &&
                Position.Y + HitBox.Height <= Main.Self.WindowHeight - 200)
            {
                Position += new Vector2(0, Speed);
            }

            if (keyboardState.IsKeyDown(Keys.Left) &&
                Position.X > 0)
            {
                Position -= new Vector2(Speed, 0);
            }

            if (keyboardState.IsKeyDown(Keys.Right) &&
                Position.X + HitBox.Width <= Main.Self.WindowWidth/2f)
            {
                Position += new Vector2(Speed, 0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HitPoints <= 0)
            {
                IsExploding = true;
                if (gameTime.TotalGameTime.TotalSeconds - _oldGameTime > 0.15)
                {
                    if (i <= 5)
                        i++;
                    if (i > 5)
                    {
                        ExplosionFinished = true;
                    }
                    _oldGameTime = gameTime.TotalGameTime.TotalSeconds;
                }

                var explosionTexture = _explosionTextures.ElementAtOrDefault(i);

                if (explosionTexture != null)
                {
                    spriteBatch.Draw(
                        explosionTexture,
                        new Vector2
                        {
                            X = Position.X - explosionTexture.Width / 2f,
                            Y = Position.Y - explosionTexture.Height / 2f
                        },
                        null,
                        Color.White,
                        AngleRadian,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);
                }

                return;
            }

            base.Draw(spriteBatch, gameTime);
        }        
    }
}
