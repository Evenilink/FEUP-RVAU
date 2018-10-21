using UnityEngine;

public class DamageableComponent : MonoBehaviour {

    [SerializeField] private float health = 100f;
    private PlayerController pc;

    private void Awake() {
        pc = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.tag == "Enemy")
            pc.Respawn();
    }
}
