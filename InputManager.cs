using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Project11
{

    public static class InputManager
    {
        private static Vector2 _direction;
        private static KeyboardState _previousKeyboardState = Keyboard.GetState();
        private static KeyboardState _currentKeyboardState = Keyboard.GetState();
        
        public static Vector2 Direction => _direction;
        public static bool Moving => _direction != Vector2.Zero;
        public static bool DashPressed => _currentKeyboardState.IsKeyDown(Keys.Q) && !_previousKeyboardState.IsKeyDown(Keys.Q);
        
        // Get movement direction from currently held keys (for dash)
        public static Vector2 GetMovementInput()
        {
            Vector2 input = Vector2.Zero;
            if (_currentKeyboardState.IsKeyDown(Keys.A)) input.X--;
            if (_currentKeyboardState.IsKeyDown(Keys.D)) input.X++;
            if (_currentKeyboardState.IsKeyDown(Keys.W)) input.Y--;
            if (_currentKeyboardState.IsKeyDown(Keys.S)) input.Y++;
            return input;
        }

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            
            _direction = Vector2.Zero;

            if (_currentKeyboardState.GetPressedKeyCount() > 0)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.A)) _direction.X--;
                if (_currentKeyboardState.IsKeyDown(Keys.D)) _direction.X++;
                if (_currentKeyboardState.IsKeyDown(Keys.W)) _direction.Y--;
                if (_currentKeyboardState.IsKeyDown(Keys.S)) _direction.Y++;
            }
        }
    }
}

