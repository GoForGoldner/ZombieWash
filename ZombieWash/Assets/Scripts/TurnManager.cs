using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] // Remove later
    private int currentTurn;

    [SerializeField]
    private Hand hand;

    [SerializeField]
    private CardDeck deck;

    [SerializeField]
    private TMP_Text turnText;

    private List<CardStatus> cardsInStations = new List<CardStatus>();

    [SerializeField]
    private Canvas loseScreenCanvas;

    [SerializeField]
    private Canvas winScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        loseScreenCanvas.gameObject.SetActive(false);
        winScreenCanvas.gameObject.SetActive(false);
        currentTurn = 1;
        updateTurnText();
    }

    public void nextTurn()
    {
        currentTurn++;
        updateTurnText();

        lowerCardsHealth();
        lowerTurnsTimer();
        checkCardsStatus();

        switch (currentTurn)
        {
            case 7:
                hand.drawCard();
                break;
            case 5:
                hand.drawCard();
                break;
            case 3:
                hand.drawCard();
                break;
            default:
                break;
        }

        checkGameWin();
    }

    void updateTurnText()
    {
        turnText.text = "Turn: " + currentTurn;
    }

    private void lowerCardsHealth()
    {
        foreach (GameObject card in hand.cardObjects)
        {
            if (card.GetComponent<DisplayCard>().health == 0) loseGame();
            card.GetComponent<DisplayCard>().health--;
        }
    }

    private void lowerTurnsTimer()
    {
        foreach (var cardStatus in cardsInStations)
        {
            cardStatus.station.updateTurnsLeft(--cardStatus.remainingTurns);
        }
    }

    private void checkCardsStatus()
    {
        for (int i = cardsInStations.Count - 1; i >= 0; i--)
        {
            if (cardsInStations[i].remainingTurns <= 0)
            {
                HandleCardCompletion(cardsInStations[i].card, cardsInStations[i].station);
                cardsInStations.RemoveAt(i);
            }
        }
    }

    private void HandleCardCompletion(GameObject card, Station station)
    {
        DisplayCard displayCard = card.GetComponent<DisplayCard>();

        if (displayCard.taskList.Count == 0)
        {
            Destroy(card);
        } else
        {
            hand.cardObjects.Add(card);
            displayCard.health = displayCard.cardScript.health;
            card.transform.localScale = new Vector3(1f, 1f, 1f) * deck.cardSizeScale;
            hand.displayHand();
            displayCard.onStation = false;
            displayCard.updateTaskText();
        }

        station.cardLeavesStation();
    }

    public void AddCardToStation(GameObject card, Station station, int turns)
    {
        cardsInStations.Add(new CardStatus(card, station, turns));
    }

    private void loseGame()
    {
        loseScreenCanvas.gameObject.SetActive(true);
    }

    private void winGame()
    {
        winScreenCanvas.gameObject.SetActive(true);
    }

    private void checkGameWin()
    {
        if (deck.cardObjects.Count + hand.cardObjects.Count + cardsInStations.Count == 0)
        {
            winGame();
        }
    }
}
