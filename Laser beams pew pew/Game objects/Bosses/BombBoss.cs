﻿using System;
using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Bosses
{
    public sealed class BombBoss : Boss
    {
        private readonly List<IProjectile> _bombs;
        public override int HitPoints { get; set; }
        public Texture2D TextureLauncher { get; set; }

        private Vector2 _positionToGo;
        private double _lastShot;
        private readonly Random _random = new Random();
        private int _currentPhase = 1;

        public BombBoss(List<IProjectile> projectiles, Player player) : base(projectiles, player)
        {
            HitPoints = 1000;

            Scale = 0.5f;

            HitPoints = 10;
            MaxHitPoints = HitPoints;
            _bombs = projectiles;
            Texture = Main.Self.Content.Load<Texture2D>("images/bomb-boss");
            TextureLauncher = Main.Self.Content.Load<Texture2D>("images/bomb-boss-launcher");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }
        
        public override void Update(GameTime gameTime)
        {
            var elapsed = gameTime.TotalGameTime.TotalMilliseconds;
            if (HitPointsPercentage > 0.7) 
                Phase1(elapsed);
            else if (HitPointsPercentage > .6)
                Phase2(elapsed);
            else
                Phase3(elapsed);
        }

        private void Phase1(double elapsed)
        {
            if (_currentPhase == 2) _currentPhase = 3;
            if (_bombs.Count < 3 && elapsed - _lastShot > 1500)
            {
                _bombs.Add(new Bomb(Position));
                _lastShot = elapsed;
            }

            Movement();
        }

        private void Phase2(double elapsed)
        {
            if (_currentPhase == 2)
            {
                _currentPhase = 3;
                _bombs.Clear();
            }

            if (_bombs.Count < 1)
            {
                var bomb = new Bomb(Position, 1f);
                _bombs.Add(bomb);

                _lastShot = elapsed;
            }
            
            if (_positionToGo.X - Position.X < 10 && _positionToGo.Y - Position.Y < 10 ||
                _positionToGo == default(Vector2))
            {
                _positionToGo = new Vector2
                {
                    X = _random.Next(Main.Self.WindowWidth/2, Main.Self.WindowWidth),
                    Y = _random.Next(0, 300)
                };
            }
            else
            {
                Position += (_positionToGo - Position) * .01f;
            }
        }

        private void Phase3(double elapsed)
        {
            if (elapsed - _lastShot > 1000)
            {
                _bombs.Add(new Bomb(Position));
                _lastShot = elapsed;
            }
        }

        private void Movement()
        {
            if (_positionToGo.X - Position.X < 10 && _positionToGo.Y - Position.Y < 10 ||
                _positionToGo == default(Vector2)) // Get new position to go
            {
                var x = _random.Next(Main.Self.WindowWidth / 2, Main.Self.WindowWidth - HitBox.Width);
                var y = _positionToGo.Y > Main.Self.WindowHeight / 2f
                    ? _random.Next(0, Main.Self.WindowHeight / 2 - HitBox.Height * 2)
                    : _random.Next(Main.Self.WindowHeight / 2, Main.Self.WindowHeight - HitBox.Height - 125);

                _positionToGo = new Vector2(x, y);
            }
            else
            {
                Position += (_positionToGo - Position) * .01f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(
                TextureLauncher,
                Position + new Vector2(Texture.Width*Scale - 100, Texture.Height*Scale / 2f),
                null,
                Color.White,
                AngleRadian,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                1f);
        }

        public override bool IsHit(IGameObject collider)
        {
            if (!(collider is Bomb bomb))
            {
                if (collider is IProjectile projectile && projectile.HitBox.Intersects(HitBox))
                {
                    projectile.HasHitSomething = true;
                }

                return false;
            }

            if (!bomb.Reflected) return false;

            return base.IsHit(collider);
        }
    }
}
