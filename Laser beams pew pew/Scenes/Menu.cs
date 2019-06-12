using Laser_beams_pew_pew.Scenes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Scenes
{
    public class Menu: IScene
    {
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Main.Self.CurrentScene = new GamePlay();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.CornflowerBlue);

            
        }
    }
}
