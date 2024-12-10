using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {
    public static PlayerData Instance { get; private set; }

    [SerializeField] private int _numOfLevels;
    public int CurrentLevel;

    public List<LevelData> _levelData = new();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < _numOfLevels; i++) {
            _levelData.Add(new LevelData(i + 1, 0));
        }
    }

    public void ChangeStarCount(int levelNumber, int newStarCount) {
        for (int i = 0; i < _levelData.Count; i++) {
            if (_levelData[i].LevelNumber == levelNumber) {
                _levelData[i].Stars = newStarCount;
                return; // Optional, stop searching once found
            }
        }
    }
    public void IncreaseCurrentLevel() {
        if (SceneManager.GetActiveScene().buildIndex - 1 == CurrentLevel) CurrentLevel++;
    }
}
