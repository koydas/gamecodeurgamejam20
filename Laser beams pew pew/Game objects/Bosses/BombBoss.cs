using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Bosses
{
    public sealed class BombBoss : Boss
    {
        private int _maxHitPoints;
        public override int HitPoints { get; set; }
        public Texture2D TextureHealthBar { get; set; }
        public Texture2D TextureLauncher { get; set; }

        public BombBoss(List<Laser> lasers, Player player) : base(lasers, player)
        {
            HitPoints = 1000;

            Scale = 0.5f;

            HitPoints = 50;
            _maxHitPoints = HitPoints;
            //_lasers = lasers;
            //_player = player;

            Texture = Main.Self.Content.Load<Texture2D>("images/bomb-boss");
            TextureLauncher = Main.Self.Content.Load<Texture2D>("images/bomb-boss-launcher");

            TextureHealthBar = Main.Self.Content.Load<Texture2D>("images/healthbar");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }
        
        public override void Update(GameTime gameTime)
        {
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
            
            float fullWidth = 6f / _maxHitPoints * HitPoints;

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
