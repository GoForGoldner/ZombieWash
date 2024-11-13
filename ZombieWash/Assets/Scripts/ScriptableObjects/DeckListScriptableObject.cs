using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckListScript", menuName = "ScriptableObjects/DeckListScriptableObject")]
public class DeckListScriptableObject : ScriptableObject
{

    public List<CardScriptableObject> cardScripts = new List<CardScriptableObject>();
}
