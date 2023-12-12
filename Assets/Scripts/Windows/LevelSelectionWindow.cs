using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionWindow : SingletonWindow<LevelSelectionWindow>
{
    [SerializeField]
    private GameObject _buttonPrefab;

    public GameObject Levels;

    private Dictionary<LevelData, GameObject> _levelDisplays = new();

    protected override void Awake()
    {
        base.Awake();
        CallbackManager.Instance.OnShowLevelSelection += Show;
    }

    private void Show(Dictionary<int, LevelData> levelDatas)
    {
        CreateLevelDisplays(levelDatas);
        base.ShowWindow();
    }

    private void CreateLevelDisplays(Dictionary<int, LevelData> levelDatas)
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            levelDatas.TryGetValue(i, out LevelData curData);

            GameObject obj = Instantiate(_buttonPrefab, Levels.transform);
            obj.GetComponent<LevelSelectItem>().PrepareLevelDisplay(curData);
            _levelDisplays.Add(curData, obj);
        }
    }
}