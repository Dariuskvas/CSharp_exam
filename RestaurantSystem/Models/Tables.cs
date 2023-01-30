namespace RestaurantSystem.Models
{
    public class Tables
    {
        private int _tableID;
        private int _tableNumberOfSeats;
        private int _tableReserveted;
        private int _orderListID;
        private int _orderMade;
        private int _tableSeatsOcupate;

        public int tableID
        {
            get { return _tableID; }
            set { _tableID = value; }
        }
        public int tableNumberOfSeats
        {
            get { return _tableNumberOfSeats; }
            set { _tableNumberOfSeats = value; }
        }
        public int tableReserveted
        {
            get { return _tableReserveted; }
            set { _tableReserveted = value; }
        }
        public int orderListID
        {
            get { return _orderListID; }
            set { _orderListID = value; }
        }
        public int orderMade
        {
            get { return _orderMade; }
            set { _orderMade = value; }
        }
        public int tableSeatsOcupate
        {
            get { return _tableSeatsOcupate; }
            set { _tableSeatsOcupate = value; }
        }

    }
}
