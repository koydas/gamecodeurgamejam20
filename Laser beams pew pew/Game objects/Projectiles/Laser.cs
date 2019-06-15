using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Projectiles
{
    public sealed class Laser : GameObject, IProjectile
    {
        public override int HitPoints { get; set; }
        public bool HasHitSomething { get; set; }

        public Laser(Vector2 position)
        {
            Scale = .075f;
            Speed = 10;
            Position = position;
            Texture = Main.Self.Content.Load<Texture2D>("images/laser");
        }

        public override void Update(GameTime gameTime)
        {
            Position += new Vector2(-Speed, 0);
        }
    }
}
