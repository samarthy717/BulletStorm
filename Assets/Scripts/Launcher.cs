using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static Launcher instance;

    public GameObject LoadingScreen;
    public TMP_Text loadingtext;

    public GameObject menubuttons;

    public GameObject CreateRoommenu;
    public TMP_InputField roomname;

    public GameObject RoomMenu;
    public TMP_Text roommenutext;

    public GameObject ErrorPanel;
    public TMP_Text errortext;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CloseMenus();
        LoadingScreen.SetActive(true);
        loadingtext.text = "Connecting.....";

        PhotonNetwork.ConnectUsingSettings();
    }

    private void CloseMenus()
    {
        LoadingScreen.SetActive(false);
        menubuttons.SetActive(false);
        CreateRoommenu.SetActive(false);
        RoomMenu.SetActive(false);
        ErrorPanel.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        loadingtext.text = "Joining Lobby..";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        CloseMenus();
        menubuttons.SetActive(true);
    }
    public void OnOpenRoomCreate()
    {
        CloseMenus();
        CreateRoommenu.SetActive(true );
    }
    public void OnCreateRoom()
    {
        if(!string.IsNullOrEmpty(roomname.text)){ 
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;

            PhotonNetwork.CreateRoom(roomname.text, options);

            CloseMenus();
            loadingtext.text = "Creating Room...";
            LoadingScreen.SetActive(true) ;
        }
    }
    public override void OnJoinedRoom()
    {
        CloseMenus();
        RoomMenu.SetActive(true);
        roommenutext.text = PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseMenus();
        ErrorPanel.SetActive(true);
        errortext.text = "Failed to Create Room :"+message;
    }
    public void OnErrorPanelClose()
    {
        CloseMenus();
        menubuttons.SetActive(true);
    }
    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenus();
        loadingtext.text = "Leaving Room...";
        LoadingScreen.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        CloseMenus();
        menubuttons.SetActive(true);
    }
}
