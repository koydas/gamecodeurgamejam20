using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Game_objects.Interfaces
{
    public interface IGameObject
    {
        Rectangle HitBox { get; }
        Texture2D Texture{ get; set; }
        Vector2 Position{ get; set; }
        float Speed{ get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
