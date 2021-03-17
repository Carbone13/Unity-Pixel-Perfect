using System.Collections.Generic;
using System.Linq;
using C13.Physics;

// It is a basic script, it make a list of entities detected by both trees
public class TreeDebugger : Entity
{
    public List<Entity> detectedByQtree;

    private void Update ()
    {
        detectedByQtree = GameManager.Instance.Tracker.GetAllEntitiesInRange(collisionCheckRange).ToList();
        //GameManager.Instance.Tracker.RebuildTreeElement(this);
        GameManager.Instance.Tracker.AskRebuild();
    }
}
