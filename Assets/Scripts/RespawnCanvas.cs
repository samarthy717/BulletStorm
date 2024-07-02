using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public static RespawnCanvas instance;
    public TMP_Text respawntext;
    public GameObject respawcanvas;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        respawcanvas.SetActive(false);
    }

    void Update()
    {
        
    }
}
