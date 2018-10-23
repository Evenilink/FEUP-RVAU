using UnityEngine;
using Vuforia;

public class PlayerController : MonoBehaviour {

    [Header("Components")]
    private PlayerMovementComponent movComp;

    [Header("Defaults")]
    private CheckpointInfo checkpointInfo;
    public struct CheckpointInfo {
        public Vector3 position;
        public Quaternion rotation;
    };

    // public GameObject leftBtn;
    // public GameObject rightBtn;
    private float hInput = 0f;

    private void Start() {
        movComp = GetComponent<PlayerMovementComponent>();
        // If player dies before reaching any checkpoint, it respawns at the start of the level.
        SetCheckpointInfo(transform.position, transform.rotation);
        // leftBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        // rightBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    private void Update() {
        // Movement.
        float hInput = Input.GetAxis("Horizontal");
        movComp.Move(hInput);

        // Jump.
        if (Input.GetButtonDown("Jump") && movComp.IsGrounded())
            movComp.Jump();
        if (Input.GetButton("Jump")) {
            if (!movComp.HoldingJump())
                movComp.SetHoldingJump(true);
        }
        else if (movComp.HoldingJump())
            movComp.SetHoldingJump(false);
    }

    public void SetCheckpointInfo(Vector3 position, Quaternion rotation) {
        checkpointInfo.position = position;
        checkpointInfo.rotation = rotation;
    }

    public CheckpointInfo GetCheckpointInfo() {
        return checkpointInfo;
    }

    public void Respawn() {
        transform.position = checkpointInfo.position;
        transform.rotation = checkpointInfo.rotation;
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

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy") {
            if (collision.gameObject.tag == "Bullet")
                Destroy(collision.gameObject);
            Respawn();
        }
    }

    /*public void OnButtonPressed(VirtualButtonBehaviour vb) {
        Debug.Log("Button " + vb.VirtualButtonName + " pressed!.");
        if (vb.VirtualButtonName.Contains("Left")) {
            hInput = -1f;
        } else if (vb.VirtualButtonName.Contains("Right")) {
            hInput = 1f;
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        Debug.Log("Button " + vb.VirtualButtonName + " released!.");
        hInput = 0;
    }*/
}
