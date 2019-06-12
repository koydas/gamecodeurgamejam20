using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.UI_Elements.Interfaces
{
    interface IUiElement
    {
        Texture2D Texture{ get; set; }
        Vector2 Position{ get; set; }
        float Speed{ get; set; }
    }
}
