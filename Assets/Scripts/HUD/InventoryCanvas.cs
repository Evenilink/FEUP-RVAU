using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class InventoryCanvas : MonoBehaviour {

    [SerializeField] private Slider jumpSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private AudioClip jumpPowerClip;
    [SerializeField] private AudioClip speedPowerClip;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        PlayerController.OnJumpPowerValueChange += OnJumpPowerValueChangeHandler;
        PlayerController.OnSpeedPowerValueChange += OnSpeedPowerValueChangeHandler;
    }

    private void OnJumpPowerValueChangeHandler(float newValue) {
        jumpSlider.value = newValue;
        if (newValue >= 0.97)
            audioSource.PlayOneShot(jumpPowerClip);
    }

    private void OnSpeedPowerValueChangeHandler(float newValue) {
        speedSlider.value = newValue;
        if (newValue >= 0.97)
            audioSource.PlayOneShot(speedPowerClip);
    }

    private void OnDestroy() {
        PlayerController.OnJumpPowerValueChange -= OnJumpPowerValueChangeHandler;
        PlayerController.OnSpeedPowerValueChange -= OnSpeedPowerValueChangeHandler;
    }
}
