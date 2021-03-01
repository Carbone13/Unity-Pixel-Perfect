using UnityEngine;
using System;

public abstract class Actor2D : MonoBehaviour
{
    [HideInInspector]
    public AABB hitbox;

    [SerializeField]
    private Vector2 remainder;
    
    private void Awake ()
    {
        hitbox = GetComponent<AABB>();
    }

    public void Move (Vector2 amount, Action xCollideCallback = null, Action yCollideCallback = null)
    {
        MoveX(amount.x, xCollideCallback);
        MoveY(amount.y, yCollideCallback);
    }
    
    private void MoveX (float amount, Action onCollide)
    {
        remainder.x += amount;
        // We round our remainder to the lowest integer, which is done by simply casting it into an integer (it remove the decimal part)
        int toMove = (int) remainder.x;

        if (toMove != 0)
        {
            int sign = Math.Sign(toMove);
            remainder.x -= toMove;

            while (toMove != 0)
            {
                if(!hitbox.CollideAt(hitbox.position + new Vector2(sign, 0)))
                {
                    transform.position = new Vector2(transform.position.x + sign, transform.position.y);
                    toMove -= sign;
                }
                else
                {
                    onCollide?.Invoke();
                    break;
                }
            }
        }
    }
    
    public void MoveY (float amount, Action onCollide)
    {
        remainder.y += amount;
        int toMove = (int) remainder.y;

        if (toMove != 0)
        {
            int sign = Math.Sign(toMove);
            remainder.y -= toMove;

            while (toMove != 0)
            {
                if(!hitbox.CollideAt(hitbox.position + new Vector2(0, sign)))
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + sign);
                    toMove -= sign;
                }
                else
                {
                    onCollide?.Invoke();
                    break;
                }
            }
        }
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
    public abstract void Squish ();
}
