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
        private double _lastMovementChange;
        private double _lastMovementChangeCoolDown;

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

            var elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (elapsedTime - _lastShotTimer > 500 && r.Next(0, 30) == 21)
            {
                Vector2 position = new Vector2
                {
                    X = Position.X + HitBox.Width / 2f,
                    Y = Position.Y + HitBox.Height / 2f,
                };

                _lasers.Add(new Laser(position));
                _lastShotTimer = elapsedTime;
            }

            if (_ship.Position.Y > Position.Y)
            {
                Position += Vector2.UnitY * Speed;
            }
            if (_ship.Position.Y < Position.Y)
            {
                Position -= Vector2.UnitY * Speed;
            }

            if (_lastMovementChangeCoolDown == 0 || elapsedTime - _lastMovementChangeCoolDown > 5000)
            {
                // Going front for 10sec
                if (_lastMovementChange == 0 || elapsedTime - _lastMovementChange < 3000 &&
                    Position.X > Main.Self.WindowWidth / 2f)
                {
                    if (_lastMovementChange == 0)
                        _lastMovementChange = elapsedTime;

                    Position -= Vector2.UnitX * Speed;

                }
                // go back
                else if (Position.X + HitBox.Width < Main.Self.WindowWidth)
                {
                    if (Position.X + HitBox.Width > Main.Self.WindowWidth)
                        _lastMovementChange = 0;

                    Position += Vector2.UnitX * Speed;
                }
                // 30sec cooldown
                else
                {
                    _lastMovementChange = 0;
                    _lastMovementChangeCoolDown = elapsedTime;
                }
            }
        }
    }
}
