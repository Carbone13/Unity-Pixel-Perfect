using System;
using UnityEngine;
using UnityEngine.UIElements;

// Simple MovingPlatform class, to show how to use Solid2D class
// Notice that we inherit from Solid2D
public class MovingPlatform : Solid2D
{
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0,2)]
    public float easeAmount;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    private int fromWaypointID;
    private float travellingPercent;
    
    private void Start ()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

    }

    private int dir = 1;
    private void Update ()
    {
        Move(Vector2.right * speed * dir * Time.deltaTime);

        if (transform.position.x >= 100) dir = -1;
        if (transform.position.x <= -100) dir = 1;
    }

    private void OnDrawGizmos ()
    {
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