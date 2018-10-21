using UnityEngine;

public class Goomba : BaseEnemy {

    private void Awake() {
        base.Awake();
    }

    private void Update () {
        transform.position += transform.right * movSpeed * Time.deltaTime;
        bool obstacleDetected, canKeepMoving;
        analyser.CheckObstacle(out obstacleDetected, out canKeepMoving);
        if (!canKeepMoving || analyser.CheckAbism())
            Flip();
	}
}
