using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePopup : MonoBehaviour
{
    public void BtnContinue()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
