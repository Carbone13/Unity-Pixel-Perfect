using UnityEngine;
using System.Linq;
using System;
using org.khelekore.prtree;

namespace C13.Physics
{
    public class Entity : MonoBehaviour
    {
        #region Collider Settings
        
        [SerializeField, Tooltip("Collision box for this Entity")]
        public new Collider collider;
        
        [SerializeField, Tooltip("This entity will only check Colliders that are in this area")]
        protected Collider collisionCheckRange;
        
        [SerializeField, Tooltip("Can we collide ?")]
        public bool Collidable = true;
        
        #endregion
        
        #region Runtime Variables
        protected Vector2 remainder;
        #endregion

        #region Protected void
        
        protected bool CollideWith (Entity other)
        {
            float x, y;

            x = Math.Abs((collider.AbsoluteX + collider.AbsoluteSize.x / 2) - (other.collider.AbsoluteX + other.collider.AbsoluteSize.x / 2));
            y = Math.Abs((collider.AbsoluteY + collider.AbsoluteSize.y / 2) - (other.collider.AbsoluteY + other.collider.AbsoluteSize.y / 2));
            
            bool xCheck = x * 2 < (collider.AbsoluteSize.x + other.collider.AbsoluteSize.x);
            bool yCheck = y * 2 < (collider.AbsoluteSize.y + other.collider.AbsoluteSize.y);
            
            return xCheck && yCheck;
        }

        protected bool CollideWith (Entity other, Vector2 at)
        {
            float x, y;

            x = Math.Abs(((int)at.x + collider.AbsoluteSize.x / 2) - (other.collider.AbsoluteX + other.collider.AbsoluteSize.x / 2));
            y = Math.Abs(((int)at.y + collider.AbsoluteSize.y / 2) - (other.collider.AbsoluteY + other.collider.AbsoluteSize.y / 2));
            
            bool xCheck = x * 2 < (collider.AbsoluteSize.x + other.collider.AbsoluteSize.x);
            bool yCheck = y * 2 < (collider.AbsoluteSize.y + other.collider.AbsoluteSize.y);
            
            return xCheck && yCheck;
        }
        
        
        protected T CollideFirst<T> () where T : Entity
        {
            return !Collidable ? null : 
                (from entity in GameManager.Instance.Tracker.Get<T>(collisionCheckRange) 
                    where entity.Collidable 
                          && entity != this 
                    where CollideWith(entity) select (T) entity).FirstOrDefault();
        }
        
        protected T CollideFirst<T> (Vector2 at) where T : Entity
        {
            return !Collidable ? null : 
                (from entity in GameManager.Instance.Tracker.Get<T>(collisionCheckRange) 
                    where entity.Collidable 
                          && entity != this 
                    where CollideWith(entity, at) select (T) entity).FirstOrDefault();
        }

        protected bool IsCollidingWith<T> () where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.Get<T>(collisionCheckRange)
                .Where(entity => entity.Collidable && entity != this)
                .Any(CollideWith);
        }
        
        protected bool IsCollidingWith<T> (Vector2 at) where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.Get<T>(collisionCheckRange)
                .Where(entity => entity.Collidable && entity != this)
                .Any(entity => CollideWith(entity, at));
        }
        
        #endregion
        
        #region Virtual Void
        // Inheritors can override these void
        
        public virtual void Awake ()
        {
            collider.owner = this;
            collisionCheckRange.owner = this;
        }

        public virtual void OnDrawGizmos ()
        {
            collider.owner = this;
            collisionCheckRange.owner = this;
            
            collider.Draw();
            collisionCheckRange.Draw();
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

    public class EntityBoundsGetter : MBRConverter<Entity>
    {
        public double getMinX (Entity item)
        {
            return item.collider.AbsoluteX;
        }

        public double getMinY (Entity item)
        {
            return item.collider.AbsoluteY;
        }

        public double getMaxX (Entity item)
        {
            return item.collider.AbsoluteX + item.collider.AbsoluteSize.x;
        }

        public double getMaxY (Entity item)
        {
            return item.collider.AbsoluteY + item.collider.AbsoluteSize.y;
        }
    }
}