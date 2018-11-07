using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShooterShell : BaseEnemy {

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    // Time it takes to fire a bullet.
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private AudioClip fireClip;
    private float shootTime = 0f;

    [Header("Jump")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float minJumpRate = 1f;
    [SerializeField] private float maxJumpRate = 3f;
    [SerializeField] private AudioClip jumpStartClip;
    private float jumpTime;

    [Header("Components")]
    private MovementComponent movComp;
    private AudioSource audioSource;

    private void Awake() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        base.Awake();
        movComp = GetComponent<MovementComponent>();
        audioSource = GetComponent<AudioSource>();
        shootTime = fireRate;
        jumpTime = Random.Range(minJumpRate, maxJumpRate);
    }

    private void Update() {
        shootTime -= Time.deltaTime;
        if (shootTime <= 0)
            Fire();

        if (canJump) {
            jumpTime -= Time.deltaTime;
            if (jumpTime <= 0) {
                if (movComp.IsGrounded()) {
                    movComp.Jump();
                    if (!isWaitingDestroy)
                        audioSource.PlayOneShot(jumpStartClip, 3);
                }
                jumpTime = Random.Range(minJumpRate, maxJumpRate);
            }
        }
    }

    private void Fire() {
        Instantiate(bullet, bulletSpawnPoint.position, transform.rotation, transform.parent);
        shootTime = fireRate;
        if (!isWaitingDestroy)
            audioSource.PlayOneShot(fireClip);
    }
}
