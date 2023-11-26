using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private uint _speed;

    void Update()
    {
        transform.Translate(0,0, _speed * Time.deltaTime);
    }
}