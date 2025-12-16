using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
 
using Microsoft.Xna.Framework.Input;
using System.Security.Principal;




namespace Project11
{
    public class Map
    {
        private readonly Point _mapTileSize = new(90, 80);
        private readonly Sprite[,] _tiles;
        private readonly bool[,] _collisionMap; // true = collision tile
        private Collision _collision;

        public Point TileSize { get; private set; }
        public Point MapSize { get; private set; }
        public Collision Collision => _collision;

        public Map()
        {
            _tiles = new Sprite[_mapTileSize.X, _mapTileSize.Y];
            _collisionMap = new bool[_mapTileSize.X, _mapTileSize.Y];

            List<Texture2D> textures = new(6);
            for (int i = 1; i < 6; i++)
                textures.Add(Globals.Content.Load<Texture2D>($"no{i}"));

            TileSize = new(textures[0].Width, textures[0].Height);
            MapSize = new(TileSize.X * _mapTileSize.X, TileSize.Y * _mapTileSize.Y);

            _collision = new Collision(TileSize);

            Random random = new();

            for (int y = 0; y < _mapTileSize.Y; y++)
            {
                for (int x = 0; x < _mapTileSize.X; x++)
                {
                    int r = random.Next(0, textures.Count);
                    _tiles[x, y] = new(textures[r], new(x * TileSize.X, y * TileSize.Y));

                    // Create a border of collision tiles
                    // You can customize this logic for your needs
                    if (x == 0 || x == _mapTileSize.X - 1 ||
                        y == 0 || y == _mapTileSize.Y - 1)
                    {
                        _collisionMap[x, y] = true;
                        _collision.AddCollisionTile(x, y);
                    }
                    // Randomly add some collision tiles (10% chance)
                    else if (random.Next(0, 100) < 10)
                    {
                        _collisionMap[x, y] = true;
                        _collision.AddCollisionTile(x, y);
                    }
                }
            }
        }

        public bool IsTileCollidable(int x, int y)
        {
            if (x < 0 || x >= _mapTileSize.X || y < 0 || y >= _mapTileSize.Y)
                return true; // Out of bounds = collision

            return _collisionMap[x, y];
        }

        public void SetTileCollision(int x, int y, bool hasCollision)
        {
            if (x < 0 || x >= _mapTileSize.X || y < 0 || y >= _mapTileSize.Y)
                return;

            _collisionMap[x, y] = hasCollision;

            // Rebuild collision system
            _collision.Clear();
            for (int ty = 0; ty < _mapTileSize.Y; ty++)
            {
                for (int tx = 0; tx < _mapTileSize.X; tx++)
                {
                    if (_collisionMap[tx, ty])
                    {
                        _collision.AddCollisionTile(tx, ty);
                    }
                }
            }
        }

        public void Draw()
        {
            for (int y = 0; y < _mapTileSize.Y; y++)
            {
                for (int x = 0; x < _mapTileSize.X; x++)
                {
                    _tiles[x, y].Draw();
                }
            }
        }

        public void DrawDebugCollision()
        {
            _collision.DrawDebug();
        }
    }
}


