using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : SingletonWindow<TutorialWindow>
{
    [SerializeField]
    private GameObject _tutorial;

    [SerializeField]
    private Button _resetBtn;

    private LevelSelectionData _levelData;

    private void Start()
    {
        _levelData = LevelSelectionData.Instance;
        bool hasSeenTutorial = _levelData.HasSeenTutorial;

        if (!hasSeenTutorial)
        {
            _tutorial.SetActive(true);
            LevelSelectionData.Instance.HasSeenTutorial = true;
        }

        _resetBtn.onClick.RemoveAllListeners();
        _resetBtn.onClick.AddListener(() =>
        {
            CallbackManager.Instance.OnLevelReset?.Invoke(_levelData.CurrentLevelData);
        });
    }
}