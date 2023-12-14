using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundUI : MonoBehaviour, IPointerUpHandler
{

    [SerializeField] private GameObject fg;

    public void OnPointerUp(PointerEventData eventData)
    {
        ToggleFG();
    }

    public void ToggleFG()
    {
        fg.SetActive(true);
    }
}
