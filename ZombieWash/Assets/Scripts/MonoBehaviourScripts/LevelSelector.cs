using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject levelsParent;
    private List<LevelButton> _levels = new();

    private PlayerData _playerData;

    public void Back() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void LoadLevel(int level) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
    }

    public void Awake() {
        Debug.Log("Setting Player Data.");
        _playerData = PlayerData.Instance;
    }

    public void Start() {
        _levels.AddRange(levelsParent.GetComponentsInChildren<LevelButton>());
    }

    public void Update() {
        DisplayStars();
        UnlockOpenLevels();
    }

    public void UnlockOpenLevels() {
        for (int i = 0; i < _levels.Count && i < _playerData.CurrentLevel; i++) {
            _levels[i].MakeInteractable();
        }
    }

    public void DisplayStars() {
        for (int i = 0; i < _playerData.CurrentLevel && i < _levels.Count; i++) {
            LevelData levelData = _playerData._levelData[i];
            _levels[i]._starCount = levelData.Stars;
            _levels[i].DisplayStars();
        }
    }
}
