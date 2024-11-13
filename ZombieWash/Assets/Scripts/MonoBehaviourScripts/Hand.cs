using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Hand : MonoBehaviour {
    [System.Serializable]
    private struct Refrences {
        public Canvas HandCanvas;
        public CardDeck CardDeck;
    }

    [SerializeField] private Sizing _sizing;
    [SerializeField] private Refrences _refrences;
    [SerializeField] [Range(1, 7)] private int _startingHandSize;

    private readonly List<GameObject> _cardObjects = new();

    // Public Functions:
    public void DrawCardFromDeck() {
        AddCardToHand(_refrences.CardDeck.DrawTopCard());
    }

    public void AddCardToHand(GameObject card) {
        if (card == null) return;

        SetCardSize(card);
        _cardObjects.Add(card);

        card.GetComponent<DisplayCard>().TurnCardFaceUp();

        DisplayHand();

        void SetCardSize(GameObject card) {
            card.transform.localScale = Vector3.one * _sizing.CardSizeScale;
        }
    }

    public void RemoveCardFromHand(GameObject card) {
        _cardObjects.Remove(card);
    }

    public void LowerCardsHealth() {
        foreach (GameObject card in _cardObjects) {
            CardStats cardStats = card.GetComponent<DisplayCard>().CardStats;
            if (cardStats.CardDied()) EventManager.Instance.OnLoseGame();
            cardStats.LowerHealth();
        }
    }

    public bool Empty() {
        return _cardObjects.Count == 0;
    }

    // Unity Related Functions:
    void Start() {
        for (int i = 0; i < _startingHandSize; i++) {
            DrawCardFromDeck();
        }
    }

    // Private Functions:
    private void DisplayHand() {
        float xOffsetTemp = _sizing.XOffset;
        float spacing = _sizing.DeckWidth / _cardObjects.Count;

        foreach (GameObject cardObject in _cardObjects) {
            // Set the parent to the Canvas
            cardObject.transform.SetParent(_refrences.HandCanvas.transform, false);

            // Adjust position based on offsets
            if (!cardObject.TryGetComponent<RectTransform>(out var rectTransform)) continue;

            cardObject.GetComponent<DisplayCard>().ChangeCardPositionAndScale(new Vector2(xOffsetTemp, _sizing.YOffset), _sizing.CardSizeScale);
            //rectTransform.anchoredPosition = new Vector2(xOffsetTemp, _yOffset);
            //cardObject.GetComponent<DisplayCard>().OriginalPosition = rectTransform.anchoredPosition;

            xOffsetTemp += spacing; // Increment xOffsetTemp for the next card
        }
    }
}
