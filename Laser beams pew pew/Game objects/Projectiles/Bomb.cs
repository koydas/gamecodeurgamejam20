using System;
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

        private Vector2 _velocity;
        private const float Gravity = -.2f;

        public Bomb(Vector2 position)
        {
            Position = position;
            Color = Color.Yellow;
            Scale = 0.2f;
            Texture = Main.Self.Content.Load<Texture2D>("images/bomb");

            _velocity = new Vector2(-3f, 7f);
        }

        public override void Update(GameTime gameTime)
        {
            Position += _velocity;
            _velocity.Y -= Gravity;

            if (Position.Y >= Main.Self.WindowHeight - Texture.Height * Scale)
            {
                _velocity.Y *= -1;
                _velocity.Y *= 0.85f;

                Position = new Vector2(Position.X, Main.Self.WindowHeight - Texture.Height * Scale);
            }
        }

        public override bool IsHit(IGameObject collider)
        {
            if (collider is IProjectile projectile && !projectile.IsExploding && collider.HitBox.Intersects(HitBox))
            {
                projectile.HasHitSomething = true;

                _velocity.X *= -1;

                return true;
            }

            return false;
        }
    }
}
