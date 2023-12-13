using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionWindow : SingletonWindow<LevelSelectionWindow>
{
    [SerializeField]
    private Button _debugBtn;
    [SerializeField]
    private Button _scoreBtn;
    [SerializeField]
    private Button _nextPageBtn;
    [SerializeField]
    private Button _lastPageBtn;
    [SerializeField]
    private Button _quitBtn;

    [SerializeField]
    private GameObject _buttonPrefab;
    [SerializeField]
    private GameObject _levelsObj;

    private Dictionary<LevelData, GameObject> _levelDisplays = new();
    public List<LevelData> Levels;

    private LevelSelectionController _selections;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ShowWindow()
    {
        _selections = LevelSelectionController.Instance;
        SetButtons();
        CreateLevelDisplays();
        //  DebugButton();
        base.ShowWindow();
    }

    private void CreateLevelDisplays()
    {
        if (_levelDisplays.Count > 0) _levelDisplays.Clear();
        else
        {
            foreach (LevelData data in Levels)
            {
                GameObject obj = Instantiate(_buttonPrefab, _levelsObj.transform);
                obj.GetComponent<LevelSelectItem>().PrepareLevelDisplay(data);
                _levelDisplays.Add(data, obj);
            }
        }
    }

    private void SetButtons()
    {
        _nextPageBtn.onClick.RemoveAllListeners();
        _nextPageBtn.onClick.AddListener(() =>
        {
            if (CanShowNextPage) ChangePage(true);
        });

        _lastPageBtn.onClick.RemoveAllListeners();
        _lastPageBtn.onClick.AddListener(() =>
        {
            if (CanShowLastPage) ChangePage(false);
        });

        _quitBtn.onClick.RemoveAllListeners();
        _quitBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.QuitGame();
        });
    }

    private bool CanShowLastPage => !_selections.IsFirstPage && _selections.HasLessPages;
    private bool CanShowNextPage => !_selections.IsLastPage && _selections.HasMorePages;

    private void ChangePage(bool showNext)
    {
        _selections.UpdatePage(showNext);
        _selections.LoadLevelImages();
    }

    private void DebugButton()
    {
        _debugBtn.onClick.RemoveAllListeners();
        _debugBtn.onClick.AddListener(() =>
        {
            _selections.PrepareNewLevels();
        });
        _scoreBtn.onClick.RemoveAllListeners();
        _scoreBtn.onClick.AddListener(() =>
        {
            LevelSelectionData.Instance.TotalScore += 200;
        });
    }
}