using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station : MonoBehaviour {

    [SerializeField] private TMP_Text _turnsLeftText;
    [field: SerializeField] public TaskScriptableObject StationTaskSript { get; private set; }
    [SerializeField] private Hand _hand;
    [SerializeField] private Movement _player;
    [SerializeField] private Vector3 _playerTravelLocation;
    [SerializeField] [Range(0f, 0.5f)] private float _sizeOfCard;


    private bool IsCardDroppedOn = false;

    private CardStatus _cardInStation = null;

    // Public Functions:
    public bool AddCardToStation(GameObject card) {
        if (!CardCanEnterStation(card)) return false;

        DisplayCard displayCard = card.GetComponent<DisplayCard>();
        displayCard.ChangeCardPositionAndScale(transform.position, _sizeOfCard);
        displayCard.CardStats.ResetHealth();
        
        _cardInStation = new CardStatus(card, StationTaskSript.TurnsToComplete);

        _player.SetDestination(_playerTravelLocation);
        _hand.RemoveCardFromHand(card);
        return true;
    }

    public void StationLowerTurn() {
        if (_cardInStation == null) return;

        UpdateTurnsLeft(_cardInStation.DecrementTurn());
        if (_cardInStation.NoMoreTurnsLeft()) {
            RemoveCard();
        }

        // Helper functions
        void UpdateTurnsLeft(int turnsLeft) {
            _turnsLeftText.text = turnsLeft.ToString();
        }

        void RemoveCard() {
            List<TaskScriptableObject> tasks = _cardInStation.card.GetComponent<DisplayCard>()?.CardStats.Tasks;
            if (tasks == null || tasks.Count == 0) {
                Destroy(_cardInStation.card);
            }
            else {
                _hand.AddCardToHand(_cardInStation.card);
                _cardInStation.card.GetComponent<DisplayCard>().CardLeftStation();
            }
            _cardInStation = null;
        }
    }
    
    // Unity Related Functions:
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Card")) {
            IsCardDroppedOn = true;
        }
    }


    // Private Functions:
    private bool CardCanEnterStation(GameObject card) {
        return IsCardDroppedOn && _cardInStation == null && CardMatchesTasks(card);

        bool CardMatchesTasks(GameObject card) {
            List<TaskScriptableObject> tasks = card.GetComponent<DisplayCard>().CardStats.Tasks;
            if (tasks == null) return false;

            foreach (TaskScriptableObject task in tasks) {
                if (task.TaskName == StationTaskSript.TaskName) {
                    tasks.Remove(task);
                    return true;
                }

            }

            return false;
        }
    }
}
