using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Worker> _startWorkers;
    [SerializeField][Range(0f, 10f)] private float _scanDelayTime;

    private Worker _newBaseBuilder;
    private WorkerSpawner _workerSpawner;
    private Queue<Worker> _freeWorkers;
    private List<Worker> _busyWorkers;
    private Queue<Copper> _resourses;
    private BaseStateMachine _stateMachine;
    private Transform _flagPosition;
    private float _runtime;
    private int _copperCount;

    public event Func<List<Copper>> ResoursesScanned;
    public event Action<Base> PlayerClicked;
    public event Action<Worker, Transform, Base> WorkerBuiltBase;

    private void Awake()
    {
        _stateMachine = new BaseStateMachine(this);
        _freeWorkers = new Queue<Worker>();
        _busyWorkers = new List<Worker>();
        _resourses = new Queue<Copper>();
    }

    private void OnEnable()
    {
        foreach (Worker worker in _startWorkers)
            AddNewWorker(worker);

        _startWorkers.Clear();
    }

    private void OnDisable()
    {
        foreach (Worker worker in _busyWorkers)
            worker.ResourseDelivered -= OnResourseDelivered;

        for (int i = 0; i < _freeWorkers.Count; i++)
        {
            Worker worker = _freeWorkers.Dequeue();
            worker.ResourseDelivered -= OnResourseDelivered;
        }

        if (_newBaseBuilder != null)
            _newBaseBuilder.CameToFlag -= OnWorkerCameToFlag;
    }

    private void Update()
    {
        _stateMachine.Run();
        _runtime += Time.deltaTime;

        if (_resourses.Count > 0 && _freeWorkers.Count > 0)
        {
            SendWorkerToResourse();
        }
        else
        {
            if (_runtime >= _scanDelayTime)
                ScanResourse();
        }
    }

    public bool TryBuildNewBase(int price)
    {
        bool isWorkerSend = false;

        if (_freeWorkers.Count > 0 && TryPayCopper(price))
        {
            _newBaseBuilder = _freeWorkers.Dequeue();
            _newBaseBuilder.TakeFlagTransform(_flagPosition);
            _newBaseBuilder.CameToFlag += OnWorkerCameToFlag;
            _newBaseBuilder.ResourseDelivered -= OnResourseDelivered;
        }

        return isWorkerSend;
    }

    public void TakeFlag(Transform flag)
    {
        _flagPosition = flag;
        _stateMachine.SetNewBaseBiuldState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerClicked?.Invoke(this);
    }

    public bool TryPayCopper(int price)
    {
        bool isPayed = false;

        if (_copperCount >= price)
        {
            _copperCount -= price;
            isPayed = true;
        }

        return isPayed;
    }

    public void AddNewWorker(Worker worker)
    {
        _freeWorkers.Enqueue(worker);
        worker.ResourseDelivered += OnResourseDelivered;
        worker.SpecifyBasePosition(transform.position);
    }

    public void RemoveWorker(Worker worker)
    {
        worker.ResourseDelivered -= OnResourseDelivered;
        _busyWorkers.Remove(worker);
    }

    public void SpawnWorker()
    {
        AddNewWorker(_workerSpawner.Spawn(transform.position));
    }

    public void TakeWorkerSpawner(WorkerSpawner workerSpawner)
    {
        _workerSpawner = workerSpawner;
    }

    private void SendWorkerToResourse()
    {
        Worker worker = _freeWorkers.Dequeue();

        worker.TakeResourse(_resourses.Dequeue());
        _busyWorkers.Add(worker);
    }

    private void ScanResourse()
    {
        List<Copper> foundedResourses = ResoursesScanned?.Invoke();

        for (int i = 0; i < foundedResourses.Count; i++)
            _resourses.Enqueue(foundedResourses[i]);

        foundedResourses.Clear();
    }

    private void OnResourseDelivered(Worker worker)
    {
        _copperCount++;
        _freeWorkers.Enqueue(worker);
        _busyWorkers.Remove(worker);
    }

    private void OnWorkerCameToFlag(Worker worker)
    {
        _stateMachine.SetWorkerBuildState();
        WorkerBuiltBase?.Invoke(worker, _flagPosition, this);
        worker.CameToFlag -= OnWorkerCameToFlag;
    }
}
