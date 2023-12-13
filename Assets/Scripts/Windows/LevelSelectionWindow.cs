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
    private GameObject _buttonPrefab;

    [SerializeField]
    private GameObject _levelsObj;

    private Dictionary<LevelData, GameObject> _levelDisplays = new();

    public List<LevelData> Levels;

    protected override void Awake()
    {
        base.Awake();
        //  CallbackManager.Instance.OnShowLevelSelection += Show;
    }

    public override void ShowWindow()
    {
        CreateLevelDisplays();
        base.ShowWindow();
    }

    private void CreateLevelDisplays()
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            LevelData curData = Levels[i];

            GameObject obj = Instantiate(_buttonPrefab, _levelsObj.transform);
            obj.GetComponent<LevelSelectItem>().PrepareLevelDisplay(curData);
            _levelDisplays.Add(curData, obj);
        }
    }
}