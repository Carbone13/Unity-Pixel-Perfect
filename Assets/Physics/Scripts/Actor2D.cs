using UnityEngine;
using System;

namespace C13.Physics
{
    [Tracked(true)]
    public abstract class Actor2D : Entity
    {
        protected void Move (Vector2 amount, Action<int> xCollideCallback = null, Action<int> yCollideCallback = null)
        {
            MoveX(amount.x, xCollideCallback);
            MoveY(amount.y, yCollideCallback);
        }

        public void MoveX (float amount, Action<int> onCollide)
        {
            remainder.x += amount;
            int toMove = (int) Math.Round(remainder.x, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.x -= toMove;
                MoveExactX(toMove, onCollide);
            }
        }
        
        public void MoveY (float amount, Action<int> onCollide)
        {
            remainder.y += amount;
            int toMove = (int) Math.Round(remainder.y, MidpointRounding.ToEven);
            if (toMove != 0)
            {
                remainder.y -= toMove;
                MoveExactY(toMove, onCollide);
            }
        }
        
        private void MoveExactX (int amount, Action<int> onCollide)
        {
            int sign = Math.Sign(amount);

            while (amount != 0)
            {
                Entity hit = CollideFirstAt<Entity>(collider.AbsolutePosition + Vector2.right * sign);

                if (hit == null)
                {
                    transform.position = new Vector2(transform.position.x + sign, transform.position.y);
                    amount -= sign;
                }
                else
                {
                    onCollide?.Invoke(sign);
                    break;
                }
            }
        }
        
        private void MoveExactY (int amount, Action<int> onCollide)
        {
            int sign = Math.Sign(amount);

            while (amount != 0)
            {
                Entity hit = CollideFirstAt<Entity>(collider.AbsolutePosition + Vector2.up * sign);

                if (hit == null)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + sign);
                    amount -= sign;
                }
                else
                {
                    onCollide?.Invoke(sign);
                    break;
                }
            }
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
// It is called when we are squished into a collider by a Solid2D
        public abstract void Squish (int direction);
    }
}
