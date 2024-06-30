using System.Text;
using TMPro;
using UnityEngine;

public class LoggerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProLogger;

    public static LoggerScript Instance { get; private set; }
    private StringBuilder _stringBuilder = new();

    [SerializeField] private bool isActive;

    private void Start()
    {
        gameObject.SetActive(isActive);
    }


    /// <summary>
    /// Singleton pattern for this mono behaviour class
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;
    }

    /// <summary>
    /// this function used to print some logs in GUI
    /// </summary>
    /// <param name="message"></param>
    public void PrintLog(string message)
    {
        _stringBuilder.Append(message);
        _stringBuilder.Append("\n");
        UpdateText();
    }

    public void UpdateText()
    {
        textMeshProLogger.SetText(_stringBuilder);
    }

    public void Clear()
    {
        _stringBuilder.Clear();
        UpdateText();
    }
}