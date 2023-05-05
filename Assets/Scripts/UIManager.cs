using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public NPCUI NpcUI { get => _npcUI; private set => _npcUI = value; }
    public GateUI GateUI { get => _gateUI; private set => _gateUI = value; }
    public ExitUIMenu ExitUIMenu { get => _exitUIMenu; private set => _exitUIMenu = value; }

    [SerializeField]
    NPCUI _npcUI;
    [SerializeField]
    GateUI _gateUI;
    [SerializeField]
    ExitUIMenu _exitUIMenu;
    private void Awake()
    {
        Instance = this;
        _npcUI=FindObjectOfType<NPCUI>();
        _gateUI=FindObjectOfType<GateUI>();
        _exitUIMenu=FindObjectOfType<ExitUIMenu>();
    }
    private void OnValidate()
    {
        if (_npcUI == null)
            Debug.LogWarning("Missing NpcUI");
        if (_gateUI == null)
            Debug.LogWarning("Missing GateUI");
        if (_exitUIMenu == null)
            Debug.LogWarning("Missing ExitUIMenu");
    }
}
