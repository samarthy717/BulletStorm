using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmusic : MonoBehaviour
{
    private bool soundon = true;
    void Start()
    {
        
    }

    public void soundcheck()
    {
        if (soundon)
        {
            soundon = false;
            gameObject.GetComponent<AudioSource>().Stop();
        }
        else
        {
            soundon = true;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    public void quitgame()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }
}
