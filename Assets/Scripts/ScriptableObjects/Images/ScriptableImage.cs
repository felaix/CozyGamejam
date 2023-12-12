using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/New Image")]
public class ScriptableImage : ScriptableObject
{
    public Sprite Image;
    public int ID;
}