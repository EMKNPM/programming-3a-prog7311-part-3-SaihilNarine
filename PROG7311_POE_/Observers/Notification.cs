namespace PROG7311_POE_.Observers
{
    public class Notification : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine(message);
        }
    }
}
