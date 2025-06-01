using UnityEngine;

public class LightingController : MonoBehaviour
{
    public Light directionalLight;

    [Header("Day Settings")] public Color dayColor = Color.white;

    public float dayIntensity = 1f;
    public Color dayAmbientColor = Color.gray;

    [Header("Evening Settings")] public Color eveningColor = new(1f, 0.5f, 0.2f);

    public float eveningIntensity = 0.5f;
    public Color eveningAmbientColor = new(0.1f, 0.1f, 0.1f);

    [Header("Night Settings")] public Color nightColor = new(0.1f, 0.1f, 0.3f);

    public float nightIntensity = 0.1f;
    public Color nightAmbientColor = Color.black;

    [Header("Transition Settings")] public float transitionDuration = 2f;

    [SerializeField] private TimeOfDaySystem timeSystem;
    private bool _inTransition;
    private Color _startAmbientColor;

    private Color _startColor;
    private float _startIntensity;
    private Color _targetAmbientColor;

    private Color _targetColor;
    private float _targetIntensity;

    private float _transitionTimer;

    private void Start()
    {
        if (!directionalLight)
        {
            Debug.LogError("Directional Light is not assigned in LightingController!", this);
            enabled = false;
            return;
        }

        if (!timeSystem && !TryGetComponent(out timeSystem))
        {
            Debug.LogError("TimeOfDaySystem not found!", this);
            enabled = false;
            return;
        }

        timeSystem.OnTimeOfDayChanged += OnTimeOfDayChanged;

        SetInitialLighting(timeSystem.CurrentTimeOfDay);
    }

    private void Update()
    {
        if (_inTransition)
        {
            _transitionTimer += Time.deltaTime;
            var t = _transitionTimer / transitionDuration;

            t = Mathf.Clamp01(t);


            directionalLight.color = Color.Lerp(_startColor, _targetColor, t);
            directionalLight.intensity = Mathf.Lerp(_startIntensity, _targetIntensity, t);
            RenderSettings.ambientLight = Color.Lerp(_startAmbientColor, _targetAmbientColor, t);

            if (t >= 1f) _inTransition = false;
        }
    }

    private void OnTimeOfDayChanged(TimeOfDaySystem.TimeOfDay timeOfDay)
    {
        _startColor = directionalLight.color;
        _startIntensity = directionalLight.intensity;
        _startAmbientColor = RenderSettings.ambientLight;

        switch (timeOfDay)
        {
            case TimeOfDaySystem.TimeOfDay.Day:
                _targetColor = dayColor;
                _targetIntensity = dayIntensity;
                _targetAmbientColor = dayAmbientColor;
                break;
            case TimeOfDaySystem.TimeOfDay.Evening:
                _targetColor = eveningColor;
                _targetIntensity = eveningIntensity;
                _targetAmbientColor = eveningAmbientColor;
                break;
            case TimeOfDaySystem.TimeOfDay.Night:
                _targetColor = nightColor;
                _targetIntensity = nightIntensity;
                _targetAmbientColor = nightAmbientColor;
                break;
        }

        _transitionTimer = 0f;
        _inTransition = true;
    }

    private void SetInitialLighting(TimeOfDaySystem.TimeOfDay timeOfDay)
    {
        Color initialColor;
        float initialIntensity;
        Color initialAmbientColor;

        switch (timeOfDay)
        {
            case TimeOfDaySystem.TimeOfDay.Day:
                initialColor = dayColor;
                initialIntensity = dayIntensity;
                initialAmbientColor = dayAmbientColor;
                break;
            case TimeOfDaySystem.TimeOfDay.Evening:
                initialColor = eveningColor;
                initialIntensity = eveningIntensity;
                initialAmbientColor = eveningAmbientColor;
                break;
            case TimeOfDaySystem.TimeOfDay.Night:
                initialColor = nightColor;
                initialIntensity = nightIntensity;
                initialAmbientColor = nightAmbientColor;
                break;
            default:
                initialColor = Color.white;
                initialIntensity = 1f;
                initialAmbientColor = Color.gray;
                break;
        }

        directionalLight.color = initialColor;
        directionalLight.intensity = initialIntensity;
        RenderSettings.ambientLight = initialAmbientColor;
    }
}