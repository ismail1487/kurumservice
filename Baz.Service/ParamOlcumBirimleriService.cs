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
    public interface IParamOlcumBirimleriService : Base.IService<ParamOlcumBirimleri>
    {
    }


    public class ParamOlcumBirimleriService : Base.Service<ParamOlcumBirimleri>, IParamOlcumBirimleriService
    {

        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamOlcumBirimleriService(IRepository<ParamOlcumBirimleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamOlcumBirimleriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}
