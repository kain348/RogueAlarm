using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Game/Movement/Thief Patroller")]
public class ThiefPatroller : MonoBehaviour
{
    private const float ReachedPointSqrDistance = 0.1f;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 2f;

    [Header("Path Settings")]
    [SerializeField] private Transform _waypointsRoot;

    private Transform[] _waypoints;
    private Rigidbody _rigidbody;
    private int _currentIndex;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (_waypointsRoot == null)
        {
            Debug.LogError($"{nameof(ThiefPatroller)} on {name}: waypoints root is not assigned.", this);
            enabled = false;

            return;
        }

        int count = _waypointsRoot.childCount;

        if (count == 0)
        {
            Debug.LogError($"{nameof(ThiefPatroller)} on {name}: waypoints root has no children.", this);
            enabled = false;

            return;
        }

        _waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            _waypoints[i] = _waypointsRoot.GetChild(i);
        }
    }

    private void FixedUpdate()
    {
        if (_waypoints == null || _waypoints.Length == 0)
            return;

        Transform targetPoint = _waypoints[_currentIndex];

        Vector3 currentPosition = _rigidbody.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, targetPoint.position, _speed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(nextPosition);

        Vector3 toTarget = targetPoint.position - currentPosition;

        if (toTarget.sqrMagnitude < ReachedPointSqrDistance)
            MoveToNextPoint();

        Vector3 moveDir = targetPoint.position - transform.position;
        moveDir.y = 0;

        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDir);
    }

    private void MoveToNextPoint()
    {
        _currentIndex++;

        if (_currentIndex >= _waypoints.Length)
            _currentIndex = 0;
    }
}