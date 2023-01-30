namespace RestaurantSystem.Interface
{
    public interface IService
    {
        public bool GetStatusTheDinnerEnd();
        public bool GetPayment();
        public void StartStopWatchForEmailSend(string checkNumber);
    }
}
