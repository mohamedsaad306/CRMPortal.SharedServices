using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.ViewModels
{
    public class RoomReservationViewModel
    {
        
    }

    public class RoomReservationFormViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string RequestTitle { get; set; }
        public Guid RoomToReserve { get; set; }

    }
}