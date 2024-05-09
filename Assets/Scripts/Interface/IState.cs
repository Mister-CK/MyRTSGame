namespace MyRTSGame.Model
{
    public interface IState<T>
    {
        public T GetState();
        public void SetState(T state);
    }
}