using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvlQuestManager : MonoBehaviour
{
    [SerializeField] private GameObject clearedLevel;
    
    [Header("Damage Quest")] 
    [SerializeField] private TextMeshProUGUI damageText;
    private float accumulatedDamage;
    [SerializeField] private GameObject damageAtTheEnd;

    [Header("Time Quest")]
    [SerializeField] private TextMeshProUGUI timeText;
    private float timer;
    private bool isTimerRunning;
    [SerializeField] private GameObject timeAtTheEnd;


    private void Start()
    {
        timer = 0f;
        isTimerRunning = false;
        
        StartTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimeText(timer);
        }

        if (clearedLevel.activeSelf)
        {
            StopTimer();
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void UpdateTimeText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timeText.text = "Time passed: " + string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void UpdateDamageQuest(float damage)
    {
        accumulatedDamage += damage;
        damageText.text = "Damage received: " + accumulatedDamage;
    }
}