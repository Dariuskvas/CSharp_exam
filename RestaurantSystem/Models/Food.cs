namespace RestaurantSystem.Models
{
    public class Food
    {
        private string _foodID;
        private string _name;
        private string _foodType;
        private int _price;

        public string foodID
        {
            get { return _foodID; }
            set { _foodID = value; }
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
        public string foodType
        {
            get { return _foodType; }
            set { _foodType = value; }
        }
    }
}
