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
        detectedByQtree = GameManager.Instance.Tracker.qtree.RetrieveObjectsInArea((Rect)quadTreeRange);
    }

    public override void OnDrawGizmos ()
    {
        base.OnDrawGizmos();
        if (GameManager.Instance == null) return;
        
        GameManager.Instance.Tracker.qtree.DrawDebug();
    }
}
