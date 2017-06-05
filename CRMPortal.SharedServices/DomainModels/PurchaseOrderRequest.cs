﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models.DomainModels
{
    public class PurchaseOrderRequest
    {
        public string RequestTitle { get; set; }
        public string NumberOfitems { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StatusReason { get; set; }
        public string RequestNumber { get; set; }
    }
}