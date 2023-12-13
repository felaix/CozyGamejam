using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
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

    //  level kann jederzeit beendet werden, bei 100% progress wird level automatisch abgeschlossen
    //  die erreichten punkte werden pro level zwischengespeichert, daraus resultiert ne punktzahl
    //  ne bestimmte punktzahl wird benötigt, um weitere  "level" freizuschalten
    //  man kann level wieder besuchen, um da weiterzumachen, wo man aufgehört hat
    //  buttons: start level & reset level

    //  => popup und neue levels darstellen

    private void Awake()
    {
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

        if (CanUnlockNextTier())
        {
            int nextTier = _selectionData.CurTierIndex + 1;

            List<ScriptableImage> levels = _selectionData.AvailableTiers[nextTier].ImageData;

            //  for each image data in next tier
            for (int i = 0; i < levels.Count; i++)
            {
                //  create levelData
                LevelData newData = new();
                newData.AddImage(levels[i].Image);
                newData.AddIndex(levels[i].ID);
                //  add levelData to dictionary
                _selectionData.LevelDatas.Add(newData.Index, newData);
                _selectionData.AddToLevelList(newData);
                _selectionData.CurTierIndex = nextTier;
            }
        }

        LoadLevelImages();
    }

    public bool IsFirstTier => _selectionData.CurTierIndex == -1;
    public bool ReachedUnlockScore => _totalScore >= NextTierUnlockScore;
    public bool CanUnlockNextTier() => IsFirstTier || ReachedUnlockScore;

    public int NextTierUnlockScore => _selectionData.AvailableTiers[_selectionData.NextTierIndex].UnlockScore;
    public float LevelScore(int index) => LevelData(index).Score;
    public float UpdateTotalScore(int index) => _totalScore += LevelScore(index);
    public int ActiveLevelsCount => _selectionData.ActiveLevelDatas.Count;

    public LevelData LevelData(int index) => _selectionData.ActiveLevelDatas[index];

    public void ResetTotalScore() => _totalScore = 0;
    public void CalculateTotalScore() { for (int i = 0; i < ActiveLevelsCount; i++) UpdateTotalScore(i); }

    private void LoadLevelImages()
    {
        GameObject selectionUI = Instantiate(_uiToSpawn);
        LevelSelectionWindow ui = selectionUI.GetComponent<LevelSelectionWindow>();
        ui.Levels = _selectionData.ActiveLevelDatas;
        ui.ShowWindow();
    }
}