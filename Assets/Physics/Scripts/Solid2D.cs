using System;
using System.Collections.Generic;
using UnityEngine;

namespace C13.Physics
{
// Solid2d is a class which you need to inherit from
// It is used for things that need to move but don't care about colliding
// BUT ! It will carry or push Actor2D if it's in its path !
// It's mainly used for Moving Platform, but you don't even need to use the Move() function
// You can just add this script to a random GameObject and make it a wall.
// If you want something to move and collide with environment, then look at Actor2D
    public abstract class Solid2D : Entity
    {
        private HashSet<Actor2D> riders = new HashSet<Actor2D>();

        // Simple move function
        protected void Move (Vector2 amount)
        {
            GetRiders();
            
            Collidable = false;
            
            MoveX(amount.x);
            MoveY(amount.y);

            Collidable = true;
            riders.Clear();
        }
        
        private void MoveX (float amount)
        {
            remainder.x += amount;
            int toMove = (int) Math.Round(remainder.x, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.x -= toMove;
                MoveExactX(toMove);
            }
        }

        private void MoveY (float amount)
        {
            remainder.y += amount;
            int toMove = (int) Math.Round(remainder.y, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.y -= toMove;
                MoveExactY(toMove);
            }
        }

        private void MoveExactX (int amount)
        {
            transform.position = new Vector2(transform.position.x + amount, transform.position.y);

            foreach (var entity in GameManager.Instance.Tracker.Get<Actor2D>(collisionCheckRange))
            {
                var actor = (Actor2D) entity;
                if (CollideWith(actor))
                {
                    actor.ClearRemainderX();

                    if (amount > 0)
                        actor.MoveX(collider.Right - actor.collider.Left, actor.Squish);
                    else
                        actor.MoveX(collider.Left - actor.collider.Right, actor.Squish);
                }
                else if (riders.Contains(actor))
                {
                    actor.MoveX(amount, null);
                }
            }
        }

        private void MoveExactY (int amount)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + amount);

            foreach (var entity in GameManager.Instance.Tracker.Get<Actor2D>(collisionCheckRange))
            {
                var actor = (Actor2D) entity;
                if (CollideWith(actor))
                {
                    actor.ClearRemainderY();

                    if (amount > 0)
                        actor.MoveY(collider.Top - actor.collider.Bottom, actor.Squish);
                    else
                        actor.MoveY(collider.Bottom - actor.collider.Top, actor.Squish);
                }
                else if (riders.Contains(actor))
                {
                    actor.MoveY(amount, null);
                }
            }
            
        }

        protected void ClearRemainder ()
        {
            remainder = Vector2.zero;
        }
        
        private void GetRiders ()
        {
            foreach (var entity in GameManager.Instance.Tracker.Get<Actor2D>(collisionCheckRange))
            {
                var actor = (Actor2D) entity;
                if (actor.IsRiding(this))
                {
                    riders.Add(actor);
                }
            }
        }
    }
}