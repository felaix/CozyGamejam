using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    //  --- REFERENCES ---
    private CallbackManager _callbacks => CallbackManager.Instance;
    private GameManager _gameManager => GameManager.Instance;

    [SerializeField]
    private GameObject _selectionObj;
    private LevelSelectionData _selectionData;
    
    private Dictionary<int, LevelData> _levelDatas = new();

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

    private void Start()    { LoadLevelSelection(); }


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
        }else
        {
            _selectionData = LevelSelectionData.Instance;
        }
    }



    //  --- SCRIPT METHODS ---
    private void LoadLevelSelection()
    {
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
            }
        }

        LoadLevelImages();
    }

    private bool CanUnlockNextTier()
    {
        Debug.LogError("ERROR - need to fix");
        if (_selectionData.CurTierIndex == -1) return true;
        else if (GetActiveLevelScore() >= GetNextTierScore()) return true;
        return false;
    }

    private int GetNextTierScore()
    {
        int nextTier = _selectionData.CurTierIndex++;
        return _selectionData.AvailableTiers[nextTier].UnlockScore;
    }

    private int GetActiveLevelScore()
    {
        int activeLevelScore = 0;

        for (int i = 0; i < _selectionData.ActiveLevelDatas.Count; i++)
        {
            activeLevelScore += (int)_selectionData.ActiveLevelDatas[i].Score;
        }

        return activeLevelScore;
    }

    private void LoadLevelImages()
    {
        _callbacks.OnShowLevelSelection?.Invoke(_selectionData.ActiveLevelDatas);
        //  compare total points with current unlocked and not unlocked tiers
        //  when new tiers are unlocked, show pop up
        //  either or not, display active levels
    }
}