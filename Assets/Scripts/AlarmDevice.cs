using UnityEngine;

[AddComponentMenu("Game/Alarm/Alarm Device")]
public abstract class AlarmDevice : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
}