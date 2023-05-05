using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartUI : MonoBehaviour
{
    [SerializeField]
    private Button _startBT, _exitBT;   

    private void Awake()
    {
        _startBT.onClick.AddListener(StartOnClick);
        _exitBT.onClick.AddListener(ExitOnCLick);
    }

    private void StartOnClick()
    {
        LogManager.Instance?.SaveLog("LoadScene GameScene");
        SceneManager.LoadScene("GameScene");
    }
    private void ExitOnCLick()
    {
        Application.Quit();
    }
    private void OnValidate()
    {
        if (_startBT == null)
            Debug.LogWarning("Missing Start Button"); 
        if (_exitBT == null)
            Debug.LogWarning("Missing Exit Button");
    }
}
