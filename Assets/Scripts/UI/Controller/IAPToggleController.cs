using TMPro;
using UnityEngine;
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
            weeklyTitleText.text = string.Empty;
            weeklyPriceText.text = string.Empty;
            monthlyTitleText.text = string.Empty;
            monthlyPriceText.text = string.Empty;
            monthlySubPriceText.text = string.Empty;
        }

        private void InitCommands()
        {
            
        }

        public void UpdateText(string weeklyTitle, string weeklyPrice, string monthlyTitle, string monthlyPrice)
        {
            weeklyTitleText.text = weeklyTitle;
            weeklyPriceText.text = $"${weeklyPrice} / Weekly";
            monthlyTitleText.text = monthlyTitle;
            monthlyPriceText.text = $"${monthlyPrice} / Monthly";
            var monthlySubPrice = int.Parse(monthlyPrice) / 4;
            monthlySubPriceText.text = $"${monthlySubPrice} USD";
        }
    }
}