using System.Text;
using TMPro;
using UnityEngine;

public class LoggerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProLogger;

    public static LoggerScript Instance { get; private set; }
    private StringBuilder _stringBuilder = new();

    [SerializeField] private bool isActive = false;

    public void setIsActive(bool activateStatus)
    {
        this.isActive = activateStatus;
        Debug.Log("is active situation is ignored for you !");
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

        textMeshProLogger.SetText(_stringBuilder);
    }
}