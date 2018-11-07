using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Components")]
    private PlayerMovementComponent movComp;
    private Rigidbody rb;

    [Header("Defaults")]
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Animator anim;

    public delegate void PlayerDie();
    public static PlayerDie OnPlayerDie;

    private void Awake() {
        startPosition = transform.localPosition;
        startRotation = transform.rotation;
        movComp = GetComponent<PlayerMovementComponent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        print(startPosition);
        // Movement.
        float hInput = Input.GetAxis("Horizontal");
        movComp.Move(hInput);
        anim.SetFloat("hInput", Mathf.Abs(hInput));

        // Jump.
        if (Input.GetButtonDown("Jump") && movComp.IsGrounded())
            movComp.Jump();
        if (Input.GetButton("Jump")) {
            if (!movComp.HoldingJump())
                movComp.SetHoldingJump(true);
        }
        else if (movComp.HoldingJump())
            movComp.SetHoldingJump(false);
        anim.SetBool("jump", !movComp.IsGrounded());

        if (Input.GetKeyDown(KeyCode.Q))
            GameMode.Instance().Restart();
    }

    public void Respawn() {
        rb.velocity = Vector3.zero;
        transform.localPosition = startPosition;
        transform.rotation = startRotation;
        movComp.SetIsRight(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "YKillZone")
            GameMode.Instance().Restart();
        else if (other.gameObject.tag == "Enemy") {
            BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
                enemy.Die();
        }
        else if (other.gameObject.tag == "Bullet")
            Destroy(other.gameObject);
        else if (other.gameObject.tag == "LevelRoot") {
            WorldGenerator.Instance().CreateNewLevel();
            Destroy(other.gameObject.GetComponent<BoxCollider>());
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy")
            GameMode.Instance().Restart();
    }
}
