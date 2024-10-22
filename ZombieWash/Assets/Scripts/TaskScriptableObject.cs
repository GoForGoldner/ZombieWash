using UnityEngine;

[CreateAssetMenu(fileName = "TaskScript", menuName = "ScriptableObjects/TaskScriptableObject")]
public class TaskScriptableObject : ScriptableObject
{
    [SerializeField]
    public string taskName;

    [SerializeField]
    public Vector3 machineTravelLocation;

    [SerializeField]
    public int turnsToComplete;
}
