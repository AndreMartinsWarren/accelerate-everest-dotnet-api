using CustomerRegister.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.SecurityTokenService;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using BadRequestException = SendGrid.Helpers.Errors.Model.BadRequestException;

namespace CustomerRegister
{
    public class CustomerService : ICustomerService
    {
        private readonly List<CustomerEntity> _customers = new();

        public bool Exists(CustomerEntity selectedCustomer)
        {
            var response = _customers
                .Any(customer => customer.Cpf == selectedCustomer.Cpf || customer.Email == selectedCustomer.Email) && (selectedCustomer.Id != selectedCustomer.Id);
            return response;
        }

        public bool DuplicatedRegister(CustomerEntity selectedCustomer)
        {
            if(_customers.Any(customer => customer.Cpf == selectedCustomer.Cpf || customer.Email == selectedCustomer.Email))
                return true;

            return false;
        }

        public void Add(CustomerEntity customer)
        {
            customer.Id = _customers.LastOrDefault()?.Id + 1 ?? 1;

            if (!Exists(customer))
            {
                if (DuplicatedRegister(customer))
                {
                    throw new BadRequestException("Email ou CPF já exitem");
                }
                else
                {
                    _customers.Add(customer);

                }
            }
        }

        public void Delete(int id)
        {
            int index = _customers.FindIndex(customer => customer.Id == id);

            if (index == -1)
            {
                throw new BadRequestException($"O usuário com id: {id} não foi encontrado");
            }
            else
            {
            _customers.RemoveAt(index);
            }
        }

        public List<CustomerEntity> GetAllCustomers()
        {
            return _customers;
        }

        public CustomerEntity GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(x => x.Id == id) ?? 
                throw new BadRequestException($"Não foi encontrado usuário com Id : {id}");
        }

        public void Update (CustomerEntity selectedCustomer, int id)
        {
            var updateCustomer = GetCustomerById(selectedCustomer.Id);
            if(updateCustomer == null)
            {
                throw new BadRequestException($"Não foi encontrado nenhum registro com este CPF: {selectedCustomer.Cpf}");
            }

            if (DuplicatedRegister(selectedCustomer))
            {
                var index = _customers.IndexOf(updateCustomer);
                _customers[index] = selectedCustomer;
            }
            else
            {
            throw new BadRequestException("Email e CPF já existem na nossa base de dados.");
            }
        }
    }
}
