using System;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Product.Service.Entities;
using Product.Service.Interfaces.Repositories;

namespace Product.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> GetByCpfCnpjAsync(string cpfCnpj)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
        }

        public async Task<Cliente> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Cliente>> GetByFilterAsync(Expression<Func<Cliente, bool>> filter)
        {
            return await _context.Clientes
                .Include(c => c.Endereco)
                .Where(filter)
                .ToListAsync();
        }

        public async Task AddAsync(Cliente entity)
        {
            await _context.Clientes.AddAsync(entity);
        }

        public async Task UpdateAsync(Cliente entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var cliente = await GetByIdAsync(id);
            _context.Clientes.Remove(cliente);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Cliente, bool>> predicate)
        {
            return await _context.Clientes.AnyAsync(predicate);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Endereco)
                .ToListAsync();
        }
    }
}
