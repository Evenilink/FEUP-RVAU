using System.Collections;
using UnityEngine;

public class LevelScrollingManager : MonoBehaviour {

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private float interpolationTime = 1f;
    private static LevelScrollingManager instance;
    private Vector3 startPosition;
    private float startHeight;
    private float lastHeight;

    private void Awake() {
        // Singleton.
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        startHeight = playerTransform.localPosition.y;
        lastHeight = startHeight;
        startPosition = transform.position;
    }

    public void ResetHeight() {
        lastHeight = startHeight;
        rootTransform.position = startPosition;
    }

    // Method invoked by the object that calls the singleton instance, in order to scroll the level.
    // It scrolls 'height' units.
    public void ScrollHeight(float height) {
        // height is always negative, since we want to scroll down, so we turn it positive to accumulate in the last height.
        lastHeight -= height;
        StopAllCoroutines();
        StartCoroutine(ScrollHeightCoroutine(height));
    }

    // Coroutine to spherically interpolate the level.
    private IEnumerator ScrollHeightCoroutine(float height) {
        Debug.Log("Scrolling: " + height);
        float startTime = Time.time;
        float fracComplete = 0f;
        Vector3 finalPosition = new Vector3(rootTransform.position.x, rootTransform.position.y + height, rootTransform.position.z);
        while (fracComplete < 1) {
            fracComplete = (Time.time - startTime) / interpolationTime;
            rootTransform.position = Vector3.Lerp(rootTransform.position, finalPosition, fracComplete);
            yield return null;
        }
    }

    public static LevelScrollingManager Instance() {
        return instance;
    }

    public float GetLastHeight() {
        return lastHeight;
    }
}
