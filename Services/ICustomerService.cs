using System.Collections.Generic;
using Test.Entities;

namespace Test.Services
{
    public interface ICustomerService
    {
        void SaveData(List<CustomerEntity> customers);
        List<CustomerEntity> LoadData();
        List<CustomerEntity> SortData(List<CustomerEntity> customers, CustomerEntity customer);
        bool CheckIdExists(List<CustomerEntity> customers, int Id);
    }
}