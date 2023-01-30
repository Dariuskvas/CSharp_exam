using RestaurantSystem.Interface;
using RestaurantSystem.Repository;
using System.Diagnostics;

namespace RestaurantSystem.Services
{
    public class PaymentService : IService
    {
        Utilities utilities = new Utilities();
        public void CreatePaymentInformation()
        {
            Console.Clear();
            int tableID;
            do
            {
                tableID = ReturnTablesValueForPayment("tableID");
                var orderListID = ReturnTablesValueForPayment("orderListID");

                if (GetStatusTheDinnerEnd() == true && tableID != 0)
                {
                    Console.WriteLine("-------------------------------------------------------------------------");
                    Console.WriteLine($"Stalas ID:{tableID}, baige pietauti. Uzsakymo numeris: {orderListID}.");

                    var totalTableAmount = GetAmountForPayment(orderListID);            //gaunu suma apmokejimui
                    Console.WriteLine($"Moketina suma: {totalTableAmount} EUR");

                    if (GetPayment() == true)
                    {
                        var orderDate = GetOrderDate(orderListID);                           //gaunu uzsakymo data
                        var paymentDate = AddRandomMinutes(orderDate);                       //Sugeneruoju pabaigos/mokejimo data
                        AddPaymentDateToDB(orderListID, paymentDate);                        //pridedu mokejimo data i DB
                        Console.WriteLine("Mokejimas uz pietus gautas.");

                        string checkNumber = CreateCheck(orderListID);                       //Sukuriamas cekis, txt failas

                        Console.WriteLine("Pagal gauta uzsakymo nr. klientams suformuojamas cekis/suvestine");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Jei norite issiusti ceki paspauskite ENTER");
                        Console.ForegroundColor = ConsoleColor.White;

                        StartStopWatchForEmailSend(checkNumber);                                            //Penkiu sekundziu tarpas pasirinkti r nori issiusti laiska
                        MakeTableFree(tableID);
                    }
                    Console.WriteLine($"Stalas ID:{tableID} atsilaisvino. Tikrinamas laukianciuju sarasa ar galima pasodinti");
                    Console.WriteLine("");
                    TableReservationService tableReservationService = new TableReservationService();
                    OrderService orderService = new OrderService();
                    if (tableReservationService.ChekWaitingPeople() == true)
                    {
                        tableReservationService.MakeTableReservation();
                        orderService.CreateDrinkFoodOrder();
                    }
                }
                else
                {
                    break;
                }
            } while (true);

        }

        private int ReturnTablesValueForPayment(string returnValue)
        {
            string commandText = $"SELECT *FROM Tables WHERE tableReserveted = 1 AND orderMade=1;";
            var values = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnValue);
            return values;
        }

        public bool GetStatusTheDinnerEnd()
        {
            return true;
        }

        public bool GetPayment()
        {
            return true;
        }

        private double GetAmountForPayment(int orderListID)
        {
            string commandText = $"SELECT SUM(FoodDrinkIDQNT * FoodDrinkPrice) as totalAmount FROM OrderList WHERE OrderListID = {orderListID}";
            var amount = DBRespositoryService.ReadDataCountReturnSum(DBRespositoryService.CreateConnection(), commandText);
            return Convert.ToDouble(amount);
        }

        private DateTime GetOrderDate(int orderListID)
        {
            string commandText = $"SELECT OrderDate FROM OrderList WHERE OrderListID = {orderListID} LIMIT 1;";
            var orderDate = DBRespositoryService.ReadDataReturnString(DBRespositoryService.CreateConnection(), commandText, "OrderDate");
            return DateTime.Parse(orderDate);
        }
        public DateTime AddRandomMinutes(DateTime dateTime)
        {
            return dateTime.AddMinutes(utilities.CreateTimeSegments());
        }

        private void AddPaymentDateToDB(int orderListID, DateTime paymentDate)
        {
            string commandText = $"UPDATE OrderList SET PaymentDate = '{paymentDate.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE orderListID = {orderListID}";
            DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandText);
        }

        private void MakeTableFree(int tableID)
        {
            string commandText = $"UPDATE Tables SET tableReserveted = 0, orderListID = 0, orderMade=0, tableSeatsOcupate=0 WHERE tableID = {tableID}";
            DBRespositoryService.InsertData(DBRespositoryService.CreateConnection(), commandText);
        }

        private string CreateCheck(int orderListID)
        {
            CreateFolderForCecks();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\cekiai\" + orderListID.ToString() + ".txt");
            File.Create(filePath).Close();

            string title = $"Uzsakymo nr: {orderListID}";
            string title2 = "----------------------------";
            string title3 = " ";
            string[] commanTitle = { title, title2, title3 };

            File.AppendAllLines(filePath, commanTitle);

            string commandText = $"SELECT * FROM OrderList WHERE OrderListID = {orderListID};";
            DBRespositoryService.ReadDataReturnList(DBRespositoryService.CreateConnection(), commandText);

            foreach (var i in RaportRespository.reports)
            {
                string body = $"{i.foodName}";
                string body2 = $"{i.foodQNT}vnt x {i.totalPrice / i.foodQNT} = {i.totalPrice} EUR ";
                string body3 = $" ";
                string[] strings = { body, body2, body3 };
                File.AppendAllLines(filePath, strings);
            }

            string end = $"Bendra moketina suma: {GetAmountForPayment(orderListID)} EUR ";
            File.AppendAllText(filePath, end);

            return filePath;
        }

        public void CreateFolderForCecks()
        {
            string directoryName = "Cekiai";
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\" + directoryName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        public void StartStopWatchForEmailSend(string checkNumber)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                if (Console.KeyAvailable)                                       //jei ivedamas bet kokia reiksme
                {
                    Console.ReadKey();
                    MailService mailService = new MailService();
                    mailService.sendEmail(checkNumber);                         //cekis issiunciamas klientui
                    break;
                }
                if (stopwatch.ElapsedMilliseconds > 3000)                       //salyga tenkinama po 5s
                {
                    break;
                }
            }
        }
    }
}
