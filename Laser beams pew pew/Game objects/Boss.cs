using System;
using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects
{
    //todo : make as singleton
    public sealed class Boss : GameObject
    {
        public static bool HasInstance;
        private readonly List<Laser> _lasers;
        private double _lastShotTimer;
        private Ship _ship;

        public override int HitPoints { get; set; }

        public Boss(List<Laser> lasers, Ship ship)
        {
            if (HasInstance)
                throw new Exception("You can't have more than one Boss at a time");

            HasInstance = true;
            Scale = 0.5f;

            HitPoints = 100;
            _lasers = lasers;
            _ship = ship;

            Speed = 4;

            Texture = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }

        public override void Update(GameTime gameTime)
        {
            Random r = new Random();

            if (gameTime.TotalGameTime.TotalMilliseconds - _lastShotTimer > 500 && r.Next(0, 30) == 21)
            {
                Vector2 position = new Vector2
                {
                    X = Position.X + HitBox.Width / 2f,
                    Y = Position.Y + HitBox.Height / 2f,
                };

                _lasers.Add(new Laser(position));
                _lastShotTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (_ship.Position.Y > Position.Y)
            {
                Position += Vector2.UnitY * Speed;
            }
            if (_ship.Position.Y < Position.Y)
            {
                Position -= Vector2.UnitY * Speed;
            }
        }
    }
}
