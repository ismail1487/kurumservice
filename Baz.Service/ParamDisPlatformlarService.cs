using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baz.Service
{
    /// <summary>
    /// DışPlatformlar ile ilgili işlevleri barındıran interface.
    /// </summary>
    public interface IParamDisPlatformlarService : IService<ParamDisPlatformlar>
    {
    }

    /// <summary>
    /// ParamDisPlatformlar ile ilgili işlevleri barındıran, <see cref="ParamDisPlatformlar"/> interface'ini baz alan class.
    /// </summary>
    public class ParamDisPlatformlarService : Service<ParamDisPlatformlar>, IParamDisPlatformlarService
    {
        /// <summary>
        /// ParamDisPlatformlar ile ilgili işlevleri barındıran servisin yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamDisPlatformlarService(IRepository<ParamDisPlatformlar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamDisPlatformlarService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}