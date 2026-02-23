using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[AddComponentMenu("Game/Alarm/Alarm Zone")]
public class AlarmZone : MonoBehaviour
{
    public event Action<ThiefMarker> Entered;
    public event Action<ThiefMarker> Exited;

    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ThiefMarker thief) == false)
            return;

        Entered?.Invoke(thief);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ThiefMarker thief) == false)
            return;

        Exited?.Invoke(thief);
    }
}