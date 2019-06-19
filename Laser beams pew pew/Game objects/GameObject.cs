using System.Linq;
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
        public float AngleRadian { get; set; }
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
                AngleRadian,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                depth);
        }

        public virtual bool IsHit(IGameObject collider)
        {
            if (collider is IProjectile projectile && collider.HitBox.Intersects(HitBox) && PixelPerfectHit(collider) && !projectile.IsExploding)
            {
                // is hit
                projectile.HasHitSomething = true;

                HitPoints -= 1;
                
                return true;
            }

            return false;
        }

        private bool PixelPerfectHit(IGameObject second)
        {
            var sectionHitted = Rectangle.Intersect(HitBox, second.HitBox);

            sectionHitted.Height = (int)(sectionHitted.Height / Scale);
            sectionHitted.Width = (int)(sectionHitted.Width / Scale);
            sectionHitted.X = sectionHitted.X  - (int)Position.X;
            sectionHitted.Y = sectionHitted.Y - (int)Position.Y;

            int size = sectionHitted.Width * sectionHitted.Height;

            Color[] buffer = new Color[size];
            Texture.GetData(0, sectionHitted, buffer, 0, size);

            var hasColor = buffer.Any(x => x != Color.Transparent);

            return hasColor;
        }
    }
}
