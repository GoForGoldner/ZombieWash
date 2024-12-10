using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public CardScriptableObject CardScript;

    public CardStats CardStats;

    private bool _cardFaceUp = false;

    [SerializeField]
    private TMP_Text _taskText;

    [SerializeField]
    private Image _frontSide;

    [SerializeField]
    private Image _backSide;

    [SerializeField]
    private Image _thumbnail;

    [SerializeField]
    private Image _healthBar;

    private const float _dropRadius = 80f;
    private Vector2 _dragOffset;
    private bool _onStation = false;
    private bool _isDragging = false;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    // Public Functions:
    public void TurnCardFaceUp() {
        _backSide.gameObject.SetActive(false);
        
        _cardFaceUp = true;
    }

    public void ChangeCardPositionAndScale(Vector3 newPosition, float scale) {
        _onStation = true;

        _rectTransform.position = newPosition;
        transform.localScale = Vector3.one * scale;

        _originalPosition = transform.localPosition;
    }

    public void ChangeCardPositionAndScale(Vector2 newPosition, float scale) {
        // Change Position
        _rectTransform.anchoredPosition = newPosition;
        _originalPosition = _rectTransform.anchoredPosition;

        // Change Scale
        transform.localScale = Vector3.one * scale; 
    }

    // Unity Related Functions:
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
        CardStats = new CardStats(CardScript);

        _thumbnail.sprite = CardScript.ZombieThumbnail;
        _backSide.sprite = CardScript.BackCard;
    }

    void Update() {
        UpdateTaskText();
        UpdateHealthBar();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            _isDragging = true;
            // Calculate the offset between the mouse position and the card position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _dragOffset);
            _dragOffset = _rectTransform.anchoredPosition - _dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            _isDragging = false;

            CheckDrop();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (_isDragging && _cardFaceUp && !_onStation) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

            // Update the card's anchored position based on the mouse position and the stored offset
            _rectTransform.anchoredPosition = localPoint + _dragOffset;
        }
    }

    public void CardLeftStation() {
        _onStation = false;
    }

    // Private Functions:
    private void UpdateTaskText() {
        // Creates the information for what tasks need to be done on a card
        string taskInfo = "Tasks:\n";
        foreach (TaskScriptableObject task in CardStats.Tasks) {
            taskInfo += "- " + task.name + " (" + task.TurnsToComplete + " turns)" + "\n";
        }

        _taskText.text = taskInfo;
    }

    private void UpdateHealthBar() {

        switch (CardStats.Health) {
            case 3:
                _healthBar.color = new Color(32f / 255f, 221f / 255f, 26f / 255f); // Green
                break;
            case 2:
                _healthBar.color = new Color(255f / 255f, 255f / 255f, 0f / 255f); // Yellow
                break;
            case 1:
                _healthBar.color = new Color(229f / 255f, 105f / 255f, 14f / 255f); // Orange
                break;
            default:
                _healthBar.color = new Color(229f / 255f, 0f / 255f, 0f / 255f); // Red
                break;
        }
    }

    private void CheckDrop() {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_rectTransform.position, _dropRadius);

        foreach (Collider2D hitCollider in hitColliders) {
            if (TryDropCard(hitCollider, out Station station)) return; // Exit early if the card was dropped successfully
        }

        // Return to original position if not placed on target
        _rectTransform.anchoredPosition = _originalPosition;
    }

    private bool TryDropCard(Collider2D hitCollider, out Station station) {
        station = null;

        if (!hitCollider.CompareTag("Station")) return false;

        station = hitCollider.GetComponent<Station>();

        if (station.AddCardToStation(gameObject)) {
            EventManager.Instance.OnEndTurn();
            return true;
        }

        return false;
    }
}
