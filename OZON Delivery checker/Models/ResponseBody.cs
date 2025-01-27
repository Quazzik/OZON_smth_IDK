namespace OZON_Delivery_checker.Models
{
    public class ResponseBody
    {
        public string DeliveryDateBegin { get; set; }

        public string DeliveryDateEnd { get; set; }

        public string DeliveryDatePeriodChangedMoment { get; set; }

        public List<Item> Items { get; set; }
    }
}
