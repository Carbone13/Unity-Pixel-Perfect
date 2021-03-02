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

    // In this example, we are considered as Riding the solid when On Top of It
    public override bool isRiding (Solid2D solid)
    {
        // We just need to check if our Collider's Bottom Edge is the same as the Solid's Top Edge
        // and we are not at its left or its right
        bool isAtLeft = hitbox.Right < solid.hitbox.Left;
        bool isAtRight = hitbox.Left > solid.hitbox.Right;

        // We use a little offset to handle the lose of precision caused by floats
        return (Math.Abs(hitbox.Bottom - solid.hitbox.Top) == 0) && !isAtLeft && !isAtRight;
    }

    public override void Squish ()
    {
        print("squish");
    }
}