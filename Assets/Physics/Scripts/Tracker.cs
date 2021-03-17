using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using RTree;
using QTree;

namespace C13.Physics
{
    public class Tracker
    {
        #region Tree
        // So to optimize research, we use a combination of Quadtree and R-Tree
        // Quadtree are fast to build/update but research is a bit slow
        // R-Tree on the contrary are a bit slow to build but research is fast
        // So we use Quadtree for Actor & Solid (i.e things that move) because we need to rebuild the tree every time
        // And we use RTree for static entities, as we just have to build the tree one time
        public QuadTree<Entity> movingTree;
        public RTree<Entity> staticTree;

        public bool rebuildAsked;
        #endregion
        
        #region Runtime Lists
        // Every Entity in the scene
        public readonly List<Entity> entities = new List<Entity>();    
        // Every Static Entity in the scene
        public readonly List<Entity> staticEntities = new List<Entity>(); 
        // Every Moving Entity in the scene
        public readonly List<Entity> movingEntities = new List<Entity>(); 
        // Every Actor in the scene
        public readonly List<Actor2D> actors = new List<Actor2D>();    
        // Every Solid in the scene
        public readonly List<Solid2D> solids = new List<Solid2D>();
        #endregion

        #region Public Getter
        
        public IEnumerable<Entity> GetAllEntitiesInRange (Collider range)
        {
            var moving = GetAllMovingInRange(range);
            var statiq = GetAllStaticInRange(range);

            if (statiq == null && moving == null)
            {
                return Array.Empty<Entity>();
            }
            
            if (statiq != null)
            {
                return statiq.Concat(moving ?? Array.Empty<Entity>());
            }
            else
            {
                return moving;
            }
        }
        
        public IEnumerable<Entity> GetAllStaticInRange (Collider range)
        {
            return staticTree.Search((Envelope) range);
        }
        
        public IEnumerable<Entity> GetAllMovingInRange (Collider range)
        {
            return movingTree.RetrieveObjectsInArea((Rect) range);
        }
        
        public IEnumerable<Entity> GetMovingInRange<T> (Collider range) where T : Entity, IMovable
        {
            return movingTree.RetrieveObjectsInArea((Rect) range).OfType<T>();
        }
        
        public IEnumerable<T> GetAll<T> () where T : Entity
        {
            if (typeof(T) == typeof(Actor2D))
            {
                return actors.AsEnumerable() as IEnumerable<T>;
            }
            if (typeof(T) == typeof(Solid2D))
            {
                return solids.AsEnumerable() as IEnumerable<T>;
            }
            
            return entities.AsEnumerable() as IEnumerable<T>;
        }

        public IEnumerable<Entity> GetAllStaticEntities ()
        {
            return staticEntities;
        }

        public IEnumerable<Entity> GetAllMovingEntities ()
        {
            return movingEntities;
        }
        
        #endregion

        #region Add/Remove
        
        public void Add (Entity entity)
        {
            entities.Add(entity);
            
            if (entity.gameObject.isStatic)
            {
                // We add it to the list, and we will bulk load all static in Initialize()
                staticEntities.Add(entity);
                
                // But if it is added at runtime, we will just insert it normally
                if (staticTreeInitialized)
                {
                    staticTree.Insert(entity);
                }
            }
            else
            {
                movingEntities.Add(entity);
                movingTree.Insert(entity);
            }

            if (entity.GetType() == typeof(Actor2D))
            {
                actors.Add(entity as Actor2D);
            }
            if (entity.GetType() == typeof(Solid2D))
            {
                solids.Add(entity as Solid2D);
            }
        }

        public void Remove (Entity entity)
        {
            entities.Remove(entity);
            
            if (entity.gameObject.isStatic)
            {
                staticEntities.Remove(entity);
            }
            else
            {
                movingEntities.Remove(entity);
            }

            if (entity.GetType() == typeof(Actor2D))
            {
                actors.Remove(entity as Actor2D);
            }
            if (entity.GetType() == typeof(Solid2D))
            {
                solids.Remove(entity as Solid2D);
            }
        }
       
        #endregion
        
        #region Tree Manager
        
        private bool staticTreeInitialized;
        
        public void Initialize ()
        {
            movingTree = new QuadTree<Entity>(1, new Rect(-1000, -1000, 2000, 2000));
            staticTree = new RTree<Entity>();
        }

        public void StartupBulkLoad ()
        {
            staticTree.BulkLoad(staticEntities);
            staticTreeInitialized = true;
        }

        public void RebuildTreeElement (Entity entity)
        {
            movingTree.Remove(entity);
            movingTree.Insert(entity);
        }

        public void RebuildTree ()
        {
            movingTree.Clear();
            foreach(Entity entity in movingEntities) movingTree.Insert(entity);

            rebuildAsked = false;
        }

        public void AskRebuild ()
        {
            rebuildAsked = true;
        }
        #endregion
    }
}