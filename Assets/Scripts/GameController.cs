using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController _gameController;
    public GameObject _gameoverGUI;
    public bool _gameOver = false;

    private void Awake()
    {
        if (_gameController == null)
        {
            _gameController = this;
            Time.timeScale = 1f;
        }
        else if (this != _gameController)
        {
            Destroy(this);
        }

        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        _gameOver = true;
        Time.timeScale = 0f;
        _gameoverGUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
