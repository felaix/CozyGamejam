using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
    public GameObject ImageObj;

    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _resetBtn;

    public void SetButtons(LevelData curData)
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadLevel(curData);
        });

        _resetBtn.onClick.RemoveAllListeners();
        _resetBtn.onClick.AddListener(() =>
        {
            CallbackManager.Instance.OnLevelReset?.Invoke(curData);
        });
    }
}