using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private uint _speed;

    void Update()
    {
        transform.Rotate(0, _speed*Time.deltaTime, 0);
    }
}
