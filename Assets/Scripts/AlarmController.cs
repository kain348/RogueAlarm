using UnityEngine;

[AddComponentMenu("Game/Alarm/Alarm Controller")]
public class AlarmController : MonoBehaviour
{
    private const int Zero = 0;

    [SerializeField] private AlarmZone _alarmZone;
    [SerializeField] private AlarmDevice _alarmDevice;

    private int _thievesInside;

    private void Awake()
    {
        if (_alarmZone == null)
        {
            Debug.LogError($"{nameof(AlarmController)} on {name}: AlarmZone is not assigned.", this);
            enabled = false;

            return;
        }

        if (_alarmDevice == null)
        {
            Debug.LogError($"{nameof(AlarmController)} on {name}: AlarmDevice is not assigned.", this);
            enabled = false;

            return;
        }
    }

    private void OnEnable()
    {
        _alarmZone.Entered += OnZoneEntered;
        _alarmZone.Exited += OnZoneExited;

        _thievesInside = Zero;
        _alarmDevice.Deactivate();
    }

    private void OnDisable()
    {
        if (_alarmZone != null)
        {
            _alarmZone.Entered -= OnZoneEntered;
            _alarmZone.Exited -= OnZoneExited;
        }

        _thievesInside = Zero;

        if (_alarmDevice != null)
            _alarmDevice.Deactivate();
    }

    private void OnZoneEntered(ThiefMarker thief)
    {
        _thievesInside++;

        if (_thievesInside == 1)
            _alarmDevice.Activate();
    }

    private void OnZoneExited(ThiefMarker thief)
    {
        if (_thievesInside > Zero)
            _thievesInside--;

        if (_thievesInside == Zero)
            _alarmDevice.Deactivate();
    }
}