using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Laser_beams_pew_pew
{
    public static class Settings
    {
        public static KeyboardLayout Keys = new KeyboardLayout
        {
            Up = Microsoft.Xna.Framework.Input.Keys.Up,
            Down = Microsoft.Xna.Framework.Input.Keys.Down,
            Left = Microsoft.Xna.Framework.Input.Keys.Left,
            Right = Microsoft.Xna.Framework.Input.Keys.Right,
            Fire = Microsoft.Xna.Framework.Input.Keys.Space
        };
        
        private static string _path = "./configs.json";

        public static void LoadSettings()
        {
            if (!File.Exists(_path)) return;

            string[] lines = File.ReadAllLines(_path);

            string json = "";
            foreach (var line in lines)
            {
                json += line;
            }

            var configs = JsonConvert.DeserializeObject<KeyboardLayout>(json);

            Keys = configs;
        }

        public static void SaveSettings()
        {
            var a = new KeyboardLayout
            {
                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Fire
            };

            var json = JsonConvert.SerializeObject(a);

            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            using (FileStream fs = File.Create(_path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
