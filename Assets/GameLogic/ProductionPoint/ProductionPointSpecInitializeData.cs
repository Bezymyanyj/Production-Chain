﻿using System;
using DefaultNamespace;
using UnityEngine;

namespace GameLogic.ProductionPoint
{
    public class ProductionPointSpecInitializeData : MonoBehaviour
    {
        [SerializeField] private InitializeData.InitData initData;

        [SerializeField] private InitializeData.LevelData[] levelsData =
            new[] { new InitializeData.LevelData(), new InitializeData.LevelData()};

        public InitializeData.InitData InitData => initData;
        public InitializeData.LevelData[] LevelsData => levelsData;

#if UNITY_EDITOR
        private void Awake()
        {
            CheckInitData();
        }
        private void CheckInitData()
        {
            if (InitData.extractor && InitData.demandProducingResources.Length < 1)
            {
                Debug.LogWarning($"<color=blue>Init Data</color>" +
                                 $"Demand Producing resources for {this} is empty");
            }

            foreach (var levelData in LevelsData)
            {
                if (levelData.demandUpgradeResource.Length < 1)
                {
                    Debug.LogWarning($"<color=blue>Init Data</color>" +
                                     $"Demand Upgrade resources for {this} is empty");
                }

                if (levelData.demandUpgradeResource.Length !=
                    levelData.demandUpgradeResourceCapacity.Length)
                {
                    Debug.LogWarning($"<color=blue>Init Data</color>" +
                                     $"Demand Upgrade resources or Capacity has mistake {this}");
                }

                foreach (var capacity in levelData.demandUpgradeResourceCapacity)
                {
                    if (capacity == 0)
                        Debug.LogWarning($"<color=blue>Init Data</color>" +
                                         $"Demand Upgrade resources capacity is 0 {this}");
                }
            }
        }
#endif
    }
    
    [Serializable]
    public class InitializeData
    {
        [Serializable]
        public struct InitData
        {
            public ResourceType resourceType;

            public ResourceType[] demandProducingResources;

            public ProductionPointType productionPointType;

            public Transform Position;

            public bool extractor;
        }
        [Serializable]
        public struct LevelData
        {
            public  ResourceType[] demandUpgradeResource;
            public  int[] demandUpgradeResourceCapacity;
            public  float productionSpeed;
            public  Color color;
        }
    }
}