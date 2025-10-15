using Baz.AOP.Logger.ExceptionLog;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Baz.Service.Base;
using Decor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baz.Service
{
    /// <summary>
    /// Kişiler arası ilişkiye ait metotların yer aldığı interface
    /// </summary>
    public interface IKisiIliskiService : IService<Iliskiler>
    {
      
        /// <summary>
        /// Bu kurumun müşteri ilişklerini getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        Result<List<Iliskiler>> MusteriList(int buKurumID);


        /// <summary>
        /// Kişiler arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<Iliskiler> KisiIliskiKaydet(KisiIliskiKayitModel model);
    }

    /// <summary>
    /// Kişiler arası ilişkiye ait metotların yer aldığı servis sınıfıdır
    /// </summary>
    public class KisiIliskiService : Service<Iliskiler>, IKisiIliskiService
    {
        private readonly ILoginUser _loginUser;
        private readonly IRequestHelper _requestHelper;

        /// <summary>
        /// işiler arası ilişkiye ait metotların yer aldığı servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="loginUser"></param>
        /// <param name="requestHelper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiIliskiService(IRepository<Iliskiler> repository, IDataMapper dataMapper, ILoginUser loginUser, IRequestHelper requestHelper, IServiceProvider serviceProvider, ILogger<Iliskiler> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _loginUser = loginUser;
            _requestHelper = requestHelper;
        }

     
        /// <summary>
        /// Kişiler arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<Iliskiler> KisiIliskiKaydet(KisiIliskiKayitModel model)
        {
            var iliskiler = new Iliskiler()
            {
                KayitEdenID = model.KayıtEdenID,
                GuncelleyenKisiID = model.GuncelleyenKisiID,
                KurumID = model.KurumID,
                BuKisiId = model.BuKisiID,
                IliskiTuruId = model.IliskiTuruID,
                BununKisiId = model.BununKisiID,
                AktifMi = 1,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                IliskiBaslamaZamani = DateTime.Now,
                IliskiBitisZamani = DateTime.Now
            };
            if (model.IliskiTuruID == (int)IliskiTipi.MusteriTemsilcisi)//11
            {
                iliskiler.BuKurumId = model.BununKisiID;
                iliskiler.BununKisiId = null;
            }

            var iliskikayit = this.Add(iliskiler);

            return iliskikayit;
        }


        /// <summary>
        /// Bu kurumun müşteri ilişklerini getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        public Result<List<Iliskiler>> MusteriList(int buKurumID)
        {
            var result = List(a => a.BuKurumId == buKurumID && a.AktifMi == 1 && a.BuKisiId != null).Value;
            return result.ToResult();
        }

       
    }
}