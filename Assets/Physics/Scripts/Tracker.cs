using System.Collections.Generic;
using System.Linq;
using org.khelekore.prtree;
using UnityEngine;

namespace C13.Physics
{
    public class Tracker
    {
        // Need to be > 1
        // Lower value = Slower Insertion, Faster search
        // Higher value = Faster Insertion, Slower search
        public const int BranchFactor = 2;
        private bool initialized;

        public PRTree<Entity> rtree;
        public readonly List<Entity> entities = new List<Entity>();    
        
        public IEnumerable<Entity> Get<T> (Collider range) where T : Entity
        {
            Rect envelope = (Rect) range;
            IEnumerable<T> ofType = rtree.Find(envelope.xMin, envelope.yMin, envelope.xMax, envelope.yMax).OfType<T>();
            return ofType;
        }

        public void Add (Entity entity)
        {
            entities.Add(entity);

            // If we already bulk loaded
            if (initialized)
            {
                // We can't add entities at runtime, so we need to re build our tree
                // It can cost a bit (even though it's not much as long as you don't add/remove object every frames), so I suggest you to 
                // 1 : add all object at Awake, and don't instantiate new one
                // or 2 : Create a second PR-Tree for "Runtime objects" so at least you wont rebuild every Awake colliders
                
                rtree = new PRTree<Entity>(new EntityBoundsGetter(), BranchFactor);
                rtree.BulkInsert(entities);
            }
        }

        public void Remove (Entity entity)
        {
            // Like adding at runtime, removing don't work, so we do the same as Inserting :
            entities.Remove(entity);
            
            rtree = new PRTree<Entity>(new EntityBoundsGetter(), BranchFactor);
            rtree.BulkInsert(entities);
        }

        public void Initialize ()
        {
            // Bulk loading
            rtree.BulkInsert(entities);
            initialized = true;
        }
    }
}