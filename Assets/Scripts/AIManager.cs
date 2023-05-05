using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void IntoAction()
    {
        PoolNPC.Instance.NpcList.ForEach(npc =>
        {
            if(npc.gameObject.activeSelf)
            {
               // if (npc.Target == null)
                    npc.SearchTarget();

            }
        }
        );
    }
    private void Update()
    {
        IntoAction();
    }

   
}
