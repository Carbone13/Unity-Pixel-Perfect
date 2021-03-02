using UnityEngine;
using C13.Physics;
using System;

public class PlatformerController : Actor2D
{
    public float MoveSpeed = 50f;
    public float RunAccelerationTime = 0.2f;
    public float JumpHeight;
    public float JumpApexTime;
    public float MinJumpHeight;
    public float coyoteTime;
    
    private float gravity;
    private float jumpVelocity;
    private float terminalVelocity;
    private float gravityScale = 1;
    private float lastGrounded;

    [Space]
    public Vector2 velocity;
    public bool isGrounded;
    public float xInput;

    private float xVelocitySmoothing;
    
    public override void Awake ()
    {
        base.Awake();
        CalculatePhysicConstants();
    }

    private void Update ()
    {
        float previousY = velocity.y;
        xInput = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.SmoothDamp(velocity.x, xInput * MoveSpeed, ref xVelocitySmoothing, RunAccelerationTime);
        
        if (Input.GetButtonDown("Jump") && ( (Time.time - lastGrounded) <= coyoteTime || isGrounded))
        {
            velocity.y = jumpVelocity;
        }
        else if(!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime * gravityScale;
        }

        if (velocity.y < 0 && previousY > 0)
        {
            print("Reached apex at y: " + transform.position.y);
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > terminalVelocity) velocity.y = terminalVelocity;
        }


        Vector2Int movedThisFrame = Move(velocity * Time.deltaTime, OnCollideX, OnCollideY);
        
        // TODO this is not good at all
        if (remainder.y > 0)
        {
            isGrounded = false;
        }

        if (isGrounded) lastGrounded = Time.time;
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
        terminalVelocity = (float) Math.Sqrt(Math.Pow(jumpVelocity, 2) + 2 * gravity *  -1f * (JumpHeight - MinJumpHeight));
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
    }
}
