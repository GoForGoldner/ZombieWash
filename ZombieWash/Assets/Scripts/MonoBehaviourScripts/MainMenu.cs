using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _rulesScreen;
    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit() {
        Application.Quit();
        Debug.Log("Player has quit!");
    }
    public void Back() {
        _rulesScreen.SetActive(false);
    }

    public void Rules() {
        _rulesScreen.SetActive(true);
    }
}
