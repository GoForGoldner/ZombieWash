using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {
    public static EventManager Instance { get; private set; }

    public event Action LoseGame;
    public event Action EndTurn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void OnLoseGame() {
        LoseGame?.Invoke();
    }

    public void OnEndTurn() {
        EndTurn?.Invoke();
    }

    public void ToMenu() {
        SceneManager.LoadScene(0);
    }
}
