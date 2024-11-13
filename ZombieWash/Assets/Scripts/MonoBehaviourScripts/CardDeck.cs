using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {
    [System.Serializable]
    private struct Refrences {
        public GameObject CardPrefab;
        public DeckListScriptableObject DeckList;
        public Canvas DeckCanvas;
    }

    [SerializeField] private Sizing _sizing;
    [SerializeField] private Refrences _refrences;
    [SerializeField] [Range(0, 50)] private int _deckSize;

    private readonly List<CardScriptableObject> _cards = new List<CardScriptableObject>();
    private readonly List<GameObject> _cardObjects = new List<GameObject>();

    // Public Functions:
    public GameObject DrawTopCard() {
        if (_cardObjects.Count == 0) return null;

        GameObject topCard = _cardObjects[^1];
        _cardObjects.RemoveAt(_cardObjects.Count - 1);

        return topCard;
    }

    public bool Empty() {
        return _cardObjects.Count == 0;
    }

    // Unity Related Functions:
    void Start() {
        SpawnDeck();
        DisplayDeck();
    }

    // Private Functions:
    private void SpawnDeck() {
        _cards.Clear(); // Clear any existing cards

        // Create a new list from the original deck list to shuffle
        List<CardScriptableObject> availableCards = new List<CardScriptableObject>(_refrences.DeckList.cardScripts);

        // Shuffle the available cards
        Shuffle(availableCards);

        // Select cards from the shuffled list without duplicates
        for (int i = 0; i < Mathf.Min(_deckSize, availableCards.Count); i++) {
            _cards.Add(availableCards[i]);
        }
    }
    private void Shuffle(List<CardScriptableObject> cards) {
        for (int i = cards.Count - 1; i > 0; i--) {
            int j = Random.Range(0, i + 1);
            // Swap cards[i] with the element at random index
            CardScriptableObject temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }


    private void DisplayDeck() {
        float xOffsetTemp = _sizing.XOffset;
        float spacing = _sizing.DeckWidth / Mathf.Max(1, _cards.Count);

        foreach (CardScriptableObject cardScript in _cards) {
            GameObject newCard = CreateCard(cardScript);
            RectTransform rectTransform = newCard.GetComponent<RectTransform>();
            DisplayCard displayCard = newCard.GetComponent<DisplayCard>();

            displayCard.ChangeCardPositionAndScale(new Vector2(xOffsetTemp, _sizing.YOffset), _sizing.CardSizeScale);
            xOffsetTemp += spacing;

            _cardObjects.Add(newCard);
        }
    }

    private GameObject CreateCard(CardScriptableObject cardScript) {
        GameObject newCard = Instantiate(_refrences.CardPrefab);
        DisplayCard displayCard = newCard.GetComponent<DisplayCard>();
        displayCard.CardScript = cardScript;
        newCard.SetActive(true);
        newCard.transform.SetParent(_refrences.DeckCanvas.transform, false);

        return newCard;
    }
}
