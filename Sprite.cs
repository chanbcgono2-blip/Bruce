using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Project11
{
    public class Sprite(Texture2D texture, Vector2 position)
    {
        private readonly Texture2D _texture = texture;
        public Vector2 Position { get; protected set; } = position;
        public Vector2 Origin { get; protected set; } = new(texture.Width / 12, texture.Height / 12);

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_texture, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
