using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using Unity.VisualScripting;
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
        HandleLevelDisplays();
        //  DebugButton();
        base.ShowWindow();
    }

    private void HandleLevelDisplays()
    {
        DestroyLevelDisplays();
        CreateLevelDisplays();
    }

    private void CreateLevelDisplays()
    {
        foreach (LevelData data in Levels)
        {
            GameObject obj = Instantiate(_buttonPrefab, _levelsObj.transform);
            obj.GetComponent<LevelSelectItem>().PrepareLevelDisplay(data);
            _levelDisplays.Add(data, obj);
        }
        //SetNavigation();
    }

    private void DestroyLevelDisplays()
    {
        foreach (KeyValuePair<LevelData, GameObject> pair in _levelDisplays)
        {
            Destroy(pair.Value);
        }

        _levelDisplays.Clear();
    }

    private void SetNavigation()
    {
        for (int i = 0; i < 2; i++)
        {
            Navigation navi = _levelsObj.transform.GetChild(i).GetChild(0).GetComponent<Button>().navigation;
            navi.mode = Navigation.Mode.Explicit;

            if (i == 3) { navi.selectOnRight = _levelsObj.transform.GetChild(0).GetChild(0).GetComponent<Button>(); return; }

            navi.selectOnRight = _levelsObj.transform.GetChild(i + 1).GetChild(0).GetComponent<Button>();
            if (i != 0) navi.selectOnLeft = _levelsObj.transform.GetChild(i -1).GetChild(0).GetComponent <Button>();
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

        //EventSystemHandler.Instance.SetSelected(_nextPageBtn.gameObject);
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