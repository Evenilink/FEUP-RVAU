using UnityEngine;

public class ActivateGazeOnDetect : DefaultTrackableEventHandler {

    [SerializeField] private GameObject gazer;

    protected override void OnTrackingFound() {
        base.OnTrackingFound();
        if (GameMode.Instance().HasGameStarted()) {
            gazer.SetActive(true);
        }
    }

    protected override void OnTrackingLost() {
        if (GameMode.Instance().HasGameStarted()) {
            gazer.SetActive(false);
        }
        base.OnTrackingLost();
    }
}
