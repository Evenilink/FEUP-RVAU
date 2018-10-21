using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    [SerializeField] protected float movSpeed;
    [SerializeField] protected bool startFacingRight = false;
    [SerializeField] protected LayerMask levelMask;
    protected LevelAnalyser analyser;

    protected void Awake() {
        analyser = GetComponent<LevelAnalyser>();
        if (!startFacingRight)
            Flip();
    }

    protected void Flip() {
        transform.rotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
    }
}
