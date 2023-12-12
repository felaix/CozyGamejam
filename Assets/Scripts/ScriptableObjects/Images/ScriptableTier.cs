using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/New Tier")]
public class ScriptableTier : ScriptableObject
{
    public List<ScriptableImage> ImageData = new();
    public int UnlockScore;
}