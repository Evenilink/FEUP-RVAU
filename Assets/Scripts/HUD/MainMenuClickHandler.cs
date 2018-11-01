using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClickHandler : MonoBehaviour {

    public void PlayBtnClick() {
        Debug.Log("Pressed play btn.");
        SceneManager.LoadScene("WorldGeneration");
    }

    public void ExitBtnClick() {
        Debug.Log("Pressed exit btn.");
        Application.Quit();
    }
}
