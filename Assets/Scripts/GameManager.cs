using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gameOverPanel;

    private bool _gameEnded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        Time.timeScale = 0f;

        _startPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
    }
    
    public void Play()
    {
        _startPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

           
    }
    
}


