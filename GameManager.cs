using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project11
{
    public class GameManager
    {
        private Coin _coin;
        private Hero _hero;
        private HealthBar _healthBar;
        private Map _map;
        private Matrix _translation;
        public Matrix Translation => _translation;

        public void Init()
        {
            _map = new();
            _coin = new(new(300, 300));
            _hero = new();
            _hero.SetBounds(_map.MapSize, _map.TileSize);
            _healthBar = new HealthBar(_hero.MaxHealth, 200, 20);
            CalculateTranslation();
        }

        private void CalculateTranslation()
        {
            var dx = (Globals.WindowSize.X / 2) - _hero.Position.X;
            dx = MathHelper.Clamp(dx, -_map.MapSize.X + Globals.WindowSize.X + (_map.TileSize.X / 2), _map.TileSize.X / 2);
            var dy = (Globals.WindowSize.Y / 2) - _hero.Position.Y;
            dy = MathHelper.Clamp(dy, -_map.MapSize.Y + Globals.WindowSize.Y + (_map.TileSize.Y / 2), _map.TileSize.Y / 2);
            _translation = Matrix.CreateTranslation(dx, dy, 0f);
        }

        public void Update()
        {
            InputManager.Update();
            _coin.Update();
            _hero.Update();

            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H))
            {
                _hero.Heal(1);
            }
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J))
            {
                _hero.TakeDamage(1);
            }

            // Update health bar
            _healthBar.SetHealth(_hero.Health);

            CalculateTranslation();
        }

        public void Draw()
        {
            _map.Draw();
            _coin.Draw();
            _hero.Draw();
        }
        
        public void DrawUI()
        {
            _healthBar.Draw();
        }
    }
}
