using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MatchManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
