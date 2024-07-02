using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifetimedestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifetime = 5f;
    void Start()
    {
        Destroy(gameObject,lifetime);
    }

}
