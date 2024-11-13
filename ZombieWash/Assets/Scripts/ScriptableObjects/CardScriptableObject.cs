using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScript", menuName = "ScriptableObjects/CardScriptableObject")]
public class CardScriptableObject : ScriptableObject {
    [Range(1, 3)] public int Health;
    [field: SerializeField] public List<TaskScriptableObject> Tasks { get; private set; }
    [field: SerializeField] public Sprite ZombieThumbnail { get; private set; }
    [field: SerializeField] public Sprite BackCard { get; private set; }
}
