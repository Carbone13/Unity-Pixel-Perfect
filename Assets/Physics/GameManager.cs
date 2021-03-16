using System;
using C13.Physics;
using org.khelekore.prtree;
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
        Tracker.rtree = new PRTree<Entity>(new EntityBoundsGetter(), Tracker.BranchFactor);
    }

    private void Update ()
    {
        if (!InitializedTracker)
        {
            Tracker.Initialize();
            InitializedTracker = true;
        }
    }

    public Tracker Tracker;
    public bool InitializedTracker;
}