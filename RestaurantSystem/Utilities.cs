using RestaurantSystem.Models;
using RestaurantSystem.Services;

namespace RestaurantSystem
{
    public class Utilities
    {
        public int currentTime = 40;

        public static int CreateRandomNumber(int minN, int maxN)
        {
            Random random = new Random();
            return random.Next(minN, maxN);
        }

        //sugeneruoja 8 grupes po atsitiktini skaiciu zmoniu
        public int[] CreateCustomerGroupsSize()
        {
            int customerGroupeQnt = 8;
            int[] customerGroupsSize = new int[customerGroupeQnt];
            int minCustomerQNT = 1;
            int maxCustomerQNT = 8;
            for (int i = 0; i < customerGroupeQnt; i++)
            {
                customerGroupsSize[i] = CreateRandomNumber(minCustomerQNT, maxCustomerQNT);
            }
            return customerGroupsSize;
        }

        // sugeneruoju stalo uzsakymo objekta
        public FoodDrinkChoise CreateMealChoise()
        {
            int tableID = 0;
            FoodDrinkChoise foodDrinkChoise = new FoodDrinkChoise();

            string commandText = $"SELECT *FROM Tables WHERE tableReserveted = 1 AND orderMade=0;";
            tableID = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, "tableID");
            var peopleAtTheTable = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, "tableSeatsOcupate");
            string[] drinkIDList = GenerateDriksChoise(peopleAtTheTable, 0, 6);
            string[] starterIDList = GenerateFoodChoise(peopleAtTheTable, 0, 3);
            string[] foodMainIDList = GenerateFoodChoise(peopleAtTheTable, 3, 6);


            foodDrinkChoise.tableID = tableID;
            foodDrinkChoise.personAtTheTable = peopleAtTheTable;
            foodDrinkChoise.drinksList = drinkIDList;
            foodDrinkChoise.foodStarterList = starterIDList;
            foodDrinkChoise.foodMainList = foodMainIDList;

            return foodDrinkChoise;
        }

        //sukurti gerimu pairinkimu lista, pagal sedinciu zmoniu skaiciu. 0 reiksme reiskia klientas nenorejo gerimo
        private string[] GenerateDriksChoise(int peopleAtTheTable, int firstDrinkID, int lirstDrinkID)
        {

            string[] CustomerDriksChoise = new string[peopleAtTheTable];

            for (int i = 0; i < peopleAtTheTable; i++)
            {
                CustomerDriksChoise[i] = $"D{CreateRandomNumber(firstDrinkID, lirstDrinkID)}";
            }
            return CustomerDriksChoise;

        }

        //sukurti maisto pairinkimu lista, pagal sedinciu zmoniu skaiciu. 0 reiksme reiskia klientas nenorejo
        private string[] GenerateFoodChoise(int peopleAtTheTable, int firstFoodId, int lastFoodId)
        {
            //sukurti gerimus
            string[] CustomerFoodChoise = new string[peopleAtTheTable];

            for (int i = 0; i < peopleAtTheTable; i++)
            {
                CustomerFoodChoise[i] = $"F{CreateRandomNumber(firstFoodId, lastFoodId)}";
            }
            return CustomerFoodChoise;
        }

        //Sukuriamas intervalas nutemis kiek zmones pietauja
        public int CreateTimeSegments()
        {
            return currentTime > 80 ? currentTime = 40 : currentTime += 5;
        }
    }
}
