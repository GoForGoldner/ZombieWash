using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private List<Image> _stars;

    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private Sprite _fullStar;


    public int _starCount = 0;

    public void MakeInteractable() {
        _button.interactable = true;
    }

    public void Update() {
        DisplayStars();
    }

    public void DisplayStars() {
        int i = 0;
        for (; i < _stars.Count && i < _starCount; i++) {
            _stars[i].sprite = _fullStar;
        }

        for (; i < _stars.Count; i++) {
            _stars[i].sprite = _emptyStar;
        }
    }
}
