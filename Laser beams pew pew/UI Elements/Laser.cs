using Laser_beams_pew_pew.UI_Elements.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.UI_Elements
{
    public class Laser: IUiElement
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        public Laser(Vector2 position)
        {
            Position = position;
            Texture = Main.Self.Content.Load<Texture2D>("images/laser");
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(5, 0);
        }
    }
}
