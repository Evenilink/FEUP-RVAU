using UnityEngine;

public class GameMode : MonoBehaviour {

    [SerializeField] private PlayerController pc;
    private static GameMode instance;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        WorldGenerator.Instance().GenerateStart();
    }

    public static GameMode Instance() {
        return instance;
    }

    public void Restart() {
        LevelScrollingManager.Instance().Restart();
        WorldGenerator.Instance().GenerateStart();
        ScoreSystem.Instance().Restart();
        pc.Respawn();
    }
}
