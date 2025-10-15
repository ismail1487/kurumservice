
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Microsoft.Extensions.Logging;

namespace Baz.Service
{
    /// <summary>
    /// Banka, şube ve şube kodlarının parametre olarak tanımlandığı servis sınıfıdır.
    /// </summary>
    public interface IIcerikSpotMetinleriService : Base.IService<IcerikSpotMetinleri>
    {
    }

    /// <summary>
    /// ParamBankalar ile ilgili işlemleri yöneten servıs sınıfı
    /// </summary>
    public class IcerikSpotMetinleriService : Base.Service<IcerikSpotMetinleri>, IIcerikSpotMetinleriService
    {
        /// <summary>
        /// ParamBankalar ile ilgili işlemleri yöneten servıs sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public IcerikSpotMetinleriService(IRepository<IcerikSpotMetinleri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<IcerikSpotMetinleri> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}