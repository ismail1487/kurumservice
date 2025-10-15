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
    public interface IUrunParametrelerService : Base.IService<UrunParametreler>
    {
    }
    public class UrunParametrelerService : Base.Service<UrunParametreler>, IUrunParametrelerService
    {

        public UrunParametrelerService(IRepository<UrunParametreler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<UrunParametrelerService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}
