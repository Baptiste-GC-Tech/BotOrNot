public abstract class BON_State
{
    protected BON_CCPlayer _player;

    public void InitPlayer(BON_CCPlayer player)
    {
        _player = player;
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpState();
}
