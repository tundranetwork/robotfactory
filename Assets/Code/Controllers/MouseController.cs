using System;
using UnityEngine;

namespace RobotFactory.Controllers
{
    public class MouseController : MonoBehaviour
    {
        [SerializeField] Camera _targetCamera;

        private Vector3 _dragStartPos;

        private void Update()
        {
            // Check where the current mouse position is in-world
            var ray = _targetCamera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, new Vector3(0, 0.1f, 0));
            float dist;

            // Perform the raycast, if it fails, return
            if (!plane.Raycast(ray, out dist))
            {
                return;
            }

            // Find the actual position for the cast's result
            var currentPos = ray.GetPoint(dist);

            UpdateDrag(currentPos);
            UpdateZoom();
        }

        private void UpdateDrag(Vector3 currentPos)
        {
            // Check if the drag-camera button is pressed and if yes move the camera
            if (Input.GetButton("Drag Camera"))
            {
                var diff = _dragStartPos - currentPos;
                var pos = _targetCamera.transform.position;
                pos += diff;
                pos.y = _targetCamera.transform.position.y; // Just to be sure
                _targetCamera.transform.position = pos;
            }
            else
            {
                // We're not dragging currently, so store the position for when we are
                _dragStartPos = currentPos;
            }
        }

        private void UpdateZoom()
        {
            var zoom = _targetCamera.orthographicSize;
            zoom += Input.GetAxis("Zoom Camera");
            zoom = Math.Max(zoom, 4.0f);
            zoom = Math.Min(zoom, 10.0f);
            _targetCamera.orthographicSize = zoom;
        }
    }
}