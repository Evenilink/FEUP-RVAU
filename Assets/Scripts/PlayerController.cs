using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 1f;

    [Header("Raycast")]
    [SerializeField] private Transform leftRaycast;
    [SerializeField] private Transform rightRaycast;

    private enum CubeSide {
        FRONT,
        BACK,
        LEFT,
        RIGHT
    }
    private CubeSide currSide = CubeSide.FRONT;

    private void Start() {

    }

    protected void Update() {
        if (!GetComponent<MeshRenderer>().enabled)
            return;

        Debug.DrawRay(transform.position, -transform.up * 10f, Color.red);
        if (!Physics.Raycast(leftRaycast.position, transform.forward, 10f)) {
            switch(currSide) {
                case CubeSide.FRONT:
                    currSide = CubeSide.RIGHT;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90f);
                    break;
            }
        }

        HandleMovement();
    }

    private void HandleMovement() {
        float hInput = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = transform.right * hInput;
        transform.position += horizontalMovement * movementSpeed * Time.deltaTime;
    }
}
