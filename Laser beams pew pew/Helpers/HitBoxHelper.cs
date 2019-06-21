using Laser_beams_pew_pew.Game_objects.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laser_beams_pew_pew.Helpers
{
    public static class HitBoxHelper
    {
        private static Texture2D _texture;

        public static void DrawHitBox(this IGameObject gameObject)
        {
            Main.Self.SpriteBatch.Draw(
                CreateHitBoxTexture(),
                gameObject.Position,
                null,
                Color.White,
                gameObject.AngleRadian,
                Vector2.Zero,
                new Vector2(gameObject.HitBox.Width, gameObject.HitBox.Height), 
                SpriteEffects.None,
                1f);
        }

        private static Texture2D CreateHitBoxTexture()
        {
            if (_texture == null)
            {
                _texture = new Texture2D(Main.Self.GraphicsDevice, 1, 1);
                // Hotpink : R: 255,G: 105,B: 180,A: 255
                _texture.SetData(new[] { new Color(255, 105, 180, 50) });

                return _texture;
            }

            return _texture;
        }
    }
}
