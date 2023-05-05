using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitUIMenu : MonoBehaviour
{
    [SerializeField]
    private Button _continueBT, _exitBT;

    private void Awake()
    {
        _continueBT.onClick.AddListener(ContinueOnCLick);
        _exitBT.onClick.AddListener(ExitOnCLick);
        gameObject.SetActive(false);
    }

    private void ContinueOnCLick()
    {
        LogManager.Instance?.SaveLog("Continue On CLick");
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    private void ExitOnCLick()
    {
        Application.Quit();        
    }
    private void OnValidate()
    {
        if (_continueBT == null)
            Debug.LogWarning("Missing Continue Button");
        if (_exitBT == null)
            Debug.LogWarning("Missing Exit Button");
    }
}
