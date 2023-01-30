namespace RestaurantSystem.Models
{
    public class Drink
    {
        private string _drinkID;
        private string _name;
        private int _price;

        public string drinkID
        {
            get { return _drinkID; }
            set { _drinkID = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int price
        {
            get { return _price; }
            set { _price = value; }
        }
    }
}
