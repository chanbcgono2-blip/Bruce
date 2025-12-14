using System;
using Microsoft.Xna.Framework;

namespace Project11
{
    public class Dash
    {
        private float _dashDistance = 150f; // Distance to dash
        private float _dashSpeed = 800f; // Speed during dash
        private float _cooldown = 1.0f; // Cooldown time in seconds
        private float _currentCooldown = 0f;
        
        private bool _isDashing = false;
        private Vector2 _dashDirection = Vector2.Zero;
        private float _dashProgress = 0f;
        private float _dashDuration = 0f;
        
        public bool IsDashing => _isDashing;
        public bool CanDash => _currentCooldown <= 0f && !_isDashing;
        
        public float DashDistance
        {
            get => _dashDistance;
            set => _dashDistance = Math.Max(0, value);
        }
        
        public float Cooldown
        {
            get => _cooldown;
            set => _cooldown = Math.Max(0, value);
        }

        public void Update(float deltaTime)
        {
            // Update cooldown
            if (_currentCooldown > 0f)
            {
                _currentCooldown -= deltaTime;
            }

            // Update dash progress
            if (_isDashing)
            {
                _dashProgress += deltaTime;
                
                // Check if dash is complete
                if (_dashProgress >= _dashDuration)
                {
                    _isDashing = false;
                    _dashProgress = 0f;
                    _dashDuration = 0f;
                    _currentCooldown = _cooldown;
                }
            }
        }

        public bool TryStartDash(Vector2 direction)
        {
            if (!CanDash || direction == Vector2.Zero)
            {
                return false;
            }

            // Normalize direction
            _dashDirection = Vector2.Normalize(direction);
            
            // Calculate dash duration based on distance and speed
            _dashDuration = _dashDistance / _dashSpeed;
            _dashProgress = 0f;
            _isDashing = true;
            
            return true;
        }

        public Vector2 GetDashMovement(float deltaTime, Vector2 minPos, Vector2 maxPos, Vector2 currentPos)
        {
            if (!_isDashing)
            {
                return Vector2.Zero;
            }

            // Calculate movement for this frame
            float moveDistance = _dashSpeed * deltaTime;
            Vector2 movement = _dashDirection * moveDistance;
            
            // Calculate target position
            Vector2 targetPos = currentPos + movement;
            
            // Clamp to bounds
            targetPos = Vector2.Clamp(targetPos, minPos, maxPos);
            
            // Return the actual movement (might be less if hitting bounds)
            Vector2 actualMovement = targetPos - currentPos;
            
            // If we can't move further (hitting bounds), stop the dash
            if (actualMovement.LengthSquared() < movement.LengthSquared() * 0.5f)
            {
                _isDashing = false;
                _dashProgress = 0f;
                _dashDuration = 0f;
                _currentCooldown = _cooldown;
            }
            
            return actualMovement;
        }

        public void Reset()
        {
            _isDashing = false;
            _currentCooldown = 0f;
            _dashProgress = 0f;
            _dashDuration = 0f;
        }
    }
}

