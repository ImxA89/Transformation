using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private FlagSetter _flagSetter;
    [SerializeField] private ClickListener _clickListener;
    [SerializeField] private WorkerSpawner _workerSpawner;
    [SerializeField] private Base _startBase;
    [SerializeField] private ResourseCollector _resourseCollector;

    private List<Base> _bases;
    private Base _waitingForFlagBase;

    public void Awake()
    {
        _bases = new List<Base>();
        _startBase.TakeWorkerSpawner(_workerSpawner);
    }

    private void OnEnable()
    {
        AddNewBase(_startBase);
    }

    private void OnDisable()
    {
        foreach (Base currentBase in _bases)
        {
            currentBase.ResoursesScanned -= OnResourseScanned;
            currentBase.PlayerClicked -= OnPlayerBaseClicked;
            currentBase.WorkerBuiltBase -= OnWorkerBuiltBase;
        }


        _clickListener.MapClicked -= OnMapClicked;
    }

    private void AddNewBase(Base newBase)
    {
        newBase.PlayerClicked += OnPlayerBaseClicked;
        newBase.ResoursesScanned += OnResourseScanned;
        _bases.Add(newBase);
    }

    private List<Copper> OnResourseScanned()
    {
        List<Copper> resourses;

        return resourses = _resourseCollector.GiveResourses();
    }

    private void OnPlayerBaseClicked(Base clickedBase)
    {
        _clickListener.MapClicked += OnMapClicked;
        _waitingForFlagBase =clickedBase;
    }

    private void OnMapClicked(Vector3 flagPosition)
    {
        Transform flag = _flagSetter.ShowFlag(flagPosition, _waitingForFlagBase);
        _waitingForFlagBase.TakeFlag(flag);
        _waitingForFlagBase.WorkerBuiltBase += OnWorkerBuiltBase;
    }

    private void OnWorkerBuiltBase(Worker worker, Transform flagPosition, Base baseOfCreater)
    {
        Vector3 basePosition = flagPosition.position;

        Destroy(flagPosition.gameObject);
        Base newBase = Instantiate(_basePrefab, basePosition, Quaternion.identity);

        newBase.TakeWorkerSpawner(_workerSpawner);
        newBase.AddNewWorker(worker);
        AddNewBase(newBase);

        _clickListener.MapClicked -= OnMapClicked;
        baseOfCreater.WorkerBuiltBase -= OnWorkerBuiltBase;
    }
}
