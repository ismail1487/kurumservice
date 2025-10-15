using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
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
    /// Erişim yetkilendirme tanimlarına  ait metotların yer aldığı servis sınıfıdır
    /// </summary>
    public interface IErisimYetkilendirmeTanimlariService : IService<ErisimYetkilendirmeTanimlari>
    {
        /// <summary>
        /// Erişim yetkilendirme tanimlari listesinin tanımlarını getiren method.
        /// </summary>
        /// <returns>erişim yetki tanımları view model listesi döndürür.</returns>
        Result<List<ErisimYetkilendirmeTanimlariListView>> ErisimYetkiTanimListGetir();

        /// <summary>
        /// Erişim yetki tanımı kaydını silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>sonuca göre true veya false döndürür.</returns>
        Result<bool> ErisimYetkiTanimiSil(int id);

        /// <summary>
        /// Erişim yetkilendirme listeleme metodu
        /// </summary>
        /// <returns></returns>
        public List<ErisimYetkilendirmeTanimlari> ErisimYetkilendirmeTanimlariListesi();
    }

    /// <summary>
    /// Erişim yetkilendirme işlemlerini yöneten servis sınıfı
    /// </summary>
    public class ErisimYetkilendirmeTanimlariService : Service<ErisimYetkilendirmeTanimlari>, IErisimYetkilendirmeTanimlariService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IKurumOrganizasyonBirimTanimlariService _kurumOrganizasyonBirimTanimlariService;
        private readonly IParamOrganizasyonBirimleriService _paramOrganizasyonBirimleriService;

        /// <summary>
        /// Erişim yetkilendirme işlemlerini yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="kurumOrganizasyonBirimTanimlariService"></param>
        /// <param name="sistemSayfalariService"></param>
        /// <param name="paramOrganizasyonBirimleriService"></param>
        /// <param name="requestHelper"></param>
        public ErisimYetkilendirmeTanimlariService(IRepository<ErisimYetkilendirmeTanimlari> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<ErisimYetkilendirmeTanimlariService> logger, IKurumOrganizasyonBirimTanimlariService kurumOrganizasyonBirimTanimlariService, ISistemSayfalariService sistemSayfalariService, IParamOrganizasyonBirimleriService paramOrganizasyonBirimleriService,  IRequestHelper requestHelper) : base(repository, dataMapper, serviceProvider, logger)
        {
            _kurumOrganizasyonBirimTanimlariService = kurumOrganizasyonBirimTanimlariService;
            _paramOrganizasyonBirimleriService = paramOrganizasyonBirimleriService;
            _requestHelper = requestHelper;
        }

        /// <summary>
        /// Erişim yetkilendirme tanimlari listesinin tanımlarını getiren method.
        /// </summary>
        /// <returns>erişim yetki tanımları view model listesi döndürür.</returns>
        public Result<List<ErisimYetkilendirmeTanimlariListView>> ErisimYetkiTanimListGetir()
        {
            var sayfalar = _requestHelper.Get<Result<List<SistemSayfalari>>>(LocalPortlar.IYSService + "/api/menu/YetkiIcinSayfaGetir/tr-TR").Result.Value;
           
            var yetkiList = this.List(w => w.AktifMi == 1 && w.SilindiMi == 0).Value;
            var yetkiViewList = new List<ErisimYetkilendirmeTanimlariListView>();
            //Yetki ve sayfalardaki organizasyon birimlerinin karşılaştırma sorgusuna göre liste çeken döngü

            foreach (var yetki in yetkiList)
            {
                var birimTanim = _kurumOrganizasyonBirimTanimlariService.SingleOrDefault(yetki.IlgiliKurumOrganizasyonBirimTanimiId);
                if (birimTanim.Value != null)
                {
                    var value = _paramOrganizasyonBirimleriService.SingleOrDefault(Convert.ToInt32(birimTanim.Value.OrganizasyonBirimTipiId));
                    if (value.Value != null)
                    {
                        var orgBirim = value.Value;
                        var sayfa = sayfalar.FirstOrDefault(x => x.TabloID == yetki.ErisimYetkisiVerilenSayfaId);
                        if (sayfa != null)
                        {
                            var yetkiView = new ErisimYetkilendirmeTanimlariListView()
                            {
                                TabloID = yetki.TabloID,
                                BirimTanimiAdi = birimTanim.Value.BirimTanim,
                                OrganizasyonBirimAdi = orgBirim.ParamTanim,
                                SayfaAdi = sayfa.SayfaTanimi,
                                KurumID = yetki.KurumID
                            };
                            yetkiViewList.Add(yetkiView);
                        }
                    }
                }
            }
            return yetkiViewList.ToResult();
        }

        /// <summary>
        /// Erişim yetki tanımı kaydını silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>sonuca göre true veya false döndürür.</returns>
        public Result<bool> ErisimYetkiTanimiSil(int id)
        {
            if (id == 0)
            {
                return Results.Fail("Silme işleminiz başarısız!", ResultStatusCode.DeleteError);
            }
            var yetki = this.SingleOrDefault(id).Value;
            yetki.AktifMi = 0;
            yetki.SilindiMi = 1;
            yetki.SilinmeTarihi = DateTime.Now;

            var result = this.Update(yetki);
            return true.ToResult();
        }

        /// <summary>
        /// Erişim yetkilendirme listeleme metodu
        /// </summary>
        /// <returns></returns>
        public List<ErisimYetkilendirmeTanimlari> ErisimYetkilendirmeTanimlariListesi()
        {
            var list = this.List(a => a.AktifMi == 1 && a.SilindiMi == 0).Value;
            return list;
        }
    }
}