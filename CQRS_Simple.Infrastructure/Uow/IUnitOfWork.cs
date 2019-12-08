using Microsoft.EntityFrameworkCore;
using System;

namespace CQRS_Simple.Infrastructure.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Commit();
    }
}