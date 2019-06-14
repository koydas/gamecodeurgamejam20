using System.Collections.Generic;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Laser_beams_pew_pew.UI_Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Scenes
{
    public class GamePlay: IScene
    {
        public Ship Ship;
        public List<Laser> Lasers = new List<Laser>();
        
        public GamePlay()
        {
            Ship = new Ship(Lasers);
        }

        public void Update(GameTime gameTime)
        {
            Ship.Update(gameTime);

            for (var index = 0; index < Lasers.Count; index++)
            {
                var laser = Lasers[index];

                laser.Update(gameTime);

                if (laser.Position.X > Main.Self.WindowWidth)
                {
                    Lasers.RemoveAt(index);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(
                Ship.Texture,
                Ship.Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                0.17f,
                SpriteEffects.None,
                1f);

            foreach (var laser in Lasers)
            {
                spriteBatch.Draw(laser.Texture, laser.Position);
            }

            spriteBatch.End();
        }
    }
}
