using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project11
{
    public class Hero : Sprite
    {

        private float _speed = 300f;
        private AnimationManager _anims = new();
        private Vector2 _minPos, _maxPos;
        private Dash _dash = new();
        private Vector2 _lastMovementDirection = Vector2.Zero;
        private Vector2 _dashDirection = Vector2.Zero;

        private int _maxHealth = 100;
        private int _currentHealth;
        public int Health => _currentHealth;
        public int MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0;


        public Hero() : base(Globals.Content.Load<Texture2D>("hero"), new Vector2(100, 100))
        {
            _currentHealth = _maxHealth;
            System.Diagnostics.Debug.WriteLine($"Hero created at position: {Position}");
            var heroTexture = Globals.Content.Load<Texture2D>("hero");
            _anims.AddAnimation(new Vector2(0, 1), new(heroTexture, 8, 8, 0.1f, 1));
            _anims.AddAnimation(new Vector2(-1, 0), new(heroTexture, 8, 8, 0.1f, 2));
            _anims.AddAnimation(new Vector2(1, 0), new(heroTexture, 8, 8, 0.1f, 3));
            _anims.AddAnimation(new Vector2(0, -1), new(heroTexture, 8, 8, 0.1f, 4));
            _anims.AddAnimation(new Vector2(-1, 1), new(heroTexture, 8, 8, 0.1f, 5));
            _anims.AddAnimation(new Vector2(-1, -1), new(heroTexture, 8, 8, 0.1f, 6));
            _anims.AddAnimation(new Vector2(1, 1), new(heroTexture, 8, 8, 0.1f, 7));
            _anims.AddAnimation(new Vector2(1, -1), new(heroTexture, 8, 8, 0.1f, 8));
        }

        public void Update()
        {
            // Update dash
            _dash.Update(Globals.TotalSeconds);

            // Check for dash input (only if not already dashing)
            if (!_dash.IsDashing && InputManager.DashPressed)
            {
                // Get current movement input (keys being held)
                Vector2 dashDirection = InputManager.GetMovementInput();
                
                // If no keys are held, use last movement direction
                if (dashDirection == Vector2.Zero && _lastMovementDirection != Vector2.Zero)
                {
                    dashDirection = _lastMovementDirection;
                }
                
                // If we have a direction, start dash
                if (dashDirection != Vector2.Zero)
                {
                    if (_dash.TryStartDash(dashDirection))
                    {
                        _dashDirection = Vector2.Normalize(dashDirection);
                    }
                }
            }

            // Handle dash movement (dash takes priority over normal movement)
            if (_dash.IsDashing)
            {
                Vector2 dashMovement = _dash.GetDashMovement(Globals.TotalSeconds, _minPos, _maxPos, Position);
                Position += dashMovement;
                Position = Vector2.Clamp(Position, _minPos, _maxPos);
                
                // Update animation based on dash direction
                _anims.Update(_dashDirection);
            }
            // Handle normal movement (only when not dashing)
            else if (InputManager.Moving)
            {
                Vector2 normalizedDirection = Vector2.Normalize(InputManager.Direction);
                Position += normalizedDirection * _speed * Globals.TotalSeconds;
                Position = Vector2.Clamp(Position, _minPos, _maxPos);
                _anims.Update(InputManager.Direction);
                _lastMovementDirection = InputManager.Direction; // Store last movement direction for dash
            }
            else
            {
                // Play idle animation when not moving
                _anims.Update(Vector2.Zero);
            }
        }
        public void TakeDamage(int damage)
        {
            _currentHealth = MathHelper.Clamp(_currentHealth - damage, 0, _maxHealth);
        }

        public void Heal(int amount)
        {
            _currentHealth = MathHelper.Clamp(_currentHealth + amount, 0, _maxHealth);
        }
        public void SetBounds(Point mapSize, Point tileSize)
        {
            _minPos = new Vector2(Origin.X, Origin.Y);
            _maxPos = new Vector2(mapSize.X - Origin.X, mapSize.Y - Origin.Y);
        }


        public void Draw()
        {
            _anims.Draw(Position);
        }
    }
}
