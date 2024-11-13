using System;
using UnityEngine;

public class CardDroppedEventArgs : EventArgs {
    public GameObject Card { get; }
    public Station Station { get; }

    public CardDroppedEventArgs(GameObject card, Station station) {
        Card = card;
        Station = station;
    }
}