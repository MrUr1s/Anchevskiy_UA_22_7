using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

    public class GameManager : MonoInstaller
    {
        public static GameManager Instance { get; private set; }
        public PoolNPC NPCPool { get => _nPCPool; private set => _nPCPool = value; }

        private PoolNPC _nPCPool;
        private string _pathPrefabNPC = "NPC";


        public override void InstallBindings()
        {
            Container.BindInstance(Resources.Load<NPC>(_pathPrefabNPC)).AsSingle();
        }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this);
            NPCPool = FindObjectOfType<PoolNPC>();
        }
    }

