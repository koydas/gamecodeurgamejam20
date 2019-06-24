using System;
using System.Collections.Generic;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Laser_beams_pew_pew.Game_objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Bosses
{
    public sealed class BombBoss : Boss
    {
        private List<IProjectile> _bombs;
        public override int HitPoints { get; set; }
        //public Texture2D TextureHealthBar { get; set; }
        public Texture2D TextureLauncher { get; set; }

        private Vector2 _positionToGo;

        public BombBoss(List<IProjectile> projectiles, Player player) : base(projectiles, player)
        {
            HitPoints = 1000;

            Scale = 0.5f;

            HitPoints = 5;
            MaxHitPoints = HitPoints;
            //_lasers = lasers;
            //_player = player;
            _bombs = projectiles;
            Texture = Main.Self.Content.Load<Texture2D>("images/bomb-boss");
            TextureLauncher = Main.Self.Content.Load<Texture2D>("images/bomb-boss-launcher");

            //TextureHealthBar = Main.Self.Content.Load<Texture2D>("images/healthbar");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }
        
        public override void Update(GameTime gameTime)
        {
            //var lifePercentage = HitPoints / (float)_maxHitPoints;
            //if (lifePercentage > 0.7) 
                Phase1();
            //else if (lifePercentage > .4)
            //    Phase2();
        }

        private void Phase1()
        {
            if (_bombs.Count <= 0)
                _bombs.Add(new Bomb(Position));

            if (_positionToGo.X - Position.X < 10 && _positionToGo.Y - Position.Y < 10 ||
                _positionToGo == default(Vector2)) // Get new position to go
            {
                Random random = new Random();
                int x = random.Next(Main.Self.WindowWidth / 2, Main.Self.WindowWidth - HitBox.Width);
                int y = _positionToGo.Y > Main.Self.WindowHeight / 2f
                    ? random.Next(0, Main.Self.WindowHeight / 2 - HitBox.Height * 2)
                    : random.Next(Main.Self.WindowHeight / 2, Main.Self.WindowHeight - HitBox.Height);

                _positionToGo = new Vector2(x, y);
            }
            else
            {
                Position += (_positionToGo - Position) * .01f;
            }
        }

        private void Phase2()
        {
            // TODO : Faire une phase 2
            Phase1();
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
            
            //float fullWidth = 6f / MaxHitPoints * HitPoints;

            //spriteBatch.Draw(
            //    TextureHealthBar,
            //    Vector2.Zero,
            //    null,
            //    Color.White,
            //    0f,
            //    Vector2.Zero,
            //    new Vector2(fullWidth, 1f),
            //    SpriteEffects.None,
            //    1f);
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
