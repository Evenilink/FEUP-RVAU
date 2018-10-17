using UnityEngine;

public class CustomTrackableEventHandler : DefaultTrackableEventHandler {

    protected override void OnTrackingFound() {
        base.OnTrackingFound();

        // Enable rigidbodies.
        var rigidbodyComponents = GetComponentsInChildren<Rigidbody>(true);
        foreach (var component in rigidbodyComponents)
            component.useGravity = true;

        // Enable player controller.
        PlayerController playerControllerComponent = GetComponentInChildren<PlayerController>(true);
        playerControllerComponent.enabled = true;
    }

    protected override void OnTrackingLost() {
        base.OnTrackingLost();

        // Disable rigidbodies.
        var rigidbodyComponents = GetComponentsInChildren<Rigidbody>(true);
        foreach (var component in rigidbodyComponents)
            component.useGravity = false;

        // Disable player controller.
        PlayerController playerControllerComponent = GetComponentInChildren<PlayerController>(true);
        playerControllerComponent.enabled = false;
    }
}
