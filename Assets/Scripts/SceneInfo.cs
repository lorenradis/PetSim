using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneInfo : ScriptableObject
{
    public string sceneName;
    public List<Vector2> entrances = new List<Vector2>();
    public Vector2 minBounds;
    public Vector2 maxBounds;
}
