using UnityEngine;

[System.Serializable]
public struct Sizing {
    public float XOffset;
    public float YOffset;
    [Range(50, 1000)] public float DeckWidth;
    [Range(0.1f, 1.0f)] public float CardSizeScale;
}
