using UnityEngine;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler {

    [SerializeField] private GameObject sceneRoot;

    protected override void OnTrackingFound() {
        base.OnTrackingFound();
        sceneRoot.SetActive(true);
    }

    protected override void OnTrackingLost() {
        sceneRoot.SetActive(false);
        base.OnTrackingLost();
    }
}
