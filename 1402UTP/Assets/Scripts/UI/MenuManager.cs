using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Button start;
    public Button quit;
    
    private void Start()
    {
        Button startBtn = start.GetComponent<Button>();
        startBtn.onClick.AddListener(StartGame);
        Button quitBtn = quit.GetComponent<Button>();
        quitBtn.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
        //EditorApplication.ExitPlaymode();
        Application.Quit();
    }
}
