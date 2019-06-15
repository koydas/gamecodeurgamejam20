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
        private readonly List<Laser> _lasers;
        private double _lastShotTimer;
        private readonly Ship _ship;
        private double _lastMovementChange;
        private double _lastMovementChangeCoolDown;
        private readonly Random _random = new Random();

        public override int HitPoints { get; set; }

        public Boss(List<Laser> lasers, Ship ship)
        {
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
            var elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;

            ShootLaser(elapsedTime);
            Move(elapsedTime);
        }

        private void Move(double elapsedTime)
        {
            MoveUpDown();
            MoveLeftRight(elapsedTime);
        }

        private void MoveLeftRight(double elapsedTime)
        {
            if (_lastMovementChangeCoolDown == 0 || elapsedTime - _lastMovementChangeCoolDown > 5000)
            {
                // Going front for 3sec
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

        private void MoveUpDown()
        {
            if (_ship.Position.Y > Position.Y)
            {
                Position += Vector2.UnitY * Speed;
            }
            if (_ship.Position.Y < Position.Y)
            {
                Position -= Vector2.UnitY * Speed;
            }
        }

        private void ShootLaser(double elapsedTime)
        {
            if (elapsedTime - _lastShotTimer > 500 && _random.Next(0, 30) == 21)
            {
                Vector2 position = new Vector2
                {
                    X = Position.X + HitBox.Width / 2f,
                    Y = Position.Y + HitBox.Height / 2f,
                };

                _lasers.Add(new Laser(position));
                _lastShotTimer = elapsedTime;
            }
        }
    }
}
