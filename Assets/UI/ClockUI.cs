using UnityEngine;

public class ClockUI: MonoBehaviour
{
    [SerializeField] private RectTransform clockHand;
    [SerializeField] private TimeOfDaySystem _timeOfDaySystem;

    private void Update()
    {
        float time = _timeOfDaySystem.currentTime;
        float shiftedTime = (time + 18f) %24f;
        float rotation = (-shiftedTime / 24f) * 360f;

        clockHand.localRotation = Quaternion.Euler(0f, 0f, rotation);
    }
}



