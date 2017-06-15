using CRMPortal.SharedServices.DomainModels;
using CRMPortal.SharedServices.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.ViewModels
{
    public class RoomReservationViewModel
    {
        public List<RoomReservationRequest> requests { get; set; }
    }

    public class RoomReservationFormViewModel
    {
        public Request request { get; set; }
        public DateTime Day { get; set; }
        
        [Display(Name="From ")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "To")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Title")]
        public string RequestTitle { get; set; }

        [Display(Name = "Room Number ")]        
        public Guid RoomToReserve { get; set; }
        public List<Models.Room> AvailableRooms { get; set; }
    }
}
class OccupationDTO
{
    public DateTime DaytoReserve { get; set; }
    public Guid roomToreserve { get; set; }
}