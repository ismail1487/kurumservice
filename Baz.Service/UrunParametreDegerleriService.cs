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
    public interface IUrunParametreDegerleriService : Base.IService<UrunParametreDegerleri>
    {
    }
    public class UrunParametreDegerleriService : Base.Service<UrunParametreDegerleri>, IUrunParametreDegerleriService
    {

        public UrunParametreDegerleriService(IRepository<UrunParametreDegerleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<UrunParametreDegerleriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}
