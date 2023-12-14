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

    [SerializeField]
    private Button _confirmBtn;

    private LevelSelectionData _levelData;

    private void Start()
    {
        _levelData = LevelSelectionData.Instance;
        bool hasSeenTutorial = _levelData.HasSeenTutorial;

        if (!hasSeenTutorial)
        {
            _tutorial.SetActive(true);
            LevelSelectionData.Instance.HasSeenTutorial = true;
            EventSystemHandler.Instance.SetSelected(_confirmBtn.gameObject);
        }

        _resetBtn.onClick.RemoveAllListeners();
        _resetBtn.onClick.AddListener(() =>
        {
            CallbackManager.Instance.OnLevelReset?.Invoke(_levelData.CurrentLevelData);
        });
    }
}