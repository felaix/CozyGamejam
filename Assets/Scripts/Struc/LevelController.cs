using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelController : MonoBehaviour
{
    //  --- REFERENCES ---
    private GameManager _gameManager => GameManager.Instance;
    private CallbackManager _callbacks => CallbackManager.Instance;
    private EventSystemHandler _eventHandler => EventSystemHandler.Instance;

    //  --- VARIABLES ---
    [SerializeField]
    private GameObject _eventObj;

    private void Awake()
    {
        NullCheck();
        _callbacks.OnLoadLevel += TestDebug;
    }

    private void TestDebug()
    {
        Debug.Log("Start Game, _callbacks.OnStartLevel?.Invoke();");
    }

    private void NullCheck()
    {
        if (_callbacks == null) CreateCallbackManager();
        if (_eventHandler == null) CreateEventHandler();
        if (_gameManager == null) CreateGameManager();
    }

    private void CreateCallbackManager()
    {
        GameObject obj = Instantiate(new GameObject("CallbackManager"));
        obj.AddComponent<CallbackManager>();
    }

    private void CreateEventHandler()
    {
        Instantiate(_eventObj);
    }

    private void CreateGameManager()
    {
        GameObject obj = Instantiate(new GameObject("GameManager"));
        obj.AddComponent<GameManager>();
        obj.GetComponent<GameManager>().StartWithMenu = false;
    }
}