﻿namespace GameLogic.Manufacture
{
    public interface ITowerController
    {
        
    }
    public class TowerController
    {
        private readonly ITowerModel towerModel;
        private readonly ITowerView towerView;


        public TowerController(ITowerModel towerModel, ITowerView towerView)
        {
            this.towerModel = towerModel;
            this.towerView = towerView;
            
            towerModel.OnUpgradeTower += OnUpgradeTower;
        }

        private void OnUpgradeTower()
        {
            towerView.OpenNewArea();
        }
    }
}