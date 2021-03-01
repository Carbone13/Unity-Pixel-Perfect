using System;
using UnityEngine;

public class Player : Actor2D
{
    public float MoveSpeed = 50.0f;
    
    private Vector2 velocity;
    private Vector2 inputs;

    private void Update ()
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity = inputs * MoveSpeed * Time.deltaTime;
        
        Move(velocity);
    }

    public override bool isRiding (Solid2D solid)
    {
        throw new System.NotImplementedException();
    }

    public override void Squish ()
    {
        throw new System.NotImplementedException();
    }
}