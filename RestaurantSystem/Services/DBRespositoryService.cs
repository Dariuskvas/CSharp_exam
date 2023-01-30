using RestaurantSystem.Models;
using RestaurantSystem.Repository;
using System.Data.SQLite;

namespace RestaurantSystem.Services
{
    public static class DBRespositoryService
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqLiteConn;
            sqLiteConn = new SQLiteConnection(@"Data Source= ..\..\..\..\database_RestaurantSystem.db; Version=3; new =false; Cpmpress =True;");
            try
            {
                sqLiteConn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(5000);
                Menu menu = new Menu();
                menu.MainMenu();
            }
            return sqLiteConn;
        }

        public static bool IsDatabaseExists(string connectionString)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        public static void CreateTableTables(SQLiteConnection conn)
        {
            SQLiteCommand sqLiteCommand;
            string createSQL = "CREATE TABLE Tables(tableID INT, tableNumberOfSeats INT, tableReserveted INT, orderListID INT, orderMade INT, tableSeatsOcupate INT)";
            sqLiteCommand = conn.CreateCommand();
            sqLiteCommand.CommandText = createSQL;
            sqLiteCommand.ExecuteNonQuery();
            Console.WriteLine("Done");
        }

        public static void CreateTableFood(SQLiteConnection conn)
        {
            SQLiteCommand sqLiteCommand;
            string createSQL = "CREATE TABLE Food(FoodID VARCHAR(3), Name VARCHAR(32), Price INT, FoodType VARCHAR(32))";
            sqLiteCommand = conn.CreateCommand();
            sqLiteCommand.CommandText = createSQL;
            sqLiteCommand.ExecuteNonQuery();

        }

        public static void CreateTableDrink(SQLiteConnection conn)
        {
            SQLiteCommand sqLiteCommand;
            string createSQL = "CREATE TABLE Drink(DrinkID VARCHAR(3), Name VARCHAR(32), Price INT)";
            sqLiteCommand = conn.CreateCommand();
            sqLiteCommand.CommandText = createSQL;
            sqLiteCommand.ExecuteNonQuery();
        }

        public static void CreateTableOrderList(SQLiteConnection conn)
        {
            SQLiteCommand sqLiteCommand;
            string createSQL = "CREATE TABLE OrderList(OrderListID INT, OrderDate VARCHAR(32), FoodDrinkID VARCHAR(3), FoodDrinkIDQNT INT, FoodDrinkPrice INT, PaymentDate VARCHAR(32), TableID INT, FoodDrinkName VARCHAR(32))";
            sqLiteCommand = conn.CreateCommand();
            sqLiteCommand.CommandText = createSQL;
            sqLiteCommand.ExecuteNonQuery();

        }

        public static int ReadDataReturnValue(SQLiteConnection conn, string commandText, string returnParameter)
        {
            int number = 0;
            using (conn)
            {
                SQLiteCommand sqLiteCommand = conn.CreateCommand();
                sqLiteCommand.CommandText = commandText;

                using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                {
                    while (sqliteReader.Read())
                    {
                        number = Convert.ToInt32(sqliteReader[returnParameter]);
                    }
                }
            }
            return number;
        }

        public static string ReadDataReturnString(SQLiteConnection conn, string commandText, string returnParameter)
        {
            string number = string.Empty;
            using (conn)
            {
                SQLiteCommand sqLiteCommand = conn.CreateCommand();
                sqLiteCommand.CommandText = commandText;

                using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                {
                    if (sqliteReader.HasRows)
                    {
                        sqliteReader.Read();
                        number = sqliteReader[returnParameter].ToString();
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                }
            }
            return number;

        }

        public static int ReadDataCountReturnSum(SQLiteConnection conn, string commandText)
        {
            try
            {
                int number = 0;
                using (conn)
                {
                    SQLiteCommand sqLiteCommand = conn.CreateCommand();
                    sqLiteCommand.CommandText = commandText;

                    using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                    {
                        if (sqliteReader.Read())
                        {
                            number = sqliteReader.GetInt32(0);
                        }
                    }

                }
                return number;
            }
            catch
            {
                return 0;
            }
        }

        public static void UpdateData(SQLiteConnection conn, string commandText)
        {
            try
            {
                using (conn)
                {

                    SQLiteCommand sqLiteCommand = conn.CreateCommand();
                    sqLiteCommand.CommandText = commandText;
                    sqLiteCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void InsertData(SQLiteConnection conn, string commandTextValue)
        {
            using (conn)
            {
                SQLiteCommand sqLiteCommand;
                sqLiteCommand = conn.CreateCommand();
                sqLiteCommand.CommandText = commandTextValue;
                sqLiteCommand.ExecuteNonQuery();
            }

        }

        public static void ReadDataReturnList(SQLiteConnection conn, string commandText)
        {
            string x = "";
            RaportRespository.reports.Clear();

            using (conn)
            {
                SQLiteCommand sqLiteCommand = conn.CreateCommand();
                sqLiteCommand.CommandText = commandText;
                using (SQLiteDataReader sqliteReader = sqLiteCommand.ExecuteReader())
                {
                    while (sqliteReader.Read())
                    {
                        Report report = new Report();
                        report.orderID = Convert.ToInt32(sqliteReader["OrderListID"]);
                        report.foodID = sqliteReader["FoodDrinkID"].ToString();
                        report.foodQNT = Convert.ToInt32(sqliteReader["FoodDrinkIDQNT"]);
                        report.totalPrice = Convert.ToInt32(sqliteReader["FoodDrinkPrice"]) * Convert.ToInt32(sqliteReader["FoodDrinkIDQNT"]);
                        report.paymentDate = sqliteReader["PaymentDate"].ToString();
                        report.tableID = Convert.ToInt32(sqliteReader["TableID"]);
                        report.foodName = sqliteReader["FoodDrinkName"].ToString();
                        RaportRespository.reports.Add(report);
                    }
                }
            }

        }

    }
}
