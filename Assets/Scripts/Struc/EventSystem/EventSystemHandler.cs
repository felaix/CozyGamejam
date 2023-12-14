using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventSystemHandler : Singleton<EventSystemHandler>
{
    private EventSystem _eventSystem;
    private GameObject _lastObj;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        //SceneManager.activeSceneChanged += LoadNavigation;
    }

    private void LoadNavigation(Scene oldScene, Scene newScene)
    {
        Invoke(nameof(SetFirstSelectedButton), .01f);
    }

    private void Start()
    {
        _eventSystem = EventSystem.current;
        SetFirstSelectedButton();
    }

    public void SetFirstSelectedButton()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        if (selectables[0].IsInteractable()) { SetSelected(selectables[0].gameObject); }
        else { Debug.Log("is not interactable btn"); }

        Debug.Log("set first selected to: " + selectables[0]);
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null) _eventSystem.SetSelectedGameObject(_lastObj);
        else _lastObj = _eventSystem.currentSelectedGameObject;
    }

    public void SetSelected(GameObject selection) => _eventSystem.SetSelectedGameObject(selection);
}