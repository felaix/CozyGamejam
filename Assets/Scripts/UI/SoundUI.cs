using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundUI : MonoBehaviour, IPointerUpHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    [SerializeField] private GameObject fg;
    [SerializeField] private GameObject masterObj;

    private bool toggled = true;

    private void OnEnable()
    {
        if (masterObj == null) return;
        EventSystemHandler.Instance.SetFirstButton(masterObj);
    }

    private void OnDisable()
    {
        EventSystemHandler.Instance.SetFirstSelectedButton();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ToggleFG();
    }


    public void ToggleFG()
    {
        toggled = !toggled;
        fg.SetActive(toggled);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleFG();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ToggleFG();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ToggleFG();
    }
}
