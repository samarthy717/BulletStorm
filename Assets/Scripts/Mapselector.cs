using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    public Button Futurecitybutton;
    public Button Sandlandbutton;
    public Button Warehousebutton;
    public Button Docksbutton;

    private ColorBlock normalColorBlock;
    private ColorBlock selectedColorBlock;

    void Start()
    {
        normalColorBlock = Futurecitybutton.colors;
        normalColorBlock.normalColor = new Color(155 / 255f, 121 / 255f, 121 / 255f); // faded color
        normalColorBlock.highlightedColor = new Color(155 / 255f, 121 / 255f, 121 / 255f);
        normalColorBlock.pressedColor = new Color(155 / 255f, 121 / 255f, 121 / 255f);
        normalColorBlock.selectedColor = new Color(155 / 255f, 121 / 255f, 121 / 255f);

        selectedColorBlock = Futurecitybutton.colors;
        selectedColorBlock.normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f); // normal color
        selectedColorBlock.highlightedColor = new Color(255 / 255f, 255 / 255f, 255 / 255f);
        selectedColorBlock.pressedColor = new Color(200 / 255f, 200 / 255f, 200 / 255f); // slightly darker for pressed state
        selectedColorBlock.selectedColor = new Color(255 / 255f, 255 / 255f, 255 / 255f);

        RequestMasterClientMapIndex();
        SetbuttonsOn();
    }

    private void Checkbuttons()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Futurecitybutton.interactable = false;
            Sandlandbutton.interactable = false;
            Warehousebutton.interactable = false;
            Docksbutton.interactable = false;
        }
    }

    private void SetbuttonsOn()
    {
        UpdateButtonColors();
    }

    public void SetFutureMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMap(1);
        }
    }

    public void SetSandMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMap(0);
        }
    }

    public void SetWarehouseMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMap(3);
        }
    }

    public void SetDocksMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMap(2);
        }
    }

    private void UpdateButtonColors()
    {
        Button selectedButton = null;

        switch (SpawnManager.mapIndex)
        {
            case 0:
                selectedButton = Sandlandbutton;
                break;
            case 1:
                selectedButton = Futurecitybutton;
                break;
            case 2:
                selectedButton = Docksbutton;
                break;
            case 3:
                selectedButton = Warehousebutton;
                break;
        }

        Futurecitybutton.colors = normalColorBlock;
        Sandlandbutton.colors = normalColorBlock;
        Warehousebutton.colors = normalColorBlock;
        Docksbutton.colors = normalColorBlock;

        if (selectedButton != null)
        {
            selectedButton.colors = selectedColorBlock;
        }
    }
    [PunRPC]
    void RequestMapIndex()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int currentMapIndex = SpawnManager.mapIndex;
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("RespondMapIndex", RpcTarget.Others, currentMapIndex);
        }
    }

    [PunRPC]
    void RespondMapIndex(int index)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SpawnManager.mapIndex = index;
            SetbuttonsOn();
        }
    }

    void RequestMasterClientMapIndex()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("RequestMapIndex", RpcTarget.MasterClient);
    }

    [PunRPC]
    void MapSelect(int index)
    {
        SpawnManager.mapIndex = index;
        UpdateButtonColors();
    }

    public void SetMap(int index)
    {
        gameObject.GetPhotonView().RPC("MapSelect", RpcTarget.All, index);
    }
}
