using UnityEngine;

public class PlayerMovementComponent : MovementComponent {

    private float lastHeight;
    private PlayerController pc;

    private void Awake() {
        base.Awake();
        pc = GetComponent<PlayerController>();
        lastHeight = transform.position.y;
    }

    private void FixedUpdate() {
        base.FixedUpdate();
    }

    public void Move(float hInput) {
        if (hInput == 0)
            return;
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

    protected override void OnReachedGround() {
        if (transform.position.y > lastHeight) {
            LevelScrollingManager.Instance().ScrollToHeight(transform.position.y - lastHeight);
            lastHeight = transform.position.y;
            pc.SetCheckpointInfo(pc.GetCheckpointInfo().position - new Vector3(0, transform.position.y - lastHeight, 0), pc.GetCheckpointInfo().rotation);
        }
    }
}
