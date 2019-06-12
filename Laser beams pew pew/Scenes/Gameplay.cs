using System.Collections.Generic;
using Laser_beams_pew_pew.Scenes.Interfaces;
using Laser_beams_pew_pew.UI_Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Laser_beams_pew_pew.Scenes
{
    public class GamePlay: IScene
    {
        public Ship Ship;
        public List<Laser> Lasers = new List<Laser>();

        public GamePlay()
        {
            Ship = new Ship();
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) && Ship.Position.Y > 0)
            {
                Ship.Position -= new Vector2(0, Ship.Speed);
            }
            if (keyboardState.IsKeyDown(Keys.Down) &&
                Ship.Position.Y + Ship.Texture.Height < Main.Self.WindowHeight)
            {
                Ship.Position += new Vector2(0, Ship.Speed);
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Lasers.Add(new Laser(Ship.Position));
            }

            foreach (var laser in Lasers)
            {
                laser.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Main.Self.GraphicsDevice.Clear(Color.HotPink);

            spriteBatch.Begin();

            spriteBatch.Draw(Ship.Texture, Ship.Position);

            foreach (var laser in Lasers)
            {
                spriteBatch.Draw(laser.Texture, laser.Position);
            }

            spriteBatch.End();
        }
    }
}
