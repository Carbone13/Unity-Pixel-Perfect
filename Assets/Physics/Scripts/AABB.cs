using System;
using UnityEngine;

namespace C13.Physics
{
    /// <summary>
    /// Simple bounding boxes : it's a rectangle
    /// Use Corner Coordinate System, meaning that the pivot is at the upper left edge and not at the center as the BoxCollider2D
    /// </summary>
    [Serializable]
    public class Collider
    {
        // Reference to the entity we belong to
        public Entity owner { private get; set; }
        
        // Editor variable, a size and an offset
        [SerializeField] private Vector2Int offset;
        [SerializeField] private Vector2Int size = new Vector2Int(1, 1);

        [Space]
        // Used to show the box in the editor
        [SerializeField] private bool debug = true;
        [SerializeField] private Color debugColor = Color.green;

        public float Left => AbsoluteX;
        public float Right => AbsoluteX + AbsoluteSize.x;
        public float Top => AbsoluteY + AbsoluteSize.y;
        public float Bottom => AbsoluteY;

        public Vector2Int AbsoluteSize => new Vector2Int(size.x * (int)owner.transform.lossyScale.x, size.y * (int)owner.transform.lossyScale.y);
        public Vector2Int AbsolutePosition => new Vector2Int(AbsoluteX, AbsoluteY);

        public int AbsoluteX
        {
            get
            {
                if (owner != null)
                {
                    return (int)owner.transform.position.x - AbsoluteSize.x / 2 + offset.x;
                }

                return offset.x;
            }
        }
        
        public int AbsoluteY
        {
            get
            {
                if (owner != null)
                {
                    return (int)owner.transform.position.y - AbsoluteSize.y / 2 + offset.y;
                }

                return offset.x;
            }
        }
        
        public void Draw ()
        {
            if (debug)
            {
                Gizmos.color = debugColor;
                // Little conversion from Corner Coordinate to Center Coordinate
                Gizmos.DrawWireCube(AbsolutePosition + (Vector2)(AbsoluteSize / 2), (Vector2)AbsoluteSize);
            }
        }
    }
}