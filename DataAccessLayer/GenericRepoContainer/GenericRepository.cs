using AllinOne.DataHandlers;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDIAccountsAPI
{
    public class GenericRepository<TEntity> : Repository<TEntity> where TEntity : class
    {
        private readonly EasyAccountDbContext _dBContext;
        public GenericRepository(EasyAccountDbContext dbContext) : base(dbContext)
        {
            _dBContext = dbContext;
        }
    }
}
