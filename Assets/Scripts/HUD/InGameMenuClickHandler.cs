using UnityEngine;

public class InGameMenuClickHandler : MonoBehaviour {

    public void RestartBtnClick() {
        GameMode.Instance().Restart();
    }
}
