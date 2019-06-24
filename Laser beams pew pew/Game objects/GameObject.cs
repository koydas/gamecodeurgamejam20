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
        public Color Color = Color.White;

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
                Color,
                AngleRadian,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                depth);
        }

        public virtual bool IsHit(IGameObject collider)
        {
            if (collider is IProjectile projectile && !projectile.IsExploding && HitBox.Intersects(collider.HitBox) && PixelPerfectHit(collider))
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
            var sectionHitted = GetOverlapsedSection(second);

            var gameObjectHasColor = HasColorInSection(Texture, sectionHitted);
            var projectileObjectHasColor = HasColorInSection(second.Texture, sectionHitted);

            return gameObjectHasColor && projectileObjectHasColor;
        }

        private Rectangle GetOverlapsedSection(IGameObject second)
        {
            var overlapsedSection = Rectangle.Intersect(HitBox, second.HitBox);

            overlapsedSection.Height = (int)(overlapsedSection.Height / Scale);
            overlapsedSection.Width = (int)(overlapsedSection.Width / Scale);
            overlapsedSection.X = overlapsedSection.X - (int)Position.X;
            overlapsedSection.Y = overlapsedSection.Y - (int)Position.Y;

            return overlapsedSection;
        }

        private bool HasColorInSection(Texture2D texture, Rectangle section)
        {
            int size = section.Width * section.Height;

            Color[] buffer = new Color[size];
            Texture.GetData(0, section, buffer, 0, size);

            var hasColor = buffer.Any(x => x != Color.Transparent);

            return hasColor;
        }
    }
}
