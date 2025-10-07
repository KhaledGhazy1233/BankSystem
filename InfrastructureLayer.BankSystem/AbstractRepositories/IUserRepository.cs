using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.AbstractRepositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
