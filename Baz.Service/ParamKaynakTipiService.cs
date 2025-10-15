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
    public interface IParamKaynakTipiService : Base.IService<ParamKaynakTipleri>
    {
    }

    public class ParamKaynakTipiService : Base.Service<ParamKaynakTipleri>, IParamKaynakTipiService
    {

        public ParamKaynakTipiService(IRepository<ParamKaynakTipleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamKaynakTipiService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}
