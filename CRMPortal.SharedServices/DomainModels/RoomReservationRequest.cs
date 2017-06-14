using CRMPortal.SharedServices.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.DomainModels
{
    public class RoomReservationRequest:Request
    {
        public DateTime Day { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid RelatedRoom { get; set; }
        
    }

}