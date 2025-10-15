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
    public interface IParamParaBirimleriService : Base.IService<ParamParaBirimleri>
    {
    }

    public class ParamParaBirimleriService : Service<ParamParaBirimleri>, IParamParaBirimleriService
    {
        /// <summary>
        /// MedyaKutuphanesi ile ilgili işlevleri barındıran servisin yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamParaBirimleriService(IRepository<ParamParaBirimleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamParaBirimleriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}
