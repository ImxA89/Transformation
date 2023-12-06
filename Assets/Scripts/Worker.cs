using System;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField][Range(1f, 10f)] private float _speed;
    [SerializeField] private float _finishDistance;

    private Vector3 _basePosition;
    private Transform _resurseForDelivery;
    private Vector3 _localPositionForTaken = new Vector3(0f, 0.5f, 0.5f);
    private bool _isResourseTaken = false;
    private Transform _flag;

    public event Action<Worker> ResourseDelivered;
    public event Action<Worker> CameToFlag;

    private void Update()
    {
        if (_resurseForDelivery != null)
        {
            if (_isResourseTaken == false)
            {
                Move(_resurseForDelivery.position);

                if (VerifyDistance(_resurseForDelivery.position))
                    PikeUpResourse();
            }
            else
            {
                Move(_basePosition);

                if (VerifyDistance(_basePosition))
                    GiveResourse();
            }
        }

        if (_flag != null)
        {
            Move(_flag.position);

            if (VerifyDistance(_flag.position))
            {
                CameToFlag?.Invoke(this);
                _flag = null;
            }
        }
    }

    public void SpecifyBasePosition (Vector3 basePosition)
    {
        _basePosition = basePosition;
    }

    public void TakeResourse (Copper copper)
    {
        _resurseForDelivery = copper.transform;
    }

    public void TakeFlagTransform(Transform flag)
    {
        _flag = flag;
    }

    private void Move(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    }

    private void PikeUpResourse()
    {
        _resurseForDelivery.SetParent(transform);
        _resurseForDelivery.SetLocalPositionAndRotation(_localPositionForTaken, Quaternion.identity);
        _isResourseTaken = true;
    }

    private bool VerifyDistance(Vector3 target)
    {
        bool isFinish = false;

        if (Vector3.Distance(transform.position, target) <= _finishDistance)
            isFinish = true;

        return isFinish;
    }

    private void GiveResourse()
    {
        Destroy(_resurseForDelivery.gameObject);
        _isResourseTaken = false;
        ResourseDelivered?.Invoke(this);
    }
}
