using System;
using Services;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class IAPUIController : MonoBehaviour
    {
        [SerializeField] private GameObject iapPanelPage;
        [Inject] private readonly IAPService _iapService;

        private void Start()
        {
            _iapService.OnInitializeCompleted.AddListener(OnInitializePurchaseCompleted);
            _iapService.Initialize();
        }

        public void CloseButton()
        {
            iapPanelPage.SetActive(false);
        }

        public void PurchaseButton()
        {
            _iapService.Purchase(ProductType.Weekly);
        }

        private void OnInitializePurchaseCompleted()
        {
            Debug.Log("on initialize completed");
        }
    }
}