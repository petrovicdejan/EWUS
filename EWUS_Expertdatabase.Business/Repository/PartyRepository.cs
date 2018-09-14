using EWUS_Expertdatabase.Common;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Business
{
    public class PartyRepository
    {
        public List<MaintenanceCompany> GetAllMaintenanceCompanies()
        {
            using (var context = new EWUSDbContext())
            {
                List<InvolvedParty> involvedParties = new List<InvolvedParty>();
                involvedParties = context.InvolvedPartys.AsNoTracking()
                    .ToList();

                IEnumerable<MaintenanceCompany> maintenanceCompanies = involvedParties.OfType<MaintenanceCompany>() as IEnumerable<MaintenanceCompany>;

                if (maintenanceCompanies != null)
                {
                    return maintenanceCompanies.ToList();
                }
                return null;
            }
        }

        public List<Customer> GetAllCustomers()
        {
            using (var context = new EWUSDbContext())
            {
                List<InvolvedParty> involvedParties = new List<InvolvedParty>();
                involvedParties = context.InvolvedPartys.AsNoTracking()
                    .ToList();

                IEnumerable<Customer> customers = involvedParties.OfType<Customer>() as IEnumerable<Customer>;

                if (customers != null)
                {
                    return customers.ToList();
                }
                return null;
            }
        }

        public MaintenanceCompany GetMaintenanceCompanyById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                MaintenanceCompany maintenanceCompany = new MaintenanceCompany();
                InvolvedParty ip = context.InvolvedPartys.Where(x => x.Id == Id)
                    .FirstOrDefault();

                maintenanceCompany = (MaintenanceCompany)ip;

                if (maintenanceCompany != null)
                {
                    return maintenanceCompany;
                }
                return null;
            }
        }

        public Result SaveMaintenanceCompany(MaintenanceCompany editMaintenanceCompany)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                MaintenanceCompany maintenanceCompany = null;
                if (editMaintenanceCompany.Id > 0)
                {
                    maintenanceCompany = ctx.MaintenanceCompanies.Where(x => x.Id == editMaintenanceCompany.Id)
                            .FirstOrDefault();
                }
                else
                {
                    maintenanceCompany = new MaintenanceCompany();
                }

                maintenanceCompany.Name = editMaintenanceCompany.Name;
                maintenanceCompany.Email = editMaintenanceCompany.Email;

                if (maintenanceCompany.Id == 0)
                    ctx.MaintenanceCompanies.Add(maintenanceCompany);

                ctx.SaveChanges();
                
                output = Result.ToResult<MaintenanceCompany>(ResultStatus.OK, typeof(MaintenanceCompany));
                output.Value = maintenanceCompany;
            }
            return output;
        }

        public Result SaveCustomer(Customer editCustomer)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            using (var ctx = new EWUSDbContext())
            {
                Customer customer = null;
                if (editCustomer.Id > 0)
                {
                    customer = ctx.Customers.Where(x => x.Id == editCustomer.Id)
                                    .Include(x => x.DocumentItems)
                                    .FirstOrDefault();

                    if (customer != null)
                    {
                        if (customer.DocumentItems != null)
                        {
                            List<DocumentItem> documents = customer.DocumentItems.ToList();
                            ctx.DocumentItems.RemoveRange(documents);
                        }
                    }
                }
                else
                {
                    customer = new Customer();
                }

                customer.Name = editCustomer.Name;
                customer.Email = editCustomer.Email;
                customer.DocumentItems = editCustomer.DocumentItems;

                if (customer.Id == 0)
                    ctx.Customers.Add(customer);

                ctx.SaveChanges();

                if (!string.IsNullOrEmpty(customer.Guid.ToString()) && customer.DocumentItems != null)
                {
                    SaveFile.SaveFileInFolder(customer.Guid.ToString(), typeof(Measure).Name, customer.DocumentItems);
                }

                output = Result.ToResult<Customer>(ResultStatus.OK, typeof(Customer));
                output.Value = customer;
            }
            return output;
        }

        public Customer GetCustomerById(long Id)
        {
            using (var context = new EWUSDbContext())
            {
                Customer customer = new Customer();
                InvolvedParty ip = context.InvolvedPartys.Where(x => x.Id == Id)
                                        .Include(x => x.DocumentItems)
                                        .FirstOrDefault();

                customer = (Customer)ip;

                if (customer != null)
                {
                    return customer;
                }
                return null;
            }
        }

        public Result DeleteCustomerById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    Customer customer = new Customer();
                    InvolvedParty ip = ctx.InvolvedPartys.Where(x => x.Id == Id)
                                            .Include(x => x.DocumentItems)
                                            .FirstOrDefault();

                    customer = (Customer)ip;

                    try
                    {
                        ctx.InvolvedPartys.Remove(customer);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087)
                        {
                            output.ExceptionMessage = Constants.ErrorMessageReferentialIntegrity;
                            output.Status = ResultStatus.Forbidden;
                        }
                        else
                        {
                            output.ExceptionMessage = "Exception could not be performed !!!";
                            output.Status = ResultStatus.InternalServerError;
                        }
                    }
                }
                output.Status = ResultStatus.OK;
            }
            catch (Exception e)
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }

        public Result DeleteMaintenanceCompanyById(long Id)
        {
            Result output = new Result();
            output.Status = ResultStatus.BadRequest;

            try
            {
                using (var ctx = new EWUSDbContext())
                {
                    MaintenanceCompany maintenanceCompany = new MaintenanceCompany();
                    InvolvedParty ip = ctx.InvolvedPartys.Where(x => x.Id == Id)
                        .FirstOrDefault();

                    maintenanceCompany = (MaintenanceCompany)ip;

                    try
                    {
                        ctx.InvolvedPartys.Remove(maintenanceCompany);

                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233087)
                        {
                            output.ExceptionMessage = Constants.ErrorMessageReferentialIntegrity;
                            output.Status = ResultStatus.Forbidden;
                        }
                        else
                        {
                            output.ExceptionMessage = "Exception could not be performed !!!";
                            output.Status = ResultStatus.InternalServerError;
                        }
                    }
                }
                output.Status = ResultStatus.OK;
            }
            catch
            {
                output.Status = ResultStatus.InternalServerError;
            }

            return output;
        }
    }
}
