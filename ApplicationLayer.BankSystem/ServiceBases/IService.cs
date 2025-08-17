using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ServiceBases
{
    public interface IService<T> : IRepository<T> where T : class

    {



    }
}
