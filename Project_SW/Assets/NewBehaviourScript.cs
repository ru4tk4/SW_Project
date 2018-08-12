using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : EventTrigger
{
    Text DescriptionUI;
    GameMaster GM;
    private void Start()
    {
        DescriptionUI = GameObject.Find("DescriptionUI").GetComponent<Text>();
        GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointerDown called." + data.pointerPress.name);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        //data.pointerEnter.GetComponent<ButtonEvent>().SetButton();
        DescriptionUI.text = GM.Descriptions[int.Parse(data.pointerEnter.name)];
        DescriptionUI.gameObject.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData data)

    {
        
        DescriptionUI.gameObject.SetActive(false);
    }
}
