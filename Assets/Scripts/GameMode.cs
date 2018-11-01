using UnityEngine;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController pc;
    private static GameMode instance;
    private enum GameState {
        FIND_TRACKABLE,
        PRESS_START_GAME,
        PLAYING
    }
    private GameState state;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;

        state = GameState.FIND_TRACKABLE;
    }

    private void Start() {
        WorldGenerator.Instance().GenerateStart();
    }

    public static GameMode Instance() {
        return instance;
    }

    public void Restart() {
        LevelScrollingManager.Instance().ResetHeight();
        WorldGenerator.Instance().GenerateStart();
        pc.Respawn();
    }

    private void ChangeState(GameState newState) {
        switch (newState) {
            case GameState.FIND_TRACKABLE:
                break;
            case GameState.PRESS_START_GAME:
                break;
            case GameState.PLAYING:
                break;
            default: break;
        }
    }
}
