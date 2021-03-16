using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using C13.Physics;

public class NewBehaviourScript : Entity
{
    public List<Entity> detectedByQtree;

    private void Update ()
    {
        Rect enveloppe = (Rect)collisionCheckRange;
        detectedByQtree = GameManager.Instance.Tracker.rtree.Find(enveloppe.xMin, enveloppe.yMin, enveloppe.xMax, enveloppe.yMax).ToList();
        
        //var items = GameManager.Instance.Tracker.rtree.Search();
    }
}
