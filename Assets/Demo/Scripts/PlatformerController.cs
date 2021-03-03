﻿using UnityEngine;
using C13.Physics;
using System;

public class PlatformerController : Actor2D
{
    public float MoveSpeed = 50f;
    public float JumpHeight;
    public float JumpApexTime;

    private float gravity;
    private float jumpVelocity;

    [Space]
    public Vector2 velocity;
    public bool isGrounded;
    public float xInput;

    
    public override void Awake ()
    {
        base.Awake();
        CalculatePhysicConstants();
    }
    
    private void Update ()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        velocity.x = xInput * MoveSpeed;
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            velocity.y = jumpVelocity;
        }
        
        if(!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        Move(velocity * Time.deltaTime, OnCollideX, OnCollideY);

        if (!hitbox.CollideAt(hitbox.position - new Vector2(0, 1))) isGrounded = false;
    }
    
    
    private void OnCollideX (int dir)
    {
        ClearRemainderX();
        velocity.x = 0;
    }

    private void OnCollideY (int dir)
    {
        if (dir == -1)
        {
            isGrounded = true;
        }
        
        ClearRemainderY();
        velocity.y = 0;
    }
    
    private void CalculatePhysicConstants ()
    {
        gravity = (2 * JumpHeight) / (float)(Math.Pow(JumpApexTime, 2));

        jumpVelocity = (float)Math.Sqrt(2 * gravity * JumpHeight);
    }
    
    public override bool isRiding (Solid2D solid)
    {
        // We just need to check if our Collider Bottom Edge is the same as the Solid Top Edge
        // and we are not at its left or its right
        bool isAtLeft = hitbox.Right < solid.hitbox.Left;
        bool isAtRight = hitbox.Left > solid.hitbox.Right;

        // We use a little offset to handle the lose of precision caused by floats
        return (Math.Abs(hitbox.Bottom - solid.hitbox.Top) == 0) && !isAtLeft && !isAtRight;
    }

    public override void Squish (int direction)
    {
        print("Squished");
        transform.position = new Vector2(0, -25);
    }
}
