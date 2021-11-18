﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.Manufacture
{
    public class ManufactureData
    {
        public event Action OnUpgrade;

        public ManufactureData(InitializeData.InitData initData, InitializeData.LevelData[] levelsData)
        {
            ManufactureType = initData.manufactureType;
            ProducingResource = initData.resourceType;
            Position = initData.Position.position;
            demandProductionResources = initData.demandProducingResources;
            Extractor = initData.extractor;

            this.levelsData = levelsData;
            CurrentLevel = levelsData[0];
            ProductionSpeed = CurrentLevel.productionSpeed;
            demandUpgradeResources = CurrentLevel.demandUpgradeResource;
        
            upgradeResources = new Dictionary<ResourceType, int>();
            for (int i = 0; i < demandUpgradeResources.Count; i++)
            {
                upgradeResources.Add(demandUpgradeResources[i], CurrentLevel.demandUpgradeResourceCapacity[i]);
            }

            productionResources = new Dictionary<ResourceType, int>();
            foreach (var resource in demandProductionResources)
            {
                productionResources.Add(resource, 0);
            }
        }

        public ManufactureType ManufactureType { get; set; }
        public ResourceType ProducingResource { get; set; }
        public InitializeData.LevelData CurrentLevel { get; set; }
        public float ProductionSpeed { get; set; }
        public Vector3 Position { get; set; }
        public bool Extractor { get; }

        public int Level
        {
            get => level;
            set
            {
                level = value;
                Upgrade(level);
            }
        }

        public List<ResourceType> DemandUpgradeResources => demandUpgradeResources;
        public List<ResourceType> DemandProductionResource => demandProductionResources;

    
        private int level;
    
        private readonly InitializeData.LevelData[] levelsData;
        private List<ResourceType> demandUpgradeResources;
        private Dictionary<ResourceType, int> upgradeResources;
        private readonly List<ResourceType> demandProductionResources;
        private readonly Dictionary<ResourceType, int> productionResources;

        public bool AddDemandResources(ResourceType resource)
        {
            if (demandUpgradeResources.Contains(resource))
            {
                upgradeResources[resource]--;
                CheckUpgradeOpportunity();
                return true;
            }

            if (demandProductionResources.Count > 0 && demandProductionResources.Contains(resource))
            {
                productionResources[resource]++;
                return true;
            }
            return false;
        }

        public bool CheckProductionOpportunity()
        {
            if (productionResources.Any(
                resource => resource.Value < 1))
            {
                return false;
            }
            foreach (var varResource in demandProductionResources)
            {
                productionResources[varResource]--;
            }
            return true;
        }

        private void CheckUpgradeOpportunity()
        {
            if(upgradeResources.All(resource => resource.Value < 1))
            {
                OnUpgrade?.Invoke();
                Upgrade(level + 1);
            }
        }

        private void Upgrade(int level)
        {
            if(level > levelsData.Length - 1 ) return;
            CurrentLevel = levelsData[level];
            ProductionSpeed = CurrentLevel.productionSpeed;
            demandUpgradeResources = CurrentLevel.demandUpgradeResource;
            upgradeResources = new Dictionary<ResourceType, int>();
            for (int i = 0; i < demandUpgradeResources.Count; i++)
            {
                upgradeResources.Add(demandUpgradeResources[i], CurrentLevel.demandUpgradeResourceCapacity[i]);
            }
        }

        public int[] GetDemandResourceAmount()
        {
            return productionResources.Values.ToArray();
        }

        public void SetDemandResourceAmount(int[] values)
        {
            // if (values.Length != productionResources.Count)
            //     throw new Exception();
            var index = 0;
            foreach (var resource in productionResources.Keys.ToList())
            {
                productionResources[resource] = values[index++];
            }
        }

        public int[] GetUpgradeResourceAmount()
        {
            return upgradeResources.Values.ToArray();
        }
        
        public void SetUpgradeResourceAmount(int[] values)
        {
            // if (values.Length != productionResources.Count)
            //     throw new Exception();
            var index = 0;
            foreach (var resource in upgradeResources.Keys.ToList())
            {
                upgradeResources[resource] = values[index++];
            }
        }
    }
}