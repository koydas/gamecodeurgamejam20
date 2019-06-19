using System.Linq;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Projectiles
{
    public sealed class Bullet : GameObject, IProjectile
    {
        private double _oldGameTime;
        private int i;
        public bool ExplosionFinished;
        private readonly Texture2D[] _explosionTextures;

        public override int HitPoints { get; set; }
        public bool HasHitSomething { get; set; }
        public bool IsExploding { get; set; }

        public Bullet(Vector2 position)
        {
            Scale = .075f;
            Speed = 10;
            Position = position;
            Texture = Main.Self.Content.Load<Texture2D>("images/bullet");

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
            Position += new Vector2(Speed, 0);
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
                            X = Position.X - explosionTexture.Width/2f*.7f,
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
    }
}
