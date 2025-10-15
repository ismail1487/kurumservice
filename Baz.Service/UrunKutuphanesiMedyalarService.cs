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
    public interface IUrunKutuphanesiMedyalarService : Base.IService<UrunKutuphanesiMedyalar>
    {
    }
    public class UrunKutuphanesiMedyalarService : Base.Service<UrunKutuphanesiMedyalar>, IUrunKutuphanesiMedyalarService
    {

        public UrunKutuphanesiMedyalarService(IRepository<UrunKutuphanesiMedyalar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<UrunKutuphanesiMedyalarService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}
