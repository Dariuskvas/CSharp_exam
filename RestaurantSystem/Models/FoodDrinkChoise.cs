namespace RestaurantSystem.Models
{
    public class FoodDrinkChoise
    {
        private int _tableID;
        private int _personAtTheTable;
        private string[] _drinksList;
        private string[] _foodStarterList;
        private string[] _foodMainList;
        public int tableID
        {
            get { return _tableID; }
            set { _tableID = value; }
        }
        public int personAtTheTable
        {
            get { return _personAtTheTable; }
            set { _personAtTheTable = value; }
        }
        public string[] drinksList
        {
            get { return _drinksList; }
            set { _drinksList = value; }
        }
        public string[] foodStarterList
        {
            get { return _foodStarterList; }
            set { _foodStarterList = value; }
        }
        public string[] foodMainList
        {
            get { return _foodMainList; }
            set { _foodMainList = value; }
        }
    }
}