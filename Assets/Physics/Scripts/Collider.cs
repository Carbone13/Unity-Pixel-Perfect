using System;
using RTree;
using UnityEngine;

namespace C13.Physics
{
    /// <summary>
    /// Simple bounding boxes : it's a rectangle
    /// Use Corner Coordinate System, meaning that the pivot is at the bottom left edge and not at the center as the BoxCollider2D
    /// </summary>
    [Serializable]
    public class Collider
    {
        #region Reference
        // Reference to the entity we belong to
        public Entity owner { get; set; }
        #endregion
        
        #region Settings
        // Editor variable, a size and an offset
        [Tooltip("Used to add an offset, mesured in pixels")]
        [SerializeField] private Vector2Int offset;
        [Tooltip("Collider size, mesured in pixels")]
        [SerializeField] private Vector2Int size = new Vector2Int(1, 1);
        [Tooltip("Should we be affected by the Transform scale ?")]
        [SerializeField] private bool scaleWithTransform = true;

        [Space]
        // Used to show the box in the editor
        [SerializeField] private bool debug = true;
        [SerializeField] private Color debugColor = Color.green;
        #endregion

        #region Public Metrics
        
        public float Left => AbsoluteX;
        public float Right => AbsoluteX + AbsoluteSize.x;
        public float Top => AbsoluteY + AbsoluteSize.y;
        public float Bottom => AbsoluteY;

        public Vector2Int AbsoluteSize => new Vector2Int(size.x * (scaleWithTransform ? (int)owner.transform.lossyScale.x : 1), size.y * (scaleWithTransform ? (int)owner.transform.lossyScale.y : 1));
        public Vector2Int AbsolutePosition => new Vector2Int(AbsoluteX, AbsoluteY);

        public int AbsoluteX => (int)owner.transform.position.x - AbsoluteSize.x / 2 + offset.x;
        public int AbsoluteY => (int)owner.transform.position.y - AbsoluteSize.y / 2 + offset.y;
        #endregion
        
        #region Converter
        
        public static explicit operator Rect (Collider toConvert)
        {
            return new Rect(toConvert.AbsolutePosition, toConvert.AbsoluteSize);
        }
        
        public static explicit operator Envelope (Collider toConvert)
        {
            Rect col = (Rect) toConvert;
            return new Envelope(col.xMin, col.yMin, col.xMax, col.yMax);
        }
        #endregion
        
        #region Public Methods
        
        public void Draw ()
        {
            if (debug)
            {
                Gizmos.color = debugColor;
                // Little conversion from Corner Coordinate to Center Coordinate
                Gizmos.DrawWireCube(AbsolutePosition + (Vector2)(AbsoluteSize / 2), (Vector2)AbsoluteSize);
            }
        }

        public bool CollideWith (Collider other)
        {
            return 
                Math.Abs((AbsoluteX + AbsoluteSize.x / 2) - (other.AbsoluteX + other.AbsoluteSize.x / 2)) * 2 < (AbsoluteSize.x + other.AbsoluteSize.x)
            &&  Math.Abs((AbsoluteY + AbsoluteSize.y / 2) - (other.AbsoluteY + other.AbsoluteSize.y / 2)) * 2 < (AbsoluteSize.y + other.AbsoluteSize.y);

        }

        public bool CollideWith (Collider other, Vector2 at)
        {
            return 
                Math.Abs(((int)at.x + AbsoluteSize.x / 2) - (other.AbsoluteX + other.AbsoluteSize.x / 2)) * 2 < (AbsoluteSize.x + other.AbsoluteSize.x)
            &&  Math.Abs(((int)at.y + AbsoluteSize.y / 2) - (other.AbsoluteY + other.AbsoluteSize.y / 2)) * 2 < (AbsoluteSize.y + other.AbsoluteSize.y);
        }
        #endregion
    }
}