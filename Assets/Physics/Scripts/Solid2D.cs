using System;
using System.Collections.Generic;
using UnityEngine;

namespace C13.Physics
{ 
    [RequireComponent(typeof(AABB))]
// Solid2d is a class which you need to inherit from
// It is used for things that need to move but don't care about colliding
// BUT ! It will carry or push Actor2D if it's in its path !
// It's mainly used for Moving Platform, but you don't even need to use the Move() function
// You can just add this script to a random GameObject and make it a wall.
// If you want something to move and collide with environment, then look at Actor2D
    public abstract class Solid2D : MonoBehaviour
    {
        [HideInInspector] public AABB hitbox;

        [SerializeField] private Vector2 remainder;

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

            remainder.x += amount.x;
            remainder.y += amount.y;

            int toMoveX = Mathf.RoundToInt(remainder.x);
            int toMoveY = Mathf.RoundToInt(remainder.y);

            if (toMoveX != 0)
            {
                remainder.x -= toMoveX;
                transform.position = new Vector2(transform.position.x + toMoveX, transform.position.y);

                foreach (Actor2D actor in Physics.actors)
                {
                    if (hitbox.OverlapWith(actor.hitbox))
                    {
                        actor.ClearRemainderX();

                        if (toMoveX > 0)
                            actor.MoveX(hitbox.Right - actor.hitbox.Left, actor.Squish);
                        else
                            actor.MoveX(hitbox.Left - actor.hitbox.Right, actor.Squish);
                    }
                    else if (riders.Contains(actor))
                    {
                        actor.ClearRemainderX();
                        actor.MoveX(toMoveX, null);
                    }
                }
            }

            if (toMoveY != 0)
            {
                remainder.y -= toMoveY;
                transform.position = new Vector2(transform.position.x, transform.position.y + toMoveY);

                foreach (Actor2D actor in Physics.actors)
                {
                    if (hitbox.OverlapWith(actor.hitbox))
                    {
                        actor.ClearRemainderY();

                        if (toMoveY > 0)
                            actor.MoveY(hitbox.Top - actor.hitbox.Bottom, actor.Squish);
                        else
                            actor.MoveY(hitbox.Bottom - actor.hitbox.Top, actor.Squish);
                    }
                    else if (riders.Contains(actor))
                    {
                        actor.ClearRemainderY();
                        actor.MoveY(toMoveY, null);
                    }
                }
            }

            // Now we reactivate collisions
            hitbox.isActive = true;
        }

        protected void ClearRemainder ()
        {
            remainder = Vector2.zero;
        }
    }
}