using C13.Physics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else 
        {
            _instance = this;
        }
        
        Tracker = new Tracker();
        Tracker.qtree = new QuadTree<Entity>(1000, new Rect(-300, -300, 600, 600));
    }

    public Tracker Tracker;
}