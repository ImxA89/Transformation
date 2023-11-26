using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _newScale;
    private float _runningTime;

    private void OnValidate()
    {
        if(_speed < 0)
            _speed = -_speed;
    }

    private void Update()
    {
        _runningTime += Time.deltaTime;
        _newScale= 1+_speed* _runningTime;
        transform.localScale = new Vector3(_newScale,_newScale,_newScale);
    }
}
