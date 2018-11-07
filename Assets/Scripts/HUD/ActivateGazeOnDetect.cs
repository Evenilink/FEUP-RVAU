using UnityEngine;

public class ActivateGazeOnDetect : MonoBehaviour {

    [SerializeField] private GameObject gazer;

    private void OnEnable() {
        gazer.SetActive(true);
    }

    private void OnDisable() {
        gazer.SetActive(false);
    }
}
