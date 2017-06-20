using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models.DomainModels
{
    public class Request
    {
        public Request()
        {
            AvailableActions = new List<string>();
        }
        public Guid Id { get; set; }
        public string RequestTitle { get; set; }
        public string RequestDetails { get; set; }
        public string RequestNumber { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String RequestType { get; set; }
        public bool isReadOnly { get; set; }

        public List<string> AvailableActions { get; set; }
        public object RequestOwner { get; set; }

    }
}