using UnityEngine;
using System.Linq;
using System;

namespace C13.Physics
{
    [Tracked]
    public class Entity : MonoBehaviour
    {
        public new Collider collider;
        public bool Collidable = true;
        
        protected Vector2 remainder;
        
        
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


        
        protected T CollideFirst<T> (Vector2? at) where T : Entity
        {
            return !Collidable ? null : (from entity in GameManager.Instance.Tracker.Get<T>() where entity.Collidable && entity != this where CollideWith(entity, at) select (T) entity).FirstOrDefault();
            
            // This can be written like that :
            /*
            if (!Collidable) return null;
            
            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (entity.Collidable && entity != this)
                {
                    if (CollideWith(entity, at))
                    {
                        return (T)entity;
                    }
                }
            }

            return null;
            */
        }

        protected bool IsCollidingWith<T> (Vector2? at = null) where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.Get<T>().Where(entity => entity.Collidable && entity != this).Any(entity => CollideWith(entity, at));
            
            // This can be written like that :
            /*
            if (!Collidable) return false;

            foreach (var entity in GameManager.Instance.Tracker.Get<T>())
            {
                if (entity.Collidable && entity != this)
                {
                    if (CollideWith(entity, at))
                    {
                        return true;
                    }
                }
            }

            return false;
            */
        }

        
        #region Virtual Void
        // Inheritors can override these void
        
        public virtual void Awake ()
        {
            collider.owner = this;
        }

        public virtual void OnDrawGizmos ()
        {
            collider.owner = this;
            collider.Draw();
        }
        
        public virtual void OnEnable ()
        {
            GameManager.Instance.Tracker.Add(this);
        }
        
        public virtual void OnDisable ()
        {
            GameManager.Instance.Tracker.Remove(this);
        }
        #endregion
    }
}