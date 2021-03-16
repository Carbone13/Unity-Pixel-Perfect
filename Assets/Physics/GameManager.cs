using C13.Physics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Tracker Tracker;
    public bool InitializedTracker;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            Instance = this;
        }
        
        Tracker = new Tracker();
        Tracker.Initialize();
    }

    private void Update ()
    {
        if (!InitializedTracker)
        {
            Tracker.StartupBulkLoad();
            InitializedTracker = true;
        }
    }
}