using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputFieldMaxHP;
    [SerializeField]
    private TMP_InputField _inputFieldSpeed;
    [SerializeField]
    private TMP_InputField _inputFieldFastHit;
    [SerializeField]
    private TMP_InputField _inputFieldStrongHit;
    [SerializeField]
    private TMP_InputField _inputFieldChanceMiss;
    [SerializeField]
    private TMP_InputField _inputFieldChanceCrit;
    [SerializeField]
    private TMP_InputField _inputFieldProbability;
    [SerializeField]
    private ColorTeam _colorTeam;
    private Material _material;
    [SerializeField]
    private Button _saveButton;

    private NPCSetting _nPCSetting;
    [SerializeField]
    private NPC _selectNPC;
    [SerializeField]
    private Gate _selectGate;
    private List<Selectable> _interactables = new();

    private float _posX;
    private void OnValidate()
    {
        if (_inputFieldMaxHP == null)
            Debug.LogWarning("Mising _inputFieldMaxHP");
        if (_inputFieldSpeed == null)
            Debug.LogWarning("Mising _inputFieldSpeed");
        if (_inputFieldFastHit == null)
            Debug.LogWarning("Mising _inputFieldFastHit");
        if (_inputFieldStrongHit == null)
            Debug.LogWarning("Mising _inputFieldStrongHit");
        if (_inputFieldChanceMiss == null)
            Debug.LogWarning("Mising _inputFieldChanceMiss");
        if (_inputFieldChanceCrit == null)
            Debug.LogWarning("Mising _inputFieldChanceCrit");
        if (_inputFieldProbability == null)
            Debug.LogWarning("Mising _inputFieldProbability");
        if (_saveButton == null)
            Debug.LogWarning("Mising SaveButton");
    }
    private void Awake()
    {
        _posX = transform.position.x;
        transform.position +=new Vector3( - ((RectTransform)transform).rect.width,0f);
        _interactables.ForEach(t => t.interactable = false);
        _interactables = GetComponentsInChildren<Selectable>().ToList();
        _saveButton = GetComponentInChildren<Button>();
    }
    private void OnDisable()
    {
        _saveButton.onClick.RemoveAllListeners();
    }

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(Save);
    }
    public void Save()
    {
        var setting = new NPCSetting(
            int.Parse(_inputFieldMaxHP.text),
            float.Parse(_inputFieldSpeed.text),
            int.Parse(_inputFieldFastHit.text),
            int.Parse(_inputFieldStrongHit.text),
            int.Parse(_inputFieldChanceMiss.text),
            int.Parse(_inputFieldChanceCrit.text),
            int.Parse(_inputFieldProbability.text),
            _colorTeam,
            _material
            );
        if (_selectNPC != null)
            _selectNPC.SetSetting(setting);
        if (_selectGate != null)
            _selectGate.GateSetting.SetNPCsetting(setting);
    }

   
    public void Load(NPCSetting select)
    {
        _nPCSetting = select;
        NPCSetting setting = (NPCSetting)_nPCSetting.Clone();
        _inputFieldMaxHP.text = setting.MaxHP.ToString();
        _inputFieldSpeed.text = setting.Speed.ToString();
        _inputFieldFastHit.text= setting.FastHit.ToString();
        _inputFieldStrongHit.text= setting.StrongHit.ToString();
        _inputFieldChanceMiss.text= setting.ChanceMiss.ToString();
        _inputFieldChanceCrit.text= setting.ChanceCrit.ToString();
        _inputFieldProbability.text= setting.Probability.ToString();
        _colorTeam = setting.Team;
        _material = setting.Material;
        Open();
    }

    internal void Load(NPC nPC)
    {
        Load(nPC.NPCSetting);
        _selectNPC = nPC;
        _selectGate = null;
    }
    internal void Load(Gate gate)
    {
        _selectNPC = null;
        _selectGate = gate;
        Load(_selectGate.GateSetting.NPCSetting);
    }
    public void Close()
    {
        LogManager.Instance?.SaveLog("Close NPCUI");
        StartCoroutine(CloseAnimation());
    }
    public void Open()
    {
        LogManager.Instance?.SaveLog("Open NPCUI");
        StartCoroutine(OpenAnimation());
    }
    IEnumerator OpenAnimation()
    {
        while (transform.position.x<= _posX)
        {
            transform.position += Vector3.right;
            yield return null;
        }
        _interactables.ForEach(t=>t.interactable = true);
    }
    IEnumerator CloseAnimation()
    {
        _interactables.ForEach(t => t.interactable = false);
        while (transform.position.x >= _posX-((RectTransform)transform).rect.width)
        {
            transform.position += Vector3.left;
            yield return null;
        }
    }
}
