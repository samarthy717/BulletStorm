using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviourPunCallbacks
{
    public GameObject bulletImpact;
    public float TimebwShots = .1f;
    private float shotcounter;

    public float maxHeat = 10f, heatPerShot = 1f, coolRate = 4f, overheatCoolRate = 5f;
    private float heatCounter;
    private bool overheated;

    public Color lowHeatColor = Color.green;
    public Color highHeatColor = Color.red;

    public GameObject muzzleflash;
    public float muzzledisplaytime = 1 / 60;
    private float muzzlecounter;

    public float GunDamage = 10f;
    public GameObject playerimpact;
    FirstPersonController fps;

    void Start()
    {
        heatCounter = 0f;
        UpdateSliderColor();
        fps=FindObjectOfType<FirstPersonController>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            muzzlecounter -= Time.deltaTime;
            if (muzzlecounter <= 0f)
            {
                muzzleflash.SetActive(false);
                muzzlecounter = 0f;
            }
            if (!overheated)
            {
                // if (photonView.IsMine)
                // {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
                if (Input.GetMouseButton(0))
                {
                    shotcounter -= Time.deltaTime;
                    if (shotcounter <= 0)
                    {
                        Shoot();
                    }
                }
                // }
                heatCounter -= coolRate * Time.deltaTime;
                UpdateSliderColor();
            }
            else
            {
                heatCounter -= overheatCoolRate * Time.deltaTime;
                if (heatCounter <= 0)
                {
                    overheated = false;
                    UIController.Instance.overheatedmessage.gameObject.SetActive(false);
                    //UIController.Instance.crosshair.gameObject.SetActive(true);
                }
            }
            if (heatCounter <= 0) heatCounter = 0;
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ray.origin = Camera.main.transform.position;

        if (gameObject.tag == "pistol")
        {
            FindObjectOfType<AudioManagerr>().Play("pistol");
        }
        if (gameObject.tag == "machinegun")
        {
            FindObjectOfType<AudioManagerr>().Play("machinegun");
        }
        if (gameObject.tag == "sniper")
        {
            FindObjectOfType<AudioManagerr>().Play("sniper");
        }


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
                //Debug.Log("playerhit"+hit.collider.gameObject.GetPhotonView().Owner.NickName);
                FindObjectOfType<AudioManagerr>().Play("impact");
                PhotonNetwork.Instantiate(playerimpact.name, hit.point, Quaternion.identity);
                if (fps != null)
                {
                    fps.TakeDamage(hit, GunDamage);
                   /* if (PhotonNetwork.LocalPlayer.ActorNumber == attackeractor)
                    {
                        UIController.Instance.killedmsg.gameObject.SetActive(true);
                        UIController.Instance.killedmsg.text = "YOU KILLED " + damer;
                        StartCoroutine(dissappearmsg());
                    }*/
                }

                //hit.collider.gameObject.GetPhotonView().RPC("DealDamage",RpcTarget.All,photonView.Owner.NickName);
            }
            else
            {
                GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + (hit.normal * (.002f)), Quaternion.LookRotation(hit.normal, Vector3.up));
                Destroy(bulletImpactObject, 5f);
            }
        }
        shotcounter = TimebwShots;

        heatCounter += heatPerShot;
        if (heatCounter >= maxHeat)
        {
            heatCounter = maxHeat;
            overheated = true;
            UIController.Instance.overheatedmessage.gameObject.SetActive(true);
            //UIController.Instance.crosshair.gameObject.SetActive(false);
        }

        UpdateSliderColor();
        muzzleflash.SetActive(true);
        muzzlecounter = muzzledisplaytime;
    }
    void UpdateSliderColor()
    {
        if (UIController.Instance != null)
        {
            float heatRatio = heatCounter / maxHeat;

            Color lerpedColor = Color.Lerp(lowHeatColor, highHeatColor, heatRatio);

            UIController.Instance.SliderFill.color = lerpedColor;
            UIController.Instance.slider.value = heatRatio;
        }
    }
    IEnumerator dissappearmsg()
    {
        yield return new WaitForSeconds(2f);
        UIController.Instance.killedmsg.gameObject.SetActive(false);
    }
}
