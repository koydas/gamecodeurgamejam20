using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects
{
    //todo : make as singleton
    public sealed class Boss : GameObject
    {
        public override int HitPoints { get; set; }

        public Boss()
        {
            HitPoints = 1;

            Texture = Main.Self.Content.Load<Texture2D>("images/ennemy-ship");

            Position = new Vector2
            {
                X = Main.Self.WindowWidth - Texture.Width,
                Y = 0
            };
        }

        public override void Update(GameTime gameTime) { }
    }
}
