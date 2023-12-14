using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }
}
