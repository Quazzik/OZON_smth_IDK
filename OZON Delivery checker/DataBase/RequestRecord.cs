using System;
using System.ComponentModel.DataAnnotations;

namespace OZON_Delivery_checker.Data
{
    public class RequestRecord
    {
        public int Id { get; set; }

        public string TrackingNumber { get; set; }

        public string DeliveryDateBegin { get; set; }

        public string DeliveryDateEnd { get; set; }

        public string DeliveryDatePeriodChangedMoment { get; set; }

        public DateTime? LastFetchedAt { get; set; }
    }
}