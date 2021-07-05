using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Test.Entities;

namespace Test.Services
{
    public class CustomerService : ICustomerService
    {
        string StrPath = "";
        public CustomerService()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            StrPath = Path.GetDirectoryName(asm.Location) + "\\customer.json";
        }

        /// <summary>
        /// Save the data in file after serializing the list
        /// </summary>
        /// <param name="customers"></param>
        public void SaveData(List<CustomerEntity> customers)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
            File.WriteAllText(StrPath, json);
        }

        /// <summary>
        /// Load the data from file and return list of customers
        /// </summary>
        /// <returns></returns>
        public List<CustomerEntity> LoadData()
        {
            List<CustomerEntity> customers = new List<CustomerEntity>();
            if (File.Exists(StrPath))
            {
                var read = File.ReadAllText(StrPath);
                customers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerEntity>>(read);
            }

            return customers;
        }

        /// <summary>
        /// Sort the collection based on last name and first name
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public List<CustomerEntity> SortData(List<CustomerEntity> customers, CustomerEntity customer)
        {
            int position = customers.Count;
            var result = new List<CustomerEntity>(customers);

            for (int i = 0; i < customers.Count; i++)
            {
                if (customer.LastName.CompareTo(customers[i].LastName) < 0)
                {
                    position = i;
                    break;
                }
                else if (customer.LastName.CompareTo(customers[i].LastName) == 0 && (customer.FirstName.CompareTo(customers[i].FirstName) < 0 || customer.FirstName.CompareTo(customers[i].FirstName) > 0))
                {
                    position = i;
                    break;
                }
                else if (customer.LastName.CompareTo(customers[i].LastName) == 0 && customer.FirstName.CompareTo(customers[i].FirstName) == 0)
                {
                    position = -1;
                    break;
                }
            }

            if (position != -1)
                result.Insert(position, customer);

            return result;
        }

        public bool CheckIdExists(List<CustomerEntity> customers, int Id)
        {
            if (customers?.Count == 0)
                return false;

            return customers.Exists(x => x.Id == Id);
        }
    }
}