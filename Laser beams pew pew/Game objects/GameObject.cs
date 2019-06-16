using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects
{
    public abstract class GameObject : IGameObject
    {
        public abstract int HitPoints { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        protected float Scale = 1f;

        public Rectangle HitBox =>
            new Rectangle
            {
                X = (int)Position.X,
                Y = (int)Position.Y,
                Width = (int)(Texture.Width * Scale),
                Height = (int)(Texture.Height * Scale)
            };

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Draw(spriteBatch, gameTime, 1f);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float depth)
        {
            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                depth);
        }

        public virtual bool IsHit(IGameObject collider)
        {
            if (collider is IProjectile projectile && collider.HitBox.Intersects(HitBox))
            {
                // is hit
                projectile.HasHitSomething = true;

                HitPoints -= 1;

                return true;
            }

            return false;
        }
    }
}
