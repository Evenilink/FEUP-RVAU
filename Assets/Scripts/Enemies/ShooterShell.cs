using UnityEngine;

public class ShooterShell : BaseEnemy {

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    // Time it takes to fire a bullet.
    [SerializeField] private float fireRate = 1.5f;
    private float shootTime = 0f;

    [Header("Jump")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float minJumpRate = 1f;
    [SerializeField] private float maxJumpRate = 3f;
    private float jumpTime;

    [Header("Components")]
    private MovementComponent movComp;

    private void Awake() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        base.Awake();
        movComp = GetComponent<MovementComponent>();
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
                if (movComp.IsGrounded())
                    movComp.Jump();
                jumpTime = Random.Range(minJumpRate, maxJumpRate);
            }
        }
    }

    private void Fire() {
        Instantiate(bullet, bulletSpawnPoint.position, transform.rotation, transform.parent);
        shootTime = fireRate;
    }
}
