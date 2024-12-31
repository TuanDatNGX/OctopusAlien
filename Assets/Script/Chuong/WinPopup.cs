using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopup : MonoBehaviour
{
    private void OnEnable()
    {
        LevelController.Instance.currentLevel += 1;
        if(LevelController.Instance.currentLevel > 5) LevelController.Instance.currentLevel = 1;
    }

    public void BtnContinue()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
