using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GateUI : MonoBehaviour
{
    [SerializeField]
    private NPCSetting _npcSetting;

    [SerializeField]
    private Gate _select;
    
    [SerializeField]
    private TMP_InputField _inputFieldDelay;
    [SerializeField]
    private TMP_InputField _inputFieldCount;
    [SerializeField]
    private Button _saveButton;

    private List<Selectable> _interactables = new(); 
    private float _posX;

    private void OnValidate()
    {
        if (_inputFieldDelay == null)
            Debug.LogWarning("Mising InputFieldDelay");
        if (_inputFieldCount == null)
            Debug.LogWarning("Mising InputFieldCount"); 
        if (_saveButton == null)
            Debug.LogWarning("Mising SaveButton");
    }

    private void Awake()
    {
        _posX = transform.position.x;
        transform.position += new Vector3(((RectTransform)transform).rect.width, 0f);
        _interactables.ForEach(t => t.interactable = false);
        _interactables = GetComponentsInChildren<Selectable>().ToList();
        _saveButton =GetComponentInChildren<Button>();
        //gameObject.SetActive(false);
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
        _select.SaveSetting(new GateSetting(
            float.Parse(_inputFieldDelay.text), int.Parse(_inputFieldCount.text), _npcSetting));
    }


    public void Load(Gate select)
    {
        UIManager.Instance.NpcUI.Load(select);
        _select = select;
        GateSetting gateSetting = select.GateSetting.Clone();
        _inputFieldDelay.text = gateSetting.SpawnDelay.ToString();
        _inputFieldCount.text = gateSetting.SpawnCount.ToString();
        _npcSetting = gateSetting.NPCSetting.Clone(); 
        Open();

    }
    public void Close()
    {
        LogManager.Instance?.SaveLog("Close  GateUI");
        StartCoroutine(CloseAnimation());
    }
    public void Open()
    {
        LogManager.Instance?.SaveLog("Open  GateUI");
        StartCoroutine(OpenAnimation());
    }
    IEnumerator OpenAnimation()
    {
        while (transform.position.x >= _posX)
        {
            transform.position += Vector3.left;
            yield return null;
        }
        _interactables.ForEach(t => t.interactable = true);
    }
    IEnumerator CloseAnimation()
    {
        _interactables.ForEach(t => t.interactable = false);
        while (transform.position.x <= _posX + ((RectTransform)transform).rect.width)
        {
            transform.position += Vector3.right;
            yield return null;
        }
    }
}
