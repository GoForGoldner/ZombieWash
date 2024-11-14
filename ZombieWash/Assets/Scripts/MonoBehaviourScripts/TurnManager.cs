using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour {
    private int _currentTurn;

    [SerializeField]
    private Hand _hand;

    [SerializeField]
    private CardDeck _deck;

    private PlayerData _playerData;

    [SerializeField] private Timer _timer;

    [SerializeField]
    private TMP_Text _turnText;

    [SerializeField] private List<Station> _stations = new List<Station>();

    [SerializeField]
    private Canvas _loseScreenCanvas;

    [SerializeField]
    private Canvas _winScreenCanvas;

    // Public Functions:
    public void EndTurn() {
        _timer.EndTimer();

        _currentTurn++;
        UpdateTurnText();

        _hand.LowerCardsHealth();

        foreach (Station station in _stations) {
            station.StationLowerTurn();
        }

        DrawCardOnTurn(_currentTurn);
        CheckForGameWin();

        _timer.StartTimer();
    }


    // Unity Related functions:
    void Start() {
        _playerData = PlayerData.Instance;
        _loseScreenCanvas.gameObject.SetActive(false);
        _winScreenCanvas.gameObject.SetActive(false);
        _currentTurn = 1;
        UpdateTurnText();

        _timer.StartTimer();
    }

    private void OnEnable() {
        EventManager.Instance.EndTurn += EndTurn;
        EventManager.Instance.LoseGame += LoseGame;
    }

    private void OnDisable() {
        EventManager.Instance.EndTurn -= EndTurn;
        EventManager.Instance.LoseGame -= LoseGame;
    }


    // Private Functions: 
    private void UpdateTurnText() {
        _turnText.text = $"Turn: {_currentTurn}";
    }

    private void LoseGame() {
        _loseScreenCanvas.gameObject.SetActive(true);
        _timer.DisableTimer();
    }

    private void WinGame() {
        _winScreenCanvas.gameObject.SetActive(true);
        _timer.DisableTimer();

        _playerData.IncreaseCurrentLevel();
        // TODO change
        _playerData.ChangeStarCount(SceneManager.GetActiveScene().buildIndex - 1, 3);
    }

    private void CheckForGameWin() {
        if (_deck.Empty() && _hand.Empty()) {
            WinGame();
        }
    }

    private void DrawCardOnTurn(int turn) {
        if (turn == 3 || turn == 5 || turn == 7) {
            _hand.DrawCardFromDeck();
        }
    }
}
