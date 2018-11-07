using UnityEngine;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController pc;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private Canvas mainMenu;

    private static GameMode instance;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        WorldGenerator.Instance().GenerateStart();
        pc.enabled = false;
        if (gameStarted) StartGame();
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
        mainMenu.gameObject.SetActive(false);
        pc.enabled = true;
        pc.transform.rotation = Quaternion.Euler(0, 180, 0);
        gameStarted = true;
    }

    public void EndGame() {
        mainMenu.gameObject.SetActive(true);
        pc.enabled = false;
        gameStarted = false;
    }
}
