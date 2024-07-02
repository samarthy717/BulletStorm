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

    public GameObject RoomBrowserPanel;
    public RoomButton theroombutton;
    private List<RoomButton> allroombuttons=new List<RoomButton>();

    public TMP_Text playernamelabel;
    private List<TMP_Text>allplayernames= new List<TMP_Text>();

    public GameObject NickNamePanel;
    public TMP_InputField PlayerNickName;
    private bool hasNickName;

    public string leveltoplay;
    public GameObject Startgamebutton;

    public GameObject TestButton;
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
#if UNITY_EDITOR
        TestButton.SetActive(true);
#endif
    }
    private void CloseMenus()
    {
        LoadingScreen.SetActive(false);
        menubuttons.SetActive(false);
        CreateRoommenu.SetActive(false);
        RoomMenu.SetActive(false);
        ErrorPanel.SetActive(false);
        RoomBrowserPanel.SetActive(false);
        NickNamePanel.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        loadingtext.text = "Joining Lobby..";
        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        CloseMenus();
        menubuttons.SetActive(true);
        PhotonNetwork.NickName = UnityEngine.Random.Range(0, 10000).ToString();

        if (!hasNickName)
        {
            NickNamePanel.SetActive(true);
            menubuttons.SetActive(false);
            if (PlayerPrefs.HasKey("playername"))
            {
                PlayerNickName.text = PlayerPrefs.GetString("playername");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playername");
        }
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

        ListAllPlayers();

        if(PhotonNetwork.IsMasterClient)
        {
            Startgamebutton.SetActive(true);
        }
        else
        {
            Startgamebutton.SetActive(false);
        }
    }
    private void ListAllPlayers()
    {
        foreach(TMP_Text player in allplayernames)
        {
            Destroy(player.gameObject);
        }
        allplayernames.Clear();

        Player[] players = PhotonNetwork.PlayerList;
        for(int i = 0; i < players.Length; i++)
        {
            TMP_Text newPlayerLabel = Instantiate(playernamelabel, playernamelabel.transform.parent);
            newPlayerLabel.text = players[i].NickName;
            newPlayerLabel.gameObject.SetActive(true);

            allplayernames.Add(newPlayerLabel);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text newPlayerLabel = Instantiate(playernamelabel, playernamelabel.transform.parent);
        newPlayerLabel.text = newPlayer.NickName;
        newPlayerLabel.gameObject.SetActive(true);

        allplayernames.Add(newPlayerLabel);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListAllPlayers();
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
    public void OnRoombrowserOpen()
    {
        CloseMenus();
        RoomBrowserPanel.SetActive(true);
    }
    public void OnRoombrowserClose()
    {
        CloseMenus();
        menubuttons.SetActive(true);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Startgamebutton.SetActive(true);
        }
        else
        {
            Startgamebutton.SetActive(false);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomButton rb in allroombuttons)
        {
            Destroy(rb.gameObject);
        }
        allroombuttons.Clear();
        theroombutton.gameObject.SetActive(false);
        for(int i=0;i<roomList.Count;i++)
        {
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(theroombutton, theroombutton.transform.parent);
                newButton.SetRoomDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                allroombuttons.Add(newButton);
            }
        }
    }
    public void QuickJoin()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;

        PhotonNetwork.CreateRoom("TestScene", options);
        CloseMenus();
        LoadingScreen.SetActive(true);
        loadingtext.text = "Creating Room...";

    }
    public void OnJoinRoom(RoomInfo inputinfo)
    {
        PhotonNetwork.JoinRoom(inputinfo.Name);

        loadingtext.text = "Joining Room..";
        CloseMenus();
        LoadingScreen.SetActive(true);
    }
    public void setnickname()
    {
        if (!string.IsNullOrEmpty(PlayerNickName.text))
        {
            PhotonNetwork.NickName= PlayerNickName.text;
            PlayerPrefs.SetString("playername",PlayerNickName.text);
            CloseMenus();
            menubuttons.SetActive(true);
            hasNickName = true;
        }
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(leveltoplay);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
