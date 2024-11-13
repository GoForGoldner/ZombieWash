using UnityEngine;

[System.Serializable]
public class CardStatus
{
    public GameObject card;
    public Station station;
    public int remainingTurns;

    public CardStatus(GameObject card, Station station, int turns)
    {
        this.card = card;
        this.station = station;
        this.remainingTurns = turns;
    }

    public CardStatus(GameObject card, int turns) {
        this.card = card;
        this.station = null;
        this.remainingTurns = turns;
    }

    public int DecrementTurn() {
        return --remainingTurns;
    }

    public bool NoMoreTurnsLeft() {
        return remainingTurns == 0;
    } 
}
