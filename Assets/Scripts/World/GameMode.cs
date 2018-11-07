using UnityEngine;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController pc;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private GameObject inGameMenuImageTarget;
    [SerializeField] private GameObject inventoryImageTarget;
    [SerializeField] private GameObject gazer;

    private static GameMode instance;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
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
        EndGame();
    }

    public void StartGame() {
        pc.enabled = true;
        pc.transform.rotation = Quaternion.Euler(0, 180, 0);
        gameStarted = true;
        mainMenu.gameObject.SetActive(false);
        inventoryImageTarget.SetActive(true);
        inGameMenuImageTarget.SetActive(true);
        gazer.SetActive(false);
    }

    public void EndGame() {
        pc.enabled = false;
        gameStarted = false;
        inGameMenuImageTarget.SetActive(false);
        inventoryImageTarget.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        gazer.SetActive(true);
    }

    public bool HasGameStarted() {
        return gameStarted;
    }
}
