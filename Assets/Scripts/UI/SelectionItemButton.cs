using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private LevelSelectItem selectionItem;


    private void Start()
    {
        selectionItem = GetComponentInParent<LevelSelectItem>();
        EventSystemHandler.Instance.SetSelected(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selectionItem == null) return;
        selectionItem.OnDeselect(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectionItem == null) return;
        selectionItem.OnSelect(eventData);
    }
}
