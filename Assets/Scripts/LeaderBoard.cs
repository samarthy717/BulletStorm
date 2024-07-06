using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{

    public static LeaderBoard Instance;

    private void Awake()
    {
        Instance = this;
    }
    public TMP_Text Playername, kills, deaths;
    public RawImage img;
    public void setdetails(string player,int kill,int death)
    {
        Playername.text = player;
        kills.text = kill.ToString();
        kills.color = Color.green;
        deaths.text = death.ToString();
        deaths.color = Color.red;
        img.color = Color.white;
    }

    public void CurrentPlayer()
    {
        img.color = new Color(141f / 255f, 218f / 255f, 118f / 255f, 255f / 255f);
    }


}
