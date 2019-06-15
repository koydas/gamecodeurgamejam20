using System;
using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Game_objects
{
    public sealed class Ship : GameObject
    {
        public override int HitPoints { get; set; }

        private readonly List<Bullet> _lasers;
        private double _lastShotTimer;
        private int _burst;
        private double _gustCooldown;

        public Ship(List<Bullet> lasers)
        {
            Scale = 0.1f;

            HitPoints = 1;

            Texture = Main.Self.Content.Load<Texture2D>("images/ship");
            Position = new Vector2(20, Main.Self.WindowHeight / 2 - HitBox.Height / 2);

            Speed = 8;
            _lasers = lasers;
            
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            Move(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Space) &&
                gameTime.TotalGameTime.TotalMilliseconds - _lastShotTimer > 200)
            {
                Vector2 position = new Vector2
                {
                    X = Position.X + HitBox.Width/2f,
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
                Position.Y + HitBox.Height <= Main.Self.WindowHeight)
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
    }
}
