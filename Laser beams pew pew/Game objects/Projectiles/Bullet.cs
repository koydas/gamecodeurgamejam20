using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Projectiles
{
    public sealed class Bullet : GameObject
    {
        public override int HitPoints { get; set; }
        public bool HasHitSomething { get; set; }

        public Bullet(Vector2 position)
        {
            Scale = .075f;
            Speed = 10;
            Position = position;
            Texture = Main.Self.Content.Load<Texture2D>("images/bullet");
        }

        public override void Update(GameTime gameTime)
        {
            Position += new Vector2(Speed, 0);
        }
    }
}
