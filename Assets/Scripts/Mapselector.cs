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

        Checkbuttons();
        UpdateButtonColors(Sandlandbutton);
    }
    private void Update()
    {
        
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
        if (SpawnManager.mapIndex == 0)
        {
            UpdateButtonColors(Sandlandbutton);
        }
        else if (SpawnManager.mapIndex == 1)
        {
            UpdateButtonColors(Futurecitybutton);
        }
        else if (SpawnManager.mapIndex == 2)
        {
            UpdateButtonColors(Docksbutton);
        }
        else
        {
            UpdateButtonColors(Warehousebutton);
        }
    }
    public void SetFutureMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.mapIndex = 1;
            UpdateButtonColors(Futurecitybutton);
        }
    }

    public void SetSandMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.mapIndex = 0;
            UpdateButtonColors(Sandlandbutton);
        }
    }

    public void SetWarehouseMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.mapIndex = 3;
            UpdateButtonColors(Warehousebutton);
        }
    }

    public void SetDocksMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnManager.mapIndex = 2;
            UpdateButtonColors(Docksbutton);
        }
    }

    private void UpdateButtonColors(Button selectedButton)
    {
        Futurecitybutton.colors = normalColorBlock;
        Sandlandbutton.colors = normalColorBlock;
        Warehousebutton.colors = normalColorBlock;
        Docksbutton.colors = normalColorBlock;

        if (selectedButton != null)
        {
            selectedButton.colors = selectedColorBlock;
        }
    }
}
