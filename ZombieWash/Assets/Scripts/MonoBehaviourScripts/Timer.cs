using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool _timerEnabled = true;
    [SerializeField] private int _timerLength;
    [SerializeField] private TMP_Text _text;

    private Coroutine _timerCoroutine;
    private bool _isTimerRunning;
    private IEnumerator TimerCoroutine() {
        for (int i = _timerLength; i > 0; i--) {
            UpdateText(i);
            yield return new WaitForSecondsRealtime(1.0f);
        }

        EventManager.Instance.OnEndTurn();
    }

    public void StartTimer() {
        if (_timerEnabled && !_isTimerRunning) {
            _isTimerRunning = true;
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    public void EndTimer() {
        if (_timerCoroutine != null) {
            StopCoroutine(_timerCoroutine);
            _isTimerRunning = false;
        }
    }

    public void DisableTimer() {
        _timerEnabled = false;
    }

    private void UpdateText(int time) {
        _text.text = time.ToString();
    } 
}
