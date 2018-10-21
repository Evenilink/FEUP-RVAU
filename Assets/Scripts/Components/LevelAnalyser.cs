using UnityEngine;

public class LevelAnalyser : MonoBehaviour {

    // Distance from the player that the raycast for obstacle detection starts.
    [SerializeField] private float distFromPlayerAndAbism = 0.2f;
    // Minimum distance from the player and an obstacle.
    [SerializeField, Range(0.09f, 0.15f)] private float minDistToObstacle = 0.09f;
    [SerializeField] private float lengthAbismRay = 3f;
    [SerializeField] private LayerMask levelMask;

    // Raycasts from a threshold to detect the end of the level.
    // It it doesn't detect the level, it rotates the gameobject accordingly.
    public void CheckObstacle(out bool obstacleDetected, out bool canKeepMoving) {
        canKeepMoving = true;
        RaycastHit hit;
        Vector3 startPosition = transform.position;
        Debug.DrawLine(startPosition, startPosition + transform.right * (distFromPlayerAndAbism + minDistToObstacle + 1f), Color.blue);
        if (obstacleDetected = Physics.Raycast(startPosition, transform.right, out hit, distFromPlayerAndAbism + minDistToObstacle + 1f, levelMask)) {
            if (hit.distance <= minDistToObstacle)
                canKeepMoving = false;
        }
    }

    public bool CheckAbism() {
        Vector3 startPosition = transform.position + transform.right * distFromPlayerAndAbism;
        Debug.DrawLine(startPosition, startPosition + -transform.up * lengthAbismRay, Color.red);
        if (!Physics.Raycast(startPosition, -transform.up, lengthAbismRay, levelMask))
            return true;
        return false;
    }
}
