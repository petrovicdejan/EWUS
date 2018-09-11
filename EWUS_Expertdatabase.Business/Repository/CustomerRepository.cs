using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EWUS_Expertdatabase.Model;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Common;

namespace EWUS_Expertdatabase.Business
{
    public class CustomerRepository
    {
        public List<Customer> GetAllCustomer()
        {
            using (var context = new EWUSDbContext())
            {
                try
                {
                    List<Customer> customers = new List<Customer>();
                    customers = context.Customers.AsNoTracking()
                    .ToList();

                    if (customers != null)
                    {
                        return customers;
                    }
                    return null;
                }
                catch (System.Exception ex)
                {

                    throw;
                }
               
            }
        }
    }
}
