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

    private void Show(List<LevelData> levels)
    {
        CreateLevelDisplays(levels);
        base.ShowWindow();
    }

    private void CreateLevelDisplays(List<LevelData> levels)
    {
        //  change input to:
        //  only levelDatas (but from all available tiers, as long as i < curTier/ or nextTier
        for (int i = 0; i < levels.Count; i++)
        {
            LevelData curData = levels[i];
            
            GameObject obj = Instantiate(_buttonPrefab, Levels.transform);
            obj.GetComponent<LevelSelectItem>().PrepareLevelDisplay(curData);
            _levelDisplays.Add(curData, obj);
        }
    }
}