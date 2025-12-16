using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Project11
{
    public class Collision
    {
        private readonly List<Rectangle> _collisionRects = new();
        private readonly Point _tileSize;

        public Collision(Point tileSize)
        {
            _tileSize = tileSize;
        }

        public void AddCollisionTile(int x, int y)
        {
            _collisionRects.Add(new Rectangle(
                x * _tileSize.X,
                y * _tileSize.Y,
                _tileSize.X,
                _tileSize.Y
            ));
        }

        public void AddCollisionRect(Rectangle rect)
        {
            _collisionRects.Add(rect);
        }

        public void Clear()
        {
            _collisionRects.Clear();
        }

        // Check if a position would collide with any collision rectangles
        public bool WillCollide(Vector2 position, Vector2 size)
        {
            Rectangle entityRect = new Rectangle(
                (int)(position.X - size.X / 2),
                (int)(position.Y - size.Y / 2),
                (int)size.X,
                (int)size.Y
            );

            foreach (var collisionRect in _collisionRects)
            {
                if (entityRect.Intersects(collisionRect))
                {
                    return true;
                }
            }

            return false;
        }

        // Get a corrected position that doesn't collide
        public Vector2 GetCorrectedPosition(Vector2 oldPosition, Vector2 newPosition, Vector2 size)
        {
            Rectangle entityRect = new Rectangle(
                (int)(newPosition.X - size.X / 2),
                (int)(newPosition.Y - size.Y / 2),
                (int)size.X,
                (int)size.Y
            );

            Vector2 correctedPosition = newPosition;

            foreach (var collisionRect in _collisionRects)
            {
                if (entityRect.Intersects(collisionRect))
                {
                    // Try moving back on X axis
                    Rectangle testRect = new Rectangle(
                        (int)(oldPosition.X - size.X / 2),
                        (int)(newPosition.Y - size.Y / 2),
                        (int)size.X,
                        (int)size.Y
                    );

                    if (!DoesRectCollide(testRect))
                    {
                        correctedPosition.X = oldPosition.X;
                    }

                    // Try moving back on Y axis
                    testRect = new Rectangle(
                        (int)(newPosition.X - size.X / 2),
                        (int)(oldPosition.Y - size.Y / 2),
                        (int)size.X,
                        (int)size.Y
                    );

                    if (!DoesRectCollide(testRect))
                    {
                        correctedPosition.Y = oldPosition.Y;
                    }

                    // If still colliding, revert to old position
                    if (WillCollide(correctedPosition, size))
                    {
                        correctedPosition = oldPosition;
                    }

                    break;
                }
            }

            return correctedPosition;
        }

        private bool DoesRectCollide(Rectangle rect)
        {
            foreach (var collisionRect in _collisionRects)
            {
                if (rect.Intersects(collisionRect))
                {
                    return true;
                }
            }
            return false;
        }

        // Optional: Draw collision rectangles for debugging
        public void DrawDebug()
        {
            if (HealthBar._pixelTexture == null)
            {
                var pixelTexture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
                pixelTexture.SetData(new[] { Color.White });
                HealthBar._pixelTexture = pixelTexture;
            }

            foreach (var rect in _collisionRects)
            {
                Globals.SpriteBatch.Draw(
                    HealthBar._pixelTexture,
                    rect,
                    Color.Red * 0.3f
                );
            }
        }

        public int CollisionCount => _collisionRects.Count;
    }
}
