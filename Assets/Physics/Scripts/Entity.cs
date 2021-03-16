using UnityEngine;
using System.Linq;
using RTree;
using QTree;

namespace C13.Physics
{
    public class Entity : MonoBehaviour, IQuadTreeObject, ISpatialData
    {
        #region Collider Settings
        
        [SerializeField, Tooltip("Collision box for this Entity")]
        public new Collider collider;
        
        [SerializeField, Tooltip("This entity will only check Colliders that are in this area, should be 2px wider than the collider")]
        protected Collider collisionCheckRange;
        
        [SerializeField, Tooltip("Can we collide ?")]
        public bool Collidable = true;
        
        #endregion
        
        #region Runtime Variables
        protected Vector2 remainder;
        #endregion

        #region Protected void

        protected T CollideFirst<T> () where T : Entity
        {
            return !Collidable ? null : 
                (from entity in GameManager.Instance.Tracker.GetAllEntitiesInRange(collisionCheckRange) 
                    where entity.Collidable 
                          && entity != this 
                    where collider.CollideWith(entity.collider) select (T) entity).FirstOrDefault();
        }
        
        protected T CollideFirst<T> (Vector2 at) where T : Entity
        {
            return !Collidable ? null : 
                (from entity in GameManager.Instance.Tracker.GetAllEntitiesInRange(collisionCheckRange) 
                    where entity.Collidable 
                          && entity != this 
                    where collider.CollideWith(entity.collider, at) select (T) entity).FirstOrDefault();
        }

        protected bool IsCollidingWith<T> () where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.GetAllEntitiesInRange(collisionCheckRange) 
                .Where(entity => entity.Collidable && entity != this)
                .Any(entity => collider.CollideWith(entity.collider));
        }
        
        protected bool IsCollidingWith<T> (Vector2 at) where T : Entity
        {
            return Collidable && GameManager.Instance.Tracker.GetAllEntitiesInRange(collisionCheckRange) 
                .Where(entity => entity.Collidable && entity != this)
                .Any(entity => collider.CollideWith(entity.collider, at));
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

        #region Trees Interface
        
        public Envelope Envelope
        {
            get
            {
                Rect coll = (Rect) collider;
                return new Envelope(coll.xMin, coll.yMin, coll.xMax, coll.yMax);
            }
        }
        
        public Rect GetBounds ()
        {
            return (Rect)collider;
        }
        #endregion
    }

    public interface IMovable
    {
    }
}