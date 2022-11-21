﻿using System.Collections.Generic;

namespace CustomerRegister.Repositories.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerEntity> GetAllCustomers();
        CustomerEntity GetCustomerById(int id);
        void Add(CustomerEntity customer);
        void Update(CustomerEntity customer, int id);
        void Delete(int id);

    }
}
