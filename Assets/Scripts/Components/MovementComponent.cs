using UnityEngine;

[RequireComponent(typeof(LevelAnalyser))]
public class MovementComponent : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] protected float movementSpeed = 0.04f;
    protected bool isRight = true;

    [Header("Jump Settings")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float lowJumpMultiplier;
    protected bool isGrounded = false;
    protected bool holdingJump = false;

    [Header("Grounded Settings")]
    [SerializeField] protected float distGroundFromCenter = 0.03f;
    [SerializeField] protected float groundCheckRadious = 0.05f;

    [SerializeField] protected LayerMask levelMask;
    protected enum RotateType { FLIP, CORNER }
    protected const float ROTATE_FROM_RIGHT = -90f;
    protected const float ROTATE_FLIP = 180F;

    protected LevelAnalyser analyser;
    protected Rigidbody rb;

    protected void Awake() {
        analyser = GetComponent<LevelAnalyser>();
        rb = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate() {
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1);
        else if (rb.velocity.y > 0 && !holdingJump)
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1);

        bool frameIsGrounded = CheckIsGrounded();
        if (frameIsGrounded && !isGrounded)
            OnReachedGround();
        isGrounded = frameIsGrounded;
    }

    public void Jump() {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }

    // Rotates this gameobject by a 'rotateAmount'.
    // If the amount if equal to the flip amount, it sets the corresponding variable.
    public void RotatePlayer(float rotateAmount) {
        if (rotateAmount == ROTATE_FLIP)
            isRight = !isRight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotateAmount, transform.eulerAngles.z);
    }

    protected bool CheckIsGrounded() {
        return Physics.CheckCapsule(transform.position, transform.position - new Vector3(0, distGroundFromCenter, 0), groundCheckRadious, levelMask) && rb.velocity.y <= 0 && rb.velocity.y >= -0.1f;
    }

    private void OnDrawGizmosSelected() {
        DebugExtension.DebugCapsule(transform.position, transform.position - new Vector3(0, distGroundFromCenter, 0), groundCheckRadious);
    }

    public bool IsGrounded() {
        return isGrounded;
    }

    public void SetHoldingJump(bool value) {
        holdingJump = value;
    }

    public bool HoldingJump() {
        return holdingJump;
    }

    protected virtual void OnReachedGround() { }
}
