namespace OnlinePharmacyAppMVC.DTO
{
    public class CartItemDTO
    {
       
        public string MedName { get; set; }
        public int StockQty { get; set; }
        public decimal Price { get; set; }
        public decimal? Amount { get; set; }
     
        
    }

}
