using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemHandler : Singleton<EventSystemHandler>
{
    private EventSystem _eventSystem;
    private GameObject _lastObj;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _eventSystem = EventSystem.current;
        SetFirstSelectedButton();
    }

    public void SetFirstSelectedButton()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        if (selectables[0].IsInteractable()) { _eventSystem.SetSelectedGameObject(selectables[0].gameObject); }
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null) _eventSystem.SetSelectedGameObject(_lastObj);
        else _lastObj = _eventSystem.currentSelectedGameObject;
    }

    public void SetFirstButton(GameObject button) => _eventSystem.SetSelectedGameObject(button);
}