using UnityEngine;
using System;

namespace C13.Physics
{
    public abstract class Actor2D : Entity, IMovable
    {
        protected bool Move (Vector2 amount, Action<int, Entity> xCollideCallback = null, Action<int, Entity> yCollideCallback = null)
        {
            bool movedX = MoveX(amount.x, xCollideCallback);
            bool movedY = MoveY(amount.y, yCollideCallback);

            // If we moved
            if (movedX || movedY)
            {
                // The tracker need to be aware of our new position
                GameManager.Instance.Tracker.RebuildTreeElement(this);
                return true;
            }
            
            return false;
        }

        public bool MoveX (float amount, Action<int, Entity> onCollide)
        {
            remainder.x += amount;
            int toMove = (int) Math.Round(remainder.x, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.x -= toMove;
                return MoveExactX(toMove, onCollide);
            }

            return false;
        }
        
        public bool MoveY (float amount, Action<int, Entity> onCollide)
        {
            remainder.y += amount;
            int toMove = (int) Math.Round(remainder.y, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.y -= toMove;
                return MoveExactY(toMove, onCollide);
            }

            return false;
        }
        
        private bool MoveExactX (int amount, Action<int, Entity> onCollide)
        {
            bool moved = false;
            int sign = Math.Sign(amount);

            while (amount != 0)
            {
                Entity hit = CollideFirst<Entity>(collider.AbsolutePosition + Vector2.right * sign);

                if (hit == null)
                {
                    transform.position = new Vector2(transform.position.x + sign, transform.position.y);
                    moved = true;
                    amount -= sign;
                }
                else
                {
                    onCollide?.Invoke(sign, hit);
                    break;
                }
            }

            return moved;
        }
        
        private bool MoveExactY (int amount, Action<int, Entity> onCollide)
        {
            bool moved = false;
            int sign = Math.Sign(amount);

            while (amount != 0)
            {
                Entity hit = CollideFirst<Entity>(collider.AbsolutePosition + Vector2.up * sign);

                if (hit == null)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + sign);
                    moved = true;
                    amount -= sign;
                }
                else
                {
                    onCollide?.Invoke(sign, hit);
                    break;
                }
            }

            return moved;
        }

        public void ClearRemainderX ()
        {
            remainder.x = 0;
        }

        public void ClearRemainderY ()
        {
            remainder.y = 0;
        }

        
        
        // Class need to make their own isRiding() function
        // Basically, they need to give condition to be considered riding
        // If it's for a platformer, it will be "if we are above the Solid"
        // But maybe we can grab its ledge, so this case need to be considered has isRiding too for example
        // When Riding a moving Solid2D, it will carry us
        public abstract bool IsRiding (Solid2D solid);

        // Class who inherit us need to implement this function
        // It is called when we are pushed into a collider by a Solid2D, so we are squished between both
        public abstract void Squish (int direction, Entity hit);
    }
}
