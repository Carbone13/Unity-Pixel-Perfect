using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace C13.Physics
{
    public class Tracker
    {
        // This dictionary list which type we need to track
        private readonly Dictionary<Type, List<Type>> TrackedEntityTypes;

        // All entities tracked, organized by their Types
        public readonly Dictionary<Type, List<Entity>> Entities;

        public List<Entity> Get<T> () where T : Entity
        {
            return Entities[typeof(T)];
        }

        public void CompleteDebug ()
        {
            foreach (var key in TrackedEntityTypes.Keys)
            {
                Debug.Log("--- " + key + " ---" + " " + TrackedEntityTypes[key].Count);
                for (int i = 0; i < TrackedEntityTypes[key].Count; i++)
                {
                    Debug.Log(TrackedEntityTypes[key][i]);
                }
            }
            /*
            foreach (var key in Entities.Keys)
            {
                Debug.Log("--- " + key + " ---" + " " + Entities[key].Count);
                for (int i = 0; i < Entities[key].Count; i++)
                {
                    Debug.Log(Entities[key][i].transform.name);
                }
            }
            */
            /*
            for (int i = 0; i < Get<Entity>().Count; i++)
            {
                Debug.Log(Get<Entity>()[i].transform.name);
            }*/
        }
        
        public void Add (Entity entity)
        {
            Type type = entity.GetType();
            if (!TrackedEntityTypes.TryGetValue(type, out var trackAs))
            {
                return;
            }

            foreach (Type t in trackAs)
            {
                Entities[t].Add(entity);
            }
        }
        
        public void Remove (Entity entity)
        {
            Type type = entity.GetType();
            if (!TrackedEntityTypes.TryGetValue(type, out var trackAs))
            {
                return;
            }
            
            foreach (Type t in trackAs)
            {
                Entities[t].Remove(entity);
            }
        }

        public Tracker ()
        {
            TrackedEntityTypes = new Dictionary<Type, List<Type>>();
            HashSet<Type> storedEntityTypes = new HashSet<Type>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(Tracked), inherit: false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                bool inherited = (attrs[0] as Tracked).Inherited;
                if (typeof(Entity).IsAssignableFrom(type))
                {
                    if (!type.IsAbstract)
                    {
                        if (!TrackedEntityTypes.ContainsKey(type))
                        {
                            TrackedEntityTypes.Add(type, new List<Type>());
                        }
                        TrackedEntityTypes[type].Add(type);
                    }
                    storedEntityTypes.Add(type);
                    if (!inherited)
                    {
                        continue;
                    }
                    foreach (Type subclass2 in GetSubclasses(type))
                    {
                        if (!subclass2.IsAbstract)
                        {
                            if (!TrackedEntityTypes.ContainsKey(subclass2))
                            {
                                TrackedEntityTypes.Add(subclass2, new List<Type>());
                            }
                            TrackedEntityTypes[subclass2].Add(type);
                        }
                    }
                    continue;
                }
                throw new Exception("Type '" + type.Name + "' cannot be Tracked because it does not derive from Entity or Component");
            }
            
            Entities = new Dictionary<Type, List<Entity>>(TrackedEntityTypes.Count);
            foreach (Type type in storedEntityTypes)
            {
                Entities.Add(type, new List<Entity>());
            }
        }

        private List<Type> GetSubclasses(Type type)
        {
            List<Type> matches = new List<Type>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type check in types)
            {
                if (type != check && type.IsAssignableFrom(check))
                {
                    matches.Add(check);
                }
            }
            return matches;
        }
    }

    [Serializable]
    public struct CollisionFlag
    {
        public bool left, right, above, below;
    }

    public class Tracked : Attribute
    {
        public readonly bool Inherited;
        
        public Tracked(bool inherited = true)
        {
            Inherited = inherited;
        }
    }
}