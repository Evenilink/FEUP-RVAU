using UnityEngine;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController pc;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject gazer;
    [SerializeField] private AudioClip startGameSound;
    [SerializeField] private AudioClip endGameSound;

    private AudioSource audioSource;

    private static GameMode instance;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        WorldGenerator.Instance().GenerateStart();
        if (gameStarted)
            StartGame();
        else EndGame();
    }

    public static GameMode Instance() {
        return instance;
    }

    public void Restart() {
        LevelScrollingManager.Instance().Restart();
        WorldGenerator.Instance().GenerateStart();
        ScoreSystem.Instance().Restart();
        pc.Respawn();
        audioSource.PlayOneShot(endGameSound);
        EndGame();
    }

    public void StartGame() {
        pc.enabled = true;
        gameStarted = true;
        pc.transform.rotation = Quaternion.Euler(0, 180, 0);
        mainMenu.gameObject.SetActive(false);
        inGameMenu.SetActive(true);
        inventory.SetActive(true);
        gazer.SetActive(false);
        audioSource.PlayOneShot(startGameSound);
    }

    public void EndGame() {
        pc.enabled = false;
        gameStarted = false;
        inGameMenu.SetActive(false);
        inventory.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        gazer.SetActive(true);        
    }

    public bool HasGameStarted() {
        return gameStarted;
    }
}
