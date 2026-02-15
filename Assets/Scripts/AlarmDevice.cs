using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Game/Alarm/Alarm Divice")]
public class AlarmDevice : MonoBehaviour
{
    [Header("Allarm Settings")]
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _volumeChangeSpeed = 1f;

    [Header("Visual Settings")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _idleColor = Color.green;
    [SerializeField] private Color _allertColor = Color.red;

    private const float MinVolume = 0f;

    private AudioSource _audioSource;
    private float _targetVolume;
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

        _audioSource.volume = MinVolume;
        _currentVolume = MinVolume;
        _targetVolume = MinVolume;

        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        UpdateVisual();
    }

    private void OnEnable()
    {
        if (_audioSource.isPlaying == false && _audioSource.clip != null)
            _audioSource.Play();
    }

    private void Update()
    {
        _currentVolume = Mathf.MoveTowards(_currentVolume, _targetVolume, _volumeChangeSpeed * Time.deltaTime);

        _audioSource.volume = _currentVolume;

        UpdateVisual();
    }

    public void Activate()
    {
        _targetVolume = _maxVolume;
    }

    public void Diactivate()
    {
        _targetVolume = MinVolume;
    }

    private void UpdateVisual()
    {
        if (_renderer == null)
            return;

        float anxietyPercentage = (_maxVolume > MinVolume) ? (_currentVolume / _maxVolume) : MinVolume;
        _renderer.material.color = Color.Lerp(_idleColor,_allertColor, anxietyPercentage);
    }
}