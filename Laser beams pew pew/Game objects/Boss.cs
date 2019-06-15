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
        public override int HitPoints { get; set; }

        private readonly List<Laser> _lasers;
        private double _lastShotTimer;
        private readonly Player _player;
        private double _lastMovementChange;
        private double _lastMovementChangeCoolDown;
        private readonly Random _random = new Random();
        private Texture2D TextureHealthBar { get; set; }

        private Vector2 _laserPosition => new Vector2
        {
            X = Position.X + HitBox.Width / 10f,
            Y = Position.Y + HitBox.Height / 3f * 1.8f,
        };

        public Boss(List<Laser> lasers, Player player)
        {
            Scale = 0.5f;

            HitPoints = 100;
            _lasers = lasers;
            _player = player;

            Speed = 4;

            Texture = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");
            TextureHealthBar = Main.Self.Content.Load<Texture2D>("images/healthbar");

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
            if (_player.Position.Y > _laserPosition.Y)
            {
                Position += Vector2.UnitY * Speed;
            }
            if (_player.Position.Y < _laserPosition.Y)
            {
                Position -= Vector2.UnitY * Speed;
            }
        }

        private void ShootLaser(double elapsedTime)
        {
            if (elapsedTime - _lastShotTimer > 500 && _random.Next(0, 30) == 21)
            {
                _lasers.Add(new Laser(_laserPosition));
                _lastShotTimer = elapsedTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            float fullWidth = 6f / 100 * HitPoints;

            spriteBatch.Draw(
                TextureHealthBar,
                Vector2.Zero,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(fullWidth, 1f), 
                SpriteEffects.None,
                1f);
        }
    }
}
