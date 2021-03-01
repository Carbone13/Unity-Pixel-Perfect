using System;
using UnityEngine;

public class AABB : MonoBehaviour
{
    // Editor variable, a size and an offset
    public Vector2 offset;
    public Vector2 size = new Vector2(1, 1);
    public bool isActive = true;

    // Used to show the box in the editor
    public bool debug;
    public Color debugColor;

    // Return the position in the scene
    public Vector2 position => transform.position + new Vector3(offset.x, offset.y, 0);
    // Return the size according to our scale
    public Vector2 sceneSize => size * transform.lossyScale;

    // These functions return edge
    // It is similar to Bounds.min/max
    public float Left => position.x - sceneSize.x / 2;
    public float Right => position.x + sceneSize.x / 2;
    public float Top => position.y + sceneSize.y / 2;
    public float Bottom => position.y - sceneSize.y / 2;

    
    // This basic function return a bool if we overlap (= collide) with other
    // We can provide a positionOffset to offset us
    // The rest of the function is just mathematical formula
    public bool OverlapWith (AABB other, Vector2? _positionOffset = null)
    {
        float x, y;
        
        if (_positionOffset == null)
        {
            x = Math.Abs(position.x - other.position.x);
            y = Math.Abs(position.y - other.position.y);
        }
        else
        {
            Vector2 _offset = (Vector2) _positionOffset;
            
            x = Math.Abs(position.x + _offset.x - other.position.x);
            y = Math.Abs(position.y + _offset.y - other.position.y);
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
            Color alterned = debugColor;
            alterned.a = 0.3f;
            
            Gizmos.color = alterned;
            Gizmos.DrawCube(position, sceneSize);

            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(position, sceneSize);
        }
    }
}
