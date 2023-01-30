namespace RestaurantSystem.Services
{
    public class TableReservationService
    {
        private static int[] _custGroupsArray;
        private List<int> _custGroupsWaiting = new List<int>();

        public int[] custGroupsArray
        {
            get { return _custGroupsArray; }
            set { _custGroupsArray = value; }
        }

        public void MakeTableReservation(int[] newCustomerArray)
        {
            _custGroupsArray = newCustomerArray;
            PrintWaitingCustomerGrups();                    // Isvedu klientu grupiu, esanciu eileje, zmoniu skaiciu
            MakeReservation();
        }

        public void MakeTableReservation()
        {
            MakeReservation();
        }

        public bool ChekWaitingPeople()
        {
            return (_custGroupsArray.Length != 0 || _custGroupsArray != null);
        }

        public void MakeReservation()
        {
            while (_custGroupsArray.Length > 0)                                                                  //skirstau rezerva
            {

                if (CheckIfWereEmptyPlaceInRestaurant(_custGroupsArray[0]) == true)                              //Tikrinu ar yra laisvu vietu, suma laisvu vietu
                {
                    var id = SelecteTableIDForReservation(_custGroupsArray[0]);
                    // pasirenku ID staliuko kuri reervuosiu
                    if (id != 0)                                                                                 // if ID = 0 netelpa prie staliu
                    {
                        ChangeTableReservationStatus(id);                                                        // rezervuoju staliuka
                        AddPersonQNTOfGroupToDB(id, _custGroupsArray[0]);
                        DeleteFirstFromArray(_custGroupsArray);                                               //istrinu is kirminio listo 
                    }
                    else                                                                                         //Sujungineju staliukus ir rezervuoju
                    {
                        JoinTable(_custGroupsArray[0]);
                    }
                }
                else if (CheckIfWereEnofPlaceInRestaurant(_custGroupsArray[0]) == true)                         //Tikrinu ar yra tiek vietu
                {
                    Console.WriteLine($"Siuo metu nera laisvos vietos {_custGroupsArray[0]} zmoniu grupei, prasome palaukti");
                    Console.WriteLine($"Jus perkelti i laukianciuju sarasa");
                    Console.WriteLine("");
                    _custGroupsWaiting.Add(_custGroupsArray[0]);                                                //itraukiu i laukianciuju sarasa
                    DeleteFirstFromArray(_custGroupsArray);                                                     //istrinu is kirminio listo
                }
                else                                                                                                 //Jei klientu grupe didesne, nei yra vietu restorane
                {
                    Console.WriteLine("Jusu gruper per didele, tiek vietos restorane nera");
                    DeleteFirstFromArray(_custGroupsArray);
                }
            }

            if (_custGroupsWaiting.Count() != 0)
            {
                Console.WriteLine("Laukianciuju zmoniu grupes ir ju skaicius");
                _custGroupsWaiting.ForEach(x => Console.Write($"{x} | "));
            }

            Console.WriteLine("");
            ConvertListToArray();

        }

        public void CancelReservation()                                 //Rezervacijos panaikinimas
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            var tableIdForCancel = Utilities.CreateRandomNumber(1, 8);
            Console.WriteLine($"Vienas staliukas (ID: {tableIdForCancel}, nerado ko uzsisakyti ir nori iseiti, bei nutraukti rezervacija");
            Console.ForegroundColor = ConsoleColor.White;

            string returnConditionText = $"SELECT orderMade FROM Tables WHERE tableID = {tableIdForCancel}";
            string returnParameter = "orderMade";

            if (GetValue(returnConditionText, returnParameter) == 0)                          //Tikrinu ar satliukas pateikes uzsakyma
            {
                ChangeTableReservationStatus(tableIdForCancel);
                AddPersonQNTOfGroupToDB(tableIdForCancel, 0);
                Console.WriteLine("Rezervacija atsaukta. Testi - 'ENTER'");
            }
            else
            {
                Console.WriteLine($"Rezervacijos atsaukti negalima, staliukas pateikes uzsakyma");
                Console.WriteLine("Grizti - 'ENTER'");
            }
            Console.ReadLine();
        }

        private bool CheckIfWereEmptyPlaceInRestaurant(int groupePersonQnt)                     //Tikrina ar yra laisvos vietos restorane
        {
            if (CountFreeSeatInRestaurant() >= groupePersonQnt)
            {
                Console.WriteLine($"Laukia {groupePersonQnt} zmoniu grupe.");
                Console.WriteLine($"Laisvos vietos restorne yra {CountFreeSeatInRestaurant()}, grupe pasodinta");
                Console.WriteLine("");
            }
            return (CountFreeSeatInRestaurant() >= groupePersonQnt);
        }

        private bool CheckIfWereEnofPlaceInRestaurant(int groupePersonQnt)                      //Tikrina ar is vis yra tiek vietu restorane
        {
            return groupePersonQnt <= CountTotalSeatInRestaurant();
        }

        private int CountTotalSeatInRestaurant()                                                //Skaiciuoja kiek is viso yra vietu restorane
        {
            string commandText = "SELECT SUM(tableNumberOfSeats) FROM Tables;";
            var totalSeat = DBRespositoryService.ReadDataCountReturnSum(DBRespositoryService.CreateConnection(), commandText);
            return totalSeat;
        }

        private int CountFreeSeatInRestaurant()                                                 //skaiciuoja laisvs vietas restorane
        {
            string commandText = "SELECT SUM(tableNumberOfSeats) FROM Tables WHERE tableReserveted = 0;";
            var freeSeat = DBRespositoryService.ReadDataCountReturnSum(DBRespositoryService.CreateConnection(), commandText);
            return freeSeat;
        }

        private int SelecteTableIDForReservation(int groupePersonQn)                           //iskoma laisvo saliuko ID reiksme, kuris turi sedimu vietu kiek zmoniu arba daugiau 
        {
            string commandText = $"SELECT tableID FROM Tables WHERE tableReserveted = 0 AND tableNumberOfSeats >={groupePersonQn} ORDER BY tableNumberOfSeats ASC LIMIT 1";
            string returnParameter = "tableID";
            var idForResevation = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnParameter);
            return idForResevation;
        }

        private void ChangeTableReservationStatus(int tableID)                                                          //Keiciu rezervavimo statusa
        {
            int newStatus = (CheckTableReservationStatus(tableID) == true) ? 0 : 1;
            string commandText = $"UPDATE Tables SET tableReserveted = {newStatus} WHERE tableID={tableID}";
            DBRespositoryService.UpdateData(DBRespositoryService.CreateConnection(), commandText);
        }

        private void AddPersonQNTOfGroupToDB(int tableID, int personQNT)
        {

            string commandText = $"UPDATE Tables SET tableSeatsOcupate = {personQNT} WHERE tableID={tableID}";
            DBRespositoryService.UpdateData(DBRespositoryService.CreateConnection(), commandText);
        }

        private bool CheckTableReservationStatus(int tableID)                                                           //Tikrinamas rezervacijos statusas
        {
            string commandText = $"SELECT tableReserveted FROM Tables WHERE tableID = {tableID}";
            string returnParameter = "tableReserveted";
            var status = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnParameter);
            return status == 1;
        }

        public void DeleteFirstFromArray(int[] custGroupsArray)
        {
            int[] newArray = new int[custGroupsArray.Length - 1];                                          //pasalinu iš Arrejaus kliengu grupe kuri netelpa restorane
            Array.Copy(custGroupsArray, 1, newArray, 0, newArray.Length);
            _custGroupsArray = newArray;
        }

        private void JoinTable(int groupePersonQn)                                                        //Sujungti ir rezervuoti stalus
        {
            string returnIDFreeTable = $"SELECT tableID FROM Tables WHERE tableReserveted = 0 ORDER BY tableNumberOfSeats ASC LIMIT 1";
            string returnParameterFreeTable = "tableID";

            if (CountFreeSeatInRestaurant() >= groupePersonQn)                                          //Jei yra laisvu vietu rezervuoju po viena
            {
                var rezervas = 0;
                while (rezervas < groupePersonQn)
                {
                    //Gaunu laisvo stalo ID
                    var ID = GetValue(returnIDFreeTable, returnParameterFreeTable);
                    // rezervuoju stala
                    ChangeTableReservationStatus(ID);
                    string returnConditionText = $"SELECT tableNumberOfSeats FROM Tables WHERE tableID = {ID} ORDER BY tableNumberOfSeats ASC LIMIT 1";
                    string returnParameter = "tableNumberOfSeats";
                    //gaunu to stalo sedimu vietu skaiciu
                    var seatsQNT = GetValue(returnConditionText, returnParameter);
                    rezervas = rezervas + seatsQNT;
                    DistributePeopleToTables(seatsQNT, groupePersonQn, ID);                                             // stalui priskiriu zmoniu skaiciu is grupes kuri didesne nei stalas
                }
                //istrinu is pirminio saraso
                DeleteFirstFromArray(_custGroupsArray);
            }
            else                                                                                             //Jei neuztenka laisvu vietu i laukianciuju sarasa
            {
                _custGroupsWaiting.Add(_custGroupsArray[0]);                                                //itraukiu i laukianciuju sarasa
                DeleteFirstFromArray(_custGroupsArray);                                                     //istrinu is kirminio listo
            }
        }

        private void DistributePeopleToTables(int seatsQNT, int likoPasodinti, int ID)
        {
            if (seatsQNT <= likoPasodinti)
            {
                AddPersonQNTOfGroupToDB(ID, seatsQNT);
                likoPasodinti -= seatsQNT;
            }
            else
                AddPersonQNTOfGroupToDB(ID, likoPasodinti);
        }

        private int GetValue(string commandTextValue, string returnParameterValue)                          //iskoma laisvo saliuko ID reiksme 
        {
            string commandText = commandTextValue;
            string returnParameter = returnParameterValue;
            var idForResevation = DBRespositoryService.ReadDataReturnValue(DBRespositoryService.CreateConnection(), commandText, returnParameter);
            return idForResevation;
        }

        private void PrintWaitingCustomerGrups()
        {
            Console.WriteLine("Laukianciu klientu grupes zmoniu skaicius:");
            if (_custGroupsArray.Count() != 0)
            {
                _custGroupsArray.ToList().ForEach(c => Console.Write($"{c} | "));
            }
            else
            {
                Console.WriteLine("Nera");
            }

            Console.WriteLine();
        }
        public void ConvertListToArray()
        {
            _custGroupsArray = _custGroupsWaiting.ToArray();
        }

    }
}
