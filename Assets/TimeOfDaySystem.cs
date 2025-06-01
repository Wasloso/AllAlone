using System;
using UnityEngine;

public class TimeOfDaySystem : MonoBehaviour
{
    public enum TimeOfDay
    {
        Day,
        Evening,
        Night
    }

    [Range(0, 24)] public float currentTime = 12f;
    public float dayDurationInMinutes = 5f;
    private TimeOfDay _lastTimeOfDay;

    private float _timeSpeed; // How fast time progresses

    public TimeOfDay CurrentTimeOfDay { get; private set; }

    private void Start()
    {
        _timeSpeed = 24f / (dayDurationInMinutes * 60f);
        UpdateTimeOfDay();
    }

    private void Update()
    {
        currentTime += Time.deltaTime * _timeSpeed;
        if (currentTime >= 24f) currentTime = 0f;

        UpdateTimeOfDay();
    }

    public event Action<TimeOfDay> OnTimeOfDayChanged;

    private void UpdateTimeOfDay()
    {
        TimeOfDay newTimeOfDay;

        if (currentTime >= 6f && currentTime < 18f)
            newTimeOfDay = TimeOfDay.Day;
        else if (currentTime >= 18f && currentTime < 20f)
            newTimeOfDay = TimeOfDay.Evening;
        else
            newTimeOfDay = TimeOfDay.Night;

        if (newTimeOfDay != _lastTimeOfDay)
        {
            CurrentTimeOfDay = newTimeOfDay;
            _lastTimeOfDay = newTimeOfDay;
            OnTimeOfDayChanged?.Invoke(newTimeOfDay);
        }
    }
}