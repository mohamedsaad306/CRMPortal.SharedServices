using CRMPortal.SharedServices.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.DomainModels
{
    public class VacationRequest:Request
    {
        public VacationRequest()
        {
            AvailableActions = new List<string>();
            RequestType = "Vacation Request";
        }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool iSAnnual { get; set; }
    }
}