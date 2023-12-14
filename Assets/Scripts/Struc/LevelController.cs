using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelController : Singleton<LevelController>
{
    public Sprite ImageToCode { get; private set; }
    public Sprite ImageToDisplay { get; private set; }
    public float Accuracy { get; private set; }

    //  --- REFERENCES ---
    private GameManager _gameManager => GameManager.Instance;
    private CallbackManager _callbacks => CallbackManager.Instance;
    private EventSystemHandler _eventHandler => EventSystemHandler.Instance;
    private LevelSelectionData _selectionDatas => LevelSelectionData.Instance;


    //  --- VARIABLES ---
    [SerializeField]
    private GameObject _eventObj;

    protected override void Awake()
    {
        base.Awake();
        NullCheck();
        DEBUGStartScene();
    }

    //  --- PREPARATION ---
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

    //  --- SCRIPT METHODS ---

    //  called in each script, that needs to be loaded at levelStart
    private void DEBUGStartScene()
    {
        if (_selectionDatas.CurLevelData.ImageToCode == null) Debug.LogError($"TO FIX: level data has no image");
        ImageToCode = _selectionDatas.CurLevelData.ImageToCode;
        ImageToDisplay = _selectionDatas.CurLevelData.ImageToDisplay;
        Accuracy = _selectionDatas.CurLevelData.Score;
    }

    //  called, when: player presses button to leave
    //                OR all points are reached
    public void FinishLevel(List<Transform> totalPoints, float score)
    {
        _callbacks.OnExitLevel?.Invoke(totalPoints, score);
    }
}