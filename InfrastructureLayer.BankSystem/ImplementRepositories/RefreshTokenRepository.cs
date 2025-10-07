using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.ImplementRepositories
{
    public class RefreshTokenRepository : Repository<UserRefreshToken>,IRefreshTokenRepository
    {
        private readonly DbSet<UserRefreshToken> _userRefreshToken;

        public RefreshTokenRepository(ApplicationDbContext dbContext) :base(dbContext)
        {
            _userRefreshToken = dbContext.Set<UserRefreshToken>();
           
        }
    }
}
