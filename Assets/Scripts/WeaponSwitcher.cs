using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviourPunCallbacks
{
    public int selectedWeapon = 0;
    FirstPersonController fps;
    private void Awake()
    {
        fps=FindObjectOfType<FirstPersonController>();
    }

    void Start()
    {
        if (fps != null)
        {
            fps.WeaponSelect(selectedWeapon);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                if (fps!=null)
                {
                fps.WeaponSelect(selectedWeapon);                    
                }
            }
        }
    }
    
}
