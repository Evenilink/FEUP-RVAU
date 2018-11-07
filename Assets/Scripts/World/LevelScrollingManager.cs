using System.Collections;
using UnityEngine;

public class LevelScrollingManager : MonoBehaviour {

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private float interpolationTime = 1f;
    private static LevelScrollingManager instance;
    private float ratio;

    private void Awake() {
        // Singleton.
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        PlayerMovementComponent.OnHitGround += ScrollHeight;
        ratio = playerTransform.localPosition.y + rootTransform.position.y;
    }

    public void Restart() {
        rootTransform.position = Vector3.zero;
    }

    // Method invoked by the object that calls the singleton instance, in order to scroll the level.
    // It scrolls 'height' units.
    public void ScrollHeight(float height) {
        StopAllCoroutines();
        StartCoroutine(ScrollHeightCoroutine(-(height - ratio)));
    }

    // Coroutine to spherically interpolate the level.
    private IEnumerator ScrollHeightCoroutine(float height) {
        float startTime = Time.time;
        float fracComplete = 0f;
        Vector3 finalPosition = new Vector3(rootTransform.position.x, height, rootTransform.position.z);
        while (fracComplete < 1) {
            fracComplete = (Time.time - startTime) / interpolationTime;
            rootTransform.position = Vector3.Lerp(rootTransform.position, finalPosition, fracComplete);
            yield return null;
        }
    }

    // Coroutine to spherically interpolate the level.
    private IEnumerator cenas() {
        float startTime = Time.time;
        float fracComplete = 0f;
        Vector3 finalPosition = new Vector3(rootTransform.position.x, rootTransform.position.y, rootTransform.position.z);
        while (fracComplete < 1) {
            fracComplete = (Time.time - startTime) / interpolationTime;
            rootTransform.position = Vector3.Lerp(rootTransform.position, finalPosition, fracComplete);
            yield return null;
        }
    }

    public static LevelScrollingManager Instance() {
        return instance;
    }

    private void OnDestroy() {
        PlayerMovementComponent.OnHitGround -= ScrollHeight;
    }
}
