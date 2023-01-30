namespace RestaurantSystem.Models
{
    public class Order
    {
        private int _orderListID;
        private DateTime _orderDate;
        private string _foodDrinkID;
        private int _foodDrinkQNT;
        private double _foodDrinkPrice;
        private int _tableID;
        private string _foodDrinkName;

        public int orderListID
        {
            get { return _orderListID; }
            set { _orderListID = value; }
        }
        public DateTime orderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }
        public string foodDrinkID
        {
            get { return _foodDrinkID; }
            set { _foodDrinkID = value; }
        }
        public int foodDrinkQNT
        {
            get { return _foodDrinkQNT; }
            set { _foodDrinkQNT = value; }
        }
        public double foodDrinkPrice
        {
            get { return _foodDrinkPrice; }
            set { _foodDrinkPrice = value; }
        }

        public int tableID
        {
            get { return _tableID; }
            set { _tableID = value; }
        }

        public string foodDrinkName
        {
            get { return _foodDrinkName; }
            set { _foodDrinkName = value; }
        }
    }
}
