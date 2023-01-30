namespace RestaurantSystem.Services
{
    static class Program
    {
        /// <summary>
        /// - Programa imituoja restorano staleliu rezervacija ir uzsakymu priemima, apmokejima ir ataskaitu issiuntima.
        /// 
        /// * Meniu pasirinkus "1":
        /// - Automatiskai sukuriamas atsitiktinis klientu srautas. 
        /// - Pagal srauta automatiskai rezervuojami staleliai. Jei zmoniu grupe didesne, nei didziausias laisvas stalas ji 
        /// sodinama prie keletos stalu. Jai netelpa zmones perkellemi i laukianciuju sarasa ir "susodinami" kai atsilaisvina stalai.
        /// - "Susodinus" pasileidzia stalelio rezervavimo atsaukimas atsitiktinai pasirinktam stalui.
        /// - Automatiskai sugeneruojamas stalo uzsakymas, kuris priklauso nuo sedinciu zmoniu skaiciaus. Pietaujama nuo 40-80 minucius.
        /// - Papietavus, automatiskai susimokama, ir automatiskai sugeneruojamas cekis *.txt failas.
        /// - Sugeneravus ceki laukiama 3 sekundes. Jei paspaudziama ENTER, isssiunciamas cekis el.pastu.
        /// - Atsilaisvina stalas. Vel tikrinama laukianciu zmoniu grupe. Ciklas sukasi kol nebelieka zmoniu.
        /// 
        /// *  Meniu pasirinkus "2"
        /// - I ekrana "isvedami" stalai ir ju rezervavimo statusas. Kadandi rezervavimas-valgymas-ir rezervo atsaukimas vyksta automatiskai, isvedamas stausas visada kai patikrini buna False (nebet tikrinti DB :) )
        ///  
        /// *  Meniu pasirinkus "3"
        /// - spausdinamos ataskaitos pagal paruostus filtrus, saugomos HTML formatu.
        /// 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Tikrinu ar DB yra, jei nera sukuriu. 
            Menu menu = new Menu();
            string connectionString = @"..\..\..\..\database_RestaurantSystem.db";
            if (File.Exists(connectionString))
            {
                menu.MainMenu();

            }
            else
            {
                CreateTableStaticDataService createTableStaticData = new CreateTableStaticDataService();
                createTableStaticData.CreateAllTableStaticData();
                menu.MainMenu();
            }
        }

    }
}