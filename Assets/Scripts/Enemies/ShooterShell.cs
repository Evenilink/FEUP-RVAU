using UnityEngine;

public class ShooterShell : BaseEnemy {

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletInsPosition;
    // Time it takes to fire a bullet.
    [SerializeField] private float fireRate = 1.5f;
    private float shootTime = 0f;

    [Header("Jump")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpRate = 3f;
    [SerializeField] private float jumpProbability = 0.6f;

    [Header("Components")]
    private MovementComponent movComp;

    private void Awake() {
        base.Awake();
        movComp = GetComponent<MovementComponent>();
    }

    private void Update() {
        shootTime -= Time.deltaTime;
        if (shootTime <= 0)
            Fire();
    }

    private void Fire() {
        Instantiate(bullet, bulletInsPosition.position, transform.rotation, transform.parent);
        shootTime = fireRate;
    }
}
