using System;
using UnityEngine;
using C13.Physics;


// Simple MovingPlatform class, to show how to use Solid2D class
// Notice that we inherit from Solid2D
[Tracked]
public class MovingPlatform : Solid2D
{
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;

    private int fromWaypointIndex;
    private int toWaypointIndex;

    
    private void Start ()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

    }

    private void Update ()
    {
        fromWaypointIndex %= globalWaypoints.Length;
        toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;

        Vector2 dir = (globalWaypoints[toWaypointIndex] - globalWaypoints[fromWaypointIndex]).normalized;

        Move(dir * speed * Time.deltaTime);

        if (transform.position == globalWaypoints[toWaypointIndex])
        {
            ClearRemainder();
            fromWaypointIndex ++;

            if (!cyclic) 
            {
                if (fromWaypointIndex >= globalWaypoints.Length-1) {
                    fromWaypointIndex = 0;
                    Array.Reverse(globalWaypoints);
                }
            }
        }
    }

    public override void OnDrawGizmos ()
    {
        base.OnDrawGizmos();
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 1f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPosition = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);
            }
        }
    }
}