using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClickHandler : MonoBehaviour {

    public void PlayBtnClick() {
        GameMode.Instance().StartGame();
    }

    public void ExitBtnClick() {
        Application.Quit();
    }
}
