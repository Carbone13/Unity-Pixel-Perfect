using UnityEngine;
using System.Linq;
using System;

namespace C13.Physics
{
    public class Entity : MonoBehaviour, IQuadTreeObject
    {
        #region Collider Settings
        public new Collider collider;
        
        [SerializeField, Tooltip("This entity will check for collision only the collider in this area")]
        protected Collider quadTreeRange;
        
        public bool Collidable = true;
        #endregion
        
        #region Runtime
        protected Vector2 remainder;
        #endregion

        #region Protected void
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
            return !Collidable ? null : (from entity in GameManager.Instance.Tracker.Get<T>((Rect)quadTreeRange) where entity.Collidable && entity != this where CollideWith(entity, at) select (T) entity).FirstOrDefault();
        }

        protected bool IsCollidingWith<T> (Vector2? at = null) where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.Get<T>((Rect)quadTreeRange).Where(entity => entity.Collidable && entity != this).Any(entity => CollideWith(entity, at));
        }
        #endregion
        
        #region Virtual Void
        // Inheritors can override these void
        
        public virtual void Awake ()
        {
            collider.owner = this;
            quadTreeRange.owner = this;
        }

        public virtual void OnDrawGizmos ()
        {
            collider.owner = this;
            quadTreeRange.owner = this;
            
            collider.Draw();
            quadTreeRange.Draw();
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

        public Rect GetBounds ()
        {
            return (Rect)collider;
        }
    }
}