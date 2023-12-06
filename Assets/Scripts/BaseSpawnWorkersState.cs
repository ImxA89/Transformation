public class BaseSpawnWorkersState : IBaseState
{
    private int _copperForSpawnWorker =3;
    private Base _base;

    public BaseSpawnWorkersState (Base currentbase)
    {
     _base = currentbase;
    }

    public void Enter() {}

    public void Run()
    {
        if (_base.TryPayCopper(_copperForSpawnWorker))
        {
            _base.SpawnWorker();
        }
    }
}
