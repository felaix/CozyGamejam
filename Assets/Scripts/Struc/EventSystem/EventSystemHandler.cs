using UnityEngine;
using UnityEngine.EventSystems;

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
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null) _eventSystem.SetSelectedGameObject(_lastObj);
        else _lastObj = _eventSystem.currentSelectedGameObject;
    }

    public void SetFirstButton(GameObject button) => _eventSystem.SetSelectedGameObject(button);
}