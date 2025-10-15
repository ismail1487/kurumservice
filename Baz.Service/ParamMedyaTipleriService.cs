
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

    public interface IParamMedyaTipleriService : Base.IService<ParamMedyaTipleri>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParamMedyaTipleriService : Base.Service<ParamMedyaTipleri>, IParamMedyaTipleriService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamMedyaTipleriService(IRepository<ParamMedyaTipleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamMedyaTipleri> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}