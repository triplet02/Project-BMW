using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject characterTooltip;
    [SerializeField] GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.tag.ToString());
        Debug.Log(eventData.pointerEnter.tag.Equals("SettingButton"));
        if (!eventData.pointerEnter.tag.Equals("SettingButton"))
        {
            characterTooltip.SetActive(true);
        } 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.pointerEnter.tag.Equals("SettingButton"))
        {
            characterTooltip.SetActive(false);
        }
    }

    public void ShowSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }
}
