using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelController : Singleton<LevelController>
{
    public Sprite CurImage { get; private set; }
    public List<Vector2> ReachedPoints { get; private set; } = new();


    //  --- REFERENCES ---
    private GameManager _gameManager => GameManager.Instance;
    private CallbackManager _callbacks => CallbackManager.Instance;
    private EventSystemHandler _eventHandler => EventSystemHandler.Instance;
    private LevelSelectionData selectionDatas => LevelSelectionData.Instance;


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
        CurImage = selectionDatas.CurLevelData.Image;
        ReachedPoints = selectionDatas.CurLevelData.ReachedPoints;
        Debug.Log($"START SCENE: Image = {CurImage.name}, ReachedPoints = {ReachedPoints.Count}");
    }

    //  called, when: player presses button to leave
    //                OR all points are reached
    public void FinishLevel(List<Vector2> totalPoints, List<Vector2> reachedPoints)
    {
        _callbacks.OnExitLevel?.Invoke(totalPoints, reachedPoints);
    }
}