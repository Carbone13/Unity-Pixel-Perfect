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

    [SerializeField]
    private Vector2 remainder;
    
    private void Awake ()
    {
        hitbox = GetComponent<AABB>();
    }

    // Simple move function
    // It won't work if the Delta Time is too small, because float are not precise enough
    protected void Move (Vector2 amount)
    {
        // If we don't move at all, no need to run this function
        if (amount.x == 0 && amount.y == 0) return;
        
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
        remainder.x += amount;
        int toMove = (int) remainder.x;
        print(toMove);
        if (toMove != 0)
        {
            print(toMove);
            remainder.x -= toMove;
            transform.position = new Vector2(transform.position.x + toMove, transform.position.y);

            foreach(Actor2D actor in Physics.actors)
            {
                if (hitbox.OverlapWith(actor.hitbox))
                {
                    if(toMove > 0)
                        actor.MoveX(hitbox.Right - actor.hitbox.Left, actor.Squish, true);
                    else
                        actor.MoveX(hitbox.Left - actor.hitbox.Right, actor.Squish, true);
                }
                else if (riders.Contains(actor))
                {
                    actor.MoveX(toMove, null);
                }
            }
        }
    }
    
    private void MoveY (float amount, ICollection<Actor2D> riders)
    {
        remainder.y += amount;
        int toMove = (int) remainder.y;

        if (toMove != 0)
        {
            remainder.y -= toMove;
            transform.position = new Vector2(transform.position.x, transform.position.y + toMove);

            foreach(Actor2D actor in Physics.actors)
            {
                if (hitbox.OverlapWith(actor.hitbox))
                {
                    if(toMove > 0)
                        actor.MoveY(hitbox.Bottom - actor.hitbox.Top, actor.Squish);
                    else
                        actor.MoveY(hitbox.Top - actor.hitbox.Bottom, actor.Squish);
                }
                else if (riders.Contains(actor))
                {
                    actor.MoveY(toMove, null);
                }
            }
        }
    }
}
