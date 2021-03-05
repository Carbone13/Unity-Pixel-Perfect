using UnityEngine;
using System;

namespace C13.Physics
{
    [Tracked]
    public class Entity : MonoBehaviour
    {
        public new Collider collider;
        public bool Collidable;
        
        [SerializeField] protected Vector2 remainder;


        public virtual void Awake ()
        {
            collider.owner = this;
        }

        public virtual void OnDrawGizmos ()
        {
            collider.owner = this;
            collider.Draw();
        }
        
        protected bool CollideWith (Entity other, Vector2? at = null)
        {
            float x, y;

            if (at == null)
            {
                x = Math.Abs((collider.AbsoluteX + collider.AbsoluteSize.x / 2) - (other.collider.AbsoluteX + other.collider.AbsoluteSize.x / 2));
                y = Math.Abs((collider.AbsoluteY + collider.AbsoluteSize.y / 2) - (other.collider.AbsoluteY + other.collider.AbsoluteSize.y / 2));
            }
            else
            {
                Vector2 _positionOverwrite = (Vector2) at;

                x = Math.Abs(((int)_positionOverwrite.x + collider.AbsoluteSize.x / 2) - (other.collider.AbsoluteX + other.collider.AbsoluteSize.x / 2));
                y = Math.Abs(((int)_positionOverwrite.y + collider.AbsoluteSize.y / 2) - (other.collider.AbsoluteY + other.collider.AbsoluteSize.y / 2));
            }

            bool xCheck = x * 2 < (collider.AbsoluteSize.x + other.collider.AbsoluteSize.x);
            bool yCheck = y * 2 < (collider.AbsoluteSize.y + other.collider.AbsoluteSize.y);

            return xCheck && yCheck;
        }

        protected T CollideFirst<T> () where T : Entity
        {
            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (CollideWith(entity))
                {
                    return (T)entity;
                }
            }

            return null;
        }

        protected T CollideFirstAt<T> (Vector2 position) where T : Entity
        {
            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (CollideWith(entity, position))
                {
                    return (T)entity;
                }
            }

            return null;
        }
        
        protected bool IsCollidingWith<T> () where T : Entity
        {
            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (CollideWith(entity))
                {
                    return true;
                }
            }

            return false;
        }
        
        protected bool IsCollidingWithAt<T> (Vector2 position) where T : Entity
        {
            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (CollideWith(entity, position))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnEnable ()
        {
            GameManager.Instance.Tracker.Add(this);
        }
        
        public void OnDisable ()
        {
            GameManager.Instance.Tracker.Remove(this);
        }
    }
}