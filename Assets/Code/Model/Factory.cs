using System;
using UnityEngine;
using Random = System.Random;

namespace RobotFactory.Model
{
	public class Factory
	{
		private Tile[,] _tiles;
		private int _width, _height;

		public Factory (int width, int height)
		{
			_width = width;
			_height = height;
			_tiles = new Tile[_width, _height];

			Debug.Log (string.Format ("Generating [{0}, {1}] world...", _width, _height));

			// Generate the map
			var random = new Random ();
			for (var x = 0; x < _width; x++) {
				for (var y = 0; y < _height; y++) {
					_tiles [x, y] = new Tile {
						Type = random.Next (0, 2) == 0 ? TileType.Grass : TileType.Floor
					};
				}
			}

			Debug.Log ("Finished world generation");
		}

		public int Width { get { return _width; } }

		public int Height { get { return _height; } }

		public event EventHandler TilesChanged;

		public Tile GetTileAt (int x, int y)
		{
			return _tiles [x, y];
		}

		public void SetTileAt (int x, int y, Tile tile)
		{
			if (x > _width || y > _height) {
				throw new IndexOutOfRangeException ();
			}

			// Actually perform the set
			_tiles [x, y] = tile;

			// Notify listeners of the change
			TilesChanged.SafeInvoke (this, EventArgs.Empty);
		}
	}
}