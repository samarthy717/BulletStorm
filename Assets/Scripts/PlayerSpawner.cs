using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;
    public float RespawnTime = 5f;

    public float Maxhealth = 100f;
    public float CurrentHealth;

    public Slider HealthSlider;
    public Image HealthFillImage;

    public Color FullHealthColor = Color.green;
    public Color HalfHealthColor = Color.yellow;
    public Color LowHealthColor = Color.red;

    void Awake()
    {
        instance = this;
    }

    public GameObject playerprefab;
    public GameObject deatheffect;
    private GameObject Player;

    void Start()
    {
        CurrentHealth = Maxhealth;
        UpdateHealthUI();
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }
    private void Update()
    {
        UpdateHealthUI();
    }
    void SpawnPlayer()
    {
        Transform PlayerPos = SpawnManager.instance.GetRandomSpawnPoint();
        Player = PhotonNetwork.Instantiate(playerprefab.name, PlayerPos.position, PlayerPos.rotation);
    }

    public void PlayerDamaged(string damager, float dmg,int actor)
    {
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0)
        {
            Die(damager);
            MatchManager.instance.UpdatePlayersStatsSend(actor, 0, 1);
        }
        else
        {
            UpdateHealthUI();
        }
    }

    public void Die(string damager)
    {
        StartCoroutine(Death(damager));
    }

    IEnumerator Death(string damager)
    {
        RespawnCanvas.instance.respawcanvas.SetActive(true);
        RespawnCanvas.instance.respawntext.text = "YOU WERE KILLED BY " + damager;
        PhotonNetwork.Instantiate(deatheffect.name, Player.transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(Player);
        MatchManager.instance.UpdatePlayersStatsSend(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
        yield return new WaitForSeconds(RespawnTime);
        RespawnCanvas.instance.respawcanvas.SetActive(false);
        CurrentHealth = Maxhealth;
        SpawnPlayer();
    }

    void UpdateHealthUI()
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = Maxhealth;
            HealthSlider.value = CurrentHealth;

            float healthPercentage = CurrentHealth / Maxhealth;

            if (healthPercentage > 0.5f)
            {
                HealthFillImage.color = Color.Lerp(HalfHealthColor, FullHealthColor, (healthPercentage - 0.5f) * 2f);
            }
            else
            {
                HealthFillImage.color = Color.Lerp(LowHealthColor, HalfHealthColor, healthPercentage * 2f);
            }
        }
    }
}
