using RestaurantSystem.Models;

namespace RestaurantSystem.Services
{
    public class OrderService
    {
        public void CreateDrinkFoodOrder()                // Pagrindine, ateina is menu
        {
            Utilities utilities = new Utilities();
            var tableID = 1;                            //Reikalingas pasileisti ciklui. //Galima naudoti do{}While su if'u bet man taip atrode paprasciau ir isvengiama kas karta papildomo if tikrinimo

            while (tableID != 0)                        //Maisto uzsakymai daromi, jei staliukas rezervuotas bet uzsakymo nera         
            {
                string commandText = $"SELECT *FROM Tables WHERE tableReserveted = 1 AND orderMade=0;";
                tableID = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, "tableID");

                //is visu pasirinkimu sukuriu Dictionary ir susumuoju staliuko pasirinkimus
                Dictionary<string, int> drink = CountAllDifferentChoices(utilities.CreateMealChoise().drinksList);
                Dictionary<string, int> starter = CountAllDifferentChoices(utilities.CreateMealChoise().foodStarterList);
                Dictionary<string, int> main = CountAllDifferentChoices(utilities.CreateMealChoise().foodMainList);

                Dictionary<string, int> totalList = Marge3DictionariesToOne(drink, starter, main);                //Klientu galutinio uzsakymo sarasas su kiekiais
                int newOrderListID = GetLastOrderID() + 1;                                                        //Susigeneruoju naujo uzsakymoID

                foreach (var item in totalList)
                {
                    Order order = new Order();
                    order.orderListID = newOrderListID;
                    order.orderDate = DateTime.Now;
                    order.foodDrinkID = item.Key;
                    order.foodDrinkQNT = item.Value;
                    order.foodDrinkPrice = GetDoubleFromDrinkOrFoodDB(item.Key, "Price");
                    order.tableID = tableID;
                    order.foodDrinkName = GetStringFromDrinkOrFoodDB(item.Key, "Name");
                    AddOrderInformationToDB(order);
                }
                ChangeOrderStatus(tableID);                                                                        // pakeiciu uzsakymo statusa Tables lenteleje
                AddOrderListID(tableID, newOrderListID);                                                           //i Tables irasau orderListID
            }
        }

        private void ChangeOrderStatus(int tableID)
        {
            //pasitikrinu rezervacijos statusa ir pakeiciu i priesinga
            string commandText = $"SELECT orderMade FROM Tables WHERE tableID = {tableID}";
            string returnParameter = "orderMade";
            var status = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnParameter);

            int newStatus = (status == 1) ? 0 : 1;
            string commandTextUpdate = $"UPDATE Tables SET orderMade = {newStatus} WHERE tableID={tableID}";
            DBRespositoryService.UpdateData(DBRespositoryService.CreateConnection(), commandTextUpdate);
        }

        private void AddOrderListID(int id, int orderListId)
        {
            //Irasau orderLisID i staliuku{Tables} lentele
            string commandTextUpdate = $"UPDATE Tables SET orderListID = {orderListId} WHERE tableID={id}";
            DBRespositoryService.UpdateData(DBRespositoryService.CreateConnection(), commandTextUpdate);
        }

        public Dictionary<string, int> CountAllDifferentChoices(string[] list)
        {
            //is paduotu listu sukuriu Dictionary kuris suskaiciuoja kiek kartu buvo pasirinktas patiekalas/gerimas prie stalo
            Dictionary<string, int> totalTable = new Dictionary<string, int>();
            foreach (string choise in list)
            {
                if (Convert.ToInt32(choise.Substring(1)) != 0)
                {
                    if (totalTable.ContainsKey(choise))
                        totalTable[choise]++;
                    else
                        totalTable[choise] = 1;
                }
            }
            return totalTable;
        }

        private Dictionary<string, int> Marge3DictionariesToOne(Dictionary<string, int> dic1, Dictionary<string, int> dic2, Dictionary<string, int> dic3)
        {
            Dictionary<string, int> combinedDictionary = dic1.Concat(dic2).Concat(dic3).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Sum(x => x.Value));
            return combinedDictionary;
        }

        private int GetLastOrderID()
        {
            string commandText = $"SELECT OrderListID FROM OrderList ORDER BY OrderListID DESC LIMIT 1"; ;
            return DBRespositoryService.ReadDataCountReturnSum(DBRespositoryService.CreateConnection(), commandText);
        }

        private void AddOrderInformationToDB(Order orderInfo)
        {
            string commandText = $"INSERT INTO OrderList(OrderListID, OrderDate, FoodDrinkID, FoodDrinkIDQNT, FoodDrinkPrice, TableID, FoodDrinkName) " +
                $"VALUES ({orderInfo.orderListID}, '{(orderInfo.orderDate).ToString()}', '{orderInfo.foodDrinkID}', {orderInfo.foodDrinkQNT}, {orderInfo.foodDrinkPrice}, {orderInfo.tableID}, '{orderInfo.foodDrinkName}');";
            DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandText);
        }

        public double GetDoubleFromDrinkOrFoodDB(string foodID, string returKey)
        {
            string commandText;
            string returnValue = returKey;
            if (foodID.Substring(0, 1) == "F")
            {
                commandText = $"SELECT Price FROM Food WHERE FoodID = '{foodID}'";
            }
            else
            {
                commandText = $"SELECT Price FROM Drink WHERE DrinkID = '{foodID}'";
            }
            return Convert.ToDouble(DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnValue));
        }

        public string GetStringFromDrinkOrFoodDB(string foodID, string returKey)
        {
            string commandText;
            string returnValue = returKey;
            if (foodID.Substring(0, 1) == "F")
            {
                commandText = $"SELECT Name FROM Food WHERE FoodID = '{foodID}'";
            }
            else
            {
                commandText = $"SELECT Name FROM Drink WHERE DrinkID = '{foodID}'";
            }
            return DBRespositoryService.ReadDataReturnString(DBRespositoryService.CreateConnection(), commandText, returnValue);
        }

    }
}