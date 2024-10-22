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
}
