using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace UI.Controller
{
    public class IAPToggleController : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI weeklyTitleText;
        [SerializeField] private TextMeshProUGUI weeklyPriceText;
        [SerializeField] private TextMeshProUGUI monthlyTitleText;
        [SerializeField] private TextMeshProUGUI monthlyPriceText;
        [SerializeField] private TextMeshProUGUI monthlySubPriceText;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            InitComponents();
            InitCommands();
        }

        private void InitComponents()
        {
            // weeklyTitleText.text = string.Empty;
            // weeklyPriceText.text = string.Empty;
            // monthlyTitleText.text = string.Empty;
            // monthlyPriceText.text = string.Empty;
            // monthlySubPriceText.text = string.Empty;
        }

        private void InitCommands()
        {
        }

        public void UpdateText(Product[] products)
        {
            var weeklyProduct = products[0];
            var monthlyProduct = products[1];

            Debug.Log(weeklyProduct.metadata.localizedPriceString);
            Debug.Log(monthlyProduct.metadata.localizedPriceString);


            weeklyTitleText.text = weeklyProduct.metadata.localizedTitle;
            weeklyPriceText.text =
                $"Weekly / {weeklyProduct.metadata.localizedPriceString}";
            
            monthlyTitleText.text = monthlyProduct.metadata.localizedTitle;
            monthlyPriceText.text =
                $"Monthly / {monthlyProduct.metadata.localizedPriceString}";
            var monthlySubPrice = monthlyProduct.metadata.localizedPrice / 4;
            monthlySubPriceText.text =
                string.Format("{0:#.00} {1}", monthlySubPrice, monthlyProduct.metadata.isoCurrencyCode);
        }
    }
}