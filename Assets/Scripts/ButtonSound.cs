using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler
{

    public string hover;
    public string clicked;
    public string selected;

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(clicked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(hover);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlaySFX(selected);
    }
}
