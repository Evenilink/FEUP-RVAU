using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClickHandler : MonoBehaviour {

    public void PlayBtnClick() {
        Debug.Log("Pressed play btn.");
        gameObject.active = false;
    }

    public void ExitBtnClick() {
        Debug.Log("Pressed exit btn.");
        Application.Quit();
    }
}
