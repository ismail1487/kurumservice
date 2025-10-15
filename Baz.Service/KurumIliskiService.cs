using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.AOP.Logger.ExceptionLog;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;

using Baz.Service.Base;
using Decor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Baz.Service
{
    /// <summary>
    /// Kurumlar arası ilişkiye ait metotların yer aldığı servis sınıfıdır
    /// </summary>
    public interface IKurumIliskiService : IService<Iliskiler>
    {
        /// <summary>
        /// Kurumlar arası ilişkiyi kurumId'ye göre listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<List<Iliskiler>> KurumIliskiList(int kurumID);

        /// <summary>
        /// Kurumlar arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>

        Result<bool> KurumIliskiSil(int tabloID);

        /// <summary>
        /// Kurumlar arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<Iliskiler> KurumIliskiKaydet(KurumIliskiKayitModel model);

        /// <summary>
        /// Kurumlar arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<Iliskiler> KurumIliskiGuncelle(KurumIliskiKayitModel model);

        /// <summary>
        /// Kurum'a ait ilişkileri Bu Kurum Id ile getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>

        Result<List<Iliskiler>> KurumIliskiGetir(int buKurumID);

        /// <summary>
        /// musteri temsilcisine göre bağlı kurum idleri getiren metod
        /// </summary>
        /// <param name="musteriTemsilciId"></param>
        /// <returns></returns>
        Result<List<int>> MusteriTemsilcisiBagliKurumIdGetir(int musteriTemsilciId);
    }

    /// <summary>
    ///  Kurumlar arası ilişkiye ait ilgili işlemleri yöneten servıs sınıfı
    /// </summary>
    public class KurumIliskiService : Service<Iliskiler>, IKurumIliskiService
    {
        private readonly IRequestHelper _requestHelper;

        /// <summary>
        ///  Kurumlar arası ilişkiye ait ilgili işlemleri yöneten servıs sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="requestHelper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KurumIliskiService(IRepository<Iliskiler> repository, IDataMapper dataMapper, IRequestHelper requestHelper, IServiceProvider serviceProvider, ILogger<KurumAdresBilgileriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _requestHelper = requestHelper;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi kurumId'ye göre listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<List<Iliskiler>> KurumIliskiList(int kurumID)
        {
            var _kurumService = _serviceProvider.GetService<IKurumService>();
            var paramIliski = new ParametreRequest()
            {
                ModelName = "ParamIliskiTurleri",
                UstId = 0,
                KurumId = 0,
                TabloID = 0,
                Tanim = "test",
                ParamKod = "",
                DilID = 1,
                EsDilID = 0
            };
            var url1 = LocalPortlar.IYSService + "/api/KureselParametreler/ListParam";
            var tanim = _requestHelper.Post<Result<List<ParametreResult>>>(url1, paramIliski).Result.Value.Where(a => a.ParamKod == "F");
            var Ids = tanim.Select(a => a.TabloID).ToList();
            // Kurum ilişki kayıtlarını getiren sorgu
            var join = (from iliski in _repository.List()
                        join kurum in _kurumService.ListForQuery() on iliski.BuKurumId equals kurum.TabloID
                        join kurum2 in _kurumService.ListForQuery() on iliski.BununKurumId equals kurum2.TabloID
                        where kurum.AktifMi == 1 && iliski.KurumID == kurumID && iliski.BuKurumId != null && iliski.AktifMi == 1 && Ids.Contains(iliski.IliskiTuruId.Value)
                        select iliski).Distinct().ToList();

            if (kurumID == 0)
            {
                return Results.Fail("Geçersiz Id", ResultStatusCode.ReadError);
            }
            return join.ToResult();
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<Iliskiler> KurumIliskiKaydet(KurumIliskiKayitModel model)
        {
            if (model.BuKurumID == 0 && model.BununKurumID == 0)
            {
                return Results.Fail("Kurumlar Geçersiz.", ResultStatusCode.CreateError);
            }
            var iliskiler = new Iliskiler()
            {
                KayitEdenID = model.KayitEdenID,
                GuncelleyenKisiID = model.GuncelleyenKisiID,
                KurumID = model.KurumID,
                BuKurumId = model.BuKurumID,
                IliskiTuruId = model.IliskiTuruID,
                BununKurumId = model.BununKurumID,
                AktifMi = 1,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                IliskiBaslamaZamani = DateTime.Now,
                IliskiBitisZamani = DateTime.Now
            };
            var list = this.KurumIliskiList(model.KurumID);
            var ax = list.Value.Any(rs1 => rs1.BuKurumId == model.BuKurumID && rs1.BununKurumId == model.BununKurumID && rs1.IliskiTuruId == model.IliskiTuruID && rs1.AktifMi == 1 && rs1.SilindiMi == 0);
            if (ax)
            {
                return Results.Fail("Aynı kayıttan ekleyemezsiniz!", ResultStatusCode.CreateError);
            }
            var iliskikayit = this.Add(iliskiler);
            return iliskikayit;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<Iliskiler> KurumIliskiGuncelle(KurumIliskiKayitModel model)
        {
            if (model.BuKurumID == 0 && model.BununKurumID == 0)
            {
                return Results.Fail("Kurumlar Geçersiz.", ResultStatusCode.UpdateError);
            }
            var iliskiler = new Iliskiler()
            {
                TabloID = model.TabloID,
                KayitEdenID = model.KayitEdenID,
                GuncelleyenKisiID = model.GuncelleyenKisiID,
                KurumID = model.KurumID,
                BuKurumId = model.BuKurumID,
                IliskiTuruId = model.IliskiTuruID,
                BununKurumId = model.BununKurumID,
                AktifMi = 1,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                IliskiBaslamaZamani = DateTime.Now,
                IliskiBitisZamani = DateTime.Now
            };
            var list = this.KurumIliskiList(model.KurumID);
            var ax = list.Value.Any(rs1 => rs1.BuKurumId == model.BuKurumID && rs1.BununKurumId == model.BununKurumID && rs1.AktifMi == 1 && rs1.SilindiMi == 0);
            if (ax)
            {
                return Results.Fail("Güncelleme işlemi başarısız!", ResultStatusCode.UpdateError);
            }
            var iliskikayit = this.Update(iliskiler);
            return iliskikayit;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>
        public Result<bool> KurumIliskiSil(int tabloID)
        {
            var iliskiler = this.SingleOrDefault(tabloID).Value;
            if (iliskiler == null)
            {
                return Results.Fail("Güncelleme işlemi başarısız!", ResultStatusCode.DeleteError);
            }
            iliskiler.AktifMi = 0;
            iliskiler.SilindiMi = 1;
            this.Update(iliskiler);
            return true.ToResult();
        }

        /// <summary>
        /// Kurum'a ait ilişkileri Bu Kurum Id ile getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        public Result<List<Iliskiler>> KurumIliskiGetir(int buKurumID)
        {
            var result = List(a => a.BuKurumId == buKurumID && a.AktifMi == 1 && a.BuKisiId == null).Value;
            return result.ToResult();
        }

        /// <summary>
        /// musteri temsilcisine göre bağlı kurum idleri getiren metod
        /// </summary>
        /// <param name="musteriTemsilciId"></param>
        /// <returns></returns>
        public Result<List<int>> MusteriTemsilcisiBagliKurumIdGetir(int musteriTemsilciId)
        {
            var list = _repository.List(a => a.BuKisiId == musteriTemsilciId && a.IliskiTuruId == (int)IliskiTipi.MusteriTemsilcisi && a.AktifMi == 1).Select(a => Convert.ToInt32(a.BuKurumId)).ToList(); // 11, param iliski turleri tablosunda kayıtlı müsteri temsilcisi ID degeri. kayıt işlemi tamamlansın dinamikleştirilecek.
            return list.ToResult();
        }
    }
}