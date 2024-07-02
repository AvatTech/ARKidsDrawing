using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign this in the Inspector
    private float elapsedTime;
    private bool isRunning = false; // To track if the timer is running
    private Coroutine timerCoroutine;

    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            timerCoroutine = StartCoroutine(UpdateTimer());
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
            StopCoroutine(timerCoroutine);
            elapsedTime = 0;
            timerText.text = "00:00";
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            // Increment elapsed time by 1 second
            elapsedTime += 1f;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);

            // Format and display the time
            timerText.text = $"{minutes:00}:{seconds:00}";

            // Wait for 1 second
            yield return new WaitForSeconds(1f);
        }
    }
}