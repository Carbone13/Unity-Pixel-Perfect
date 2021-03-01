using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AABB))]
// Solid2d is a class which you need to inherit from
// It is used for things that need to move but don't care about colliding
// BUT ! It will carry or push Actor2D if it's in its path !
// It's mainly used for Moving Platform, but you don't even need to use the Move() function
// You can just add this script to a random GameObject and make it a wall.
// If you want something to move and collide with environment, then look at Actor2D
public abstract class Solid2D : MonoBehaviour
{
    [HideInInspector]
    public AABB hitbox;

    private void Awake ()
    {
        hitbox = GetComponent<AABB>();
    }

    // Simple move function
    // It won't work if the Delta Time is too small, because float are not precise enough
    protected void Move (Vector2 amount)
    {
        // If we don't move at all, no need to run this function
        if (amount.magnitude == 0) return;
        
        // First we need to be aware of all our riders
        List<Actor2D> riders = this.GetAllRiders();

        // We deactivate collision detection so Actor2D won't get stuck on us
        hitbox.isActive = false;
        
        MoveX(amount.x, riders);
        MoveY(amount.y, riders);

        // Now we reactivate collisions
        hitbox.isActive = true;
    }

    private void MoveX (float amount, ICollection<Actor2D> riders)
    {
        amount = Mathf.RoundToInt(amount * 10000f) / 10000f;
        // Store our direction
        int direction = Math.Sign(amount);
        
        // Let's start by moving
        transform.position = new Vector2((transform.position.x + amount), transform.position.y);

        // Loop through every actor in the scene
        foreach (Actor2D actor in Physics.actors)
        {
            // To see if we now overlap it, making us wanting to push it
            if (hitbox.OverlapWith(actor.hitbox))
            {
                // If we do, we move it to the corresponding edge
                switch (direction)
                {
                    // Right Edge, because we are heading right
                    case 1:
                        actor.transform.position = new Vector2((actor.transform.position.x + (hitbox.Right - actor.hitbox.Left)), actor.transform.position.y);
                        break;
                    // Left Edge, because we are heading left
                    case -1:
                        actor.transform.position = new Vector2((actor.transform.position.x + (hitbox.Left - actor.hitbox.Right)), actor.transform.position.y);
                        break;
                }

                // And if it now collide with something else, it means it is squished between us and something else
                if (actor.hitbox.CollideAt())
                {
                    actor.Squish();
                }
            }
            // Maybe it is riding us ?
            else if(riders.Contains(actor))
            {
                // If it does, we move it in the same direction, with the same amount as we did
                // TODO maybe i should think about something to put in the OnCollide callback
                //actor.MoveX(amount, null);
            }
        }
    }
    
    private void MoveY (float amount, ICollection<Actor2D> riders)
    {
        amount = Mathf.RoundToInt(amount * 10000f) / 10000f;
        // Store our direction
        int direction = Math.Sign(amount);
        
        // Let's start by moving
        transform.position = new Vector2(transform.position.x, Mathf.RoundToInt((transform.position.y + amount) * 10000) / 10000);

        // Loop through every actor in the scene
        foreach (Actor2D actor in Physics.actors)
        {
            // To see if we now overlap it, making us wanting to push it
            if (hitbox.OverlapWith(actor.hitbox))
            {
                // If we do, we move it to the corresponding edge
                // And if it now collide with something else, it means it is squished between us and something else

                switch (direction)
                {
                    // Top Edge, because we are heading up
                    case 1:
                        actor.transform.position = new Vector2(actor.transform.position.x, Mathf.RoundToInt((actor.transform.position.y + (hitbox.Top - actor.hitbox.Bottom)) * 10000) / 10000);
                        break;
                    // Bottom Edge, because we are heading down
                    case -1:
                        actor.transform.position = new Vector2(actor.transform.position.x, Mathf.RoundToInt((actor.transform.position.y + (hitbox.Bottom - actor.hitbox.Top)) * 10000) / 10000);
                        break;
                }

                if (actor.hitbox.CollideAt())
                {
                    actor.Squish();
                }
            }
            // Else, maybe it is riding us ?
            else if(riders.Contains(actor))
            {
                // If it does, we move it in the same direction, with the same amount as we did
                // TODO maybe i should think about something to put in the OnCollide callback
                //actor.MoveY(amount, null);
            }
        }
    }
}
