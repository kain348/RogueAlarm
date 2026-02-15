using UnityEngine;

[RequireComponent(typeof(Collider))]
[AddComponentMenu("Game/Alarm/AlarmZone")]
public class AlarmZone : MonoBehaviour
{
    [Header("Alarm Settings")]
    [SerializeField] private AlarmDevice _alarmDevice;

    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;

        if (_alarmDevice == null)
        {
            Debug.LogWarning($"{typeof(AlarmZone)} on {name}: AlarmDevice is not assigned.", this);
            enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThiefMarker thief))
            _alarmDevice?.Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ThiefMarker thief))
            _alarmDevice?.Diactivate();
    }
}