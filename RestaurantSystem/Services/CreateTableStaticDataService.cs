using RestaurantSystem.Models;

namespace RestaurantSystem.Services
{
    public class CreateTableStaticDataService
    {
        //Sukuriu DB lenteles ir reikalingas reiksmes
        public void CreateAllTableStaticData()
        {
            CreateTables();
            CreateFood();
            CreateDrink();
            DBRespositoryService.CreateTableOrderList(DBRespositoryService.CreateConnection());
        }

        private void CreateTables()
        {
            DBRespositoryService.CreateTableTables(DBRespositoryService.CreateConnection());
            int[] tableID = { 1, 2, 3, 4, 5, 6, 7 };
            int[] tableNumberOfSeats = { 4, 4, 2, 6, 1, 4, 2 };
            for (var i = 0; i <= tableID.Length; i++)
            {
                try
                {
                    Tables tables = new Tables();
                    tables.tableID = tableID[i];
                    tables.tableNumberOfSeats = tableNumberOfSeats[i];
                    tables.orderMade = 0;
                    tables.tableReserveted = 0;
                    tables.orderListID = 0;
                    string commandTextValue = $"INSERT INTO TABLES (tableID, tableNumberOfSeats, tableReserveted, orderMade, orderListID) VALUES ({tables.tableID}, {tables.tableNumberOfSeats}, {tables.tableReserveted}, {tables.orderMade}, {tables.orderListID});";
                    DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandTextValue);
                }
                catch
                {
                }
            }
        }

        private void CreateFood()
        {
            DBRespositoryService.CreateTableFood(DBRespositoryService.CreateConnection());
            string[] foodID = { "F1", "F2", "F3", "F4", "F5" };
            string[] name = { "Salotos1", "Sriuba", "Kelsnys1", "Kepsnys2", "Žuvis" };
            int[] price = { 5, 5, 10, 12, 12 };
            string[] foodType = { "Starteris", "Starteris", "Pagrindinis", "Pagrindinis", "Pagrindinis" };
            for (var i = 0; i <= foodID.Length; i++)
            {
                try
                {
                    Food food = new Food();
                    food.foodID = foodID[i];
                    food.name = name[i];
                    food.price = price[i];
                    food.foodType = foodType[i];
                    string commandTextValue = $"INSERT INTO Food (FoodID, Name, FoodType, Price) VALUES ('{food.foodID}','{food.name}','{food.foodType}', {Convert.ToInt32(food.price)});";
                    DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandTextValue);
                }
                catch
                {
                }
            }
        }

        private void CreateDrink()
        {
            DBRespositoryService.CreateTableDrink(DBRespositoryService.CreateConnection());
            string[] drinkID = { "D1", "D2", "D3", "D4", "D5", "D6" };
            string[] name = { "Gazuotas mineralinis", "Stalo vanduo", "Cola", "Sprite", "Vynas", "Alus" };
            int[] price = { 4, 3, 5, 5, 10, 10 };
            for (var i = 0; i <= drinkID.Length; i++)
            {
                try
                {
                    Drink drink = new Drink();
                    drink.drinkID = drinkID[i];
                    drink.name = name[i];
                    drink.price = price[i];
                    string commandTextValue = $"INSERT INTO Drink (DrinkID, Name, Price) VALUES ('{drink.drinkID}', '{drink.name}', {Convert.ToInt32(drink.price)});";
                    DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandTextValue);
                }
                catch
                {
                }
            }
        }

    }
}
