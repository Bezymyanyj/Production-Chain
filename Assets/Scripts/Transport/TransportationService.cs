﻿using System;
using System.Collections.Generic;
using DefaultNamespace.ProductionPoint;
using UnityEngine;

namespace DefaultNamespace.Transport
{
    public class TransportationService : MonoBehaviour
    {
        [SerializeField] private ProductionPointInitialize productionInit;

        [SerializeField] private TransportViewFactory transportViewFactory;
        // private ResourceType sentResource;
        // private ResourceType receivedResource;

        private IProductionPointModel senderPoint;
        private IProductionPointModel receiverPoint;

        private readonly List<IProductionPointModel> senders = new List<IProductionPointModel>();

        // public event Action OnCancel;
        // public event Action OnSuccess;
            

        private void Start()
        {
            foreach (var modelFactory in productionInit.ProductionModelFactories)
            {
                modelFactory.Model.OnClick += CallTransportService;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cancel();
            }
        }


        private void CallTransportService(IProductionPointModel productionModel)
        {
            Debug.Log($"Transport service get a message");
            if (senderPoint == null && !CheckSenders(productionModel))
            {
                senderPoint = productionModel;
                return;
            }

            if (receiverPoint != null || senderPoint == productionModel) return;
            receiverPoint = productionModel;
            if (CheckResourceMatch())
            {
                Success();
            }
            else
            {
                Cancel();
                
            }
        }

        private void Success()
        {
            senders.Add(senderPoint);
            
            var transportModelFactory = new TransportModelFactory(
                senderPoint, receiverPoint);
            var model = transportModelFactory.Model;
            model.OnDestroy += OnDestroyBridge;
            
            transportViewFactory.Initiate();
            var view = transportViewFactory.View;

            var transportControllerFactory = new TransportControllerFactory(model, view);
            var controller = transportControllerFactory.Controller;
            
            model.CreateBridge();
            
            senderPoint = null;
            receiverPoint = null;
        }

        private void Cancel()
        {
            senderPoint = null;
            receiverPoint = null;
        }

        private void OnDestroyBridge(IProductionPointModel sender, ITransportModel transportModel)
        {
            senders.Remove(sender);
            transportModel.OnDestroy -= OnDestroyBridge;
        }

        private bool CheckResourceMatch()
        {
            return receiverPoint.DemandResources.ContainsKey(senderPoint.ProducingResourceType);
        }

        private bool CheckSenders(IProductionPointModel sender)
        {
            return senders.Contains(sender);
        }
    }
}