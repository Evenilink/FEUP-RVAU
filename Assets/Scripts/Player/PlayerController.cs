using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 0.04f;
    [SerializeField] private bool startFacingRight = true;
    private bool isRight = true;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    private bool jumpPressed = false;
    private bool isGrounded = false;

    [Header("Components")]
    private LevelAnalyser analyser;
    private Rigidbody rb;

    [Header("Defaults")]
    [SerializeField] private LayerMask levelMask;
    private enum RotateType { FLIP, CORNER }
    private const float ROTATE_FROM_RIGHT = -90f;
    private const float ROTATE_FLIP = 180F;
    private float lastHeight;
    private struct CheckpointInfo {
        public Vector3 position;
        public Quaternion rotation;
    };
    private CheckpointInfo checkpointInfo;

    private void Start() {
        analyser = GetComponent<LevelAnalyser>();
        rb = GetComponent<Rigidbody>();
        lastHeight = transform.position.y;
        if (!startFacingRight)
            RotatePlayer(ROTATE_FLIP);
        // If player dies before reaching any checkpoint, it respawns at the start of the level.
        SetCheckpointInfo(transform.position, transform.rotation);
    }

    private void Update() {
        HandleMovement();

        if (Input.GetButtonDown("Jump") && isGrounded)
            jumpPressed = true;

        if (Input.GetKeyDown(KeyCode.Q))
            Respawn();

    }

    private void FixedUpdate() {
        HandleIsGrounded();
        HandleJump();
    }

    private void HandleMovement() {
        float hInput = Input.GetAxis("Horizontal");
        if (hInput < 0 && isRight || hInput > 0 && !isRight)
            RotatePlayer(ROTATE_FLIP);

        if (hInput != 0) {
            bool obstacleDetected, canKeepMoving;
            analyser.CheckObstacle(out obstacleDetected, out canKeepMoving);
            if (canKeepMoving) {
                // No need to calculate if there's an abism if we know there's an obstacle in front of the player.
                if (!obstacleDetected && analyser.CheckAbism())
                    RotatePlayer(isRight ? ROTATE_FROM_RIGHT : -ROTATE_FROM_RIGHT);
                // Since we're rotating the character when he flips (instead of applying a negative scale) we're only interested in the absolute value of the input.
                Vector3 horizontalMovement = transform.right * Mathf.Abs(hInput);
                transform.position += horizontalMovement * movementSpeed * Time.deltaTime;
            }
        }
    }

    // Rotates this gameobject by a 'rotateAmount'.
    // If the amount if equal to the flip amount, it sets the corresponding variable.
    private void RotatePlayer(float rotateAmount) {
        if (rotateAmount == ROTATE_FLIP)
            isRight = !isRight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotateAmount, transform.eulerAngles.z);
    }

    private void HandleJump() {
        if (jumpPressed) {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            jumpPressed = false;
        }

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1);
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1);
    }

    private void HandleIsGrounded() {
        bool currIsGrounded = Physics.CheckCapsule(transform.position, transform.position - new Vector3(0, 0.03f, 0), 0.05f, levelMask);
        if (currIsGrounded && !isGrounded) {
            if (transform.position.y > lastHeight) {
                LevelScrollingManager.Instance().ScrollToHeight(transform.position.y - lastHeight);
                lastHeight = transform.position.y;
                Debug.Log("Before: " + checkpointInfo.position);
                SetCheckpointInfo(checkpointInfo.position - new Vector3(0, transform.position.y - lastHeight, 0), checkpointInfo.rotation);
                Debug.Log("After: " + checkpointInfo.position);
            }
        }
        isGrounded = currIsGrounded;
    }

    private void SetCheckpointInfo(Vector3 position, Quaternion rotation) {
        checkpointInfo.position = position;
        checkpointInfo.rotation = rotation;
    }

    private void Respawn() {
        transform.position = checkpointInfo.position;
        transform.rotation = checkpointInfo.rotation;
        isRight = true;
    }

    private void OnDrawGizmosSelected() {
        DebugExtension.DebugCapsule(transform.position, transform.position - new Vector3(0, 0.03f, 0), 0.05f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Checkpoint") {
            SetCheckpointInfo(other.gameObject.transform.position, other.gameObject.transform.rotation);
            Destroy(other.gameObject);
        }
    }
}
