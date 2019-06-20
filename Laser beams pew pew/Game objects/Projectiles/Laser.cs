using System;
using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Projectiles
{
    public sealed class Laser : GameObject, IProjectile
    {
        public override int HitPoints { get; set; }
        public bool HasHitSomething { get; set; }
        public bool IsExploding { get; set; }

        public Laser(Vector2 position, float angle = 0f)
        {
            Scale = .075f;
            Speed = .7f;
            AngleRadian = (float)(Math.PI / 180) * angle;
            Position = position;
            Texture = Main.Self.Content.Load<Texture2D>("images/laser");
        }

        public override void Update(GameTime gameTime)
        {
            var milliseconds = gameTime.ElapsedGameTime.Milliseconds;

            var velocityX = (float)Math.Cos(AngleRadian) * Speed * milliseconds;
            var velocityY = (float)Math.Sin(AngleRadian) * Speed * milliseconds;

            Position += new Vector2(velocityX, velocityY);
        }
    }
}
