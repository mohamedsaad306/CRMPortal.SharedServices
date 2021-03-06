﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class UnitOfWork : IDisposable
    {

        private readonly OrganizationServiceContext _context;
        public IOrganizationService OrganizaionService { get; set; }


        public UnitOfWork(OrganizationServiceContext ctx, IOrganizationService orgService)
        {
            _context = ctx;
            OrganizaionService = orgService;

            AccModel = new AccountModel(_context, OrganizaionService);

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public AccountModel AccModel { get; set; }
        public HelpDeskModel HelpDeskModel { get { return new HelpDeskModel(_context, OrganizaionService); } }
        public ContactModel ContactModel { get { return new ContactModel(_context, OrganizaionService); } }
        public PurchaseOrderModel PurchaseOrderModel { get { return new PurchaseOrderModel(_context, OrganizaionService); } }
        public RoomReservationModel RoomReservationModel { get { return new RoomReservationModel(_context, OrganizaionService); } }
        public RoomModel Rooms { get { return new RoomModel(_context, OrganizaionService); } }

        public TransportationModel Transportations { get { return new TransportationModel(_context, OrganizaionService); } }
        public CustomerModel Customers { get { return new CustomerModel(_context, OrganizaionService); } }
        public HelpDeskCategoryModel HelpDeskRequestCategories { get { return new HelpDeskCategoryModel(_context, OrganizaionService); } }
        public HelpDeskSubCategoryModel HelpDeskRequestSubCategories { get { return new HelpDeskSubCategoryModel(_context, OrganizaionService); } }
        public VacationModel VacationRequests { get { return new VacationModel(_context, OrganizaionService); } }
    }
}