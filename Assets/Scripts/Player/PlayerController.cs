using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Components")]
    private PlayerMovementComponent movComp;
    private Rigidbody rb;

    [Header("Defaults")]
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Animator anim;

    [Header("Powers")]
    [SerializeField] private float speedIncreasePercentage = 0.2f;
    [SerializeField] private float speedDuration = 5f;
    [SerializeField] private float speedCooldown = 18f;
    private float currSpeedDuration = 0f;
    private float currSpeedCooldown = 0f;
    private bool speedEnabled = false;
    [SerializeField] private float jumpIncreasePercentage = 0.2f;
    [SerializeField] private float jumpDuration = 6f;
    [SerializeField] private float jumpCooldown = 20f;
    private float currJumpDuration = 0f;
    private float currJumpCooldown = 0f;
    private bool jumpEnabled = false;

    public delegate void PlayerDie();
    public static PlayerDie OnPlayerDie;
    public delegate void SpeedPower(float newValue);
    public static SpeedPower OnSpeedPowerValueChange;
    public delegate void JumpPower(float newValue);
    public static JumpPower OnJumpPowerValueChange;

    private void Awake() {
        startPosition = transform.localPosition;
        startRotation = transform.rotation;
        movComp = GetComponent<PlayerMovementComponent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
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

        HandlePowers();
    }

    private void HandlePowers() {
        // Jump Power.
        if (Input.GetButtonDown("Power Jump") && !jumpEnabled && currJumpCooldown <= 0) {
            print("Jump activated.");
            jumpEnabled = true;
            currJumpCooldown = jumpCooldown;
            currJumpDuration = 0f;
            movComp.SetJumpPowerPercentage(jumpIncreasePercentage);
        }
        if (jumpEnabled) {
            currJumpDuration += Time.deltaTime;
            if (currJumpDuration >= jumpDuration) {
                jumpEnabled = false;
                print("Jump deactivated.");
                movComp.SetJumpPowerPercentage(0);
            }
        }
        else currJumpCooldown -= Time.deltaTime;
        if (currJumpCooldown >= 0 && OnJumpPowerValueChange != null)
            OnJumpPowerValueChange((jumpCooldown - currJumpCooldown) / jumpCooldown);

        // Speed Power.
        if (Input.GetButtonDown("Power Speed") && !speedEnabled && currSpeedCooldown <= 0) {
            print("Speed activated.");
            speedEnabled = true;
            currSpeedCooldown = speedCooldown;
            currSpeedDuration = 0f;
            movComp.SetspeedPowerPercentage(speedIncreasePercentage);
        }
        if (speedEnabled) {
            currSpeedDuration += Time.deltaTime;
            if (currSpeedDuration >= speedDuration) {
                speedEnabled = false;
                print("Speed deactivated.");
                movComp.SetspeedPowerPercentage(0);
            }
        }
        else currSpeedCooldown -= Time.deltaTime;
        if (currSpeedCooldown >= 0 && OnSpeedPowerValueChange != null)
            OnSpeedPowerValueChange((speedCooldown - currSpeedCooldown) / speedCooldown);
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
