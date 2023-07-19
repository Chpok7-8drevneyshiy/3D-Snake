using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Solo()
    {
        SceneManager.LoadScene("SoloScene");
    }

    public void Duo()
    {
        SceneManager.LoadScene("DuoScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
}
