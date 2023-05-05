using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class Gate : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    private ColorTeam _colorTeam;
    [SerializeField]
    private GateSetting _gateSetting;
    private SpawnGate _spawnGate;   
    public GateSetting GateSetting { get => _gateSetting;private set => _gateSetting = value; }

    private void OnValidate()
    {
        if (GateSetting.SpawnCount < 0)
            Debug.LogError("GateSetting.SpawnCount < 0");
        if (GateSetting.SpawnDelay < 0)
            Debug.LogError("GateSetting.SpawnDelay < 0");
        if (GateSetting.NPCSetting.MaxHP < 0)
            Debug.LogError("GateSetting.NPCSetting.MaxHP  < 0");
        if (GateSetting.NPCSetting.Speed < 0)
            Debug.LogError("GateSetting.NPCSetting.Speed  < 0");
        if (GateSetting.NPCSetting.FastHit < 0)
            Debug.LogError("GateSetting.NPCSetting.FastHit  < 0");
        if (GateSetting.NPCSetting.StrongHit < 0)
            Debug.LogError("GateSetting.NPCSetting.StrongHit  < 0");
        if (GateSetting.NPCSetting.ChanceCrit < 0)
            Debug.LogError("GateSetting.NPCSetting.ChanceCrit  < 0");
        if (GateSetting.NPCSetting.ChanceMiss < 0)
            Debug.LogError("GateSetting.NPCSetting.ChanceMiss  < 0");
        if (GateSetting.NPCSetting.Material ==null)
            Debug.LogError("Missing GateSetting.NPCSetting.Material");

    }
    private void Awake()
    {
        _spawnGate = GetComponentInChildren<SpawnGate>();
    }
    private void Start()
    {
        StartCoroutine(SpawnNPC());
    }

    public void SaveSetting(GateSetting gateSetting)
    {

        GateSetting=(GateSetting)gateSetting.Clone(); 
    }
    public IEnumerator SpawnNPC()
    {
        while (true)
        {
            if (PoolNPC.Instance.NpcList.Where(t => t.gameObject.activeSelf && t.NPCSetting.Team == GateSetting.NPCSetting.Team).Count() < GateSetting.SpawnCount)
            {
                var npc = PoolNPC.Instance.NpcList.FirstOrDefault(t => !t.gameObject.activeSelf);
                if (npc == null)
                {
                    npc = PoolNPC.Instance.AddUnit();
                }
                npc.gameObject.SetActive(true);
                npc.SetSetting(GateSetting.NPCSetting);
                npc.transform.position = _spawnGate.transform.position;
            }
            yield return new WaitForSeconds(GateSetting.SpawnDelay);
        }
    }


    private void OnDrawGizmos()
    {
        _spawnGate = GetComponentInChildren<SpawnGate>();
        Gizmos.color = GetColor(_colorTeam);
        Gizmos.DrawCube(_spawnGate.transform.position, Vector3.one);
    }

    private Color GetColor(ColorTeam colorTeam)
    {
        switch (colorTeam)
        {
            case ColorTeam.Red:
                return Color.red;
            case ColorTeam.Green:
                return Color.green;
            case ColorTeam.Blue:
                return Color.blue;
            default:
                throw new NotImplementedException();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LogManager.Instance?.SaveLog("OnPointerClick to Gate");
        //UIManager.Instance.GateUI.Open();
      //  UIManager.Instance.NpcUI.Open();
        UIManager.Instance.GateUI.Load(this);
    }
}