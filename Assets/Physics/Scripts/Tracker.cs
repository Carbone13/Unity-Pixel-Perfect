using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace C13.Physics
{
    public class Tracker
    {
        public QuadTree<Entity> qtree;
            
        public IEnumerable<Entity> Get<T> (Rect location) where T : Entity
        {
            return qtree.RetrieveObjectsInArea(location).OfType<T>();
        }

        public void Add (Entity entity)
        {
            qtree.Insert(entity);
        }
        
        public void Remove (Entity entity)
        {
            qtree.Remove(entity);
        }
    }
}