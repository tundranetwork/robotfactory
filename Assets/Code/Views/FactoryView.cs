using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RobotFactory.Model;

namespace RobotFactory.Controllers
{
	public class FactoryView : MonoBehaviour
	{
		[SerializeField] private Mesh _grassMesh;
		[SerializeField] private Mesh _floorMesh;
		[SerializeField] private Mesh _wallMesh;
		[SerializeField] private Material _material;

		private readonly List<Vector2I> _outdatedTiles = new List<Vector2I> ();
		private Factory _factory;
		private bool _viewOutdated = true;

		private void Start ()
		{
			FindObjectOfType<ModelManager> ().Link (OnReadyToLink);
		}

		private void OnReadyToLink (ModelManager manager)
		{
			_factory = manager.Require<Factory> ();
			_factory.TileChanged += OnTileChanged;

			// Generate the initial view based on the factory
			GenerateView ();
		}

		private void Update ()
		{
			if (_viewOutdated) {
				// Go over all changed tiles
				foreach (var pos in _outdatedTiles) {
					RemoveGameObjectForTile (pos);
					AddGameObjectForTile (pos);
				}
				_outdatedTiles.Clear ();
				_viewOutdated = false;
			}
		}

		private void OnTileChanged (object sender, TileChangedEventArgs e)
		{
			_viewOutdated = true;
			_outdatedTiles.Add (e.Position);
		}

		private void GenerateView ()
		{
			Debug.Log ("Generating factory view...");

			// Create a game object for every tile
			for (var x = 0; x < _factory.Width; x++) {
				for (var y = 0; y < _factory.Height; y++) {
					AddGameObjectForTile (new Vector2I (x, y));
				}
			}
		}

		private void AddGameObjectForTile (Vector2I pos)
		{
			var tile = _factory.GetTileAt (pos);

			// Create the actual object for this tile
			var tileGo = new GameObject ();
			tileGo.name = GenerateName (pos);
			tileGo.transform.position = new Vector3 (pos.x, 0, pos.y);
			tileGo.transform.eulerAngles = new Vector3 (-90, 180, 0); // Blender coordinates grr
			tileGo.transform.parent = transform;

			// Set the mesh data
			var meshFilter = tileGo.AddComponent<MeshFilter> ();
			switch (tile.Type) { // TODO: Load these in dynamically
			case TileType.Grass:
				meshFilter.sharedMesh = _grassMesh;
				break;
			case TileType.Floor:
				meshFilter.sharedMesh = _floorMesh;
				break;
			case TileType.Wall:
				meshFilter.sharedMesh = _wallMesh;
				break;
			}
			var meshRenderer = tileGo.AddComponent<MeshRenderer> ();
			meshRenderer.material = _material;
		}

		private void RemoveGameObjectForTile (Vector2I pos)
		{
			Destroy (transform.FindChild (GenerateName (pos)).gameObject);
		}

		private static string GenerateName (Vector2I pos)
		{
			return "Tile (" + pos.x + ", " + pos.y + ")";
		}
	}
}