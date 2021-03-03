using System;
using UnityEngine;

namespace C13.Physics
{
    /// <summary>
    /// Simple bounding boxes : it's a rectangle
    /// Use Corner Coordinate System, meaning that the pivot is at the upper left edge and not at the center as the BoxCollider2D
    /// </summary>
    public class AABB : MonoBehaviour
    {
        // Editor variable, a size and an offset
        public Vector2Int offset;
        public Vector2Int size = new Vector2Int(1, 1);
        public bool isActive = true;
        // Used to show the box in the editor
        public bool debug;
        public Color debugColor;

        
        // Return the position in the scene
        public Vector2 position => (Vector2) transform.position - (sceneSize / 2) + offset;
        // Return the size according to our scale
        public Vector2 sceneSize => (Vector2) size * transform.lossyScale;

        // These functions return edge
        // It is similar to Bounds.min/max
        public float Left => position.x;
        public float Right => position.x + sceneSize.x;
        public float Top => position.y + sceneSize.y;
        public float Bottom => position.y;
        
        
        /// <summary>
        /// Return true if colliding with <c>other</c>.
        /// It will check at this box current position, but you can run the test on another position using <c>positionOverwrite</c>.
        /// </summary>
        public bool OverlapWith (AABB other, Vector2? positionOverwrite = null)
        {
            float x, y;

            if (positionOverwrite == null)
            {
                x = Math.Abs((position.x + sceneSize.x / 2) - (other.position.x + other.sceneSize.x / 2));
                y = Math.Abs((position.y + sceneSize.y / 2) - (other.position.y + other.sceneSize.y / 2));
            }
            else
            {
                Vector2 _positionOverwrite = (Vector2) positionOverwrite;

                x = Math.Abs((_positionOverwrite.x + sceneSize.x / 2) - (other.position.x + other.sceneSize.x / 2));
                y = Math.Abs((_positionOverwrite.y + sceneSize.y / 2) - (other.position.y + other.sceneSize.y / 2));
            }

            bool xCheck = x * 2 < (sceneSize.x + other.sceneSize.x);
            bool yCheck = y * 2 < (sceneSize.y + other.sceneSize.y);

            return xCheck && yCheck;
        }

        // Register this box when enabled;
        private void OnEnable ()
        {
            this.RegisterBox();
        }
        
        // Unregister it when disabled
        private void OnDisable ()
        {
            this.UnregisterBox();
        }

        // Draw the box in the editor
        private void OnDrawGizmos ()
        {
            if (debug)
            {
                Gizmos.color = debugColor;
                // Little conversion from Corner Coordinate to Center Coordinate
                Gizmos.DrawWireCube(position + sceneSize / 2, sceneSize);
            }
        }
    }
}