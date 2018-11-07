using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundAnimation : MonoBehaviour {

    private AudioSource audioSource;
    private BaseEnemy baseEnemy;

    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioClip heavyFootstep;
    [SerializeField] private AudioClip jumpStart;
    [SerializeField] private AudioClip jumpEnd;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        baseEnemy = GetComponent<BaseEnemy>();
    }

    private void PlayStep() {
            audioSource.PlayOneShot(footstep);
    }

    private void PlayJumpStart() {
        audioSource.PlayOneShot(jumpStart);
    }

    private void PlayJumpEnd() {
        audioSource.PlayOneShot(jumpEnd);
    }

    private void PlayHeavyFootstep() {
        if (baseEnemy == null || !baseEnemy.isWaitingDestroy)
        audioSource.PlayOneShot(heavyFootstep);
    }
}
