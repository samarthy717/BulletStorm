using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public TMP_Text roomname;
    private RoomInfo info;

    public void SetRoomDetails(RoomInfo InputInfo)
    {
        info = InputInfo;
        roomname.text = info.Name;
    }
    public void OpenRoom()
    {
        Launcher.instance.OnJoinRoom(info);
    } 
  
}
