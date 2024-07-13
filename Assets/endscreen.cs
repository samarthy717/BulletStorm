using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endscreen : MonoBehaviour
{
    // Start is called before the first frame update
    public static endscreen instance;
    public GameObject endscreenPrefab;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
