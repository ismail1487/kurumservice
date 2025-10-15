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
  
    public interface IParamIcerikTipleriService : Base.IService<ParamIcerikTipleri>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParamIcerikTipleriService : Base.Service<ParamIcerikTipleri>, IParamIcerikTipleriService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamIcerikTipleriService(IRepository<ParamIcerikTipleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SistemSayfalariService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}