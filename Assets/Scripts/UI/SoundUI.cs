using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundUI : MonoBehaviour, IPointerUpHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    [SerializeField] private GameObject schein;
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject newImg;
    [SerializeField] private GameObject masterObj;

    private bool toggled = true;

    private void OnEnable()
    {
        if (masterObj == null) return;
        EventSystemHandler.Instance.SetSelected(masterObj);
    }

    private void OnDisable()
    {
        EventSystemHandler.Instance.SetFirstSelectedButton();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //ToggleFG();
    }


    public void ToggleFG()
    {
        toggled = !toggled;
        //fg.SetActive(toggled);
    }

    public void Toggle()
    {
        toggled = !toggled;

        if (toggled)
        {
            schein.SetActive(true);
            bg.SetActive(true);
            newImg.SetActive(false);
        }else
        {
            schein.SetActive(false);
            bg.SetActive(false);
            newImg.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ToggleFG();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Toggle();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Toggle();
    }
}
