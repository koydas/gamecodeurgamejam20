﻿using System;
using System.Linq;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Projectiles
{
    public sealed class Bomb : GameObject, IProjectile
    {
        public override int HitPoints { get; set; }
        public bool HasHitSomething { get; set; }
        public bool IsExploding { get; set; }
        public bool ExplosionFinished { get; set; }

        private Vector2 _velocity;
        private const float Gravity = -.2f;
        public bool Reflected;
        private readonly Texture2D[] _explosionTextures;
        private int i;
        private double _oldGameTime;
        private readonly Random _random = new Random();

        public Bomb(Vector2 position, float? scale = null)
        {
            Position = position;
            Color = Color.Yellow;
            Scale = scale ?? 0.2f;
            Texture = Main.Self.Content.Load<Texture2D>("images/bomb");

            _velocity = new Vector2(-3f, 7f);

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
            Position += _velocity;
            _velocity.Y -= Gravity;

            var bottom = Main.Self.WindowHeight - Texture.Height * Scale - 125;
            if (Position.Y >= bottom)
            {
                _velocity.Y *= -1;
                _velocity.Y *= _random.Next(85, 100) / 100f;

                Position = new Vector2(Position.X, bottom);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HasHitSomething)
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
                            X = Position.X - explosionTexture.Width / 2f * .7f,
                            Y = Position.Y - explosionTexture.Height / 2f
                        },
                        null,
                        Color.White,
                        AngleRadian,
                        Vector2.Zero,
                        0.7f,
                        SpriteEffects.None,
                        1f);
                }

                return;
            }

            base.Draw(spriteBatch, gameTime);
        }

        public override bool IsHit(IGameObject collider)
        {
            if (collider is Bullet bullet && !bullet.IsExploding && bullet.HitBox.Intersects(HitBox))
            {
                bullet.HasHitSomething = true;
            }

            var projectile = collider as IProjectile;

            if (projectile != null && projectile.Position.X > Main.Self.WindowWidth / 2f)
            {
                return false;
            }

            if (!Reflected && projectile != null && !projectile.IsExploding  && collider.HitBox.Intersects(HitBox))
            {
                projectile.HasHitSomething = true;

                _velocity.X *= -1;
                Reflected = true;

                return true;
            }

            return false;
        }
    }
}
