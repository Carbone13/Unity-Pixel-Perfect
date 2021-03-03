using UnityEngine;
using System;

namespace C13.Physics
{
    [RequireComponent(typeof(AABB))]
    public abstract class Actor2D : MonoBehaviour
    {
        [HideInInspector] public AABB hitbox;

        [SerializeField] protected Vector2 remainder;

        public virtual void Awake ()
        {
            hitbox = GetComponent<AABB>();
        }

        // Return what we actually moved
        protected void Move (Vector2 amount, Action<int> xCollideCallback = null, Action<int> yCollideCallback = null)
        {
            MoveX(amount.x, xCollideCallback);
            MoveY(amount.y, yCollideCallback);
        }

        public void MoveX (float amount, Action<int> onCollide)
        {
            remainder.x += amount;
            int toMove = Mathf.RoundToInt(remainder.x);

            if (toMove != 0)
            {
                int sign = Math.Sign(toMove);
                remainder.x -= toMove;

                while (toMove != 0)
                {
                    if (!hitbox.CollideAt(hitbox.position + new Vector2(sign, 0)))
                    {
                        transform.position = new Vector2(transform.position.x + sign, transform.position.y);
                        toMove -= sign;
                    }
                    else
                    {
                        onCollide?.Invoke(sign);
                        break;
                    }
                }
            }
        }

        public void MoveY (float amount, Action<int> onCollide)
        {
            remainder.y += amount;
            int toMove = Mathf.RoundToInt(remainder.y);

            if (toMove != 0)
            {
                int sign = Math.Sign(toMove);
                remainder.y -= toMove;

                while (toMove != 0)
                {
                    if (!hitbox.CollideAt(hitbox.position + new Vector2(0, sign)))
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + sign);
                        toMove -= sign;
                    }
                    else
                    {
                        onCollide?.Invoke(sign);
                        break;
                    }
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

        private void OnEnable ()
        {
            this.RegisterActor();
        }

        private void OnDisable ()
        {
            this.UnregisterActor();
        }

        // Class need to make their own isRiding() function
        // Basically, they need to give condition to be considered riding
        // If it's for a platformer, it will be "if we are above the Solid"
        // But maybe we can grab its ledge, so this case need to be considered has isRiding too for example
        // When Riding a moving Solid2D, it will carry us
        public abstract bool isRiding (Solid2D solid);

        // Class who inherit us need to implement this function
        // It is called when we are squished into a collider by a Solid2D
        public abstract void Squish (int direction);
    }
}