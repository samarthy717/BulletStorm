using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public TMP_Text overheatedmessage;
    public Slider slider;
    public Image SliderFill;
    public TMP_Text killedmsg;
    private void Awake()
    {
        Instance = this; 
        if(overheatedmessage != null ) overheatedmessage.gameObject.SetActive(false);
        if(killedmsg!=null) killedmsg.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
