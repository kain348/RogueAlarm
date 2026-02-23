using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Game/Alarm/Alarm Device")]
public class AlarmDevice : MonoBehaviour
{
    private const float MinVolume = 0f;

    [Header("Alarm Settings")]
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _volumeChangeSpeed = 1f;

    [Header("Visual Settings")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _idleColor = Color.green;
    [SerializeField] private Color _alertColor = Color.red;

    private AudioSource _audioSource;
    private Coroutine _volumeCoroutine;
    private Material _material;
    private float _currentVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;

        if (_audioSource.clip == null)
        {
            Debug.LogError($"{nameof(AlarmDevice)} on {name}: AudioSource has no AudioClip assigned.", this);
            enabled = false;
            return;
        }

        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        if (_renderer != null)
            _material = _renderer.material;

        SetVolume(MinVolume);
    }

    private void OnEnable()
    {
        if (_audioSource != null && _audioSource.isPlaying == false)
            _audioSource.Play();
    }

    private void OnDisable()
    {
        StopVolumeCoroutine();

        if (_audioSource != null)
            SetVolume(MinVolume);
    }

    public void Activate()
    {
        if (_audioSource == null)
            return;

        StartChangeVolume(_maxVolume);
    }

    public void Deactivate()
    {
        if (_audioSource == null)
            return;

        StartChangeVolume(MinVolume);
    }

    private void StartChangeVolume(float targetVolume)
    {
        StopVolumeCoroutine();
        _volumeCoroutine = StartCoroutine(ChangeVolumeRoutine(targetVolume));
    }

    private void StopVolumeCoroutine()
    {
        if (_volumeCoroutine == null)
            return;

        StopCoroutine(_volumeCoroutine);
        _volumeCoroutine = null;
    }

    private IEnumerator ChangeVolumeRoutine(float targetVolume)
    {
        while (Mathf.Approximately(_currentVolume, targetVolume) == false)
        {
            _currentVolume = Mathf.MoveTowards(
                _currentVolume,
                targetVolume,
                _volumeChangeSpeed * Time.deltaTime);

            ApplyVolumeAndVisual();

            yield return null;
        }

        _currentVolume = targetVolume;
        ApplyVolumeAndVisual();

        _volumeCoroutine = null;
    }

    private void SetVolume(float volume)
    {
        _currentVolume = volume;
        ApplyVolumeAndVisual();
    }

    private void ApplyVolumeAndVisual()
    {
        _audioSource.volume = _currentVolume;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (_material == null)
            return;

        float anxietyPercentage = (_maxVolume > MinVolume) ? (_currentVolume / _maxVolume) : MinVolume;
        _material.color = Color.Lerp(_idleColor, _alertColor, anxietyPercentage);
    }
}