using System;
using System.Linq;
using UnityEngine;
using RobotFactory.Model;

namespace RobotFactory.Controllers
{
    public class FactoryController : MonoBehaviour
    {
        [SerializeField] private Mesh _floorMesh;
        [SerializeField] private Mesh _grassMesh;
        [SerializeField] private Material _material;

        private Factory _factory;
        private bool _viewOutdated = true;

        private void Start()
        {
            // Initialize the factory itself
            _factory = new Factory(100, 100);
            _factory.TilesChanged += OnTilesChanged;
        }

        private void OnTilesChanged(object sender, EventArgs e)
        {
            _viewOutdated = true;
        }

        private void Update()
        {
            if (_viewOutdated)
            {
                ClearView();
                GenerateView();
                _viewOutdated = false;
            }
        }

        private void ClearView()
        {
            foreach (Transform child in transform.Cast<Transform>())
            {
                Destroy(child.gameObject);
            }
        }

        private void GenerateView()
        {
            Debug.Log("Generating factory view...");

            // Create a game object for every tile
            for (var x = 0; x < _factory.Width; x++)
            {
                for (var y = 0; y < _factory.Height; y++)
                {
                    var tile = _factory.GetTileAt(x, y);

                    // Create the actual object for this tile
                    var tileGo = new GameObject();
                    tileGo.name = "Tile (" + x + ", " + y + ")";
                    tileGo.transform.position = new Vector3(x, 0, y);
                    tileGo.transform.eulerAngles = new Vector3(-90, 180, 0); // Blender coordinates grr
                    tileGo.transform.parent = transform;

                    // Set the mesh data
                    var meshFilter = tileGo.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = tile.Type == TileType.Floor ? _floorMesh : _grassMesh;
                    var meshRenderer = tileGo.AddComponent<MeshRenderer>();
                    meshRenderer.material = _material;
                }
            }
        }
    }
}