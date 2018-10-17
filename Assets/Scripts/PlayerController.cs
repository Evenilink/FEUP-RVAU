using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float movementSpeed = 1f;

    private void Start() {

    }

    protected void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        float hInput = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = transform.right * hInput;
        transform.position += horizontalMovement * movementSpeed * Time.deltaTime;
    }
}
