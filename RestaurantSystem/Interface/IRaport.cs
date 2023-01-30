namespace RestaurantSystem.Interface
{
    public interface IRaport
    {
        public void GetSalesByDate(string firstDate, string lastDate);

        public void CreateReportForFinancialDep();
        public void CreateHTMLForReport();
    }
}
