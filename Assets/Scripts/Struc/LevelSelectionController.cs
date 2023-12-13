using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelSelectionController : Singleton<LevelSelectionController>
{
    //  --- REFERENCES ---
    private CallbackManager _callbacks => CallbackManager.Instance;
    private GameManager _gameManager => GameManager.Instance;
    [SerializeField]
    private GameObject _uiToSpawn;

    [SerializeField]
    private GameObject _selectionObj;
    private LevelSelectionData _selectionData;


    private float _totalScore;

    public void UpdatePage(bool next) { if (next) ActivePage++; else ActivePage--; }
    public int ActivePage { get; private set; } = 1;
    public int LastPage => _selectionData.ActiveLevelDatas.Count / 3;
    public bool IsFirstPage => ActivePage == 1;
    public bool IsLastPage => ActivePage == LastPage;
    public bool HasLessPages => ActivePage > 1;
    public bool HasMorePages => LastPage > ActivePage;

    protected override void Awake()
    {
        base.Awake();
        _callbacks.OnLevelReset += ResetLevel;
        NullCheck();
    }

    private void ResetLevel(LevelData data)
    {
        Debug.Log("RESET LEVEL");
        data.Score = 0;
    }

    private void Start() { LoadLevelSelection(); }


    //  --- PREPARATION ---
    private void NullCheck()
    {
        if (_callbacks == null) CreateCallbackManager();
        if (_gameManager == null) CreateGameManager();
        if (_selectionData == null) CreateSelectionData();
    }

    private void CreateCallbackManager()
    {
        GameObject obj = Instantiate(new GameObject("CallbackManager"));
        obj.AddComponent<CallbackManager>();
    }
    private void CreateGameManager()
    {
        GameObject obj = Instantiate(new GameObject("GameManager"));
        obj.AddComponent<GameManager>();
        obj.GetComponent<GameManager>().StartWithMenu = false;
    }
    private void CreateSelectionData()
    {
        if (LevelSelectionData.Instance == null)
        {
            GameObject obj = Instantiate(_selectionObj);
            _selectionData = obj.GetComponent<LevelSelectionData>();
        }
        else
        {
            _selectionData = LevelSelectionData.Instance;
        }
    }

    //  --- SCRIPT METHODS ---
    private void LoadLevelSelection()
    {
        ResetTotalScore();
        CalculateTotalScore();

        if (CanUnlockNextTier()) PrepareNewLevels();
        LoadLevelImages();
    }

    public void PrepareNewLevels()
    {
        Levels = _selectionData.AvailableTiers[_selectionData.NextTierIndex].ImageData;
        for (int i = 0; i < Levels.Count; i++) UpdateDataReferences(LevelData(Levels[i]));
        _selectionData.CurTierIndex = _selectionData.NextTierIndex;
    }

    private List<ScriptableImage> Levels;
    private LevelData LevelData(ScriptableImage img) => GetCreatedLevelData(img);
    private LevelData GetCreatedLevelData(ScriptableImage img)
    {
        LevelData newData = new();
        newData.AddImageToCode(img.ImageToCode);
        newData.AddImageToDisplay(img.ImageToDisplay);
        newData.AddIndex(img.ID);
        return newData;
    }

    private void UpdateDataReferences(LevelData newData)
    {
        _selectionData.LevelDatas.Add(newData.Index, newData);
        _selectionData.AddToLevelList(newData);
    }

    public void LoadLevelImages()
    {
        GameObject selectionUI = Instantiate(_uiToSpawn);
        LevelSelectionWindow ui = selectionUI.GetComponent<LevelSelectionWindow>();

        ui.Levels = GetPageLevels();
        ui.ShowWindow();
    }

    public List<LevelData> GetPageLevels()
    {
        List<LevelData> toReturn = new();
        int first = 0;
        int last = 0;

        if (ActivePage == 1) { first = 0; last = 2; }
        else if (ActivePage == 2) { first = 3; last = 5; }
        else if (ActivePage == 3) { first = 6; last = 8; }

        for (int i = first; i <= last; i++)
        {
            toReturn.Add(_selectionData.ActiveLevelDatas[i]);
        }
        return toReturn;
    }

    public bool IsFirstTier => _selectionData.CurTierIndex == -1;
    public bool ReachedUnlockScore => _selectionData.TotalScore >= NextTierUnlockScore();
    public bool CanUnlockNextTier()
    {
        return IsFirstTier || ReachedUnlockScore;
    }

    public int NextTierUnlockScore()
    {
        return _selectionData.AvailableTiers[_selectionData.NextTierIndex].UnlockScore;
    }
    public int ActiveLevelsCount => _selectionData.ActiveLevelDatas.Count;

    public float LevelScore(int index) => LevelData(index).Score;
    public float UpdateTotalScore(int index) => _selectionData.TotalScore += LevelScore(index);

    public LevelData LevelData(int index) => _selectionData.ActiveLevelDatas[index];

    public void ResetTotalScore() => _selectionData.TotalScore = 0;
    public void CalculateTotalScore() { for (int i = 0; i < ActiveLevelsCount; i++) UpdateTotalScore(i); }
}