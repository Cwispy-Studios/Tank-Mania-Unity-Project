namespace CwispyStudios.TankMania.GameEvents
{
  public interface IGameEventListener<T>
  {
    public void OnEventRaised( T item );
  }
}
