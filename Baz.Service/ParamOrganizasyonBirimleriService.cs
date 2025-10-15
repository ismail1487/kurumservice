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
    /// Kurum Organizasyon birimlerine ait metotların yer aldığı servis sınıfıdır
    /// </summary>
    public interface IParamOrganizasyonBirimleriService : IService<ParamOrganizasyonBirimleri>
    {
    }

    /// <summary>
    /// Kurum Organizasyon birimlerine ait işlemleri  yöneten  servis sınıfıdır
    /// </summary>
    public class ParamOrganizasyonBirimleriService : Service<ParamOrganizasyonBirimleri>, IParamOrganizasyonBirimleriService
    {
        /// <summary>
        /// Kurum Organizasyon birimlerine ait işlemleri  yöneten  servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamOrganizasyonBirimleriService(IRepository<ParamOrganizasyonBirimleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ParamOrganizasyonBirimleriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}