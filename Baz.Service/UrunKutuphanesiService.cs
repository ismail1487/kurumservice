using Baz.Mapper.Pattern;
using Baz.Repository.Pattern;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Model.Entity;

namespace Baz.Service
{
        public interface IUrunKutuphanesiService : Base.IService<UrunKutuphanesi>
        {
        }
        public class UrunKutuphanesiService : Base.Service<UrunKutuphanesi>, IUrunKutuphanesiService
    {

            public UrunKutuphanesiService(IRepository<UrunKutuphanesi> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<UrunKutuphanesiService> logger) : base(repository, dataMapper, serviceProvider, logger)
            {
            }

        }
}
