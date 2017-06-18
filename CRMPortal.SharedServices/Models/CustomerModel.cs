using CRMPortal.SharedServices.DomainModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class CustomerModel : Repository
    {
        public CustomerModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<Entity> GetAllCustomers(int PageIndex = 0, int pageSize = 10)
        {
            List<Entity> customers = new List<Entity>();

            List<Entity> accounts = Context.CreateQuery("account").Skip(PageIndex * pageSize).Take(10).ToList();
            List<Entity> contacts = Context.CreateQuery("contacts").Skip(PageIndex * pageSize).Take(10).ToList();

            customers.AddRange(accounts);
            customers.AddRange(contacts);

            return customers;
        }
        public List<Entity> GetAllCustomers()
        {
            List<Entity> customers = new List<Entity>();

            List<Account> accounts;
            List <Contact> contacts;

            using (crmcontgenerated c = new crmcontgenerated(OrgService))
            {

                accounts = c.AccountSet.ToList();

                contacts = (
                    from conts in c.ContactSet
                    where conts.new_IsInternal==false
                    select conts
                    ).ToList();
            }
            
            customers.AddRange(accounts);
            customers.AddRange(contacts);

            return customers;
        }
        public List<Entity> GetCustomersForEmployee(Guid empId)
        {
            List<Entity> customers = new List<Entity>();

            List<Account> accounts; //Context.CreateQuery("account").Skip(PageIndex * pageSize).Take(10).ToList();
            List<Contact> contacts; //Context.CreateQuery("contacts").Skip(PageIndex * pageSize).Take(10).ToList();

            using (crmcontgenerated c = new crmcontgenerated(OrgService))
            {

                accounts = (
                    from acs in c.AccountSet
                    where acs.OwnerId == new EntityReference("systemuser", empId)
                    select acs
                    ).ToList();
                contacts = (
                    from conts in c.ContactSet
                    where conts.OwnerId == new EntityReference("systemuser", empId)
                    select conts
                    ).ToList();
            }

            customers.AddRange(accounts);
            customers.AddRange(contacts);

            return customers;
        }
        internal List<SharedServices.DomainModels.Customer> GetAllAsCustomer()
        {
            List<Entity> customers = GetAllCustomers();
            List<Customer> viewCustomers = new List<Customer>();

            foreach (var c in customers)
            {
                string name;
                if (c.LogicalName == Contact.EntityLogicalName)
                {
                    name = (c.Attributes.Contains("firstname")) ? c["firstname"].ToString() : "" + c["lastname"].ToString();
                }
                else
                {
                    name = c["name"].ToString();
                }
                Customer tc = new Customer()
                {
                    Id = c.Id,
                    LogicalName = c.LogicalName,
                    Name = name
                };

                viewCustomers.Add(tc);
            }
            return viewCustomers;
        }
    }
}