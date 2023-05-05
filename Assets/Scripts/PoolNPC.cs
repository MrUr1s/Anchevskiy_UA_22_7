using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Ziggurat;

public class PoolNPC : MonoBehaviour
{
    public static PoolNPC Instance;
    [Inject]
    private NPC _unitEnvironment;
    [SerializeField]
    private List<NPC> _npcList=new();

    public List<NPC> NpcList => _npcList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        for (var i = 0; i < 10; i++)
        {
            AddUnit();
        }
    }

    public NPC GetUnitEnemyTeam(NPC NPC)
    {
        float min = float.MaxValue;
        var units= NpcList.Where(t => t.gameObject.activeSelf && NPC.NPCSetting.Team!=t.NPCSetting.Team);
       
            var unit = units.FirstOrDefault();
        foreach (var item in units)

            if (min >= (NPC.transform.position - item.transform.position).sqrMagnitude)
            {
                min = (NPC.transform.position - item.transform.position).sqrMagnitude;
                unit = item;
            }
        

            return unit;
       
    }

    public NPC AddUnit()
    {
        var unit = Instantiate(_unitEnvironment, GameManager.Instance.NPCPool.transform);
        unit.gameObject.SetActive(false);
        NpcList.Add(unit);
        return unit;
    }
    
}
