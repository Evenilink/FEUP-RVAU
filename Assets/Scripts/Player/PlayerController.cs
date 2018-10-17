using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 0.04f;
    private bool isRight = true;

    [Header("Raycast")]
    [SerializeField] private LayerMask levelMask;
    [SerializeField] private float raycastThreshold = 0.03f;

    [Header("Constants")]
    private const float ROTATE_FROM_RIGHT = -90f;
    private const float ROTATE_FLIP = 180F;

    private enum RotateType { FLIP, CORNER }

    private void Update() {
        HandleCorners();
        HandleMovement();
    }

    private void HandleMovement() {
        float hInput = Input.GetAxis("Horizontal");
        if (hInput < 0 && isRight || hInput > 0 && !isRight)
            RotatePlayer(ROTATE_FLIP);
        // Since we're rotating the character when he flips (instead of applying a negative scale) we're only interested in the absolute value of the input.
        Vector3 horizontalMovement = transform.right * Mathf.Abs(hInput);
        transform.position += horizontalMovement * movementSpeed * Time.deltaTime;
    }
    
    private void RotatePlayer(float rotateAmount) {
        if (rotateAmount == ROTATE_FLIP)
            isRight = !isRight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotateAmount);
    }

    private void HandleCorners() {
        Debug.DrawLine(transform.position + transform.right * raycastThreshold, transform.position + transform.right * raycastThreshold - Vector3.up * 10f, Color.red);
        if (!Physics.Raycast(transform.position + transform.right * raycastThreshold, -Vector3.up, 10f, levelMask))
            RotatePlayer(isRight ? ROTATE_FROM_RIGHT : -ROTATE_FROM_RIGHT);
    }

    /*private void HandleCorners() {
        Transform leftLocalTransform;
        if (isRight)
            leftLocalTransform = leftRaycast;
        else leftLocalTransform = rightRaycast;
        if (CheckCorner(transform))
            return;

        //Transform rightLocalTransform = (leftLocalTransform == leftRaycast ? rightRaycast : leftRaycast);
        //CheckCorner(rightLocalTransform);
    }

    private bool CheckCorner(Transform raycast) {
        Vector3 rayDir = -transform.up;
        Color color = (raycast == leftRaycast ? Color.red : Color.blue);

        Debug.DrawRay(raycast.position, -transform.up * raycastLength, raycast == leftRaycast ? Color.red : Color.blue);
        if (cornerTransition && Physics.Raycast(raycast.position, -transform.up, raycastLength)) {
            cornerTransition = false;
            return true;
        }
        else if (!cornerTransition && !Physics.Raycast(raycast.position, -transform.up, raycastLength)) {
            cornerTransition = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90f);
            return true;
        }
        return false;
    }*/
}
