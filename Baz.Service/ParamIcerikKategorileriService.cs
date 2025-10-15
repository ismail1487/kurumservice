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

    public interface IParamIcerikKategorileriService : Base.IService<ParamIcerikKategoriler>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParamIcerikKategorileriService : Base.Service<ParamIcerikKategoriler>, IParamIcerikKategorileriService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamIcerikKategorileriService(IRepository<ParamIcerikKategoriler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamIcerikKategoriler> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}