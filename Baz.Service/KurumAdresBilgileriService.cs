using Baz.Service.Base;
using Baz.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Repository.Pattern;
using Baz.Mapper.Pattern;
using Microsoft.Extensions.Logging;
using Baz.ProcessResult;

namespace Baz.Service
{
    /// <summary>
    /// Kurum adres bilgilerine ait methodların yer aldığı servis sınıfıdır.
    /// </summary>
    public interface IKurumAdresBilgileriService : IService<KurumAdresBilgileri>
    {
        /// <summary>
        /// Id ile Kurum adres verisini getiren method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<List<KurumAdresBilgileri>> KurumIdileGetir(int kurumID);

        /// <summary>
        /// Id'ye göre kurum adres bilgisini silindi yapan method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<bool> KurumBilgileriSilindiYap(int kurumID);
    }

    /// <summary>
    /// Kurum adres bilgileri ile ilgili işlemleri yöneten servis sınıfı
    /// </summary>
    public class KurumAdresBilgileriService : Service<KurumAdresBilgileri>, IKurumAdresBilgileriService
    {
        /// <summary>
        /// Kurum adres bilgileri ile ilgili işlemleri yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KurumAdresBilgileriService(IRepository<KurumAdresBilgileri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumAdresBilgileriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        /// <summary>
        /// Id ile Kurum adres verisini getiren method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<List<KurumAdresBilgileri>> KurumIdileGetir(int kurumID)
        {
            var result = List(x => x.IlgiliKurumId == kurumID && x.AktifMi == 1);
            return result;
        }

        /// <summary>
        /// Id'ye göre kurum adres bilgisini silindi yapan method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<bool> KurumBilgileriSilindiYap(int kurumID)
        {
            var list = this.List(x => x.IlgiliKurumId == kurumID).Value;

            foreach (var item in list)
            {
                item.SilindiMi = 1;
                item.AktifMi = 0;
                item.SilinmeTarihi = DateTime.Now;
                this.Update(item);
            }
            return true.ToResult();
        }
    }
}