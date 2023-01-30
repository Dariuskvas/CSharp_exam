using RestaurantSystem.Services;

namespace RestaurantSystem
{
    public class Menu
    {
        //Programos pagrindiniai pasirinkimai
        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Restorano staliuku ir uzsakymu valdymo sistema");
            Console.WriteLine("Pasirinkite is meniu. [1, 2 arba 3]");
            Console.WriteLine();
            Console.WriteLine("[1] - Stalelio rezervavimas");
            Console.WriteLine("[2] - Laisvi stalai");
            Console.WriteLine("[3] - Ataskaita");
            var choise = Console.ReadLine();

            if (choise == "1")
            {
                Console.Clear();
                TableReservationService tableReservationService = new TableReservationService();
                Utilities utilities = new Utilities();
                tableReservationService.MakeTableReservation(utilities.CreateCustomerGroupsSize());
                tableReservationService.CancelReservation();

                OrderService orderService = new OrderService();
                orderService.CreateDrinkFoodOrder();

                PaymentService paymentService = new PaymentService();
                paymentService.CreatePaymentInformation();
                Console.WriteLine("Gristi i pagrindini Menu - 'ENTER'");
                Console.ReadLine();

                Menu menu = new Menu();
                menu.MainMenu();


            }
            else if (choise == "2")
            {
                Console.Clear();
                ReportsService reportsService = new ReportsService();
                reportsService.GetTableReservationStatus(DBRespositoryService.CreateConnection());
                Console.WriteLine();
                Console.WriteLine("Grizti - 'ENTER'");
                Console.ReadLine();
                Console.Clear();
                MainMenu();
            }
            else if (choise == "3")
            {
                Console.Clear();
                ReportsService reportsService = new ReportsService();
                reportsService.CreateReportForFinancialDep();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Klaida pasirinkime, pasirinkite dar kartą iš meniu.");
                Console.WriteLine();
                MainMenu();
            }
        }

    }
}
