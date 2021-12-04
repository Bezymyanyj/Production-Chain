﻿using System;
using System.Collections.Generic;
using GameLogic.Manufacture;
using UnityEngine;

namespace CommonBaseUI.Data
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private SaveLoadJsonWindows saveLoadJsonWindows;
        [SerializeField] private SaveLoadJsonWeb saveLoadJsonWeb;
        //[SerializeField] private SaveLoadAsyncTest saveLoadAsyncTest;

        public static DataManager Instance;
        
        private SaveLoadJson saveLoadJson;
        private ManufactureDataManager manufactureDataManager;

        public GameSettingsDataManager GameSettingsDataManager { get; private set; }


        private void Awake()
        {
            #if UNITY_WEBGL
                saveLoadJson = saveLoadJsonWeb;
            #else
                saveLoadJson = saveLoadJsonWindows;
            #endif
            
            GameSettingsDataManager = new GameSettingsDataManager(saveLoadJson);
            Instance = this;
        }

        // public void CreateManufactureDataManager(List<IManufactureModel> manufactures)
        // {
        //     manufactureDataManager = new ManufactureDataManager(saveLoadJson, manufactures);
        // }
        
        public void Save()
        {
            manufactureDataManager.SaveData();
        }

        public void Load()
        {
            manufactureDataManager.LoadData();
        }
    }
}