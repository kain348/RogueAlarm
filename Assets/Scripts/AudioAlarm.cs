using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Game/Alarm/Audio Alarm")]
public class AudioAlarm : AlarmDevice
{
    private const float MinVolume = 0f;

    [Header("Audio Settings")]
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _volumeChangeSpeed = 1f;

    private AudioSource _audioSource;
    private Coroutine _volumeCoroutine;
    private float _currentVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;

        if (_audioSource.clip == null)
        {
            Debug.LogError($"{nameof(AudioAlarm)} on {name}: AudioSource has no AudioClip assigned.", this);
            enabled = false;

            return;
        }

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

    public override void Activate()
    {
        if (_audioSource == null)
            return;

        StartChangeVolume(_maxVolume);
    }

    public override void Deactivate()
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

            ApplyVolume();

            yield return null;
        }

        _currentVolume = targetVolume;
        ApplyVolume();

        _volumeCoroutine = null;
    }

    private void SetVolume(float volume)
    {
        _currentVolume = volume;
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        if (_audioSource == null)
            return;

        _audioSource.volume = _currentVolume;
    }
}