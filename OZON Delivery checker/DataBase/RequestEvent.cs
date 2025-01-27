using OZON_Delivery_checker.Data;
using System.ComponentModel.DataAnnotations;

namespace OZON_Delivery_checker.DataBase
{
    public class RequestEvent
    {
        public int Id { get; set; }

        public int RequestRecordId { get; set; }

        public string EventName { get; set; }

        public string Moment { get; set; }

        public RequestRecord RequestRecord { get; set; }
    }
}
