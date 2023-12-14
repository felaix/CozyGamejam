using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : SingletonWindow<TutorialWindow>
{
    [SerializeField]
    private GameObject _tutorial;

    private void Start()
    {
        bool hasSeenTutorial = LevelSelectionData.Instance.HasSeenTutorial;

        if (!hasSeenTutorial)
        {
            _tutorial.SetActive(true);
            LevelSelectionData.Instance.HasSeenTutorial = true;
        }
    }
}