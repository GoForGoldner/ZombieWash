using UnityEngine;

[CreateAssetMenu(fileName = "TaskScript", menuName = "ScriptableObjects/TaskScriptableObject")]
public class TaskScriptableObject : ScriptableObject {
    [field: SerializeField] public string TaskName { get; private set; }
    [field: SerializeField] public int TurnsToComplete { get; private set; }
}
