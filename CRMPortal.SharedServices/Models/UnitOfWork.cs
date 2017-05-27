using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class UnitOfWork:IDisposable
    {

    private readonly OrganizationServiceContext _context;


        public UnitOfWork(OrganizationServiceContext ctx )
        {
            _context = ctx;
            AccModel = new AccountModel(_context);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public AccountModel AccModel { get; set; }
        public HelpDeskModel HelpDeskModel { get { return new HelpDeskModel(_context); }  }
    }
}