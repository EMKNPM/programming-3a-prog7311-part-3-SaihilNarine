namespace PROG7311_POE_.Observers
{
    public class ContractSubject
    {
        List<IObserver> observers = new();

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Notify(string message)
        {
            foreach (var obs in observers)
                obs.Update(message);
        }
    }
}
