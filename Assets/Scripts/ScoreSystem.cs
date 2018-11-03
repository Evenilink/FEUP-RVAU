using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

    [SerializeField] private float quadHeight = 0.35f;
    [SerializeField] private Text scoreText;
    [SerializeField] private Transform playerTransform;

    private static ScoreSystem instance;
    private int score = 0;
    private float lastHeight;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        lastHeight = playerTransform.localPosition.y;
        PlayerMovementComponent.OnHitGround += CalculateScore;
        PlayerController.OnPlayerDie += OnPlayerDieHandler;
    }

    private void CalculateScore(float newHeight) {
        if (newHeight > lastHeight + quadHeight)
            SetNewScore(newHeight, score + 1);
    }

    private void OnPlayerDieHandler() {
        SetNewScore(playerTransform.localPosition.y, 0);
    }

    private void SetNewScore(float newHeight, int newScore) {
        lastHeight = newHeight;
        score = newScore;
        scoreText.text = score.ToString();
    }
}
