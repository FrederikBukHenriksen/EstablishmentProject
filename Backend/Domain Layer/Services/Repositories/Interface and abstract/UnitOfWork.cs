namespace WebApplication1.Domain.Services.Repositories
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public interface IUnitOfWork : IDisposable
    {
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext _context;

        public UnitOfWork(IDatabaseContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
