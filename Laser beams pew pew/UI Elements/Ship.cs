using Laser_beams_pew_pew.UI_Elements.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.UI_Elements
{
    public class Ship: IUiElement
    {
        public Ship()
        {
            Texture = Main.Self.Content.Load<Texture2D>("images/ship");
            Position = new Vector2(20, Main.Self.WindowHeight / 2 - Texture.Height / 2);
            Speed = 5;
        }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
    }
}
