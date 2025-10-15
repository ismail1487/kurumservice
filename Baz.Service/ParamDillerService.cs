using Baz.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Mapper.Pattern;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;



namespace Baz.Service
{
    /// <summary>
    /// MedyaKutuphanesi ile ilgili işlevleri barındıran interface.
    /// </summary>
    public interface IParamDillerService : IService<ParamDiller>
    {
    }

    /// <summary>
    /// diller ile ilgili işlevleri barındıran, <see cref="ParamDillerService"/> interface'ini baz alan class.
    /// </summary>
    public class ParamDillerService : Service<ParamDiller>, IParamDillerService
    {
        /// <summary>
        /// MedyaKutuphanesi ile ilgili işlevleri barındıran servisin yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamDillerService(IRepository<ParamDiller> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamDillerService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}