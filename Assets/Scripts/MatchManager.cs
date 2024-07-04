using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static MatchManager instance;
    public List<PlayerInfo> allplayers = new List<PlayerInfo>();
    private int index;

    public enum Eventcodes : byte
    {
        NewPlayers,
        ListPlayers,
        Updatestats
    }

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
        else
        {
            NewPlayersSend(PhotonNetwork.NickName);
        }
    }

    void Update()
    {
    }

    public void OnEvent(EventData photonevent)
    {
        if (photonevent.Code < 200)
        {
            Eventcodes eventcodes = (Eventcodes)photonevent.Code;
            object[] data = (object[])photonevent.CustomData;

            switch (eventcodes)
            {
                case Eventcodes.NewPlayers:
                    NewPlayersRecieve(data);
                    break;
                case Eventcodes.ListPlayers:
                    ListPlayersRecieve(data);
                    break;
                case Eventcodes.Updatestats:
                    UpdatePlayersStatsRecieve(data);
                    break;
            }
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void NewPlayersSend(string username)
    {
        object[] package = new object[4];
        package[0] = username;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = 0;
        package[3] = 0;

        PhotonNetwork.RaiseEvent(
           (byte)Eventcodes.NewPlayers,
           package,
           new RaiseEventOptions { Receivers = ReceiverGroup.All },
           new SendOptions { Reliability = true }
        );
    }

    public void NewPlayersRecieve(object[] data)
    {
        string username = (string)data[0];
        int actorNumber = Convert.ToInt32(data[1]);
        int value1 = Convert.ToInt32(data[2]);
        int value2 = Convert.ToInt32(data[3]);

        PlayerInfo newplayerinfo = new PlayerInfo(username, actorNumber, value1, value2);

        allplayers.Add(newplayerinfo);
        ListPlayersSend();
    }

    public void ListPlayersSend()
    {
        object[] package = new object[allplayers.Count];

        for (int i = 0; i < allplayers.Count; i++)
        {
            object[] piece = new object[4];
            piece[0] = allplayers[i].Name;
            piece[1] = allplayers[i].actornumber;
            piece[2] = allplayers[i].kills;
            piece[3] = allplayers[i].deaths;

            package[i] = piece;
        }

        PhotonNetwork.RaiseEvent(
            (byte)Eventcodes.ListPlayers,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }

    public void ListPlayersRecieve(object[] data)
    {
        allplayers.Clear();
        for (int i = 0; i < data.Length; i++)
        {
            object[] playerData = (object[])data[i];
            PlayerInfo plyrinfo = new PlayerInfo((string)playerData[0], Convert.ToInt32(playerData[1]), Convert.ToInt32(playerData[2]), Convert.ToInt32(playerData[3]));
            allplayers.Add(plyrinfo);

            if (PhotonNetwork.LocalPlayer.ActorNumber == plyrinfo.actornumber)
            {
                index = i;
            }
        }
    }

    public void UpdatePlayersStatsSend(int actor,int statstoupdate,int val)
    {
        object[] package = { actor, statstoupdate, val };
        PhotonNetwork.RaiseEvent(
            (byte)Eventcodes.Updatestats,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }

    public void UpdatePlayersStatsRecieve(object[] data)
    {
        int actor = (int)data[0];
        int statsToupdate = (int)data[1];
        int  val= (int)data[2];

        for(int i=0;i<allplayers.Count;i++)
        {
            if (allplayers[i].actornumber == actor)
            {
                switch (statsToupdate)
                {
                    case 0:
                        allplayers[i].kills += val;
                        break;
                    case 1:
                        allplayers[i].deaths += val;
                        break;
                }
                break;
            }
        }
    }
}

[System.Serializable]
public class PlayerInfo
{
    public string Name;
    public int actornumber, kills, deaths;

    public PlayerInfo(string _name, int _actornumber, int _kills, int _deaths)
    {
        Name = _name;
        actornumber = _actornumber;
        kills = _kills;
        deaths = _deaths;
    }
}
