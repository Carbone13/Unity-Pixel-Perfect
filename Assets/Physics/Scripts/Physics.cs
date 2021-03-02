using System;
using System.Collections.Generic;
using UnityEngine;

namespace C13.Physics
{
    // Static class used to track every AABB, and to operate the different checks
    public static class Physics
    {
        // This list contains every AABBs in the scene we need to check
        public static readonly List<AABB> boxes = new List<AABB>();

        // This list contains every Actor2D in the scene
        public static readonly List<Actor2D> actors = new List<Actor2D>();

        // Basic function, called by an AABB, it loops through every AABB and return true
        // if we collide with at least one of them
        // We can input an offset, it will modify the AABB position
        public static AABB CollideAt (this AABB _caller, Vector2? position = null)
        {
            // Loop through every registered AABBs
            foreach (AABB box in boxes)
            {
                // If we are looking at a box which is not us and is active
                if (box != _caller && box.isActive)
                {
                    // ... and we overlap it
                    if (_caller.OverlapWith(box, position))
                    {
                        // we hit someone Sir !
                        return box;
                    }
                }
            }

            // Or...
            // We looped through every AABBs, but we overlap none Sir !
            return null;
        }
        

        // Return every Actor2D currently riding the caller
        public static List<Actor2D> GetAllRiders (this Solid2D _caller)
        {
            List<Actor2D> riders = new List<Actor2D>();

            // Loop through every Actor2D
            foreach (Actor2D actor in actors)
            {
                // If this one is riding us...
                if (actor.isRiding(_caller))
                {
                    //... add it to our list
                    riders.Add(actor);
                }
            }

            // And finally return this list
            return riders;
        }

        public static void RegisterBox (this AABB _caller)
        {
            boxes.Add(_caller);
        }

        public static void UnregisterBox (this AABB _caller)
        {
            boxes.Remove(_caller);
        }

        public static void RegisterActor (this Actor2D _caller)
        {
            actors.Add(_caller);
        }

        public static void UnregisterActor (this Actor2D _caller)
        {
            actors.Remove(_caller);
        }
    }

    [Serializable]
    public struct CollisionFlag
    {
        public bool left, right, above, below;
    }
}