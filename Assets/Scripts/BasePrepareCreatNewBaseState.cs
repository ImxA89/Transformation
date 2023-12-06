public class BasePrepareCreatNewBaseState : IBaseState
{
    private int _copperCountForNewBase = 5;
    private Base _base;
    private bool _isSent;

    public BasePrepareCreatNewBaseState(Base currentBase)
    {
        _base = currentBase;
    }

    public void Enter()
    {
        _isSent = false;
    }

    public void Run()
    {
        if (_isSent == false && _base.TryBuildNewBase(_copperCountForNewBase))
        {
            _isSent = true;
        }
    }
}
