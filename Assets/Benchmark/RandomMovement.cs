using System;
using System.Data.SqlTypes;
using UnityEngine;
using C13.Physics;
using Random = UnityEngine.Random;


public class RandomMovement : Actor2D
{
    public SystemType Mode;
    public float speed = 10f;
    
    public Vector2 direction;
    private Rigidbody2D rb;
    
    public override void Awake ()
    {
        base.Awake();
        
        if(Mode == SystemType.Builtin) rb = GetComponent<Rigidbody2D>();

        transform.position = new Vector2(Random.Range(-170, 170), Random.Range(-80, 80));
    }

    private void Update ()
    {
        if (Mode == SystemType.Builtin)
        {
            Builtin();
        }
        else
        {
            Custom();
        }
    }

    private void Builtin ()
    {
        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        rb.velocity = (direction * speed);
    }

    private void Custom ()
    {
        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized;
        Move(direction * speed * Time.deltaTime * 60);
    }


    public override bool IsRiding (Solid2D solid)
    {
        return false;
    }

    public override void Squish (int direction, Entity hit)
    {
        throw new NotImplementedException();
    }
}

public enum SystemType
{
    Custom,
    Builtin
}