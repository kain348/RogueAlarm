using System.Collections;
using UnityEngine;

[AddComponentMenu("Game/Alarm/Visual Alarm")]
public class VisualAlarm : AlarmDevice
{
    private const float MinIntensity = 0f;
    private const float MaxIntensity = 1f;

    [Header("Visual Settings")]
    [SerializeField] private Color _idleColor = Color.green;
    [SerializeField] private Color _alertColor = Color.red;
    [SerializeField] private float _colorChangeSpeed = 1f;

    private Material _material;
    private Coroutine _colorCoroutine;
    private Renderer _renderer;
    private float _currentIntensity;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        if (_renderer == null)
        {
            Debug.LogError($"{nameof(VisualAlarm)} on {name}: Renderer is not assigned.", this);
            enabled = false;

            return;
        }

        _material = _renderer.material;
        SetIntensity(MinIntensity);
    }

    private void OnDisable()
    {
        StopColorCoroutine();

        if (_material != null)
            SetIntensity(MinIntensity);
    }

    public override void Activate()
    {
        if (_material == null)
            return;

        StartChangeIntensity(MaxIntensity);
    }

    public override void Deactivate()
    {
        if (_material == null)
            return;

        StartChangeIntensity(MinIntensity);
    }

    private void StartChangeIntensity(float targetIntensity)
    {
        StopColorCoroutine();
        _colorCoroutine = StartCoroutine(ChangeIntensityRoutine(targetIntensity));
    }

    private void StopColorCoroutine()
    {
        if (_colorCoroutine == null)
            return;

        StopCoroutine(_colorCoroutine);
        _colorCoroutine = null;
    }

    private IEnumerator ChangeIntensityRoutine(float targetIntensity)
    {
        while (Mathf.Approximately(_currentIntensity, targetIntensity) == false)
        {
            _currentIntensity = Mathf.MoveTowards(
                _currentIntensity,
                targetIntensity,
                _colorChangeSpeed * Time.deltaTime);

            ApplyColor();

            yield return null;
        }

        _currentIntensity = targetIntensity;
        ApplyColor();

        _colorCoroutine = null;
    }

    private void SetIntensity(float intensity)
    {
        _currentIntensity = intensity;
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (_material == null)
            return;

        _material.color = Color.Lerp(_idleColor, _alertColor, _currentIntensity);
    }
}