using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Alarm/Alarm System")]
public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private AlarmZone _alarmZone;
    [SerializeField] private List<AlarmDevice> _alarmDevices = new List<AlarmDevice>();

    private bool _isThiefInside;

    private void Awake()
    {
        if (_alarmZone == null)
        {
            Debug.LogError($"{nameof(AlarmSystem)} on {name}: AlarmZone is not assigned.", this);
            enabled = false;

            return;
        }

        if (_alarmDevices == null || _alarmDevices.Count == 0)
        {
            Debug.LogError($"{nameof(AlarmSystem)} on {name}: AlarmDevice is not assigned.", this);
            enabled = false;

            return;
        }

        for (int i = 0; i < _alarmDevices.Count; i++)
        {
            if (_alarmDevices[i] != null)
                continue;

            Debug.LogError($"{nameof(AlarmSystem)} on {name}: Alarm device at index {i} is null.", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        _alarmZone.Entered += OnZoneEntered;
        _alarmZone.Exited += OnZoneExited;

        _isThiefInside = false;
        DeactivateAllDevices();
    }

    private void OnDisable()
    {
        if (_alarmZone != null)
        {
            _alarmZone.Entered -= OnZoneEntered;
            _alarmZone.Exited -= OnZoneExited;
        }

        _isThiefInside = false;

        if (_alarmDevices != null)
            DeactivateAllDevices();
    }

    private void OnZoneEntered(ThiefMarker thief)
    {
        if (_isThiefInside)
            return;

        _isThiefInside = true;

        ActivateAllDevices();
    }

    private void OnZoneExited(ThiefMarker thief)
    {
        if (_isThiefInside == false)
            return;

        _isThiefInside = false;

        DeactivateAllDevices();
    }

    private void ActivateAllDevices()
    {
        for (int i = 0; i < _alarmDevices.Count; i++)
        {
            _alarmDevices[i].Activate();
        }
    }

    private void DeactivateAllDevices()
    {
        for (int i = 0; i < _alarmDevices.Count; i++)
        {
            _alarmDevices[i].Deactivate();
        }
    }
}