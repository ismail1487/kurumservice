using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace Baz.Service
{
    /// <summary>
    /// KurumKişiler tablosuna ait işlemlerin yer aldığı servis sınıfıdır
    /// </summary>
    public interface IKurumlarKisilerService : IService<KurumlarKisiler>
    {
        /// <summary>
        /// KurumKisiler tablosunda kişi Id'ye göre bilgileri getiren metot
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<List<KurumlarKisiler>> KisiIDileListGetir(int kisiID);

        /// <summary>
        /// organizasyon birimine göre kiş ıdlerini getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<List<int>> KurumkisiGetirOrganizasyonKurumBirimIDYeGore(List<int> model);

        /// <summary>
        /// Organizsyon birim Id ile ilgili birime tanımlı kişileri listeleyen metot.
        /// </summary>
        /// <param name="organizasyonBirimId">Organizsyon Birim Id</param>
        /// <returns>Kisi temel bilgiler listesi döndürür. <see cref="KisiTemelBilgiler"/></returns>
        Result<List<KisiTemelBilgiler>> ListKisiId(int organizasyonBirimId);

        /// <summary>
        /// Kişi müşteri temsilcisi mi kontrolü sağlayan metot.
        /// </summary>
        /// <param name="kisiId">kişi Id</param>
        /// <returns>sonucu true veya false olarak döndürür.</returns>
        Result<bool> KisiMusteriTemsilcisiMi(int kisiId);
    }

    /// <summary>
    /// KurumKişiler tablosuna ait işlemleri yöneten servis sınıfı
    /// </summary>
    public class KurumlarKisilerService : Service<KurumlarKisiler>, IKurumlarKisilerService
    {
        private readonly IKisiService _kisiService;

        /// <summary>
        /// KurumKişiler tablosuna ait işlemleri yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="kisiService"></param>
        public KurumlarKisilerService(IRepository<KurumlarKisiler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumlarKisilerService> logger, IKisiService kisiService) : base(repository, dataMapper, serviceProvider, logger)
        {
            _kisiService = kisiService;
        }

        /// <summary>
        /// KurumKisiler tablosunda kişi Id'ye göre bilgileri getiren metot
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<List<KurumlarKisiler>> KisiIDileListGetir(int kisiID)
        {
            var list = this.List(a => a.IlgiliKisiId == kisiID && a.AktifMi == 1);
            return list;
        }

        /// <summary>
        /// organizasyon birimine göre kiş ıdlerini getirir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<List<int>> KurumkisiGetirOrganizasyonKurumBirimIDYeGore(List<int> model)
        {
            if (model.Count == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            var list = this.List(a => model.Contains(a.KurumOrganizasyonBirimTanimId) && a.AktifMi == 1).Value.Select(x => x.IlgiliKisiId).ToList().ToResult();

            return list;
        }

        /// <summary>
        /// Organizsyon birim Id ile ilgili birime tanımlı kişileri listeleyen metot.
        /// </summary>
        /// <param name="organizasyonBirimId">Organizsyon Birim Id</param>
        /// <returns>Kisi temel bilgiler listesi döndürür. <see cref="KisiTemelBilgiler"/></returns>
        public Result<List<KisiTemelBilgiler>> ListKisiId(int organizasyonBirimId)
        {
            if (organizasyonBirimId == 0)
            {
                return Results.Fail("Güncelleme işleminiz başarısız!", ResultStatusCode.ReadError);
            }
            var ids = _repository.List(p => p.KurumOrganizasyonBirimTanimId == organizasyonBirimId).Select(p => p.IlgiliKisiId).ToList();
            return _kisiService.ListForQery().Where(p => ids.Contains(p.TabloID) && p.AktifMi == 1).ToList().ToResult();
        }

        /// <summary>
        /// Kişi müşteri temsilcisi mi kontrolü sağlayan metot.
        /// </summary>
        /// <param name="kisiId">kişi Id</param>
        /// <returns>sonucu true veya false olarak döndürür.</returns>
        public Result<bool> KisiMusteriTemsilcisiMi(int kisiId)
        {
            var _organizasyon = _serviceProvider.GetService<IKurumOrganizasyonBirimTanimlariService>();
            var temsilciOrgTanim = _organizasyon.List(a => a.BirimTanim.ToLower().Contains("müşteri temsilcisi") && a.AktifMi == 1).Value.Select(a => a.TabloID);
            var kontrol = _repository.List(a => a.IlgiliKisiId == kisiId && a.AktifMi == 1 && temsilciOrgTanim.Contains(a.KurumOrganizasyonBirimTanimId)).Any();
            if (kontrol)
                return true.ToResult();
            return false.ToResult();
        }
    }
}