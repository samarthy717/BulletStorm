using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
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

    void Start()
    {
        heatCounter = 0f;
        UpdateSliderColor();
    }

    void Update()
    {
        muzzlecounter-=Time.deltaTime;
        if (muzzlecounter <= 0f)
        {
            muzzleflash.SetActive(false);
            muzzlecounter = 0f;
        }
        if (!overheated)
        {
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
            heatCounter -= coolRate * Time.deltaTime;
            UpdateSliderColor();
        }
        else
        {
            heatCounter -= overheatCoolRate * Time.deltaTime;
            if (heatCounter < 0)
            {
                overheated = false;
                UIController.Instance.overheatedmessage.gameObject.SetActive(false);
                //UIController.Instance.crosshair.gameObject.SetActive(true);
            }
        }
        if (heatCounter <= 0) heatCounter = 0;
    }

    void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ray.origin = Camera.main.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + (hit.normal * (.002f)), Quaternion.LookRotation(hit.normal, Vector3.up));
            Destroy(bulletImpactObject, 5f);
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
        // Calculate the ratio of heatCounter to maxHeat
        float heatRatio = heatCounter / maxHeat;

        // Interpolate color between lowHeatColor and highHeatColor based on the heatRatio
        Color lerpedColor = Color.Lerp(lowHeatColor, highHeatColor, heatRatio);

        // Update the fill color of the slider
        UIController.Instance.SliderFill.color = lerpedColor;
        UIController.Instance.slider.value = heatRatio;
    }
}
