using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Project11
{
    public class HealthBar
    {
        private static Texture2D _pixelTexture;
        private int _maxHealth;
        private int _currentHealth;
        private int _width;
        private int _height;
        private int _margin = 10; 
        private Color _backgroundColor;
        private Color _healthColor;
        private Color _borderColor;
        private string _name;
        private static SpriteFont _font;


        public HealthBar(int maxHealth, int width = 200, int height = 20)
        {
            _name = "Health";
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _width = width;
            _height = height;
            _backgroundColor = Color.Black;
            _healthColor = Color.DarkRed;
            _borderColor = Color.White;
        }

        public void SetHealth(int health)
        {
            _currentHealth = MathHelper.Clamp(health, 0, _maxHealth);
        }


        public void TakeDamage(int damage)
        {
            _currentHealth = MathHelper.Clamp(_currentHealth - damage, 0, _maxHealth);
        }

        public void Heal(int amount)
        {
            _currentHealth = MathHelper.Clamp(_currentHealth + amount, 0, _maxHealth);
        }

        public bool IsDead => _currentHealth <= 0;

        private static void EnsurePixelTexture()
        {
            // Create pixel texture only once and cache it
            if (_pixelTexture == null)
            {
                _pixelTexture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
                _pixelTexture.SetData(new[] { Color.White });
            }
        }

        

        public void Draw()
        {
           
            EnsurePixelTexture();

           
            float healthPercentage = (float)_currentHealth / _maxHealth;
            int healthBarWidth = (int)(_width * healthPercentage);

           
            int x = Globals.WindowSize.X - _width - _margin;
            int y = _margin;

            
            Rectangle backgroundRect = new Rectangle(x, y, _width, _height);
            Rectangle healthRect = new Rectangle(x, y, healthBarWidth, _height);
            Rectangle borderRect = new Rectangle(x - 1, y - 1, _width + 2, _height + 2);

            
            if (healthPercentage > 0.6f)
                _healthColor = Color.Green;
            else if (healthPercentage > 0.3f)
                _healthColor = Color.Yellow;
            else
                _healthColor = Color.Red;

           
            Globals.SpriteBatch.Draw(_pixelTexture, borderRect, _borderColor);

           
            Color backgroundColorWithAlpha = new Color(_backgroundColor, 200);
            Globals.SpriteBatch.Draw(_pixelTexture, backgroundRect, backgroundColorWithAlpha);

           
            if (healthBarWidth > 0)
            {
                Globals.SpriteBatch.Draw(_pixelTexture, healthRect, _healthColor);
            }
        }
    }
}
