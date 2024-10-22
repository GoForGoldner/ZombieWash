using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScript", menuName = "ScriptableObjects/CardScriptableObject")]
public class CardScriptableObject : ScriptableObject
{
    [SerializeField]
    [Range(1, 3)]
    public int health;
    [SerializeField]
    public List<TaskScriptableObject> tasks;
    [SerializeField]
    public Sprite zombieThumbnail;
    [SerializeField]
    public Sprite backCard;
}
