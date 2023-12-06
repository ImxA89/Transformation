using System.Collections.Generic;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private GameObject _flagPrefab;

    private Dictionary<Base, GameObject> _baseFlags;

    private void Start()
    {
        _baseFlags = new Dictionary<Base, GameObject>();
    }

    public Transform ShowFlag(Vector3 flagPosition, Base currentBase)
    {
        GameObject flag;

        if (_baseFlags.ContainsKey(currentBase) && _baseFlags[currentBase] != null)
        {
            MoveFlag(flagPosition, _baseFlags[currentBase]);
            flag = _baseFlags[currentBase];
        }
        else
        {
            flag = CreatFlag(flagPosition);
            _baseFlags[currentBase] = flag;
        }

        return flag.transform;
    }

    private GameObject CreatFlag(Vector3 flagPosition)
    {
      return Instantiate(_flagPrefab, flagPosition, Quaternion.identity);
    }

    private void MoveFlag (Vector3 newFlagPosition, GameObject flag)
    {
        flag.transform.position = newFlagPosition;
    }
}
