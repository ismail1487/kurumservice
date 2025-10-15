using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Baz.Service
{
    public interface IIcerikKutuphanesiMedyalarService : Base.IService<IcerikKutuphanesiMedyalar>
    {
    }
    public class IcerikKutuphanesiMedyalarService : Base.Service<IcerikKutuphanesiMedyalar>, IIcerikKutuphanesiMedyalarService
    {

        public IcerikKutuphanesiMedyalarService(IRepository<IcerikKutuphanesiMedyalar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<IcerikKutuphanesiService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }

}