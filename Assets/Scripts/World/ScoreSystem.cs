using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

    [SerializeField] private float quadHeight = 0.95f;
    [SerializeField] private Text scoreText;
    [SerializeField] private Transform playerTransform;

    private static ScoreSystem instance;
    private int score = 0;
    private float lastHeight;
    private float startHeight;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Update() {
        
    }

    private void Start() {
        //lastHeight = playerTransform.localPosition.y;
        startHeight = playerTransform.localPosition.y;
        PlayerMovementComponent.OnHitGround += CalculateScore;
    }

    private void CalculateScore(float newHeight) {
        if (newHeight > lastHeight + quadHeight)
            SetNewScore(newHeight, score + 1);
    }

    public void Restart() {
        SetNewScore(playerTransform.localPosition.y, 0);
    }

    private void SetNewScore(float newHeight, int newScore) {
        lastHeight = newHeight;
        score = newScore;
        scoreText.text = score.ToString();
    }

    public static ScoreSystem Instance() {
        return instance;
    }

    private void OnDestroy() {
        PlayerMovementComponent.OnHitGround -= CalculateScore;
    }
}
