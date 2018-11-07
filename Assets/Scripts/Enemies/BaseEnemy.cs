using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    [SerializeField] protected float movSpeed;
    [SerializeField] protected LayerMask levelMask;
    [SerializeField] protected AudioClip[] dieSounds;

    public bool isWaitingDestroy = false;
    protected LevelAnalyser analyser;

    protected void Awake() {
        analyser = GetComponent<LevelAnalyser>();
    }

    protected void Flip() {
        transform.rotation = Quaternion.Euler(0, 180, 0) * transform.rotation;
    }

    public void Die() {
        transform.localScale = Vector3.zero;
        isWaitingDestroy = true;
        int index = Random.Range(0, dieSounds.Length);
        GetComponent<AudioSource>().PlayOneShot(dieSounds[index]);
        Destroy(gameObject, 2);
    }
}
