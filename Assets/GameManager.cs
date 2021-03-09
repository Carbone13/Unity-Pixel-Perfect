using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using C13.Physics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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
        
    }

    private void Start ()
    {
        //Tracker.CompleteDebug();
    }

    public Tracker Tracker;

}