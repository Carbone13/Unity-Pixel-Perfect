using System;
using UnityEngine;

// Simple bounding boxes
// Use Corner Coordinate System, meaning that the pivot is at the upper left edge and not at the center as the BoxCollider2D
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
    public Vector2 position => (Vector2)transform.position + offset;
    // Return the size according to our scale
    public Vector2 sceneSize => (Vector2)size * transform.lossyScale;

    // These functions return edge
    // It is similar to Bounds.min/max
    public float Left => position.x;
    public float Right => position.x + sceneSize.x;
    public float Top => position.y + sceneSize.y ;
    public float Bottom => position.y;

    
    // This basic function return a bool if we overlap (= collide) with other
    // We can provide a positionOffset to offset us
    // The rest of the function is just mathematical formula
    public bool OverlapWith (AABB other, Vector2? _positionOverwrite = null)
    {
        float x, y;

        if (_positionOverwrite == null)
        {
            x = Math.Abs((position.x + sceneSize.x / 2) - (other.position.x + other.sceneSize.x / 2));
            y = Math.Abs((position.y + sceneSize.y / 2) - (other.position.y + other.sceneSize.y / 2));
        }
        else
        {
            Vector2 positionOverwrite = (Vector2) _positionOverwrite;
            
            x = Math.Abs((positionOverwrite.x + sceneSize.x / 2) - (other.position.x + other.sceneSize.x / 2));
            y = Math.Abs((positionOverwrite.y + sceneSize.y / 2) - (other.position.y + other.sceneSize.y / 2));
        }
        
        bool xCheck = x * 2 < (sceneSize.x + other.sceneSize.x);
        bool yCheck = y * 2 < (sceneSize.y + other.sceneSize.y);

        return xCheck && yCheck;
    }

    private void OnEnable ()
    {
        this.RegisterBox();
    }

    private void OnDisable ()
    {
        this.UnregisterBox();
    }

    private void OnDrawGizmos ()
    {
        if (debug)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(position + sceneSize / 2, sceneSize);
        }
    }
}
