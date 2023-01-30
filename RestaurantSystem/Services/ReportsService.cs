using RestaurantSystem.Interface;
using RestaurantSystem.Repository;
using System.Data.SQLite;

namespace RestaurantSystem.Services
{
    public class ReportsService : IRaport
    {
        Menu menu = new Menu();

        // HTML formato ataskaita, pagal filtrus
        public void CreateReportForFinancialDep()
        {
            SetReportFilter();
        }

        //Pasirinkimai, reikalingu ataskaitu, finansu departamentui
        private void SetReportFilter()
        {
            Console.WriteLine("Pasirinkite filtro parametrus.");
            Console.WriteLine("[1] - sios dienos uzsakymai");
            Console.WriteLine("[2] - visi uzsakymai");
            Console.WriteLine("[3] - pasirinkto stalo visi uzsakymai");

            var choise = Console.ReadLine();
            if (choise == "1")
            {
                string ldate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59).ToString();
                string fdate = DateTime.Today.AddHours(00).AddMinutes(00).AddSeconds(00).ToString();
                GetSalesByDate(fdate, ldate);
            }
            else if (choise == "2")
            {
                GetSalesByDate(string.Empty, string.Empty);
            }
            else if (choise == "3")
            {
                GetIDForFiltration();

            }
            else
            {
                Console.WriteLine("pasirinkite is pateikto meniu, ivestas pasirinkimas neatitinka galimu reiksmiu");
                Console.WriteLine("");
                SetReportFilter();
            }
        }

        //Ataskaita formuoja staleliu rezervacijos statusa
        public void GetTableReservationStatus(SQLiteConnection conn)
        {
            using (conn)
            {
                SQLiteCommand sqLiteCommand = conn.CreateCommand();
                sqLiteCommand.CommandText = @"SELECT * FROM Tables ORDER BY tableID ASC";

                using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                {
                    while (sqliteReader.Read())
                    {
                        var id = sqliteReader["tableID"];
                        var seatQnt = sqliteReader["tableNumberOfSeats"];
                        var stausRezerv = sqliteReader["tableReserveted"].ToString() == "1";

                        Console.WriteLine($"Staliuko ID: {id}  |  vietu skaicius: {seatQnt}  |  Stalelis rezervuotas: {stausRezerv}");
                    }
                }

            }

        }

        //Ataskaita pagal pasirinktas datas
        public void GetSalesByDate(string firstDate, string lastDate)
        {
            firstDate = (firstDate == string.Empty) ? "1999-01-01 00:00:00" : firstDate;
            lastDate = (lastDate == string.Empty) ? DateTime.Today.AddDays(1).ToString() : lastDate;

            string commandText = $"SELECT * FROM OrderList WHERE PaymentDate >= '{firstDate}' AND PaymentDate <= '{lastDate}';";
            DBRespositoryService.ReadDataReturnList(DBRespositoryService.CreateConnection(), commandText);

            CreateHTMLForReport();
            Console.WriteLine("Ataskaita sukurta.");
            Console.ReadLine();
            menu.MainMenu();
        }

        //Stalo ID pasirinkimas ataskaitu filtrams
        private void GetIDForFiltration()
        {
            Console.WriteLine("Pasirinkit stalo ID nuo 1 iki 7");
            string newChoise = Console.ReadLine();

            try
            {
                int newIntChoise = Convert.ToInt32(newChoise);
                if (ReturTrueIsIDIsValid(newIntChoise) == true)
                {
                    GetInfoByTableID(newIntChoise);
                }
                else
                {
                    Console.WriteLine("Ivestas skaicius neatitinka stalo ID");
                    GetIDForFiltration();
                }

            }
            catch
            {
                Console.WriteLine("Ivestas kriterijus ne skaicius");
                GetIDForFiltration();
            }
        }

        //Ataskaita pagal pasirinkta stalo ID
        private void GetInfoByTableID(int tableID)
        {
            string commandText = $"SELECT * FROM OrderList WHERE TableID = {tableID};";
            DBRespositoryService.ReadDataReturnList(DBRespositoryService.CreateConnection(), commandText);

            CreateHTMLForReport();
            Console.WriteLine("Ataskaita sukurta.");
            Console.ReadLine();
            menu.MainMenu();

        }

        //Ataskaitos formavimas, HTML formatavimas
        public void CreateHTMLForReport()
        {
            CreateFolderForRaports();
            string date = DateTime.Now.ToString("yyyy MM dd HH mm ss").Replace(" ", "");
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Raportai\Raport" + date + ".html");
            string style = @"style ='border: 1px solid #000;'";
            double totalCost = RaportRespository.reports.Sum(x => x.totalPrice);
            int totalDishes = RaportRespository.reports.Sum(x => x.foodQNT);

            string content = string.Empty;
            content += $"<h2>Ataskaita</h2>";
            content += $"<table {style}><tr {style}><th {style}>Order ID</th><th {style}>Table ID</th>" +
            $"<th {style}>Payment Date</th><th {style}>Food ID</th><th {style}>Food Name</th><th {style}>QNT</th><th {style}>Total Price</th></tr>";

            foreach (var i in RaportRespository.reports)
            {
                content += $"<tr><td>{i.orderID}</td><td>{i.tableID}</td><td>{i.paymentDate}</td><td>{i.foodID}</td><td>{i.foodName}</td><td>{i.foodQNT}</td><td>{i.totalPrice}</td></tr>";
            }
            content += $"</table><br>";
            content += $"<p></p>";
            content += $"<p>Total amount: {totalCost} EUR </p>";
            content += $"<p>Total Dishes/Drinks was ordered: {totalDishes} vnt </p>";

            File.WriteAllText(filePath, content);
        }

        //Folderio sukurimas ataskaitoms
        public void CreateFolderForRaports()
        {
            string directoryName = "Raportai";
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\" + directoryName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        private bool ReturTrueIsIDIsValid(int serchID)
        {
            string commandText = $"SELECT tableID FROM Tables WHERE tableID = {serchID}";
            string returnParameter = "tableID";
            var status = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnParameter);
            return status != 0;
        }
    }
}
