using UnityEngine.Animations.Rigging;
using UnityEngine;

public class IKcontroller : MonoBehaviour
{
    [SerializeField] private Transform _headTarget;
    [SerializeField] private Transform _handTarget;

    [SerializeField] private MultiAimConstraint _headLook;
    [SerializeField] private TwoBoneIKConstraint _handIK;

    [SerializeField] private float _smoothHead;

    private Vector3 _heatTargetStart;
    private Transform _stareTarget;

    private void Awake()
    {
        _heatTargetStart = _headTarget.localPosition;
        _handIK.weight = 0;
    }

    public void StareAt(Transform target, bool start)
    {
        if (start)
        {
            _stareTarget = target;
        }
        else if (_stareTarget != null && _stareTarget.Equals(target)) 
        {
            _stareTarget = null;
        }
    }

    public void MoveRightHand(Vector3 position, float weight)
    {
        _handTarget.position = position;
        _handIK.weight = weight;
    }

    private void Update()
    {
        if (_stareTarget == null)
        {
            _headTarget.localPosition = Vector3.Lerp(_headTarget.localPosition, _heatTargetStart, _smoothHead * Time.deltaTime);
        }
        else
        {
            _headTarget.position = Vector3.Lerp(_headTarget.position, _stareTarget.position, _smoothHead * Time.deltaTime);
        }
    }
}
