using Baz.AletKutusu;
using Baz.AOP.Logger.ExceptionLog;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Baz.Service.Helper;
using Castle.Core.Internal;
using Decor;
using Microsoft.AspNetCore.Components.Forms;
//using BazWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Security;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Baz.Model.Entity.ViewModel.KaynakTanimlariRezerveVM;
using static Baz.Service.Helper.ExpressionUtils;

namespace Baz.Service
{
    /// <summary>
    /// Veritabanında KurumTemelBilgiler tablosun ile ilgil işlemleri içeren ve yöneten interface.
    /// </summary>
    public interface IKurumService : IService<KurumTemelBilgiler>
    {
        /// <summary>
        /// verilen string parametresini içeren kurumları listeleyip getiren method.
        /// </summary>
        /// <param name="kurumAdi">eşleşme için kullanılacak string parametresi</param>
        /// <returns>listeme sonucunu döndürür.</returns>
        Result<List<KurumTemelBilgiler>> KurumlarList(string kurumAdi);
        Result<List<SistemMenuTanimlariWM>> SistemSayfaBasliklariGetir();
        /// <summary>
        /// Kurum temel bilgileri kurum Id'ye göre listeleyen metot(kendisi hariç)
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        Result<List<KurumTemelBilgiler>> List(int kurumId);

        /// <summary>
        /// kurum idleri ve kaşılaştırma tiplerine göre listeleme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="_kurumIdleri"></param>
        /// <returns></returns>
        Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> _kurumIdleri);

        /// <summary>
        /// Kurum temel bilgileri kurum Id'ye göre listeleyen metot(kendisi ile)
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        Result<List<KurumTemelBilgiler>> ListKendisiIle(int kurumId);

        /// <summary>
        /// Temel Kurum Kaydı işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="model">kaydedilecek kurum temel kayı verisini içeren <see cref="KurumTemelKayitModel"/> modeli.</param>
        /// <returns>listeme sonucunu döndürür.</returns>
        Result<KurumTemelKayitModel> KurumTemelVerileriKaydet(KurumTemelKayitModel model);

        /// <summary>
        /// Id'ye göre sonucun döndürüldüğü methodtur.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Result<KurumTemelKayitModel> KurumTemelVerileriGetir(int id);

        /// <summary>
        /// Temel Kurum güncelleme işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="model">kaydedilecek kurum temel kayı verisini içeren <see cref="KurumTemelKayitModel"/> modeli.</param>
        /// <returns>listeme sonucunu döndürür.</returns>
        Result<KurumTemelKayitModel> KurumTemelVerileriGuncelle(KurumTemelKayitModel model);

        /// <summary>
        /// Temel Kurum silindi yapma işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<bool> TemelKurumSilindiYap(int kurumID);



        /// <summary>
        /// Id'ye göre Kurum İdari verilerin döndürüldüğü methodtur.
        /// </summary>
        /// <returns></returns>
        Result<KurumIdariProfilModel> KurumIdarilVerileriGetir(int kurumID);

        /// <summary>
        /// Kuruma ait Müşteri temsilcilerini getiren metod
        /// </summary>
        /// <returns></returns>
        Result<List<GenelViewModel>> KurumMusteriTemsilcisiGetir();

        /// <summary>
        /// Musteri temsilcisi Idye göre bağlı kurumlarını getiren method
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        Result<List<KurumTemelBilgiler>> MusteriTemsilcisiBagliKurumlarList(int musteriTemsilcisiId);

        /// <summary>
        /// Amirlere Ast Musteri Temsilcisi Kurumlarini Getiren method
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        Result<List<KurumTemelBilgiler>> AmirlereAstMusteriTemsilcisiKurumlariniGetir(int amirId);

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kurumlara erişmesi gereken kullanıcılar için kullanılacak kurum listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KurumTemelBilgiler"></see></returns>
        public Result<List<KurumTemelBilgiler>> HiyerarsiDisiKisilerKurumListesi();

        /// <summary>
        /// Sorguların listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public IQueryable<KurumTemelBilgiler> ListForQuery();


        /// <summary>
        /// İçerik kategorileri Listeleyen Metod
        /// </summary>
        public Result<List<ParamIcerikKategoriler>> IcerikKategorileriGetir();


        /// <summary>
        /// İçeriklerin Kaydedilmesini metod
        /// </summary>
        public Result<IcerikKutuphanesiMedyalarVM> IcerikKaydet(IcerikKutuphanesiMedyalarVM model);

        /// <summary>
        /// İçeriklerin Güncelleyen metod
        /// </summary>
        public Result<IcerikKutuphanesiMedyalarVM> IcerikGuncelle(IcerikKutuphanesiMedyalarVM model);

        /// <summary>
        /// İçeriklerin Verilerini Çeken metod
        /// </summary>
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikVeriGetir(int urlId);

        /// <summary>
        /// İçerikleri Silen metod
        /// </summary>
        public Result<bool> IcerikSil(int TabloId);

        /// <summary>
        /// İçeriklerin Kaydedilmesini metod
        /// </summary>
        public Result<MedyaKutuphanesi> MedyaKayit(MedyaKutuphanesi model);


        /// <summary>
        /// ıcerik kutuphanesinin bağımsız içerik bilgilerini listeleyen metod
        /// </summary>
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikListesi();

        public Result<IcerikKutuphanesiMedyalarVM> IcerikVerileriniGetir(int icerikID);

        /// <summary>
        /// Urun kategorileri Listeleyen Metod
        /// </summary>
        public Result<List<ParamUrunKategoriler>> UrunKategorileriGetir();

        /// <summary>
        /// Parabirimlerini kategorileri Listeleyen Metod
        /// </summary>
        public Result<List<ParamParaBirimleri>> UrunParaBirimiGetir();

        /// <summary>
        /// Urunleri Kayıteden Metod
        /// </summary>
        public Result<UrunKutuphanesiMedyalarVM> UrunKaydet(UrunKutuphanesiMedyalarVM model);

        /// <summary>
        /// Urunleri Listeleyen Metod
        /// </summary>
        public Result<List<UrunKutuphanesiMedyalarVM>> UrunListesi();
        /// <summary>
        /// Ürünlerin Silen metod
        /// </summary>
        public Result<bool> UrunSil(int TabloId);

        public Result<List<UrunKutuphanesiMedyalarVM>> UrunVeriGetir(int urlId);

        public Result<UrunKutuphanesiMedyalarVM> UrunGuncelle(UrunKutuphanesiMedyalarVM model);
        /// <summary>
        ///Urun Kategorilerin listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamUrunMarkalar>> ParamUrunMarkalarınıGetir();
        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamOlcumBirimleri>> UrunParametreDegerGetir();
        public Result<List<ParamOlcumBirimleri>> UrunOlcumBirimiData();
        public Result<List<ParamIcerikBlokKategorileri>> IcerikBlokKategorilerGetir();
        public Result<List<IcerikKutuphanesi>> IcerikKutuphanesiGetir();

        /// <summary>
        /// Veri getiren Kaynak Rezerve Methdları
        /// </summary>
        public Result<List<ParamKaynakTipleri>> KaynakTipiGetir();
        public Result<KaynakTanimlariRezerveVM> RezerveKaynakGuncelle(KaynakTanimlariRezerveVM model);
        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveVeriGetir(int urlId);
        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveListele();
        public Result<KaynakTanimlariRezerveVM> KaynakRezerveKayit(KaynakTanimlariRezerveVM model);
        public Result<bool> SilKaynak(int id);

        /// <returns></returns>
        public Result<SliderTemelBilgilerMedyalarVM> SliderKaydet(SliderTemelBilgilerMedyalarVM model);

        /// <summary>
        /// Urunleri Listeleyen Metod
        /// </summary>
        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderListesi();
        /// <summary>
        /// Ürünlerin Silen metod
        /// </summary>
        public Result<bool> SliderSil(int TabloId);
        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderVeriGetir(int urlId);
        public Result<SliderTemelBilgilerMedyalarVM> SliderGuncelle(SliderTemelBilgilerMedyalarVM model);
        ///<summary>
        ///Slider Methodlar
        ///</summary>
    }

    /// <summary>
    /// Veritabanında KurumTemelBilgiler tablosun ile ilgil işlemleri içeren ve yöneten class, Service class'ı ve IKurumService interface'ini baz alır.
    /// </summary>
    public class KurumService : Service<KurumTemelBilgiler>, IKurumService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestHelper _requestHelper;
        private readonly IKurumAdresBilgileriService _kurumAdresBilgileriService;
        private readonly IKurumBankaBilgileriService _kurumBankaBilgileriService;
        private readonly ILoginUser _loginUser;
        private readonly IKurumIliskiService _kurumIliskiService;
        private readonly IKisiIliskiService _kisiIliskiService;
        private readonly IParamIcerikTipleriService _paramIcerikTipleriService;
        private readonly IParamIcerikKategorileriService _paramIcerikKategorileriService;
        private readonly IIcerikKutuphanesiService _icerikKutuphanesiService;
        private readonly IIcerikGrubuEssizBilgilerService _icerikGrubuEssizBilgilerService;
        private readonly IMedyaKutuphanesiService _medyaKutuphanesiService;
        private readonly IIcerikKutuphanesiMedyalarService _icerikKutuphanesiMedyalarService;
        private readonly IParamDillerService _paramDillerService;
        private readonly IParamDisPlatformlarService _paramDisPlatformlarService;
        private readonly ISistemSayfalariService _sistemSayfalariService;
        private readonly ISistemMenuTanimlariAyrintilarService _sistemMenuTanimlariAyrintilarService;
        private readonly IIcerikSpotMetinleriService _icerikSpotMetinleriService;
        private readonly IParamMedyaTipleriService _paramMedyaTipleriService;
        private readonly IUrunKutuphanesiMedyalarService _urunKutuphanesiMedyalarService;
        private readonly IUrunKutuphanesiService _urunKutuphanesiService;
        private readonly IUrunParametrelerService _urunParametrelerService;
        private readonly IParamUrunKategorilerService _paramUrunKategorilerService;
        private readonly IParamParaBirimleriService _paramParaBirimleriService;
        private readonly IParamUrunMarkalarService _paramUrunMarkalarService;
        private readonly IParamOlcumKategorileriService _paramOlcumKategorileriService;
        private readonly IUrunParametreDegerleriService _urunParametreDegerleriService;
        private readonly IParamOlcumBirimleriService _paramOlcumBirimleriService;
        private readonly IUrunIcerikBloklariMedyalarService _urunIcerikBloklariMedyalarService;
        private readonly IParamIcerikBlokKategorileriService _paramIcerikBlokKategorileriService;
        private readonly IUrunIcerikBloklariService _urunIcerikBloklariService;
        private readonly IKaynakTanimlariService _kaynakTanimlariService;
        private readonly IParamKaynakTipiService _paramKaynakTipiService;
        private readonly IKaynakRezerveTanimlariService _kaynakRezerveTanimlariService;
        private readonly IKaynakTanimlariMedyalarService _kaynakTanimlariMedyalarService;
        private readonly IKaynakGunIciIstisnaTanimlariService _kaynakGunIciIstisnaTanimlariService;
        private readonly IKaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService;
        private readonly ISliderResimlerMedyaService _sliderResimlerMedyaService;
        private readonly ISliderTemelBilgilerService _sliderTemelBilgilerService;
        private bool isDevelopment;

        /// <summary>
        /// Veritabanında KurumTemelBilgiler tablosun ile ilgil işlemleri içeren ve yöneten classın yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="requestHelper"></param>
        /// <param name="kurumAdresBilgileriService"></param>
        /// <param name="kurumBankaBilgileriService"></param>
        /// <param name="kurumIliskiService"></param>
        /// <param name="kisiIliskiService"></param>
        /// <param name="loginUser"></param>
        public KurumService(IRepository<KurumTemelBilgiler> repository, IDataMapper dataMapper,
            IServiceProvider serviceProvider, ILogger<KurumService> logger, IRequestHelper requestHelper,
            ISliderResimlerMedyaService sliderResimlerMedyaService, ISliderTemelBilgilerService sliderTemelBilgilerService,
            IKurumAdresBilgileriService kurumAdresBilgileriService, IParamMedyaTipleriService paramMedyaTipleriService, IIcerikSpotMetinleriService icerikSpotMetinleriService, IParamIcerikKategorileriService paramIcerikKategorileriService, ISistemMenuTanimlariAyrintilarService sistemMenuTanimlariAyrintilarService, ISistemSayfalariService sistemSayfalariService, IKurumBankaBilgileriService kurumBankaBilgileriService, IKurumIliskiService kurumIliskiService, IParamIcerikTipleriService paramIcerikTipleriService, IParamDisPlatformlarService paramDisPlatformlarService, IIcerikKutuphanesiService icerikKutuphanesiService, IIcerikGrubuEssizBilgilerService icerikGrubuEssizBilgilerService, IParamDillerService paramDillerService, IKisiIliskiService kisiIliskiService, IMedyaKutuphanesiService medyaKutuphanesiService, IIcerikKutuphanesiMedyalarService icerikKutuphanesiMedyalarService, ILoginUser loginUser, IUrunParametrelerService urunParametrelerService, IUrunKutuphanesiService urunKutuphanesiService, IUrunKutuphanesiMedyalarService urunKutuphanesiMedyalarService, IParamUrunKategorilerService paramUrunKategorilerService, IParamParaBirimleriService paramParaBirimleriService, IParamOlcumKategorileriService paramOlcumKategorileriService, IUrunParametreDegerleriService urunParametreDegerleriService, IParamUrunMarkalarService paramUrunMarkalarService, IParamOlcumBirimleriService paramOlcumBirimleriService, IUrunIcerikBloklariMedyalarService urunIcerikBloklariMedyalarService, IParamIcerikBlokKategorileriService paramIcerikBlokKategorileriService, IUrunIcerikBloklariService urunIcerikBloklariService, IParamKaynakTipiService paramKaynakTipiService, IKaynakTanimlariService kaynakTanimlariService, IKaynakRezerveTanimlariService kaynakRezerveTanimlariService, IKaynakTanimlariMedyalarService kaynakTanimlariMedyalarServicei, IKaynakGunIciIstisnaTanimlariService kaynakGunIciIstisnaTanimlariService, IKaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService kaynakRezerveTanimlariAralikBaremleriKapasiteTanimi) : base(repository, dataMapper, serviceProvider,
            logger)
        {
            _requestHelper = requestHelper;
            _kurumAdresBilgileriService = kurumAdresBilgileriService;
            _kurumIliskiService = kurumIliskiService;
            _kisiIliskiService = kisiIliskiService;
            _loginUser = loginUser;
            _kurumBankaBilgileriService = kurumBankaBilgileriService;
            _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
            _paramIcerikTipleriService = paramIcerikTipleriService;
            _paramIcerikKategorileriService = paramIcerikKategorileriService;
            _icerikKutuphanesiService = icerikKutuphanesiService;
            _icerikGrubuEssizBilgilerService = icerikGrubuEssizBilgilerService;
            _medyaKutuphanesiService = medyaKutuphanesiService;
            _icerikKutuphanesiMedyalarService = icerikKutuphanesiMedyalarService;
            _paramDillerService = paramDillerService;
            _paramDisPlatformlarService = paramDisPlatformlarService;
            _sistemSayfalariService = sistemSayfalariService;
            _sistemMenuTanimlariAyrintilarService = sistemMenuTanimlariAyrintilarService;
            _icerikSpotMetinleriService = icerikSpotMetinleriService;
            _paramMedyaTipleriService = paramMedyaTipleriService;
            _paramUrunKategorilerService = paramUrunKategorilerService;
            _urunKutuphanesiMedyalarService = urunKutuphanesiMedyalarService;
            _urunKutuphanesiService = urunKutuphanesiService;
            _urunParametrelerService = urunParametrelerService;
            _paramParaBirimleriService = paramParaBirimleriService;
            _paramOlcumKategorileriService = paramOlcumKategorileriService;
            _urunParametreDegerleriService = urunParametreDegerleriService;
            _paramUrunMarkalarService = paramUrunMarkalarService;

            _paramOlcumBirimleriService = paramOlcumBirimleriService;
            _urunIcerikBloklariMedyalarService = urunIcerikBloklariMedyalarService;
            _urunIcerikBloklariService = urunIcerikBloklariService;
            _paramIcerikBlokKategorileriService = paramIcerikBlokKategorileriService;

            _paramKaynakTipiService = paramKaynakTipiService;
            _kaynakTanimlariService = kaynakTanimlariService;
            _kaynakRezerveTanimlariService = kaynakRezerveTanimlariService;
            _kaynakTanimlariMedyalarService = kaynakTanimlariMedyalarServicei;
            _kaynakGunIciIstisnaTanimlariService = kaynakGunIciIstisnaTanimlariService;
            _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService = kaynakRezerveTanimlariAralikBaremleriKapasiteTanimi;
            _sliderResimlerMedyaService = sliderResimlerMedyaService;
            _sliderTemelBilgilerService = sliderTemelBilgilerService;
        }


        /// <summary>
        /// verilen string parametresini içeren kurumları listeleyip getiren method.
        /// </summary>
        /// <param name="kurumAdi">eşleşme için kullanılacak string parametresi</param>
        /// <returns>listeme sonucunu döndürür.</returns>

        public Result<List<KurumTemelBilgiler>> KurumlarList(string kurumAdi)
        {
            //if (kurumAdi == null)
            //{
            //    return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            //}
            //var result = List(x => x.KurumTicariUnvani.Contains(kurumAdi));
            //return result;
            return kurumAdi == null
                ? Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError)
                : List(x => x.KurumTicariUnvani.Contains(kurumAdi));
        }

        /// <summary>
        /// Kurum temel bilgileri kurum Id'ye göre listeleyen metot(kendisi hariç)
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> List(int kurumId)
        {
            if (kurumId == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            return List(p => p.KurumID == kurumId && p.AktifMi == 1 && p.SilindiMi == 0);
        }

        /// <summary>
        /// kurum idleri ve kaşılaştırma tiplerine göre listeleme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="_kurumIdleri"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> _kurumIdleri)
        {
            var result = _repository.List(x => x.AktifMi == 1 && _kurumIdleri.Contains(x.TabloID)).AsQueryable().WhereBuilder(comparisonType, buildPredicateModels).ToList();
            return result.Select(p => new KeyValueModel() { Key = p.KurumKisaUnvan, Value = p.TabloID.ToString() }).ToList().ToResult();
        }
        public Result<List<SistemMenuTanimlariWM>> SistemSayfaBasliklariGetir()
        {

            var menuTanimlari = _sistemMenuTanimlariAyrintilarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;

            var menuTanimlariListesi2 = new List<SistemMenuTanimlariWM>();

            var listelerListesi = new List<SistemMenuTanimlariWM>();
            var destekListesi = new List<SistemMenuTanimlariWM>();
            var maliListesi = new List<SistemMenuTanimlariWM>();
            var formlarAnketlerListesi = new List<SistemMenuTanimlariWM>();
            var tanimlamalarListesi = new List<SistemMenuTanimlariWM>();
            var tasarimPersoneliListesi = new List<SistemMenuTanimlariWM>();
            var dovmeAkisModuluListesi = new List<SistemMenuTanimlariWM>();
            var dovmePersoneliListesi = new List<SistemMenuTanimlariWM>();
            var organizasyonListesi = new List<SistemMenuTanimlariWM>();
            var anaOperasyonelAkisListesi = new List<SistemMenuTanimlariWM>();
            var anaBasliklarListesi = new List<SistemMenuTanimlariWM>();


            foreach (var item in menuTanimlari)
            {
                var menuBaglantiliSayfaID = item.MenuBaglantiliSistemSayfaId;
                //var menuUrlleri = _sistemMenuTanimlariAyrintilarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0&&x.MenuBaglantiliSistemSayfaId== menuBaglantiliSayfaID).Value.FirstOrDefault().MenuBaglantiliSistemSayfaId;
                var menuUrlleriItem = _sistemSayfalariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == menuBaglantiliSayfaID).Value.FirstOrDefault();
                var menuUrlleri = menuUrlleriItem != null ? menuUrlleriItem.SayfaUrl : null;
                var ustID = item.UstId;
                var siraNo = item.SiraNo;

                if (ustID != null)
                {

                    if (ustID == 442)//Listeler:442
                    {
                        var listeler = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo,
                        };
                        listelerListesi.Add(listeler);
                    }
                    else if (ustID == 441)//Destek:441
                    {
                        var destek = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        destekListesi.Add(destek);
                    }
                    else if (ustID == 440)//Mali:440
                    {
                        var mali = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo

                        };
                        maliListesi.Add(mali);
                    }
                    else if (ustID == 439)//Formlar / Anketler:439
                    {
                        var formlarAnketler = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        formlarAnketlerListesi.Add(formlarAnketler);
                    }
                    else if (ustID == 438)//Tanımlamalar:438
                    {
                        var tanimlamalar = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        tanimlamalarListesi.Add(tanimlamalar);
                    }
                    else if (ustID == 437)//Tasarım Personeli:437
                    {
                        var tasarimPersoneli = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        tasarimPersoneliListesi.Add(tasarimPersoneli);
                    }
                    else if (ustID == 436) //Dövme Akış Modülü:436
                    {
                        var dovmeAkisModulu = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        dovmeAkisModuluListesi.Add(dovmeAkisModulu);
                    }
                    else if (ustID == 435)  //Dövme Personeli:435
                    {
                        var dovmePersoneli = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        dovmePersoneliListesi.Add(dovmePersoneli);
                    }
                    else if (ustID == 434)   //Organizasyon:434
                    {
                        var organizayson = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        }; organizasyonListesi.Add(organizayson);
                    }
                    else if (ustID == 433)//Ana Operasyonel Akış:433
                    {
                        var anaOperasyonelAkis = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        anaOperasyonelAkisListesi.Add(anaOperasyonelAkis);
                    }
                    else if (ustID == 0)
                    {
                        var anaBasliklar = new SistemMenuTanimlariWM()
                        {
                            MenuBasligi = item.MenuBasligi,
                            TabloID = item.TabloID,
                            UstID = item.UstId ?? -1,
                            MenuBaglantiliSistemSayfaID = item.MenuBaglantiliSistemSayfaId ?? 0,
                            SistemMenuTanimID = item.SistemMenuTanimId ?? 1,
                            SayfaTanimi = menuUrlleri,
                            SiraNo = siraNo
                        };
                        anaBasliklarListesi.Add(anaBasliklar);
                    }


                }

            }
            var menuTanimlariListesi = new SistemMenuTanimlariWM()
            {

                Listeler = listelerListesi,
                Destek = destekListesi,
                Mali = maliListesi,
                FormlarAnketler = formlarAnketlerListesi,
                Tanimlamalar = tanimlamalarListesi,
                TasarimPersoneli = tasarimPersoneliListesi,
                DovmeAkisModulu = dovmeAkisModuluListesi,
                DovmePersoneli = dovmePersoneliListesi,
                Organizayson = organizasyonListesi,
                AnaOperasyonelAkis = anaOperasyonelAkisListesi,
                AnaBasliklar = anaBasliklarListesi

            };
            menuTanimlariListesi2.Add(menuTanimlariListesi);
            return menuTanimlariListesi2.ToResult();

        }

        /// <summary>
        /// Kurum temel bilgileri kurum Id'ye göre listeleyen metot(kendisi ile birlikte)
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> ListKendisiIle(int kurumId)
        {
            if (kurumId == 0)
            {
                throw new OctapullException(OctapullExceptions.MissingDataError);
            }
            return List(p => p.AktifMi == 1 && p.KurumID == kurumId || p.TabloID == kurumId);// 19.11.2020 Alt kurumların yanında kurumun kendisi de gelmesi gerekiyordu.
        }

        /// <summary>
        /// Temel Kurum Kaydı işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="model">kaydedilecek kurum temel kayı verisini içeren <see cref="KurumTemelKayitModel"/> modeli.</param>
        /// <returns>listeme sonucunu döndürür.</returns>

        public Result<KurumTemelKayitModel> KurumTemelVerileriKaydet(KurumTemelKayitModel model)
        {
            var ayniKurum = this.List(x => x.KurumKisaUnvan == model.KurumAdi && x.AktifMi == 1 && x.KurumID == model.KurumId).Value;

            if (ayniKurum.Count > 0)
            {
                throw new OctapullException(OctapullExceptions.DuplicateDataError);
            }
            try
            {
                DataContextConfiguration().BeginNewTransactionIsNotFound();
                var kurumTemelBilgilerModel = new KurumTemelBilgiler()
                {
                    KurumTicariUnvani =
                        model.KurumAdi, //formda alınmadığı için kısa ünvan basıldı. 12.11.2020 Mustafa Can Semerci.
                    KurumKisaUnvan = model.KurumAdi,
                    EpostaAdresi = model.EpostaAdresi,
                    FaxNo = model.FaxNo,
                    TicaretSicilNo = model.TicaretSicilNo,
                    KurumVergiDairesiId = model.KurumVergiDairesiId,
                    WebSitesi = model.WebSitesi,
                    KurulusTarihi = Convert.ToDateTime(model.KurulusTarihi).Year == 1 ? null : Convert.ToDateTime(model.KurulusTarihi),
                    KurumLogoId = model.KurumLogo,
                    AktifMi = 1,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    KurumVergiNo = model.VergiNo,
                    KurumID = _loginUser.KurumID,
                    KurumUlkeId = model.KurumUlkeId,
                    KurumSehirId = model.KurumSehirId,
                    KisiID = _loginUser.KisiID
                };


                var temelResult = this.Add(kurumTemelBilgilerModel).Value;

                model.TabloID = temelResult.TabloID;
                //Ek tablo kayıtlarını yapar
                if (model.AdresListesi != null)
                {
                    foreach (var adres in model.AdresListesi)
                    {
                        var KisiAdresModel = new KurumAdresBilgileri()
                        {
                            KurumID = _loginUser.KurumID,
                            IlgiliKurumId = temelResult.TabloID,
                            LokasyonTipi = adres.LokasyonTipi,
                            Adres = adres.Adres,
                            Ulke = adres.Ulke,
                            Sehir = adres.Sehir,
                            AktifMi = 1,
                            KayitTarihi = DateTime.Now,
                            GuncellenmeTarihi = DateTime.Now,
                            DilID = 1,
                            SilindiMi = 0,
                            KisiID = _loginUser.KisiID
                        };
                        _kurumAdresBilgileriService.Add(KisiAdresModel);
                    }
                }

                if (model.BankaListesi != null)
                {
                    foreach (var banka in model.BankaListesi)
                    {
                        var kurumBankaModel = new KurumBankaBilgileri()
                        {
                            KurumID = _loginUser.KurumID,
                            IlgiliKurumId = temelResult.TabloID,
                            BankaId = banka.BankaId,
                            SubeId = banka.SubeId,
                            HesapNo = banka.HesapNo,
                            Iban = banka.Iban,
                            AktifMi = 1,
                            KayitTarihi = DateTime.Now,
                            GuncellenmeTarihi = DateTime.Now,
                            DilID = 1,
                            SilindiMi = 0,
                            KisiID = _loginUser.KisiID
                        };
                        _kurumBankaBilgileriService.Add(kurumBankaModel);
                    }
                }
                //Kurum tipi müşteri temsilcisi ise ilişki kayıtlarını atar
                if (model.KurumTips != null)
                {
                    foreach (var tips in model.KurumTips)
                    {
                        var kurumIliskiModel = new KurumIliskiKayitModel()
                        {
                            KayitEdenID = _loginUser.KisiID,
                            GuncelleyenKisiID = _loginUser.KisiID,
                            KurumID = _loginUser.KurumID,
                            BuKurumID = temelResult.TabloID,
                            BununKurumID = _loginUser.KurumID,
                            IliskiTuruID = tips.TabloID,
                        };
                        _kurumIliskiService.KurumIliskiKaydet(kurumIliskiModel);
                    }
                }
                //Kurum-Kişi müştteri temsilcisi kaydını atar
                if (model.MusteriTemsilciIds != null)
                {
                    foreach (var Ids in model.MusteriTemsilciIds)
                    {
                        var rs = new KisiIliskiKayitModel()
                        {
                            BuKisiID = Ids.TabloID,
                            BununKisiID = temelResult.TabloID,
                            GuncelleyenKisiID = _loginUser.KisiID,
                            KayıtEdenID = _loginUser.KisiID,
                            KurumID = _loginUser.KurumID,
                            IliskiTuruID = (int)IliskiTipi.MusteriTemsilcisi//11
                        };
                        _kisiIliskiService.KisiIliskiKaydet(rs);
                    }
                }
                DataContextConfiguration().Commit();
                //Kurumdaki hedef kurum-kişileri güncelleme
                //Session güncellemesi
                var sessionId = _httpContextAccessor.HttpContext.Request.Headers["sessionid"][0];
                _requestHelper.Get<Result<bool>>(LocalPortlar.UserLoginregisterService + "/api/LoginRegister/KimlikGuncelle/" + sessionId);
            }
            catch (Exception)
            {
                DataContextConfiguration().RollBack();
                throw;
            }

            return model.ToResult();
        }




        /// <summary>
        /// Id'ye göre sonucun döndürüldüğü methodtur.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Result<KurumTemelKayitModel> KurumTemelVerileriGetir(int id)
        {
            var medyaKutuphanesiService = _serviceProvider.GetService<IMedyaKutuphanesiService>();
            var result = base.SingleOrDefault(id);
            var model = new KurumTemelKayitModel()
            {
                TabloID = result.Value.TabloID,
                KurumAdi = result.Value.KurumKisaUnvan,
                FaxNo = result.Value.FaxNo,
                EpostaAdresi = result.Value.EpostaAdresi,
                KurulusTarihi = Convert.ToDateTime(result.Value.KurulusTarihi).Year == 1 ? null : Convert.ToDateTime(result.Value.KurulusTarihi),
                KurumLogo = result.Value.KurumLogoId,
                KurumVergiDairesiId = result.Value.KurumVergiDairesiId,
                TicaretSicilNo = result.Value.TicaretSicilNo,
                VergiNo = result.Value.KurumVergiNo,
                WebSitesi = result.Value.WebSitesi,
                KurumUlkeId = result.Value.KurumUlkeId,
                KurumSehirId = result.Value.KurumSehirId
            };

            if (result.Value.KurumLogoId > 0)
            {
                var ppResult = medyaKutuphanesiService.SingleOrDefault(result.Value.KurumLogoId);
                if (ppResult.IsSuccess)
                    model.KurumResimUrl = ppResult.Value.MedyaUrl;
            }

            var adreslist = _kurumAdresBilgileriService.KurumIdileGetir(id);
            var adresModelList = new List<KurumAdresModel>();
            foreach (var adres in adreslist.Value)
            {
                var adresList = new KurumAdresModel()
                {
                    Adres = adres.Adres,
                    LokasyonTipi = adres.LokasyonTipi,
                    Sehir = Convert.ToInt32(adres.Sehir),
                    Ulke = Convert.ToInt32(adres.Ulke)
                };
                adresModelList.Add(adresList);
            }

            model.AdresListesi = adresModelList;

            var bankaList = _kurumBankaBilgileriService.KurumIdileGetir(id);
            var bankaListModel = new List<KurumBankaModel>();
            if (bankaList.Value != null)
            {
                foreach (var banka in bankaList.Value)
                {
                    var bankalst = new KurumBankaModel()
                    {
                        HesapNo = banka.HesapNo,
                        Iban = banka.Iban,
                        BankaId = Convert.ToInt32(banka.BankaId),
                        SubeId = Convert.ToInt32(banka.SubeId)
                    };
                    bankaListModel.Add(bankalst);
                }
            }

            model.BankaListesi = bankaListModel;

            var url = LocalPortlar.IYSService + "/api/GenelParametreler/GetParametreNames";

            model = _requestHelper.Post<Result<KurumTemelKayitModel>>(url, model).Result?.Value ?? model;

            var paramIliski = new ParametreRequest()
            {
                ModelName = "ParamIliskiTurleri",
                UstId = 0,
                KurumId = 0,
                TabloID = 0,
                Tanim = "test",
                DilID = 1,
                EsDilID = 0
            };

            var url1 = LocalPortlar.IYSService + "/api/KureselParametreler/ListParam";

            model.KurumTips = new List<KurumTipi>();
            var kurums = _kurumIliskiService.KurumIliskiGetir(id)?.Value ?? new List<Iliskiler>();
            if (kurums.Count > 0)
            {
                foreach (var ks in kurums)
                {
                    var tanim = _requestHelper.Post<Result<List<ParametreResult>>>(url1, paramIliski).Result.Value.Find(a => a.TabloID == ks.IliskiTuruId).Tanim;
                    var kt = new KurumTipi()
                    {
                        TabloID = Convert.ToInt32(ks.IliskiTuruId),
                        Tanim = tanim
                    };
                    model.KurumTips.Add(kt);
                }
            }

            model.MusteriTemsilciIds = new List<MusteriTemsilcisi>();
            var kisiservice = _serviceProvider.GetService<IKisiService>();
            var musteris = _kisiIliskiService.MusteriList(id).Value;
            if (musteris.Count > 0)
            {
                foreach (var ms in musteris)
                {
                    var k = kisiservice.SingleOrDefault(Convert.ToInt32(ms.BuKisiId)).Value;
                    var mst = new MusteriTemsilcisi()
                    {
                        TabloID = Convert.ToInt32(ms.BuKisiId),
                        AdSoyad = k.KisiAdi + " " + k.KisiSoyadi
                    };
                    model.MusteriTemsilciIds.Add(mst);
                }
            }

            return model.ToResult();
        }

        /// <summary>
        /// Id'ye göre Kurum İdari verilerin döndürüldüğü methodtur.
        /// </summary>
        /// <returns></returns>
        public Result<KurumIdariProfilModel> KurumIdarilVerileriGetir(int kurumID)
        {
            var medyaKutuphanesiService = _serviceProvider.GetService<IMedyaKutuphanesiService>();
            var kisiService = _serviceProvider.GetService<IKisiService>();
            var paramBankalarService = _serviceProvider.GetService<IParamBankalarService>();

            var result = base.SingleOrDefault(kurumID);
            var model = new KurumIdariProfilModel()
            {
                TabloID = result.Value.TabloID,
                KurumAdi = result.Value.KurumKisaUnvan,
                FaxNo = result.Value.FaxNo,
                EpostaAdresi = result.Value.EpostaAdresi,
                KurumLogo = result.Value.KurumLogoId,
                TicaretSicilNo = result.Value.TicaretSicilNo,
                VergiNo = result.Value.KurumVergiNo
            };

            if (result.Value.KurumLogoId > 0)
            {
                var ppResult = medyaKutuphanesiService.SingleOrDefault(result.Value.KurumLogoId);
                if (ppResult.IsSuccess)
                    model.KurumResimUrl = ppResult.Value.MedyaUrl;
            }

            var paramBankalar = paramBankalarService.List(x => x.UstId == 0);
            var paramSubeler = paramBankalarService.List(x => x.UstId != 0);
            var bankaList = _kurumBankaBilgileriService.KurumIdileGetir(kurumID);
            var bankaListModel = new List<KurumIdariBankaModel>();
            if (bankaList.Value != null)
            {
                foreach (var banka in bankaList.Value)
                {
                    if (banka.SubeId != 0 && banka.BankaId != 0)
                    {
                        var b = paramBankalar.Value.SingleOrDefault(x => x.TabloID == banka.BankaId.Value).ParamTanim;
                        var s = paramSubeler.Value.SingleOrDefault(x => x.TabloID == banka.SubeId.Value).ParamTanim;
                        var bankalst = new KurumIdariBankaModel()
                        {
                            BankaAdi = b,
                            SubeAdi = s,
                            HesapNo = banka.HesapNo,
                            Iban = banka.Iban,
                            BankaId = Convert.ToInt32(banka.BankaId),
                            SubeId = Convert.ToInt32(banka.SubeId)
                        };
                        bankaListModel.Add(bankalst);
                    }
                }
            }

            model.KurumBankaListesi = bankaListModel;
            /*

            var kurumLisans = _kurumLisansService.List(t => t.IlgiliKurumId == kurumID && t.AktifMi == 1);
            var kurumLisansModel = new List<KurumLisansBilgileri>();
                        foreach (var lisans in kurumLisans.Value)
            {
                var aktif = _requestHelper.Get<Result<int>>(LocalPortlar.LisansServis +
                    "/api/KurumLisans/KurumLisansaBagliAktifKisiSayisi/" + kurumID + "/" + lisans.TabloID);
                var lisansList = new KurumLisansBilgileri()
                {
                    Name = lisans.Name,
                    LisansId = lisans.LisansId,
                    SonKullanimTarihi = lisans.SonKullanimTarihi,
                    LisansKisiSayisi = aktif.Result.Value + "/" + lisans.LisansKisiSayisi,
                    OtomatikLisansYenileme = lisans.OtomatikLisansYenileme,
                };
                kurumLisansModel.Add(lisansList);
            }
            
            model.KurumLisansListesi = new List<KurumLisansBilgileri>();  //kurumLisansModel;

            var kurumLisansKisi = _lisansKurumKisiAbonelikTanimlariService.List(t => t.LisansAboneKurumId == kurumID && t.AktifMi == 1);
            var kurumLisansKisiModel = new List<KurumKisiLisansBilgileri>();
            foreach (var lisans in kurumLisansKisi.Value)
            {
                var lisansResult = _kurumLisansService.SingleOrDefault(lisans.LisansGenelTanimId);
                var kisi = kisiService.SingleOrDefault(lisans.LisansAboneKisiId.Value).Value;
                if (kisi != null && lisansResult.Value != null)
                {
                    var lisansKisiList = new KurumKisiLisansBilgileri()
                    {
                        LisansAboneKisiAdi = kisi.KisiAdi,
                        LisansAboneKisiMail = kisi.KisiEposta,
                        Name = lisansResult.Value.Name,
                        LisansGenelTanimId = lisans.LisansGenelTanimId,
                        LisansAboneKisiId = lisans.LisansAboneKisiId,
                        LisansAboneKurumId = lisans.LisansAboneKurumId,
                        LisansAbonelikBaslangicTarihi = lisans.LisansAbonelikBaslangicTarihi,
                        LisansAbonelikBitisTarihi = lisans.LisansAbonelikBitisTarihi
                    };
                    kurumLisansKisiModel.Add(lisansKisiList);
                }
            }

            model.KurumKisiLisansListesi = kurumLisansKisiModel;
            */
            return model.ToResult();
        }

        /// <summary>
        /// Temel Kurum güncelleme işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="model">kaydedilecek kurum temel kayı verisini içeren <see cref="KurumTemelKayitModel"/> modeli.</param>
        /// <returns>listeme sonucunu döndürür.</returns>

        public Result<KurumTemelKayitModel> KurumTemelVerileriGuncelle(KurumTemelKayitModel model)
        {
            var oldKurum = this.List(x => x.KurumKisaUnvan == model.KurumAdi && x.AktifMi == 1 && x.KurumID == _loginUser.KurumID).Value.Any(a => a.TabloID != model.TabloID);
            if (oldKurum)
            {
                return Results.Fail("Aynı isimli kurum bulunmaktadır.", ResultStatusCode.CreateError);
            }
            var kurumTemelBilgilerModel = this.SingleOrDefault(model.TabloID).Value;
            if (kurumTemelBilgilerModel == null)
            {
                return Results.Fail("Güncelleme işleminiz gerçekleşmemiştir!", ResultStatusCode.UpdateError);
            }

            DataContextConfiguration().BeginNewTransactionIsNotFound();
            kurumTemelBilgilerModel.KurumTicariUnvani =
                model.KurumAdi; //formda alınmadığı için kısa ünvan basıldı. 12.11.2020 Mustafa Can Semerci.
            kurumTemelBilgilerModel.KurumKisaUnvan = model.KurumAdi;
            kurumTemelBilgilerModel.EpostaAdresi = model.EpostaAdresi;
            kurumTemelBilgilerModel.FaxNo = model.FaxNo;
            kurumTemelBilgilerModel.TicaretSicilNo = model.TicaretSicilNo;
            kurumTemelBilgilerModel.KurumVergiDairesiId = model.KurumVergiDairesiId;
            kurumTemelBilgilerModel.WebSitesi = model.WebSitesi;
            kurumTemelBilgilerModel.KurulusTarihi = Convert.ToDateTime(model.KurulusTarihi).Year == 1 ? null : model.KurulusTarihi;
            kurumTemelBilgilerModel.KurumLogoId = model.KurumLogo;
            kurumTemelBilgilerModel.AktifMi = 1;
            kurumTemelBilgilerModel.KayitTarihi = DateTime.Now;
            kurumTemelBilgilerModel.GuncellenmeTarihi = DateTime.Now;
            kurumTemelBilgilerModel.KurumVergiNo = model.VergiNo;
            kurumTemelBilgilerModel.KurumUlkeId = model.KurumUlkeId;
            kurumTemelBilgilerModel.KurumSehirId = model.KurumSehirId;

            var update = this.Update(kurumTemelBilgilerModel);

            //Mevcut bilgileri pasife alıp modelden gelen yeni bilgileri ekler
            var mevcutAdresList = _kurumAdresBilgileriService.KurumIdileGetir(model.TabloID).Value;
            foreach (var adres in mevcutAdresList)
            {
                _kurumAdresBilgileriService.Delete(adres.TabloID);
            }

            foreach (var adres in model.AdresListesi)
            {
                var KisiAdresModel = new KurumAdresBilgileri()
                {
                    KurumID = _loginUser.KurumID,
                    IlgiliKurumId = kurumTemelBilgilerModel.TabloID,
                    LokasyonTipi = adres.LokasyonTipi,
                    Adres = adres.Adres,
                    Ulke = adres.Ulke,
                    Sehir = adres.Sehir,
                    AktifMi = 1,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    DilID = 1,
                    SilindiMi = 0,
                    KisiID = _loginUser.KisiID
                };
                _kurumAdresBilgileriService.Add(KisiAdresModel);
            }

            var mevcutBankaList = _kurumBankaBilgileriService.KurumIdileGetir(model.TabloID).Value;
            foreach (var banka in mevcutBankaList)
            {
                _kurumBankaBilgileriService.Delete(banka.TabloID);
            }

            if (model.BankaListesi != null)
            {
                foreach (var banka in model.BankaListesi)
                {
                    var kurumBankaModel = new KurumBankaBilgileri()
                    {
                        KurumID = _loginUser.KurumID,
                        IlgiliKurumId = kurumTemelBilgilerModel.TabloID,
                        BankaId = banka.BankaId,
                        SubeId = banka.SubeId,
                        HesapNo = banka.HesapNo,
                        Iban = banka.Iban,
                        AktifMi = 1,
                        KayitTarihi = DateTime.Now,
                        GuncellenmeTarihi = DateTime.Now,
                        DilID = 1,
                        SilindiMi = 0,
                        KisiID = _loginUser.KisiID
                    };
                    _kurumBankaBilgileriService.Add(kurumBankaModel);
                }
            }

            if (model.KurumTips != null)
            {
                var kurums = _kurumIliskiService.KurumIliskiGetir(update.Value.TabloID).Value;

                foreach (var ks in kurums)
                {
                    _kurumIliskiService.Delete(ks.TabloID);
                }
                foreach (var kt in model.KurumTips)
                {
                    var kurumIliskiModel = new KurumIliskiKayitModel()
                    {
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        BuKurumID = update.Value.TabloID,
                        BununKurumID = _loginUser.KurumID,
                        IliskiTuruID = kt.TabloID
                    };
                    _kurumIliskiService.KurumIliskiKaydet(kurumIliskiModel);
                }
            }

            //Müşteri temsilcisi ise müşteri temsilcisi ilişkisi atar
            var musteris = _kisiIliskiService.MusteriList(update.Value.TabloID).Value;
            if (musteris.Count > 0)
            {
                foreach (var ms in musteris)
                {
                    _kurumIliskiService.Delete(ms.TabloID);
                }
            }

            if (model.MusteriTemsilciIds != null)
            {
                foreach (var Ids in model.MusteriTemsilciIds)
                {
                    var rs = new KisiIliskiKayitModel()
                    {
                        BuKisiID = Ids.TabloID,
                        BununKisiID = update.Value.TabloID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KayıtEdenID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        IliskiTuruID = (int)IliskiTipi.MusteriTemsilcisi//11
                    };
                    _kisiIliskiService.KisiIliskiKaydet(rs);
                }
            }
            DataContextConfiguration().Commit();
            //O kurumdaki hedef kitlelerdeki tanımlı kişileri-kurumları günceller

            return model.ToResult();
        }

        /// <summary>
        /// Temel Kurum silindi yapma işlemini gerçekleştiren method.
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>

        public Result<bool> TemelKurumSilindiYap(int kurumID)
        {
            var kurumBilgiler = this.SingleOrDefault(kurumID).Value;
            if (kurumBilgiler == null)
            {
                return Results.Fail("işlem başarısız.", ResultStatusCode.DeleteError);
            }
            if (kurumBilgiler.TabloID == _loginUser.KurumID)
            {
                return Results.Fail("Bağlı olduğunuz kurumu silemezsiniz!", ResultStatusCode.DeleteError);
            }
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            kurumBilgiler.AktifMi = 0;
            kurumBilgiler.SilindiMi = 1;
            kurumBilgiler.GuncellenmeTarihi = DateTime.Now;
            kurumBilgiler.SilinmeTarihi = DateTime.Now;
            this.Update(kurumBilgiler);

            //Kurumun ek bilgilerini silindi olaraka atar
            var adr = _kurumAdresBilgileriService.KurumBilgileriSilindiYap(kurumID);
            if (!adr.Value)
            {
                throw new ArgumentException("Parameter cannot be null");
            }

            var bnk = _kurumBankaBilgileriService.KurumBilgileriSilindiYap(kurumID);
            if (!bnk.Value)
            {
                throw new ArgumentException("Parameter cannot be null");
            }

            var tb = _kurumIliskiService.KurumIliskiGetir(kurumID).Value;
            foreach (var t in tb)
            {
                _kurumIliskiService.Delete(t.TabloID);
            }
            //Müşteri temilcisi ilişkileri siler
            var musteris = _kisiIliskiService.MusteriList(kurumID).Value;
            foreach (var ms in musteris)
            {
                _kurumIliskiService.Delete(ms.TabloID);
            }

            DataContextConfiguration().Commit();
            //O kurumdaki hedef kitlelerdeki tanımlı kişileri-kurumları günceller
            return true.ToResult();
        }

        /// <summary>
        /// Kuruma ait Müşteri temsilcilerini getiren metod
        /// </summary>
        /// <returns></returns>
        public Result<List<GenelViewModel>> KurumMusteriTemsilcisiGetir()
        {
            var kurumlarKisilerService = _serviceProvider.GetService<IKurumlarKisilerService>();
            var kurumOrganizasyonBirimTanimlariService = _serviceProvider.GetService<IKurumOrganizasyonBirimTanimlariService>();
            var kisiService = _serviceProvider.GetService<IKisiService>();

            var kurumorganizasyon = kurumOrganizasyonBirimTanimlariService.List(a =>
                    a.BirimTanim == "Müşteri Temsilcisi" && a.KurumID == _loginUser.KurumID && a.AktifMi == 1).Value
                .FirstOrDefault().TabloID;
            var kurumkisi = new List<GenelViewModel>();
            var kurumkisilist = kurumlarKisilerService.List(a =>
            a.IlgiliKurumId == _loginUser.KurumID &&
            a.AktifMi == 1 &&
            a.KurumOrganizasyonBirimTanimId == kurumorganizasyon).Value;
            foreach (var kisi in kurumkisilist)
            {
                var temp = kisiService.SingleOrDefault(kisi.IlgiliKisiId).Value;
                if (temp != null)
                {
                    kurumkisi.Add(new GenelViewModel
                    {
                        TabloID = kisi.IlgiliKisiId,
                        Deger = temp.KisiAdi + " " + temp.KisiSoyadi
                    });
                }
            }

            return kurumkisi.ToResult();
        }

        /// <summary>
        /// Musteri temsilcisi Idye göre bağlı kurumlarını getiren method
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> MusteriTemsilcisiBagliKurumlarList(int musteriTemsilcisiId)
        {
            var _kisiService = _serviceProvider.GetService<IKisiService>();
            var _iliskiler = _serviceProvider.GetService<IKurumIliskiService>();
            var kurumIdList = _iliskiler.MusteriTemsilcisiBagliKurumIdGetir(musteriTemsilcisiId);
            var kurumlar = _repository.List(a => kurumIdList.Value.Contains(a.TabloID)).ToList();

            var kisi = _kisiService.SingleOrDefault(musteriTemsilcisiId).Value;
            var kurum = _repository.SingleOrDefault(kisi.KurumID);
            kurumlar.Add(kurum);

            return kurumlar.Distinct().ToList().ToResult();
        }

        /// <summary>
        /// Amirlere Ast Musteri Temsilcisi Kurumlarini Getiren method
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> AmirlereAstMusteriTemsilcisiKurumlariniGetir(int amirId)
        {
            var _kisiService = _serviceProvider.GetService<IKisiService>();
            var _kurumlarKisiler = _serviceProvider.GetService<IKurumlarKisilerService>();
            Result<List<KisiOrganizasyonBirimView>> astlarList = _kisiService.KisiAstlarListGetir(amirId);
            List<KurumTemelBilgiler> result = new();
            var kisi = _kisiService.SingleOrDefault(amirId).Value;
            var kurum = _repository.SingleOrDefault(kisi.KurumID);
            if (_kurumlarKisiler.KisiMusteriTemsilcisiMi(amirId).Value)
            {
                var temp = MusteriTemsilcisiBagliKurumlarList(amirId);
                result.AddRange(temp.Value);
            }
            result.Add(kurum);
            //Kişinin ast müşteri temsilcilerine bağlı kurumları getiren döngü
            foreach (var ast in astlarList.Value)
            {
                var kontrol = _kurumlarKisiler.KisiMusteriTemsilcisiMi(ast.KisiId);
                if (kontrol.Value)
                {
                    var temp1 = MusteriTemsilcisiBagliKurumlarList(ast.KisiId);
                    if (ast.KisiAstlar != null && ast.KisiAstlar.Count > 0)
                    {
                        var temp2 = AmirlereAstMusteriTemsilcisiKurumlariniGetir(ast.KisiId);
                        if (temp2.Value != null)
                        {
                            result.AddRange(temp2.Value);
                        }
                    }
                    if (temp1.Value[0] != null)
                    {
                        result.AddRange(temp1.Value);
                    }
                }
            }
            return result.Distinct().ToList().ToResult();
        }

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kurumlara erişmesi gereken kullanıcılar için kullanılacak kurum listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KurumTemelBilgiler"></see></returns>
        public Result<List<KurumTemelBilgiler>> HiyerarsiDisiKisilerKurumListesi()
        {
            var kurumId = _loginUser.KurumID;
            var result = List(a => a.AktifMi == 1 && a.KurumID == kurumId);
            return result;
        }

        /// <summary>
        /// Sorguların listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public IQueryable<KurumTemelBilgiler> ListForQuery()
        {
            return _repository.List();
        }


        /// <summary>
        /// İçerik Tiplerinin listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamIcerikTipleri>> IcerikTipleriGetir()
        {
            var result = _paramIcerikTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return result;
        }

        /// <summary>
        ///İçerik Kategorilerin listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamIcerikKategoriler>> IcerikKategorileriGetir()
        {
            var result = _paramIcerikKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return result;
        }


        /// <summary>
        ///İçeriklerin Kategorilerine bağlı altkategori listesi oluşturan metod..
        /// </summary>
        /// <returns></returns>
        public Result<IcerikKutuphanesiMedyalarVM> IcerikKaydet(IcerikKutuphanesiMedyalarVM model)
        {
            try
            {
                var Icerik = new IcerikKutuphanesi
                {
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    ParamIcerikTipiID = model.ParamIcerikTipiID,
                    ParamIcerikKategoriID = model.ParamIcerikKategoriID,
                    ParamAltKategoriID = model.ParamAltKategoriID,
                    ParamAnaKategoriID = model.ParamAnaKategoriID,
                    TaslakMi = model.TaslakMi,
                    IcerikYayinlanmaZamani = model.IcerikYayinlanmaZamani,
                    IcerikKaldirilmaZamani = model.IcerikKaldirilmaZamani,
                    IcerikBaslik = model.IcerikBaslik,
                    IcerikTamMetin = model.IcerikTamMetin,
                };
                var IcerikKayit = _icerikKutuphanesiService.Add(Icerik);
                foreach (var item in model.IcerikSpot)
                {
                    var IcerikSpotBilgi = new IcerikSpotMetinleri
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        SpotMetinleri = item.SpotMetni,
                        IcerikKutuphanesiID = IcerikKayit.Value.TabloID
                    };
                    _icerikSpotMetinleriService.Add(IcerikSpotBilgi);

                }
                foreach (var item in model.Medyalar)
                {
                    var KayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value;
                    foreach (var medya in KayitliMedya)
                    {
                        medya.MedyaTipiId = item.ParamMedyaTipiID;
                        medya.MedyaAciklama = item.MedyaAciklama;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    var IcerikMedya = new IcerikKutuphanesiMedyalar
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        ParamMedyaTipiID = item.ParamMedyaTipiID,
                        MedyaAdi = item.MedyaAdi,
                        GosterimAdi = item.GosterimAdi,
                        MedyaAltMetin = item.AltMetin,
                        MedyaKutuphanesiID = item.MedyaID,
                        SiraNo = item.SiraNo,
                        IcerikKutuphanesiID = IcerikKayit.Value.TabloID
                    };
                    _icerikKutuphanesiMedyalarService.Add(IcerikMedya);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model.ToResult();
        }

        public Result<IcerikKutuphanesiMedyalarVM> IcerikGuncelle(IcerikKutuphanesiMedyalarVM model)
        {
            try
            {
                var icerik1 = _icerikKutuphanesiService.List(x => x.TabloID == model.TabloID);
                var icerik = icerik1.Value.FirstOrDefault();

                // İçerik güncelle
                icerik.GuncellenmeTarihi = DateTime.Now;
                icerik.GuncelleyenKisiID = _loginUser.KisiID;
                icerik.ParamIcerikTipiID = model.ParamIcerikTipiID;
                icerik.ParamIcerikKategoriID = model.ParamIcerikKategoriID;
                icerik.ParamAltKategoriID = model.ParamAltKategoriID;
                icerik.ParamAnaKategoriID = model.ParamAnaKategoriID;
                icerik.TaslakMi = model.TaslakMi;
                icerik.IcerikYayinlanmaZamani = model.IcerikYayinlanmaZamani;
                icerik.IcerikKaldirilmaZamani = model.IcerikKaldirilmaZamani;
                icerik.IcerikBaslik = model.IcerikBaslik;
                icerik.IcerikTamMetin = model.IcerikTamMetin;
                _icerikKutuphanesiService.Update(icerik);


                // Mevcut spotlar
                var mevcutSpotlar = _icerikSpotMetinleriService.List(x => x.IcerikKutuphanesiID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
                if (model.IcerikSpot.Count == mevcutSpotlar.Count)
                {
                    // Spotları sil
                    foreach (var spot in mevcutSpotlar)
                    {
                        spot.SilindiMi = 1;
                        spot.AktifMi = 0;
                        spot.GuncellenmeTarihi = DateTime.Now;
                        spot.GuncelleyenKisiID = _loginUser.KisiID;
                        _icerikSpotMetinleriService.Update(spot);
                    }

                    foreach (var item in model.IcerikSpot)
                    {
                        var yeniSpot = new IcerikSpotMetinleri
                        {
                            SpotMetinleri = item.SpotMetni,
                            IcerikKutuphanesiID = model.TabloID,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitEdenID = _loginUser.KisiID,
                            GuncelleyenKisiID = _loginUser.KisiID,
                            KayitTarihi = DateTime.Now,
                            GuncellenmeTarihi = DateTime.Now,
                            KurumID = _loginUser.KurumID,
                            KisiID = _loginUser.KisiID
                        };
                        _icerikSpotMetinleriService.Add(yeniSpot);
                    }
                }
                else
                {
                    var gelenSpotlar = model.IcerikSpot.Select(x => x.SpotMetni).ToList();
                    var SilinecekSpotlar = mevcutSpotlar.Where(m => !gelenSpotlar.Contains(m.SpotMetinleri)).ToList();

                    foreach (var spot in SilinecekSpotlar)
                    {
                        spot.SilindiMi = 1;
                        spot.AktifMi = 0;
                        spot.GuncellenmeTarihi = DateTime.Now;
                        spot.GuncelleyenKisiID = _loginUser.KisiID;
                        _icerikSpotMetinleriService.Update(spot);
                    }

                    var mevcutSpotMetinleri = mevcutSpotlar.Select(x => x.SpotMetinleri).ToList();

                    foreach (var item in model.IcerikSpot)
                    {
                        if (!mevcutSpotMetinleri.Contains(item.SpotMetni))
                        {
                            var yeniSpot = new IcerikSpotMetinleri
                            {
                                SpotMetinleri = item.SpotMetni,
                                IcerikKutuphanesiID = model.TabloID,
                                AktifMi = 1,
                                SilindiMi = 0,
                                KayitEdenID = _loginUser.KisiID,
                                GuncelleyenKisiID = _loginUser.KisiID,
                                KayitTarihi = DateTime.Now,
                                GuncellenmeTarihi = DateTime.Now,
                                KurumID = _loginUser.KurumID,
                                KisiID = _loginUser.KisiID
                            };
                            _icerikSpotMetinleriService.Add(yeniSpot);
                        }
                    }

                }

                // Mevcut medyalar
                var mevcutMedyalar = _icerikKutuphanesiMedyalarService.List(x => x.IcerikKutuphanesiID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
                if (model.Medyalar.Count == mevcutMedyalar.Count)
                {
                    foreach (var medya in mevcutMedyalar)
                    {
                        medya.SilindiMi = 1;
                        medya.AktifMi = 0;
                        medya.GuncellenmeTarihi = DateTime.Now;
                        medya.GuncelleyenKisiID = _loginUser.KisiID;
                        _icerikKutuphanesiMedyalarService.Update(medya);
                    }

                    foreach (var item in model.Medyalar)
                    {
                        var KayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value;
                        foreach (var medya in KayitliMedya)
                        {
                            medya.MedyaTipiId = item.ParamMedyaTipiID;
                            medya.MedyaAciklama = item.MedyaAciklama;
                            _medyaKutuphanesiService.Update(medya);
                        }
                        var yeniMedya = new IcerikKutuphanesiMedyalar
                        {
                            MedyaAdi = item.MedyaAdi,
                            GosterimAdi = item.GosterimAdi,
                            MedyaAltMetin = item.AltMetin,
                            SiraNo = item.SiraNo,
                            MedyaKutuphanesiID = item.MedyaID,
                            IcerikKutuphanesiID = model.TabloID,
                            KayitTarihi = DateTime.Now,
                            GuncellenmeTarihi = DateTime.Now,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitEdenID = _loginUser.KisiID,
                            GuncelleyenKisiID = _loginUser.KisiID,
                            KurumID = _loginUser.KurumID,
                            KisiID = _loginUser.KisiID
                        };
                        _icerikKutuphanesiMedyalarService.Add(yeniMedya);
                    }
                }
                else
                {
                    var gelenMedyaIDs = model.Medyalar.Select(x => x.MedyaID).ToHashSet();
                    var mevcutMedyaIDs = mevcutMedyalar.Select(x => x.MedyaKutuphanesiID).ToHashSet();

                    // 1. Silinecek medyalar
                    var silinecekler = mevcutMedyalar
                        .Where(x => !gelenMedyaIDs.Contains(x.MedyaKutuphanesiID))
                        .ToList();

                    foreach (var medya in silinecekler)
                    {
                        medya.SilindiMi = 1;
                        medya.AktifMi = 0;
                        medya.GuncellenmeTarihi = DateTime.Now;
                        medya.GuncelleyenKisiID = _loginUser.KisiID;
                        _icerikKutuphanesiMedyalarService.Update(medya);
                    }

                    // 2. Güncelle + Ekle
                    foreach (var item in model.Medyalar)
                    {
                        var mevcut = mevcutMedyalar.FirstOrDefault(x => x.MedyaKutuphanesiID == item.MedyaID);

                        // Medya kutuphanesi güncellemesi
                        var kayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value.FirstOrDefault();
                        if (kayitliMedya != null)
                        {
                            kayitliMedya.MedyaTipiId = item.ParamMedyaTipiID;
                            kayitliMedya.MedyaAciklama = item.MedyaAciklama;
                            _medyaKutuphanesiService.Update(kayitliMedya);
                        }

                        // Mevcut yoksa yeni ekle
                        if (mevcut == null)
                        {
                            var yeniMedya = new IcerikKutuphanesiMedyalar
                            {
                                MedyaAdi = item.MedyaAdi,
                                GosterimAdi = item.GosterimAdi,
                                MedyaAltMetin = item.AltMetin,
                                SiraNo = item.SiraNo,
                                MedyaKutuphanesiID = item.MedyaID,
                                IcerikKutuphanesiID = model.TabloID,
                                KayitTarihi = DateTime.Now,
                                GuncellenmeTarihi = DateTime.Now,
                                AktifMi = 1,
                                SilindiMi = 0,
                                KayitEdenID = _loginUser.KisiID,
                                GuncelleyenKisiID = _loginUser.KisiID,
                                KurumID = _loginUser.KurumID,
                                KisiID = _loginUser.KisiID
                            };
                            _icerikKutuphanesiMedyalarService.Add(yeniMedya);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return model.ToResult();


        }
        /// <summary>
        /// İçerikleri Silen metod
        /// </summary>
        public Result<bool> IcerikSil(int TabloId)
        {
            var Icerikmain = _icerikKutuphanesiService.List(x => x.TabloID == TabloId).Value;
            var Icerik = Icerikmain.FirstOrDefault();

            if (Icerik != null)
            {
                Icerik.PasifEdenKisiID = _loginUser.KisiID;
                Icerik.PasiflikTarihi = DateTime.Now;
                Icerik.SilenKisiID = _loginUser.KisiID;
                Icerik.SilinmeTarihi = DateTime.Now;
                Icerik.AktifMi = 0;
                Icerik.SilindiMi = 1;

                _icerikKutuphanesiService.Update(Icerik);
                //Spotları Pasif et
                var IcerikSpot = _icerikSpotMetinleriService.List(s => s.IcerikKutuphanesiID == TabloId).Value;
                foreach (var spot in IcerikSpot)
                {
                    spot.PasifEdenKisiID = _loginUser.KisiID;
                    spot.PasiflikTarihi = DateTime.Now;
                    spot.SilenKisiID = _loginUser.KisiID;
                    spot.SilinmeTarihi = DateTime.Now;
                    spot.SilindiMi = 1;
                    spot.AktifMi = 0;

                    _icerikSpotMetinleriService.Update(spot);
                }
                //Medyayı Pasif et
                var IcerikMedya = _icerikKutuphanesiMedyalarService.List(x => x.IcerikKutuphanesiID == TabloId).Value;
                foreach (var medya in IcerikMedya) {

                    medya.PasifEdenKisiID = _loginUser.KisiID;
                    medya.PasiflikTarihi = DateTime.Now;
                    medya.SilenKisiID = _loginUser.KisiID;
                    medya.SilinmeTarihi = DateTime.Now;
                    medya.SilindiMi = 1;
                    medya.AktifMi = 0;
                    _icerikKutuphanesiMedyalarService.Update(medya);
                }
                return true.ToResult();
            }
            else
            {
                return null;
            }
        }
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikVeriGetir(int urlId)
        {
            var icerikKutuphanesiData = _icerikKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == urlId).Value.Select(x => new { x.TabloID, x.IcerikBaslik, x.IcerikTamMetin, x.TaslakMi, x.IcerikYayinlanmaZamani, x.IcerikKaldirilmaZamani, x.ParamAltKategoriID, x.ParamAnaKategoriID, x.ParamIcerikKategoriID, }).ToList();
            var icerikMedyalar = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var icerikMedyalarIDler = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.Select(x => x.MedyaKutuphanesiID);
            var spotlar = _icerikSpotMetinleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var kategoriler = _paramIcerikKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && icerikMedyalarIDler.Contains(x.TabloID)).Value;
            var medyatipleri = _paramMedyaTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var icerikKutuphanesiBilgileri = new List<IcerikKutuphanesiMedyalarVM>();

            var ilgiliMedyalar = icerikMedyalar.Where(m => m.IcerikKutuphanesiID == urlId).ToList();
            var medyalar = new List<MedyaVM>();
            foreach (var medyaBilgi in ilgiliMedyalar)
            {
                var medyatipi = medyatipleri.FirstOrDefault(m => m.TabloID == medyaBilgi.ParamMedyaTipiID);
                var medya = new MedyaVM()
                {
                    MedyaAdi = medyaBilgi.MedyaAdi,
                    GosterimAdi = medyaBilgi.GosterimAdi,
                    SiraNo = medyaBilgi.SiraNo,
                    AltMetin = medyaBilgi.MedyaAltMetin,
                    MedyaID = medyaBilgi.MedyaKutuphanesiID,
                    MedyaUrl = medyaKutuphanesi.FirstOrDefault(m => m.TabloID == medyaBilgi.MedyaKutuphanesiID)?.MedyaUrl,
                    ParamMedyaTipiID = medyaBilgi.ParamMedyaTipiID,
                    MedyaTipiAdi = medyatipi.ParamTanim,
                    MedyaAciklama = medyaKutuphanesi.FirstOrDefault(m => m.TabloID == medyaBilgi.MedyaKutuphanesiID)?.MedyaAciklama

                }; medyalar.Add(medya);
            }

            var IcerikModelListesi = icerikKutuphanesiData.Select(icerik => new IcerikKutuphanesiMedyalarVM
            {
                TabloID = icerik.TabloID,
                IcerikBaslik = icerik.IcerikBaslik,
                IcerikTamMetin = icerik.IcerikTamMetin,
                TaslakMi = icerik.TaslakMi,
                IcerikYayinlanmaZamani = icerik.IcerikYayinlanmaZamani,
                IcerikKaldirilmaZamani = icerik.IcerikKaldirilmaZamani,
                ParamAltKategoriID = icerik.ParamAltKategoriID,
                ParamAnaKategoriID = icerik.ParamAnaKategoriID,
                ParamIcerikKategoriID = icerik.ParamIcerikKategoriID,

                IcerikSpot = spotlar.Where(x => x.IcerikKutuphanesiID == icerik.TabloID)
                .Select(x => new SpotModel
                {
                    SpotMetni = x.SpotMetinleri
                }).ToList(),
                Medyalar = medyalar
            }).ToList();

            return IcerikModelListesi.ToResult();
        }

        public Result<MedyaKutuphanesi> MedyaKayit(MedyaKutuphanesi model)
        {
            //  medyaTabloID = model.TabloID;

            return model.ToResult();
        }
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikListesi()
        {

            var icerikKutuphanesiData = _icerikKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.Select(x => new { x.TabloID, x.IcerikBaslik, x.IcerikTamMetin, x.TaslakMi, x.IcerikYayinlanmaZamani, x.IcerikKaldirilmaZamani, x.ParamAltKategoriID, x.ParamAnaKategoriID, x.ParamIcerikKategoriID, }).ToList();
            var icerikMedyalar = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var icerikMedyalarIDler = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.Select(x => x.MedyaKutuphanesiID);
            var spotlar = _icerikSpotMetinleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var kategoriler = _paramIcerikKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && icerikMedyalarIDler.Contains(x.TabloID)).Value;
            var medyatipleri = _paramMedyaTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var icerikKutuphanesiBilgileri = new List<IcerikKutuphanesiMedyalarVM>();

            foreach (var item in icerikKutuphanesiData)
            {
                var ilgiliMedyalar = icerikMedyalar.Where(m => m.IcerikKutuphanesiID == item.TabloID).ToList();
                if (!ilgiliMedyalar.Any())
                    continue;
                var medyalar = new List<MedyaVM>();
                foreach (var medyaBilgi in ilgiliMedyalar)
                {
                    var medyatipi = medyatipleri.FirstOrDefault().ParamTanim;
                    var medya = new MedyaVM()
                    {
                        MedyaAdi = medyaBilgi.MedyaAdi,
                        GosterimAdi = medyaBilgi.GosterimAdi,
                        SiraNo = medyaBilgi.SiraNo,
                        AltMetin = medyaBilgi.MedyaAltMetin,
                        MedyaID = medyaBilgi.MedyaKutuphanesiID,
                        MedyaUrl = medyaKutuphanesi.FirstOrDefault(m => m.TabloID == medyaBilgi.MedyaKutuphanesiID)?.MedyaUrl,
                        ParamMedyaTipiID = medyaBilgi.ParamMedyaTipiID,
                        MedyaTipiAdi = medyatipi

                    }; medyalar.Add(medya);
                }
                var spotmetinler = new List<SpotModel>();
                var spotBilgi = spotlar.Where(x => x.IcerikKutuphanesiID == item.TabloID).ToList();
                foreach (var spotMetinler in spotBilgi)
                {
                    var spot = new SpotModel()
                    {
                        SpotMetni = spotMetinler.SpotMetinleri,

                    }; spotmetinler.Add(spot);
                }
                var altkategori = kategoriler.FirstOrDefault(x => x.TabloID == item.ParamAltKategoriID).ParamTanim;
                var anakategori = kategoriler.FirstOrDefault(x => x.TabloID == item.ParamAnaKategoriID).ParamTanim;
                var ParamIcerikKategori = kategoriler.FirstOrDefault(x => x.TabloID == item.ParamIcerikKategoriID).ParamTanim;
                var icerikKutuphanesiBilgiler = new IcerikKutuphanesiMedyalarVM()
                {
                    TabloID = item.TabloID,//sonradan eklendi
                    IcerikBaslik = item.IcerikBaslik,
                    IcerikTamMetin = item.IcerikTamMetin,
                    TaslakMi = item.TaslakMi,
                    IcerikYayinlanmaZamani = item.IcerikYayinlanmaZamani,
                    IcerikKaldirilmaZamani = item.IcerikKaldirilmaZamani,
                    ParamAltKategoriID = item.ParamAltKategoriID,
                    ParamAnaKategoriID = item.ParamAnaKategoriID,
                    ParamIcerikKategoriID = item.ParamIcerikKategoriID,
                    ParamAltKategoriAdi = altkategori,
                    ParamAnaKategoriAdi = anakategori,
                    ParamIcerikKategoriAdi = ParamIcerikKategori,
                    Medyalar = medyalar,
                    IcerikSpot = spotmetinler
                };
                icerikKutuphanesiBilgileri.Add(icerikKutuphanesiBilgiler);
            }
            return icerikKutuphanesiBilgileri.ToResult();
        }

        public Result<IcerikKutuphanesiMedyalarVM> IcerikVerileriniGetir(int icerikID)
        {
            var medyaList = new List<MedyaVM>();
            //var akordionIcerikList = new List<AkordionIcerik>();
            var icerikKutuphanesiMedyalarVM = new IcerikKutuphanesiMedyalarVM();
            var grupBasligi = "";
            var altKategoriBilgisi = "";
            var hangiIcerikCevirisininBasligi = "";
            //İçerik Kütüphanesinden VMyi doldurmak için gerekli verileri alıyorum
            var icerikKutuphanesiVerileri = _icerikKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == icerikID).Value.FirstOrDefault();

            var icerikEssizID = icerikKutuphanesiVerileri.IcerikGrubuEssizID;
            var icerikTipID = icerikKutuphanesiVerileri.ParamIcerikTipiID;
            var icerikKategoriID = icerikKutuphanesiVerileri.ParamIcerikKategoriID;
            var icerikAltKategoriID = icerikKutuphanesiVerileri.ParamAltKategoriID;
            var ilgiliDilID = icerikKutuphanesiVerileri.ParamDilID;
            var icerikBaslik = icerikKutuphanesiVerileri.IcerikBaslik;
            var icerikSpot = icerikKutuphanesiVerileri.IcerikSpot;
            var icerikAnaMetin = icerikKutuphanesiVerileri.IcerikTamMetin;
            var taslakMi = icerikKutuphanesiVerileri.TaslakMi;
            var grubunParcasiMi = icerikKutuphanesiVerileri.IcerikGrubununParcasiMi;
            var yayinlanmaZamani = icerikKutuphanesiVerileri.IcerikYayinlanmaZamani;
            var paramdisPlatformID = icerikKutuphanesiVerileri.ParamDisPlatformID;

            //
            var icerikKutuphanesiMedyalar = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.IcerikKutuphanesiID == icerikID);

            //Bağlantili içeriği bularak çeviri olan metinlerin hangi türkçe metne ait olduğnu buluyorum
            var farkliDildeGirilenIcerikMi = icerikKutuphanesiVerileri.IcerikFarkliDildeGirilenIcerikMi;
            var baglantiliAnaIcerikID = icerikKutuphanesiVerileri.BaglantiliAnaIcerikID;

            if (baglantiliAnaIcerikID != null)
            {
                hangiIcerikCevirisininBasligi = _icerikKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == baglantiliAnaIcerikID).Value.FirstOrDefault().IcerikBaslik;

            }
            //
            if (icerikEssizID != null)
            {
                var icerikKutuphanesiEssizBilgiller = _icerikGrubuEssizBilgilerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == icerikEssizID).Value.FirstOrDefault();
                grupBasligi = icerikKutuphanesiEssizBilgiller.IcerikGrupBasligi;
            }

            //
            var kategoriBilgisi = _paramIcerikKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == icerikKategoriID).Value.FirstOrDefault().ParamTanim;
            if (icerikAltKategoriID != null)
            {
                altKategoriBilgisi = _paramIcerikKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.UstId == icerikAltKategoriID).Value.FirstOrDefault().ParamTanim;
            }
            var tipBilgisi = _paramIcerikTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == icerikTipID).Value.FirstOrDefault().ParamTanim;
            var medyaBilgisi = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.IcerikKutuphanesiID == icerikID).Value;
            var dilBilgisi = _paramDillerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == ilgiliDilID).Value.FirstOrDefault().ParamTanim;
            //Medya Bilgileri
            foreach (var item in medyaBilgisi)
            {
                var medyaKutuphanesiID = item.MedyaKutuphanesiID;
                var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == medyaKutuphanesiID).Value.FirstOrDefault();
                //var baseMedya = DownloadMedyaFile(medyaKutuphanesi.TabloID);
                //var medyaUrl = ConvertFileContentToDataUri(baseMedya.Result);
                var medyaUrl = medyaKutuphanesi.MedyaUrl;
                var medyaID = medyaKutuphanesi.TabloID;
                //MedyaAdi burda medya açıklaması olarak yollandı
                //var medyaIcerik = new MedyaVM()
                //{
                //    MedyaUrl = medyaUrl,
                //    MedyaAdi = item.MedyaAciklama,
                //    AcordionIndex = medyaID,
                //};
                //medyaList.Add(medyaIcerik);
            }



            //var akordionIcerik = new AkordionIcerik()
            //{
            //    IcerikBaslik = icerikBaslik,
            //    IcerikSpot = icerikSpot,
            //    IcerikTamMetin = icerikAnaMetin,
            //    Medyalar = medyaList,
            //};
            //akordionIcerikList.Add(akordionIcerik);


            //icerikKutuphanesiMedyalarVM = new IcerikKutuphanesiMedyalarVM()
            //{
            //    TabloID = icerikID,
            //    IlgiliDilID = ilgiliDilID,
            //    ParamIcerikTipiID = icerikTipID,
            //    ParamIcerikKategoriID = icerikKategoriID,
            //    ParamAltKategoriID = icerikAltKategoriID,
            //    IcerikGrupBasligi = icerikEssizID.ToString(), //Grup başlığı idsi 
            //    IcerikYayinlanmaZamani = yayinlanmaZamani,
            //    AkordionIcerik = akordionIcerikList,
            //    BaglantiliAnaIcerikID = baglantiliAnaIcerikID,
            //    TaslakMi = taslakMi,
            //    IcerikGrubununParcasiMi = grubunParcasiMi,
            //    IcerikGrubuEssizID = icerikEssizID,
            //    ParamDisPlatformID = paramdisPlatformID,
            //};

            return icerikKutuphanesiMedyalarVM.ToResult();

        }
        #region UrunGiris-KurumService

        public Result<UrunKutuphanesiMedyalarVM> UrunGuncelle(UrunKutuphanesiMedyalarVM model)
        {
            try
            {
                var now = DateTime.Now;
                var userId = _loginUser.KisiID;
                var kurumId = _loginUser.KurumID;

                // 1. Ürünü getir ve güncelle
                var urun = _urunKutuphanesiService.List(x => x.TabloID == model.TabloID).Value.FirstOrDefault();
                if (urun != null)
                {
                    urun.GuncellenmeTarihi = now;
                    urun.GuncelleyenKisiID = userId;
                    urun.ParamUrunTipiID = model.ParamUrunTipiID;
                    urun.ParamUrunKategoriID = model.ParamUrunKategoriID;
                    urun.ParamAltKategoriID = model.ParamAltKategoriID;
                    urun.ParamAnaKategoriID = model.ParamAnaKategoriID;
                    urun.TaslakMi = model.TaslakMi;
                    urun.UrunAdi = model.UrunAdi;
                    urun.UrunAcciklama = model.UrunAciklama;
                    _urunKutuphanesiService.Update(urun);
                }

                // 2. UrunParametreleri güncelle veya ekle
                var mevcutParametreler = _urunParametreDegerleriService.List(x => x.UrunKutuphanesiID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;

                model.UrunParametre
                    .Select(item =>
                    {
                        var mevcut = mevcutParametreler.FirstOrDefault();
                        if (mevcut != null)
                        {
                            mevcut.ParamUrunMarkaID = item.UrunMarkaID;
                            mevcut.ParamUrunModelID = item.UrunModelID;
                            mevcut.ParamUrunSeriID = item.UrunSeriID;
                            mevcut.UrunHiz = item.UrunHiz;
                            mevcut.UrunHacim = item.UrunHacim;
                            mevcut.UrunOlcum = item.UrunOlcum;
                            mevcut.UrunHizBirimID = item.UrunHizBirimID;
                            mevcut.UrunHacimBirimID = item.UrunHacimBirimID;
                            mevcut.UrunOlcumBirimID = item.UrunOlcumBirimID;
                            mevcut.GuncelleyenKisiID = userId;
                            mevcut.GuncellenmeTarihi = now;
                            mevcut.KurumID = kurumId;
                            mevcut.KisiID = userId;
                            _urunParametreDegerleriService.Update(mevcut);
                            return null;
                        }
                        return new UrunParametreDegerleri
                        {
                            AktifMi = 1,
                            SilindiMi = 0,
                            GuncellenmeTarihi = now,
                            KayitTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId,
                            UrunKutuphanesiID = model.TabloID,
                            ParamUrunMarkaID = item.UrunMarkaID,
                            ParamUrunModelID = item.UrunModelID,
                            ParamUrunSeriID = item.UrunSeriID,
                            UrunHiz = item.UrunHiz,
                            UrunHacim = item.UrunHacim,
                            UrunOlcum = item.UrunOlcum,
                            UrunHizBirimID = item.UrunHizBirimID,
                            UrunHacimBirimID = item.UrunHacimBirimID,
                            UrunOlcumBirimID = item.UrunOlcumBirimID
                        };
                    })
                    .Where(x => x != null)
                    .ToList()
                    .ForEach(x => _urunParametreDegerleriService.Add(x));

                // 3. Icerik blokları güncelle veya ekle
                var mevcutBloklar = _urunIcerikBloklariService.List(x => x.UrunID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
                model.IcerikBloklar
                    .Select(item =>
                    {
                        var mevcut = mevcutBloklar.FirstOrDefault();
                        if (mevcut != null)
                        {
                            mevcut.IcerikKutuphanesiID = item.IcerikKutuphanesiID.ToString();
                            mevcut.ParamIcerikBlokKategoriID = item.ParamBlokKategoriID;
                            mevcut.ParamIcerikBlokAltKategoriID = item.ParamBlokAltKategoriID;
                            mevcut.IcerikBlokTanim = item.IcerikBlokTanim;
                            mevcut.GuncelleyenKisiID = userId;
                            mevcut.GuncellenmeTarihi = now;
                            mevcut.KurumID = kurumId;
                            mevcut.KisiID = userId;
                            _urunIcerikBloklariService.Update(mevcut);
                            return null;
                        }
                        return new UrunIcerikBloklari
                        {
                            AktifMi = 1,
                            AktiflikTarihi = now,
                            AktifEdenKisiID = userId,
                            SilindiMi = 0,
                            GuncellenmeTarihi = now,
                            KayitTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId,
                            UrunID = model.TabloID,
                            ParamIcerikBlokAltKategoriID = item.ParamBlokAltKategoriID,
                            ParamIcerikBlokKategoriID = item.ParamBlokKategoriID,
                            IcerikBlokTanim = item.IcerikBlokTanim,
                            IcerikKutuphanesiID = item.IcerikKutuphanesiID.ToString()
                        };
                    })
                    .Where(x => x != null)
                    .ToList()
                    .ForEach(x => _urunIcerikBloklariService.Add(x));

                // 4. UrunFiyatVM güncelle/ekle
                var mevcutFiyatlar = _urunParametrelerService.List(x => x.UrunID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
                model.UrunFiyatVM
                    .Select(item =>
                    {
                        var mevcut = mevcutFiyatlar.FirstOrDefault(x => x.ParamParaBirimID == item.ParaBirimiID);
                        if (mevcut != null)
                        {
                            mevcut.ListeFiyati = item.UrunFiyat;
                            mevcut.GuncelleyenKisiID = userId;
                            mevcut.GuncellenmeTarihi = now;
                            mevcut.KurumID = kurumId;
                            mevcut.KisiID = userId;
                            _urunParametrelerService.Update(mevcut);
                            return null;
                        }
                        return new UrunParametreler
                        {
                            UrunID = model.TabloID,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KayitTarihi = now,
                            GuncellenmeTarihi = now,
                            KurumID = kurumId,
                            KisiID = userId,
                            ListeFiyati = item.UrunFiyat,
                            ParamParaBirimID = item.ParaBirimiID
                        };
                    })
                    .Where(x => x != null)
                    .ToList()
                    .ForEach(x => _urunParametrelerService.Add(x));

                // 5. Medyalar güncelle/ekle/sil
                var mevcutUrunMedyalar = _urunKutuphanesiMedyalarService
                    .List(x => x.UrunKutuphanesiID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0)
                    .Value;

                model.Medyalar.ForEach(item =>
                {
                    // 1. MedyaKutuphanesi'ni güncelle/ekle
                    var medya = _medyaKutuphanesiService
                        .List(x => x.TabloID == item.MedyaID && x.AktifMi == 1 && x.SilindiMi == 0)
                        .Value.FirstOrDefault();

                    if (medya != null)
                    {
                        medya.MedyaTipiId = item.ParamMedyaTipiID;
                        medya.MedyaAciklama = item.MedyaAciklama;
                        medya.GuncelleyenKisiID = userId;
                        medya.GuncellenmeTarihi = now;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    else
                    {
                        medya = new MedyaKutuphanesi
                        {
                            MedyaAdi = item.MedyaAdi,
                            MedyaUrl = item.MedyaUrl,
                            MedyaTipiId = item.ParamMedyaTipiID,
                            MedyaAciklama = item.MedyaAciklama,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitTarihi = now,
                            GuncellenmeTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId
                        };
                        _medyaKutuphanesiService.Add(medya);
                    }

                    // 2. UrunKutuphanesiMedyalar kaydını güncelle/ekle
                    var mevcut = mevcutUrunMedyalar.FirstOrDefault(m => m.MedyaKutuphanesiID == medya.TabloID);
                    if (mevcut != null)
                    {
                        mevcut.MedyaAdi = item.MedyaAdi;
                        mevcut.GosterimAdi = item.GosterimAdi;
                        mevcut.MedyaAltMetin = item.AltMetin;
                        mevcut.SiraNo = item.SiraNo;
                        mevcut.GuncelleyenKisiID = userId;
                        mevcut.GuncellenmeTarihi = now;
                        _urunKutuphanesiMedyalarService.Update(mevcut);
                    }
                    else
                    {
                        var yeniUrunMedya = new UrunKutuphanesiMedyalar
                        {
                            UrunKutuphanesiID = model.TabloID,
                            MedyaKutuphanesiID = medya.TabloID,
                            MedyaAdi = item.MedyaAdi,
                            GosterimAdi = item.GosterimAdi,
                            MedyaAltMetin = item.AltMetin,
                            SiraNo = item.SiraNo,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitTarihi = now,
                            GuncellenmeTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId
                        };
                        _urunKutuphanesiMedyalarService.Add(yeniUrunMedya);
                    }
                });

                // 3. Kullanılmayan UrunMedyalar'ı sil
                var silinecekler = mevcutUrunMedyalar
                    .Where(m => !model.Medyalar.Any(x => x.MedyaID == m.MedyaKutuphanesiID))
                    .ToList();

                silinecekler.ForEach(m =>
                {
                    m.AktifMi = 0;
                    m.SilindiMi = 1;
                    m.GuncelleyenKisiID = userId;
                    m.GuncellenmeTarihi = now;
                    _urunKutuphanesiMedyalarService.Update(m);
                });


            }
            catch (Exception ex)
            {
                return null;
            }

            return model.ToResult();
        }


        public Result<List<UrunKutuphanesiMedyalarVM>> UrunVeriGetir(int urlId)
        {
            var urunKutuphanesiData = _urunKutuphanesiService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == urlId)
                .Value
                .Select(x => new
                {
                    x.TabloID,
                    x.UrunAdi,
                    x.UrunAcciklama,
                    x.TaslakMi,
                    x.UrunYayinlanmaZamani,
                    x.UrunKaldirilmaZamani,
                    x.ParamAltKategoriID,
                    x.ParamAnaKategoriID,
                    x.ParamUrunKategoriID
                })
                .ToList();

            var urunMedyalar = _urunKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var medyatipleri = _paramMedyaTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var kategoriler = _paramUrunKategorilerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var paramDegerler = _urunParametreDegerleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var icerikBloklari = _urunIcerikBloklariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var urunIcerikMedya = _urunIcerikBloklariMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var parabirimleri = _urunParametrelerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var paramparabirimleri = _paramParaBirimleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;

            var urunKutuphanesiBilgileri = urunKutuphanesiData.Select(item =>
            {
                var ilgiliMedyalar = urunMedyalar
                    .Where(m => m.UrunKutuphanesiID == item.TabloID)
                    .Select(m =>
                    {
                        var medyaKutup = medyaKutuphanesi.FirstOrDefault(k => k.TabloID == m.MedyaKutuphanesiID);
                        var medyaTipi = medyatipleri.FirstOrDefault(t => t.TabloID == medyaKutup?.MedyaTipiId);
                        return new UrunMedyaVM
                        {
                            MedyaAdi = m.MedyaAdi,
                            GosterimAdi = m.GosterimAdi,
                            SiraNo = m.SiraNo,
                            AltMetin = m.MedyaAltMetin,
                            MedyaID = m.MedyaKutuphanesiID,
                            MedyaUrl = medyaKutup?.MedyaUrl,
                            ParamMedyaTipiID = medyaKutup?.MedyaTipiId ?? 0,
                            MedyaTipiAdi = medyaTipi?.ParamTanim,
                            MedyaAciklama = medyaKutup?.MedyaAciklama
                        };
                    })
                    .ToList();

                var IcerikMedya = urunIcerikMedya
                    .Where(m => m.UrunKutuphanesiID == item.TabloID)
                    .Select(Imedya =>
                    {
                        var medyaKutup = medyaKutuphanesi.FirstOrDefault(k => k.TabloID == Imedya.MedyaKutuphanesiID);
                        var medyaTipi = medyatipleri.FirstOrDefault(t => t.TabloID == medyaKutup?.MedyaTipiId);
                        return new IcerikMedyaVM
                        {
                            SiraNo = Imedya.SiraNo,
                            UstMetin = Imedya.MedyaUstMetin,
                            MedyaID = Imedya.MedyaKutuphanesiID,
                            MedyaUrl = medyaKutup?.MedyaUrl,
                            ParamMedyaTipiID = medyaKutup?.MedyaTipiId ?? 0,
                            MedyaTipiAdi = medyaTipi?.ParamTanim,
                            MedyaAciklama = medyaKutup?.MedyaAciklama,
                        };
                    })
                    .ToList();

                var UrunParam = paramDegerler
                    .Where(p => p.UrunKutuphanesiID == item.TabloID)
                    .Select(param => new UrunParametreVM
                    {
                        UrunHacim = param.UrunHacim,
                        UrunHiz = param.UrunHiz,
                        UrunOlcum = param.UrunOlcum,
                        UrunHizBirimID = param.UrunHizBirimID,
                        UrunHacimBirimID = param.UrunHacimBirimID,
                        UrunOlcumBirimID = param.UrunOlcumBirimID,
                        UrunMarkaID = param.ParamUrunMarkaID,
                        UrunModelID = param.ParamUrunModelID,
                        UrunSeriID = param.ParamUrunSeriID,
                        
                    })
                    .ToList();

                var UrunIcerikBlok = icerikBloklari
                    .Where(b => b.UrunID == item.TabloID)
                    .Select(param => new IcerikBlokVM
                    {
                        IcerikKutuphanesiID = param.IcerikKutuphanesiID._ToInt32(),
                        IcerikBlokTanim = param.IcerikBlokTanim,
                        ParamBlokAltKategoriID = param.ParamIcerikBlokAltKategoriID,
                        ParamBlokKategoriID = param.ParamIcerikBlokKategoriID
                    })
                    .ToList();

                var UrunFiyat = parabirimleri.LastOrDefault(x => x.UrunID == item.TabloID);
                var UrunParaBirimID = paramparabirimleri.FirstOrDefault(x => x.TabloID == UrunFiyat?.ParamParaBirimID)?.TabloID ?? 0;
                var UrunParaBirimi = paramparabirimleri.FirstOrDefault(x => x.TabloID == UrunFiyat?.ParamParaBirimID)?.ParamTanim ?? "Bilinmiyor";

                var urunFiyatVM = new List<ParamParaBilimleriVM>
        {
            new ParamParaBilimleriVM { ParaBirimiID = UrunParaBirimID, UrunFiyat = UrunFiyat?.ListeFiyati ?? 0 }
        };

                var altkategori = kategoriler.FirstOrDefault(k => k.TabloID == item.ParamAltKategoriID)?.ParamTanim;
                var anakategori = kategoriler.FirstOrDefault(k => k.TabloID == item.ParamAnaKategoriID)?.ParamTanim;
                var ParamUrunKategori = kategoriler.FirstOrDefault(k => k.TabloID == item.ParamUrunKategoriID)?.ParamTanim;

                return new UrunKutuphanesiMedyalarVM
                {
                    TabloID = item.TabloID,
                    UrunAdi = item.UrunAdi,
                    UrunAciklama = item.UrunAcciklama,
                    TaslakMi = item.TaslakMi,
                    UrunYayinlanmaZamani = item.UrunYayinlanmaZamani,
                    UrunKaldirilmaZamani = item.UrunKaldirilmaZamani,
                    ParamAltKategoriID = item.ParamAltKategoriID,
                    ParamAnaKategoriID = item.ParamAnaKategoriID,
                    ParamUrunKategoriID = item.ParamUrunKategoriID,
                    ParamAltKategoriAdi = altkategori,
                    ParamAnaKategoriAdi = anakategori,
                    ParamUrunKategoriAdi = ParamUrunKategori,
                    Medyalar = ilgiliMedyalar,
                    IcerikMedyalar = IcerikMedya,
                    UrunParametre = UrunParam,
                    IcerikBloklar = UrunIcerikBlok,
                    UrunFiyatVM = urunFiyatVM
                };
            }).ToList();

            return urunKutuphanesiBilgileri.ToResult();
        }

        /// <summary>
        ///Urun Verilierini listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamUrunKategoriler>> UrunKategorileriGetir()
        {
            var result = _paramUrunKategorilerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return result;
        }
        /// <summary>
        ///Urun Kategorilerin listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamUrunMarkalar>> ParamUrunMarkalarınıGetir()
        {
            var result = _paramUrunMarkalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return result;
        }

        /// <summary>
        ///Urun Kategorilerin listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamOlcumBirimleri>> UrunParametreDegerGetir()
        {
            var paramKodlar = _paramOlcumKategorileriService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0)
                .Value
                .Select(p => p.ParamKod)
                .ToList();

            var tumKategoriler = _paramOlcumBirimleriService
                .List(x => paramKodlar.Contains(x.ParamKod) && x.AktifMi == 1 && x.SilindiMi == 0)
                .Value
                .ToList();

            return tumKategoriler.ToResult();
        }

        public Result<List<IcerikKutuphanesi>> IcerikKutuphanesiGetir()
        {
            var param = _icerikKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return param;
        }
        public Result<List<ParamOlcumBirimleri>> UrunOlcumBirimiData()
        {
            var param = _paramOlcumBirimleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return param;
        }
        public Result<List<ParamIcerikBlokKategorileri>> IcerikBlokKategorilerGetir()
        {
            var param = _paramIcerikBlokKategorileriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return param;
        }

        /// <summary>
        /// Parabirimlerini kategorileri Listeleyen Metod
        /// </summary>
        public Result<List<ParamParaBirimleri>> UrunParaBirimiGetir()
        {
            var result = _paramParaBirimleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
            return result;
        }
        ///<summary>
        ///DateTime Kontrol Helper
        ///</summary>
        //private DateTime GuvenliSqlDateTimeKontrol(DateTime date)
        //{
        //    return date < new DateTime(1753, 1, 1) ? DateTime.Now : date;
        //}

        /// <summary>
        /// Urunleri Listeleyen Metod
        /// </summary>
        public Result<List<UrunKutuphanesiMedyalarVM>> UrunListesi()
        {
            var urunKutuphanesiData = _urunKutuphanesiService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0)
                .Value
                .ToList();

            if (!urunKutuphanesiData.Any())
                return new List<UrunKutuphanesiMedyalarVM>().ToResult();

            var urunMedyalar = _urunKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var urunMedyalarIDler = urunMedyalar.Select(x => x.MedyaKutuphanesiID).ToHashSet();
            var kategoriler = _paramUrunKategorilerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            var medyatipleri = _paramMedyaTipleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && urunMedyalarIDler.Contains(x.TabloID)).Value.ToList();
            var parabirimleri = _urunParametrelerService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var paramparabirimleri = _paramParaBirimleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var paramDegerler = _urunParametreDegerleriService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;

            var urunKutuphanesiBilgileri = urunKutuphanesiData
                .Where(u => urunMedyalar.Any(m => m.UrunKutuphanesiID == u.TabloID))
                .Select(u =>
                {
                    var ilgiliMedyalar = urunMedyalar.Where(m => m.UrunKutuphanesiID == u.TabloID).ToList();
                    var medyalar = ilgiliMedyalar.Select(m =>
                    {
                        var medyaKutup = medyaKutuphanesi.FirstOrDefault(k => k.TabloID == m.MedyaKutuphanesiID);
                        var medyaTipi = medyatipleri.FirstOrDefault(t => t.TabloID == medyaKutup?.MedyaTipiId);
                        return new UrunMedyaVM
                        {
                            MedyaAdi = m.MedyaAdi,
                            GosterimAdi = m.GosterimAdi,
                            SiraNo = m.SiraNo,
                            AltMetin = m.MedyaAltMetin,
                            MedyaID = m.MedyaKutuphanesiID,
                            MedyaUrl = medyaKutup?.MedyaUrl,
                            ParamMedyaTipiID = medyaKutup?.MedyaTipiId ?? 0,
                            MedyaTipiAdi = medyaTipi?.ParamTanim
                        };
                    }).ToList();

                    var UrunFiyat = parabirimleri.LastOrDefault(x => x.UrunID == u.TabloID);
                    var UrunParaBirimi = paramparabirimleri.LastOrDefault(x => x.TabloID == UrunFiyat?.ParamParaBirimID)?.ParamTanim ?? "Bilinmiyor";
                    var urunFiyatVM = new List<ParamParaBilimleriVM> {
                new() { ParaBirimTanim = UrunParaBirimi, UrunFiyat = UrunFiyat?.ListeFiyati ?? 0 }
                    };

                    var UrunParam = paramDegerler
                        .Where(p => p.UrunKutuphanesiID == u.TabloID)
                        .Select(p => new UrunParametreVM
                        {
                            UrunHacim = p.UrunHacim,
                            UrunHiz = p.UrunHiz,
                            UrunOlcum = p.UrunOlcum,
                            UrunHizBirimID = p.UrunHizBirimID,
                            UrunHacimBirimID = p.UrunHacimBirimID,
                            UrunOlcumBirimID = p.UrunOlcumBirimID,
                            UrunMarkaID = p.ParamUrunMarkaID,
                            UrunModelID = p.ParamUrunModelID,
                            UrunSeriID = p.ParamUrunSeriID
                        }).ToList();

                    return new UrunKutuphanesiMedyalarVM
                    {
                        TabloID = u.TabloID,
                        UrunAdi = u.UrunAdi,
                        UrunAciklama = u.UrunAcciklama,
                        TaslakMi = u.TaslakMi,
                        UrunYayinlanmaZamani = u.UrunYayinlanmaZamani,
                        UrunKaldirilmaZamani = u.UrunKaldirilmaZamani,
                        ParamAltKategoriID = u.ParamAltKategoriID,
                        ParamAnaKategoriID = u.ParamAnaKategoriID,
                        ParamUrunKategoriID = u.ParamUrunKategoriID,
                        ParamAltKategoriAdi = kategoriler.FirstOrDefault(k => k.TabloID == u.ParamAltKategoriID)?.ParamTanim,
                        ParamAnaKategoriAdi = kategoriler.FirstOrDefault(k => k.TabloID == u.ParamAnaKategoriID)?.ParamTanim,
                        ParamUrunKategoriAdi = kategoriler.FirstOrDefault(k => k.TabloID == u.ParamUrunKategoriID)?.ParamTanim,
                        Medyalar = medyalar,
                        UrunFiyatVM = urunFiyatVM,
                        UrunParametre = UrunParam
                    };
                }).ToList();

            return urunKutuphanesiBilgileri.ToResult();
        }

        /// <summary>
        /// Urunleri Kayıteden Metod
        /// </summary>
        public Result<UrunKutuphanesiMedyalarVM> UrunKaydet(UrunKutuphanesiMedyalarVM model) {
            try
            {

                var Urun = new UrunKutuphanesi
                {
                    AktifMi = 1,
                    AktiflikTarihi = DateTime.Now,
                    AktifEdenKisiID = _loginUser.KisiID,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    UrunGirisZamani = DateTime.Now < new DateTime(1753, 1, 1) ? DateTime.Now : DateTime.Now,
                    UrunYayinlanmaZamani = DateTime.Now < new DateTime(1753, 1, 1) ? DateTime.Now : DateTime.Now,
                    ParamUrunTipiID = model.ParamUrunTipiID,
                    TaslakMi = model.TaslakMi,
                    ParamAltKategoriID = model.ParamAltKategoriID,
                    ParamAnaKategoriID = model.ParamAnaKategoriID,
                    ParamUrunKategoriID = model.ParamUrunKategoriID,
                    UrunAdi = model.UrunAdi,
                    UrunAcciklama = model.UrunAciklama,
                };
                var UrunKayit = _urunKutuphanesiService.Add(Urun);
                foreach (var item in model.UrunFiyatVM) {
                    var UrunParam = new UrunParametreler
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        UrunID = UrunKayit.Value.TabloID,
                        ListeFiyati = item.UrunFiyat,
                        ParamParaBirimID = item.ParaBirimiID,
                    };
                    _urunParametrelerService.Add(UrunParam);
                }
                foreach (var item in model.UrunParametre)
                {
                    var paramnew = new UrunParametreDegerleri
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        UrunKutuphanesiID = UrunKayit.Value.TabloID,
                        ParamUrunMarkaID = item.UrunMarkaID,
                        ParamUrunModelID = item.UrunModelID,
                        ParamUrunSeriID = item.UrunSeriID,
                        UrunHiz = item.UrunHiz,
                        UrunHacim = item.UrunHacim,
                        UrunOlcum = item.UrunOlcum,
                        UrunHizBirimID = item.UrunHizBirimID,
                        UrunHacimBirimID = item.UrunHacimBirimID,
                        UrunOlcumBirimID = item.UrunOlcumBirimID
                    }; _urunParametreDegerleriService.Add(paramnew);
                }
                foreach (var item in model.IcerikBloklar)
                {
                    var IcerikBlok = new UrunIcerikBloklari
                    {
                        AktifMi = 1,
                        AktiflikTarihi = DateTime.Now,
                        AktifEdenKisiID = _loginUser.KisiID,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        UrunID = UrunKayit.Value.TabloID,
                        ParamIcerikBlokAltKategoriID = item.ParamBlokAltKategoriID,
                        ParamIcerikBlokKategoriID = item.ParamBlokKategoriID,
                        IcerikBlokTanim = item.IcerikBlokTanim,
                        IcerikKutuphanesiID = item.IcerikKutuphanesiID.ToString()
                    }; _urunIcerikBloklariService.Add(IcerikBlok);
                }

                //Medyalar
                foreach (var item in model.Medyalar)
                {
                    var KayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value;
                    foreach (var medya in KayitliMedya)
                    {
                        medya.MedyaTipiId = item.ParamMedyaTipiID;
                        medya.MedyaAciklama = item.MedyaAciklama;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    var UrunMedya = new UrunKutuphanesiMedyalar
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        MedyaAdi = item.MedyaAdi,
                        GosterimAdi = item.GosterimAdi,
                        MedyaAltMetin = item.AltMetin,
                        MedyaKutuphanesiID = item.MedyaID,
                        SiraNo = item.SiraNo,
                        UrunKutuphanesiID = UrunKayit.Value.TabloID
                    };
                    _urunKutuphanesiMedyalarService.Add(UrunMedya);
                }
                foreach (var item in model.IcerikMedyalar)
                {
                    var KayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value;
                    foreach (var medya in KayitliMedya)
                    {
                        medya.MedyaTipiId = item.ParamMedyaTipiID;
                        medya.MedyaAciklama = item.MedyaAciklama;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    var UrunMedya = new UrunIcerikBloklariMedyalar
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        MedyaKutuphanesiID = item.MedyaID,
                        SiraNo = item.SiraNo,
                        UrunKutuphanesiID = UrunKayit.Value.TabloID,
                        MedyaUstMetin = item.UstMetin,
                        MedyaAciklama = item.MedyaAciklama
                    };
                    _urunIcerikBloklariMedyalarService.Add(UrunMedya);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model.ToResult();

        }
        public Result<bool> UrunSil(int TabloId)
        {
            var urun = _urunKutuphanesiService.List(x => x.TabloID == TabloId).Value.FirstOrDefault();

            if (urun == null)
                return false.ToResult();

            var now = DateTime.Now;
            Action<dynamic> pasifEt = x =>
            {
                x.PasifEdenKisiID = _loginUser.KisiID;
                x.PasiflikTarihi = now;
                x.SilenKisiID = _loginUser.KisiID;
                x.SilinmeTarihi = now;
                x.AktifMi = 0;
                x.SilindiMi = 1;
            };

            // Ana ürün
            pasifEt(urun);
            _urunKutuphanesiService.Update(urun);

            // Fiyatlar
            _urunParametrelerService.List(x => x.UrunID == TabloId).Value
                .ToList()
                .ForEach(para => { pasifEt(para); _urunParametrelerService.Update(para); });

            // Medyalar
            _urunKutuphanesiMedyalarService.List(x => x.UrunKutuphanesiID == TabloId).Value
                .ToList()
                .ForEach(medya => { pasifEt(medya); _urunKutuphanesiMedyalarService.Update(medya); });

            return true.ToResult();
        }
        #endregion

        #region Kaynak Rezerve
        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveListele()
        {
            var KaynakDbo = _kaynakTanimlariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            var Rezerve = _kaynakRezerveTanimlariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
                        var kapasite = _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            //
            var MedyalarKaynak = _kaynakTanimlariMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var MedyaIdler = MedyalarKaynak.Select(x => x.ResimID).ToList();
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && MedyaIdler.Contains(x.TabloID)).Value;


            var KaynakRezerveBilgileri = KaynakDbo
                .Select(item => new KaynakTanimlariRezerveVM
                {
                    TabloID = item.TabloID,
                    KaynakTanimVM = new KaynakTanim
                    {
                        KaynakAdi = item.KaynakAdi,
                        KaynakAciklama = item.KaynakAciklama,
                        ParamKaynakTipiID = item.ParamKaynakTipiID,
                        KaynakIkonID = 0
                    },

                    KaynakRezerveTanimVM = Rezerve
                        .Where(r => r.KaynakTanimID == item.TabloID)
                        .Select(rez => new KaynakRezerveTanim
                        {
                            KaynakTanimID = rez.KaynakTanimID,
                            RezerveSaatBaslangicDegeri = rez.RezerveSaatBaslangicDegeri.ToString(),
                            RezerveSaatBitisDegeri = rez.RezerveSaatBitisDegeri.ToString(),
                            UygunGunTipleri = rez.UygunGunTipleri
                        })
                        .LastOrDefault(),
                    
                    KapsiteVM = kapasite
                        .Where(kapa => kapa.KaynakRezerveTanimID == item.TabloID)
                        .Select(kap => new Kapsite
                        {
                            KaynakRezerveTanimID = kap.KaynakRezerveTanimID,
                            Kapasaite = kap.Kapasaite,
                            KapasiteBirimID = kap.KapasiteBirimID
                        })
                        .ToList(),

                    MedyaVM = (from medyaBilgi in MedyalarKaynak
                               join medyaKutup in medyaKutuphanesi
                                   on medyaBilgi.ResimID equals medyaKutup.TabloID
                               select new Medya
                               {
                                   ResimID = medyaBilgi.ResimID,
                                   MedyaUrl = medyaKutup.MedyaUrl
                               }).ToList()
                })
                .ToList();

            return KaynakRezerveBilgileri.ToResult();
        }

        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveVeriGetir(int urlId)
        {
            var KaynakDbo = _kaynakTanimlariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == urlId).Value.ToList();
            var Rezerve = _kaynakRezerveTanimlariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            var kapasite = _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();
            //
            var MedyalarKaynak = _kaynakTanimlariMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TanimID == urlId).Value;
            var MedyaIdler = MedyalarKaynak.Select(x => x.ResimID).ToList();
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && MedyaIdler.Contains(x.TabloID)).Value;
            var IstisnaKaynak = _kaynakGunIciIstisnaTanimlariService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.KaynakTanimID == urlId).Value;

            var KaynakRezerveBilgileri = KaynakDbo
                .Select(item => new KaynakTanimlariRezerveVM
                {
                    TabloID = item.TabloID,
                    KaynakTanimVM = new KaynakTanim
                    {
                        KaynakAdi = item.KaynakAdi,
                        KaynakAciklama = item.KaynakAciklama,
                        ParamKaynakTipiID = item.ParamKaynakTipiID,
                        KaynakIkonID = 0
                    },
                    KaynakRezerveTanimVM = Rezerve
                        .Where(r => r.KaynakTanimID == item.TabloID)
                        .Select(rez => new KaynakRezerveTanim
                        {
                            KaynakTanimID = rez.KaynakTanimID,
                            RezerveSaatBaslangicDegeri = rez.RezerveSaatBaslangicDegeri.ToString(),
                            RezerveSaatBitisDegeri = rez.RezerveSaatBitisDegeri.ToString(),
                            UygunGunTipleri = rez.UygunGunTipleri
                        })
                        .LastOrDefault(),
                    KapsiteVM = kapasite
                        .Where(kapa => kapa.KaynakRezerveTanimID == item.TabloID)
                        .Select(kap => new Kapsite
                        {
                            KaynakRezerveTanimID = kap.KaynakRezerveTanimID,
                            Kapasaite = kap.Kapasaite,
                            KapasiteBirimID = kap.KapasiteBirimID
                        })
                        .ToList(),

                    MedyaVM = (from medyaBilgi in MedyalarKaynak
                               join medyaKutup in medyaKutuphanesi
                                   on medyaBilgi.ResimID equals medyaKutup.TabloID
                               select new Medya
                               {
                                   ResimID = medyaBilgi.ResimID,
                                   MedyaUrl = medyaKutup.MedyaUrl
                               }).ToList(),
                    KaynakIstisnalariVM = IstisnaKaynak
                        .Where(i => i.KaynakTanimID == item.TabloID)
                        .Select(i => new KaynakIstisnalari
                        {
                            IstisnaSaatBaslangicDegeri = i.IstisnaSaatBaslangicDegeri.ToString(),
                            IstisnaSaatBitisDegeri = i.IstisnaSaatBitisDegeri.ToString(),
                            IstisnaTarihBaslangicDegeri = i.IstisnaTarihBaslangicDegeri,
                            IstisnaTarihtBitisDegeri = i.IstisnaTarihtBitisDegeri
                        })
                        .ToList()
                })
                .ToList();

            return KaynakRezerveBilgileri.ToResult();
        }

        public Result<KaynakTanimlariRezerveVM> RezerveKaynakGuncelle(KaynakTanimlariRezerveVM model)
        {

            try
            {
                var kaynakTanimVM = model.KaynakTanimVM;
                var kaynakRezerveVM = model.KaynakRezerveTanimVM;

                // ========== 1. KAYNAK GÜNCELLEME ==========
                var kaynak = _kaynakTanimlariService.List(x => x.TabloID == model.TabloID).Value.FirstOrDefault();

                kaynak.KaynakAdi = kaynakTanimVM.KaynakAdi;
                kaynak.KaynakAciklama = kaynakTanimVM.KaynakAciklama;
                kaynak.ParamKaynakTipiID = kaynakTanimVM.ParamKaynakTipiID;
                kaynak.GuncellenmeTarihi = DateTime.Now;
                kaynak.GuncelleyenKisiID = _loginUser.KisiID;

                _kaynakTanimlariService.Update(kaynak);

                // ========== 2. REZERVE GÜNCELLEME ==========
                var rezerve = _kaynakRezerveTanimlariService.List(x => x.KaynakTanimID == model.TabloID).Value.FirstOrDefault();
                TimeSpan span;
                TimeSpan.TryParse(kaynakRezerveVM.RezerveSaatBitisDegeri, out span);
                TimeSpan spanEND;
                TimeSpan.TryParse(kaynakRezerveVM.RezerveSaatBaslangicDegeri, out spanEND);
                rezerve.RezerveSaatBaslangicDegeri = spanEND;
                rezerve.RezerveSaatBitisDegeri = span;
                rezerve.UygunGunTipleri = kaynakRezerveVM.UygunGunTipleri;
                rezerve.GuncellenmeTarihi = DateTime.Now;
                rezerve.GuncelleyenKisiID = _loginUser.KisiID;

                _kaynakRezerveTanimlariService.Update(rezerve);

                // ========== 3. MEDYA GÜNCELLEME ==========
                var mevcutMedyalar = _kaynakTanimlariMedyalarService.List(x =>
                    x.AktifMi == 1 && x.SilindiMi == 0 && x.TanimID == model.TabloID).Value;

                var gelenMedyaIDs = model.MedyaVM.Select(x => x.ResimID).ToHashSet();
                var mevcutMedyaIDs = mevcutMedyalar.Select(x => x.ResimID).ToHashSet();

                //foreach (var medya in silinecekler)
                //{
                //    medya.SilindiMi = 1;
                //    medya.AktifMi = 0;
                //    medya.GuncellenmeTarihi = DateTime.Now;
                //    medya.GuncelleyenKisiID = _loginUser.KisiID;
                //    _kaynakTanimlariMedyalarService.Update(medya);
                //}

                mevcutMedyalar
                    .Where(x => !gelenMedyaIDs.Contains(x.ResimID))
                    .ToList()
                    .ForEach(medya =>
                    {
                        medya.SilindiMi = 1;
                        medya.AktifMi = 0;
                        medya.GuncellenmeTarihi = DateTime.Now;
                        medya.GuncelleyenKisiID = _loginUser.KisiID;
                        _kaynakTanimlariMedyalarService.Update(medya);
                    });

                // 3.2 Yeni Eklenecekler
                model.MedyaVM
                    .Where(x => !mevcutMedyaIDs.Contains(x.ResimID))
                    .Select(gelen => new KaynakTanimlariMedyalar
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        TanimID = model.TabloID,
                        ResimID = gelen.ResimID
                    }).ToList().
                    ForEach(yeniMedya => _kaynakTanimlariMedyalarService.Add(yeniMedya));

                //foreach (var gelen in model.MedyaVM)
                //{
                //    if (!mevcutMedyaIDs.Contains(gelen.ResimID))
                //    {
                //        var yeniMedya = new KaynakTanimlariMedyalar
                //        {
                //            AktifMi = 1,
                //            SilindiMi = 0,
                //            GuncellenmeTarihi = DateTime.Now,
                //            KayitTarihi = DateTime.Now,
                //            KayitEdenID = _loginUser.KisiID,
                //            GuncelleyenKisiID = _loginUser.KisiID,
                //            KurumID = _loginUser.KurumID,
                //            KisiID = _loginUser.KisiID,
                //            TanimID = model.TabloID,
                //            ResimID = gelen.ResimID
                //        };
                //        _kaynakTanimlariMedyalarService.Add(yeniMedya);
                //    }
                //}

                //LINQ

                // ========== 4. İSTİSNA GÜNCELLEME ==========
                var mevcutIstisnalar = _kaynakGunIciIstisnaTanimlariService.List(x => x.KaynakTanimID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value.ToList();

                // Gelen ve mevcut istisnaları karşılaştırarak eklenecek ve silinecekleri belirle
                var eklenecekIstisnalar = model.KaynakIstisnalariVM
                    .Where(gelen => !mevcutIstisnalar.Any(mevcut =>
                        mevcut.IstisnaSaatBaslangicDegeri.ToString() == gelen.IstisnaSaatBaslangicDegeri &&
                        mevcut.IstisnaSaatBitisDegeri.ToString() == gelen.IstisnaSaatBitisDegeri &&
                        mevcut.IstisnaTarihBaslangicDegeri == FixSqlDate(gelen.IstisnaTarihBaslangicDegeri) &&
                        mevcut.IstisnaTarihtBitisDegeri == FixSqlDate(gelen.IstisnaTarihtBitisDegeri)))
                    .ToList();

                var silinecekIstisnalar = mevcutIstisnalar
                    .Where(mevcut =>
                        // Gelenler arasında tam eşleşen yoksa
                        !model.KaynakIstisnalariVM.Any(gelen =>
                            (string.IsNullOrEmpty(gelen.IstisnaSaatBaslangicDegeri) ? null : gelen.IstisnaSaatBaslangicDegeri) == mevcut.IstisnaSaatBaslangicDegeri?.ToString() &&
                            (string.IsNullOrEmpty(gelen.IstisnaSaatBitisDegeri) ? null : gelen.IstisnaSaatBitisDegeri) == mevcut.IstisnaSaatBitisDegeri?.ToString() &&
                            FixSqlDate(gelen.IstisnaTarihBaslangicDegeri) == mevcut.IstisnaTarihBaslangicDegeri &&
                            FixSqlDate(gelen.IstisnaTarihtBitisDegeri) == mevcut.IstisnaTarihtBitisDegeri
                        )
                        // Ek olarak: frontend'ten null geldiyse otomatik sil
                        || model.KaynakIstisnalariVM.Any(gelen =>
                            string.IsNullOrEmpty(gelen.IstisnaSaatBaslangicDegeri) &&
                            string.IsNullOrEmpty(gelen.IstisnaSaatBitisDegeri) &&
                            string.IsNullOrEmpty(gelen.IstisnaTarihBaslangicDegeri.ToString()) &&
                            string.IsNullOrEmpty(gelen.IstisnaTarihtBitisDegeri.ToString())
                        )
                    )
                    .ToList();

                // Yeni istisnaları ekle
                eklenecekIstisnalar
                .Select(istisna => new KaynakGunIciIstisnaTanimlari
                {
                    AktifMi = 1,
                    SilindiMi = 0,
                    AktifEdenKisiID = _loginUser.KisiID,
                    AktiflikTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KaynakTanimID = model.TabloID,
                    IstisnaSaatBaslangicDegeri = TimeSpan.Parse(istisna.IstisnaSaatBaslangicDegeri),
                    IstisnaSaatBitisDegeri = TimeSpan.Parse(istisna.IstisnaSaatBitisDegeri),
                    IstisnaTarihBaslangicDegeri = FixSqlDate(istisna.IstisnaTarihBaslangicDegeri),
                    IstisnaTarihtBitisDegeri = FixSqlDate(istisna.IstisnaTarihtBitisDegeri)
                })
                .ToList()
                .ForEach(newIstisna => _kaynakGunIciIstisnaTanimlariService.Add(newIstisna));

                //foreach (var istisna in eklenecekIstisnalar)
                //{
                //    _kaynakGunIciIstisnaTanimlariService.Add(new KaynakGunIciIstisnaTanimlari
                //    {
                //        AktifMi =1,
                //        SilindiMi = 0,
                //        AktifEdenKisiID = _loginUser.KisiID,
                //        AktiflikTarihi = DateTime.Now,
                //        GuncellenmeTarihi = DateTime.Now,
                //        GuncelleyenKisiID = _loginUser.KisiID,
                //        KaynakTanimID = model.TabloID,
                //        IstisnaSaatBaslangicDegeri = TimeSpan.Parse(istisna.IstisnaSaatBaslangicDegeri),
                //        IstisnaSaatBitisDegeri = TimeSpan.Parse(istisna.IstisnaSaatBitisDegeri),
                //        IstisnaTarihBaslangicDegeri = FixSqlDate(istisna.IstisnaTarihBaslangicDegeri),
                //        IstisnaTarihtBitisDegeri = FixSqlDate(istisna.IstisnaTarihtBitisDegeri)
                //    });
                //}

                // Silinecek istisnaları soft delete yap
                silinecekIstisnalar
                .ToList()
                .ForEach(istisna =>
                {
                    istisna.SilindiMi = 1;
                    istisna.AktifMi = 0;
                    istisna.SilenKisiID = _loginUser.KisiID;
                    istisna.SilinmeTarihi = DateTime.Now;
                    istisna.PasifEdenKisiID = _loginUser.KisiID;
                    _kaynakGunIciIstisnaTanimlariService.Update(istisna);
    });
                //foreach (var istisna in silinecekIstisnalar)
                //{
                //    istisna.SilindiMi = 1;
                //    istisna.AktifMi = 0;
                //    istisna.SilenKisiID = _loginUser.KisiID;
                //    istisna.SilinmeTarihi = DateTime.Now;
                //    istisna.PasifEdenKisiID = _loginUser.KisiID;
                //    _kaynakGunIciIstisnaTanimlariService.Update(istisna);
                //}

                //5. Kapasite Güncelleme
                var kapasite = _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.List(x => x.KaynakRezerveTanimID == model.TabloID).Value.FirstOrDefault();
                kapasite.Kapasaite = model.KapsiteVM.FirstOrDefault().Kapasaite;
                kapasite.KapasiteBirimID = model.KapsiteVM.FirstOrDefault().KapasiteBirimID;
                kapasite.KaynakRezerveTanimID = model.TabloID;
                kapasite.GuncellenmeTarihi = DateTime.Now;
                kapasite.GuncelleyenKisiID = _loginUser.KisiID;

                _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.Update(kapasite);
                return model.ToResult();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DateTime? FixSqlDate(DateTime? date)
        {
            if (!date.HasValue) return null;
            if (date.Value < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) return null;
            return date;
        }

        public Result<List<ParamKaynakTipleri>> KaynakTipiGetir()
        {
            var result = _paramKaynakTipiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0);
               return result;
        }
        public Result<bool> SilKaynak(int id)
        {
            var kaynak = _kaynakTanimlariService.List(x => x.TabloID == id).Value;
            var kapasite = _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.List(x => x.KaynakRezerveTanimID == id).Value;
            var rezerve = _kaynakRezerveTanimlariService.List(x => x.KaynakTanimID == kaynak.FirstOrDefault().TabloID).Value;
            if (kaynak != null)
            {
                kaynak.Select(item =>
                {
                    item.PasifEdenKisiID = _loginUser.KisiID;
                    item.PasiflikTarihi = DateTime.Now;
                    item.SilenKisiID = _loginUser.KisiID;
                    item.SilinmeTarihi = DateTime.Now;
                    item.AktifMi = 0;
                    item.SilindiMi = 1;


                    _kaynakTanimlariService.Update(item);
                    return item;
                }).ToList();

                rezerve.Select(item =>
                {
                    item.PasifEdenKisiID = _loginUser.KisiID;
                    item.PasiflikTarihi = DateTime.Now;
                    item.SilenKisiID = _loginUser.KisiID;
                    item.SilinmeTarihi = DateTime.Now;
                    item.AktifMi = 0;
                    item.SilindiMi = 1;


                    _kaynakRezerveTanimlariService.Update(item);
                    return item;
                }).ToList();

                kapasite.Select(item =>
                {
                    item.PasifEdenKisiID = _loginUser.KisiID;
                    item.PasiflikTarihi = DateTime.Now;
                    item.SilenKisiID = _loginUser.KisiID;
                    item.SilinmeTarihi = DateTime.Now;
                    item.AktifMi = 0;
                    item.SilindiMi = 1;
                    _kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.Update(item);
                    return item;
                }).ToList();

                return true.ToResult();
            }
            return false.ToResult();
        }
        public Result<KaynakTanimlariRezerveVM> KaynakRezerveKayit(KaynakTanimlariRezerveVM model)
        {
            try { 

                var KaynakTanimVM = model.KaynakTanimVM;
                var Kaynak = new KaynakTanimlari
                {
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    KaynakAdi = KaynakTanimVM.KaynakAdi,
                    KaynakAciklama = KaynakTanimVM.KaynakAciklama,
                    ParamKaynakTipiID = KaynakTanimVM.ParamKaynakTipiID,
                    //KaynakIkonID = KaynakTanimVM.KaynakIkonID,
                };_kaynakTanimlariService.Add(Kaynak);
                var KaynakRezerve = model.KaynakRezerveTanimVM;
                TimeSpan span;
                TimeSpan.TryParse(KaynakRezerve.RezerveSaatBitisDegeri, out span);
                TimeSpan spanEND;
                TimeSpan.TryParse(KaynakRezerve.RezerveSaatBaslangicDegeri, out spanEND);
                var Rezerve = new KaynakRezerveTanimlari
                {
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    RezerveSaatBaslangicDegeri = spanEND,
                    RezerveSaatBitisDegeri = span,
                    KaynakTanimID = Kaynak.TabloID,
                    UygunGunTipleri = KaynakRezerve.UygunGunTipleri
                };_kaynakRezerveTanimlariService.Add(Rezerve);

                var kapasite = model.KapsiteVM.FirstOrDefault();
                var kapasiteObj = new KaynakRezerveTanimlariAralikBaremleriKapasiteTanimi
                {
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    KaynakRezerveTanimID = Kaynak.TabloID,
                    Kapasaite = kapasite.Kapasaite,
                    KapasiteBirimID = kapasite.KapasiteBirimID
                };_kaynakRezerveTanimlariAralikBaremleriKapasiteTanimiService.Add(kapasiteObj);

                var kayitliMedyaList = model.MedyaVM
                    .SelectMany(item => _medyaKutuphanesiService
                        .List(x => x.TabloID == item.ResimID).Value
                        .Select(medya =>
                        {
                            medya.MedyaAciklama = "yok";
                            return medya;
                        })
                    )
                    .ToList();

                kayitliMedyaList.ForEach(m => _medyaKutuphanesiService.Update(m));


                // 2- Yeni KaynakTanimlariMedyalar kayıtları
                var yeniMedyaList = model.MedyaVM
                    .Select(item => new KaynakTanimlariMedyalar
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        TanimID = Kaynak.TabloID,
                        ResimID = item.ResimID
                    })
                    .ToList();

                // Tek seferde Add et
                yeniMedyaList.ForEach(m => _kaynakTanimlariMedyalarService.Add(m));


                var Istisnalar = model.KaynakIstisnalariVM.
                    Select(items => new KaynakGunIciIstisnaTanimlari
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        KaynakTanimID = Kaynak.TabloID,
                        IstisnaSaatBaslangicDegeri = TimeSpan.Parse(items.IstisnaSaatBaslangicDegeri),
                        IstisnaSaatBitisDegeri = TimeSpan.Parse(items.IstisnaSaatBitisDegeri),
                        IstisnaTarihBaslangicDegeri = items.IstisnaTarihBaslangicDegeri,
                    }).ToList();
                Istisnalar.ForEach(i => _kaynakGunIciIstisnaTanimlariService.Add(i));

                //foreach (var items in model.KaynakIstisnalariVM)
                //{
                //    var Istisna = new KaynakGunIciIstisnaTanimlari
                //    {
                //        AktifMi = 1,
                //        SilindiMi = 0,
                //        GuncellenmeTarihi = DateTime.Now,
                //        KayitTarihi = DateTime.Now,
                //        KayitEdenID = _loginUser.KisiID,
                //        GuncelleyenKisiID = _loginUser.KisiID,
                //        KurumID = _loginUser.KurumID,
                //        KisiID = _loginUser.KisiID,
                //        KaynakTanimID = Kaynak.TabloID,
                //        IstisnaSaatBaslangicDegeri = TimeSpan.Parse(items.IstisnaSaatBaslangicDegeri),
                //        IstisnaSaatBitisDegeri = TimeSpan.Parse(items.IstisnaSaatBitisDegeri),
                //        IstisnaTarihBaslangicDegeri = items.IstisnaTarihBaslangicDegeri,
                //        IstisnaTarihtBitisDegeri = items.IstisnaTarihtBitisDegeri,
                //    };_kaynakGunIciIstisnaTanimlariService.Add(Istisna);
                //}
                    return model.ToResult();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region UrunGiris-KurumService

        public Result<SliderTemelBilgilerMedyalarVM> SliderGuncelle(SliderTemelBilgilerMedyalarVM model)
        {
            try
            {
                var now = DateTime.Now;
                var userId = _loginUser.KisiID;
                var kurumId = _loginUser.KurumID;

                // 1. Ürünü getir ve güncelle
                var slider = _sliderTemelBilgilerService.List(x => x.TabloID == model.TabloID).Value.FirstOrDefault();
                if (slider != null)
                {
                    slider.GuncellenmeTarihi = now;
                    slider.GuncelleyenKisiID = userId;
                    slider.SliderGorselTanim = model.SliderGorselTanim;
                    slider.SliderGorselAciklama = model.SliderGorselAciklama;
                    _sliderTemelBilgilerService.Update(slider);
                }

                // 5. Medyalar güncelle/ekle/sil
                var mevcutUrunMedyalar = _sliderResimlerMedyaService
                    .List(x => x.sliderTemelBilgilerID == model.TabloID && x.AktifMi == 1 && x.SilindiMi == 0)
                    .Value;

                model.SliderResimlerMedyalarVM.ForEach(item =>
                {
                    // 1. MedyaKutuphanesi'ni güncelle/ekle
                    var medya = _medyaKutuphanesiService
                        .List(x => x.TabloID == item.MedyaID && x.AktifMi == 1 && x.SilindiMi == 0)
                        .Value.FirstOrDefault();

                    if (medya != null)
                    {
                        medya.MedyaAciklama = item.MedyaAciklama;
                        medya.GuncelleyenKisiID = userId;
                        medya.GuncellenmeTarihi = now;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    else
                    {
                        medya = new MedyaKutuphanesi
                        {
                            MedyaAdi = item.MedyaAdi,
                            MedyaUrl = item.MedyaUrl,
                            MedyaAciklama = item.MedyaAciklama,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitTarihi = now,
                            GuncellenmeTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId
                        };
                        _medyaKutuphanesiService.Add(medya);
                    }

                    // 2. UrunKutuphanesiMedyalar kaydını güncelle/ekle
                    var mevcut = mevcutUrunMedyalar.FirstOrDefault(m => m.medyaKutuphanesiID == medya.TabloID);
                    if (mevcut != null)
                    {
                        mevcut.MedyaAdi = item.MedyaAdi;
                        mevcut.GosterimAdi = item.GosterimAdi;
                        mevcut.SiraNo = item.SiraNo;
                        mevcut.GuncelleyenKisiID = userId;
                        mevcut.GuncellenmeTarihi = now;
                        _sliderResimlerMedyaService.Update(mevcut);
                    }
                    else
                    {
                        var yeniUrunMedya = new Baz.Model.Entity.SliderResimlerMedya
                        {
                            sliderTemelBilgilerID = model.TabloID,
                            medyaKutuphanesiID = medya.TabloID,
                            MedyaAdi = item.MedyaAdi,
                            GosterimAdi = item.GosterimAdi,
                            SiraNo = item.SiraNo,
                            AktifMi = 1,
                            SilindiMi = 0,
                            KayitTarihi = now,
                            GuncellenmeTarihi = now,
                            KayitEdenID = userId,
                            GuncelleyenKisiID = userId,
                            KurumID = kurumId,
                            KisiID = userId
                        };
                        _sliderResimlerMedyaService.Add(yeniUrunMedya);
                    }
                });

                // 3. Kullanılmayan UrunMedyalar'ı sil
                var silinecekler = mevcutUrunMedyalar
                    .Where(m => !model.SliderResimlerMedyalarVM.Any(x => x.MedyaID == m.medyaKutuphanesiID))
                    .ToList();

                silinecekler.ForEach(m =>
                {
                    m.AktifMi = 0;
                    m.SilindiMi = 1;
                    m.GuncelleyenKisiID = userId;
                    m.GuncellenmeTarihi = now;
                    _sliderResimlerMedyaService.Update(m);
                });


            }
            catch (Exception ex)
            {
                return null;
            }

            return model.ToResult();
        }


        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderVeriGetir(int urlId)
        {
            var  sliderKutuphanesiData = _sliderTemelBilgilerService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == urlId)
                .Value
                .Select(x => new
                {
                    x.TabloID,
                    x.SliderGorselAciklama,
                    x.SliderGorselTanim,
                })
                .ToList();

            var sliderMedyalar = _sliderResimlerMedyaService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;
            var medyaKutuphanesi = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;

            var sliderKutuphanesiBilgileri = sliderKutuphanesiData.Select(item =>
            {
                var ilgiliMedyalar = sliderMedyalar
                    .Where(m => m.sliderTemelBilgilerID == item.TabloID)
                    .Select(m =>
                    {
                        var medyaKutup = medyaKutuphanesi.FirstOrDefault(k => k.TabloID == m.medyaKutuphanesiID);
                        return new Baz.Model.Entity.ViewModel.SliderResimlerMedya
                        {
                            MedyaAdi = m.MedyaAdi,
                            GosterimAdi = m.GosterimAdi,
                            SiraNo = m.SiraNo,
                            MedyaID = m.medyaKutuphanesiID,
                            MedyaUrl = medyaKutup?.MedyaUrl,
                            MedyaAciklama = medyaKutup?.MedyaAciklama
                        };
                    })
                    .ToList();

                return new SliderTemelBilgilerMedyalarVM
                {
                    TabloID = item.TabloID,
                    SliderGorselTanim = item.SliderGorselTanim,
                    SliderGorselAciklama = item.SliderGorselAciklama,
                    SliderResimlerMedyalarVM = ilgiliMedyalar,

                };
            }).ToList();

            return sliderKutuphanesiBilgileri.ToResult();
        }
        ///<summary>
        ///DateTime Kontrol Helper
        ///</summary>
        //private DateTime GuvenliSqlDateTimeKontrol(DateTime date)
        //{
        //    return date < new DateTime(1753, 1, 1) ? DateTime.Now : date;
        //}

        /// <summary>
        /// Urunleri Listeleyen Metod
        /// </summary>
        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderListesi()
        {
            var sliderKutuphanesiData = _sliderTemelBilgilerService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0)
                .Value
                .ToList();

            if (!sliderKutuphanesiData.Any())
                return new List<SliderTemelBilgilerMedyalarVM>().ToResult();

            // Slider -> Medya ilişkisi
            var sliderMedyalar = _sliderResimlerMedyaService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0)
                .Value;

            // Tüm medya kayıtları
            var medyaKutuphanesi = _medyaKutuphanesiService
                .List(x => x.AktifMi == 1 && x.SilindiMi == 0)
                .Value
                .ToList();

            var sliderBilgileri = sliderKutuphanesiData
                .Select(s =>
                {
                    // Bu slider'a bağlı medyalar
                    var ilgiliMedyalar = sliderMedyalar
                        .Where(m => m.sliderTemelBilgilerID == s.TabloID)
                        .Select(m =>
                        {
                            var medyaKutup = medyaKutuphanesi.FirstOrDefault(k => k.TabloID == m.medyaKutuphanesiID);
                            return new Baz.Model.Entity.ViewModel.SliderResimlerMedya
                            {
                                MedyaAdi = m.MedyaAdi,
                                GosterimAdi = m.GosterimAdi,
                                SiraNo = m.SiraNo,
                                MedyaID = m.medyaKutuphanesiID,
                                MedyaUrl = medyaKutup?.MedyaUrl,
                                MedyaAciklama = medyaKutup?.MedyaAciklama
                            };
                        })
                        .ToList();

                    return new SliderTemelBilgilerMedyalarVM
                    {
                        TabloID = s.TabloID,
                        SliderGorselTanim = s.SliderGorselTanim,
                        SliderGorselAciklama = s.SliderGorselAciklama,
                        SliderResimlerMedyalarVM = ilgiliMedyalar
                    };
                })
                .ToList();

            return sliderBilgileri.ToResult();
        }


        /// <summary>
        /// Urunleri Kayıteden Metod
        /// </summary>
        public Result<SliderTemelBilgilerMedyalarVM> SliderKaydet(SliderTemelBilgilerMedyalarVM model)
        {
            try
            {

                var Slider = new SliderTemelBilgiler
                {
                    AktifMi = 1,
                    AktiflikTarihi = DateTime.Now,
                    AktifEdenKisiID = _loginUser.KisiID,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    KayitEdenID = _loginUser.KisiID,
                    GuncelleyenKisiID = _loginUser.KisiID,
                    KurumID = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    SliderGorselTanim = model.SliderGorselTanim,
                    SliderGorselAciklama = model.SliderGorselAciklama,
                };
                var sliderKayit = _sliderTemelBilgilerService.Add(Slider);
                //Medyalar
                foreach (var item in model.SliderResimlerMedyalarVM)
                {
                    var KayitliMedya = _medyaKutuphanesiService.List(x => x.TabloID == item.MedyaID).Value;
                    foreach (var medya in KayitliMedya)
                    {
                        medya.MedyaAciklama = item.MedyaAciklama;
                        _medyaKutuphanesiService.Update(medya);
                    }
                    var sliderMedya = new Baz.Model.Entity.SliderResimlerMedya
                    {
                        AktifMi = 1,
                        SilindiMi = 0,
                        GuncellenmeTarihi = DateTime.Now,
                        KayitTarihi = DateTime.Now,
                        KayitEdenID = _loginUser.KisiID,
                        GuncelleyenKisiID = _loginUser.KisiID,
                        KurumID = _loginUser.KurumID,
                        KisiID = _loginUser.KisiID,
                        MedyaAdi = item.MedyaAdi,
                        GosterimAdi = item.GosterimAdi,
                        medyaKutuphanesiID = item.MedyaID,
                        SiraNo = item.SiraNo,
                        sliderTemelBilgilerID = sliderKayit.Value.TabloID
                    };
                    _sliderResimlerMedyaService.Add(sliderMedya);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model.ToResult();

        }
        public Result<bool> SliderSil(int TabloId)
        {
            var slider = _sliderTemelBilgilerService.List(x => x.TabloID == TabloId).Value.FirstOrDefault();

            if (slider == null)
                return false.ToResult();

            var now = DateTime.Now;
            Action<dynamic> pasifEt = x =>
            {
                x.PasifEdenKisiID = _loginUser.KisiID;
                x.PasiflikTarihi = now;
                x.SilenKisiID = _loginUser.KisiID;
                x.SilinmeTarihi = now;
                x.AktifMi = 0;
                x.SilindiMi = 1;
            };

            // Ana ürün
            pasifEt(slider);
            _sliderTemelBilgilerService.Update(slider);

            return true.ToResult();
        }
        #endregion
        public string ConvertFileContentToDataUri(FileContentResult fileContentResult)
        {
            // Dosya içeriğini alın
            byte[] fileContent = fileContentResult.FileContents;

            // Dosya içeriğini Base64'e dönüştürün
            string base64Data = Convert.ToBase64String(fileContent);

            // MIME türünü alın
            string mimeType = fileContentResult.ContentType;

            // Veri URI oluşturun
            string dataUri = $"data:{mimeType};base64,{base64Data}";

            return dataUri;
        }
        public async Task<FileContentResult> DownloadMedyaFile(int id)
        {
            try
            {

                HttpClientHandler clientHandler = new();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (isDevelopment)
                    {
                        return true; // for development, trust all certificates
                    }
                    return sslPolicyErrors == SslPolicyErrors.None;
                };

                HttpClient _client = new(clientHandler);
                _client.Timeout = Timeout.InfiniteTimeSpan;

                HttpResponseMessage response = await _client.GetAsync(LocalPortlar.MedyaKutuphanesiService + "/api/MedyaKutuphanesi/GetMedyaFile/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var contentType = response.Content.Headers.ContentType.MediaType;
                    return new FileContentResult(content, contentType);
                }
                else
                {
                    throw new Exception("Medya not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Result<List<MedyaKutuphanesi>> IcerikMedyalariGetir()
        {
            var data = new List<MedyaKutuphanesi>();
            var icerikMedyalar = _icerikKutuphanesiMedyalarService.List(x => x.AktifMi == 1 && x.SilindiMi == 0).Value;

            foreach (var item in icerikMedyalar)
            {
                var icerikKutupID = item.IcerikKutuphanesiID;
                var medyaID = item.MedyaKutuphanesiID;
                var medyalar = _medyaKutuphanesiService.List(x => x.AktifMi == 1 && x.SilindiMi == 0 && x.TabloID == medyaID).Value.FirstOrDefault();
                //var baseMedya = DownloadMedyaFile(medyalar.TabloID);
                var medyaurl = medyalar.MedyaUrl;
                //var medyaurl = ConvertFileContentToDataUri(baseMedya.Result);
                var medyaAciklama = item.MedyaAciklama;
                //TabloID icerikkutuphanesiıd olarak yollandı MedyaTipiId ise medyaKutuphanesiID olarak gönderiliyor
                var bilgi = new MedyaKutuphanesi()
                {
                    TabloID = icerikKutupID,
                    MedyaAdi = medyaAciklama,
                    MedyaUrl = medyaurl,
                    MedyaTipiId = medyaID,
                };
                data.Add(bilgi);
            }

            return data.ToResult();

        }

        public Result<IcerikKutuphanesi> IcerikBilgileriSil(int id)
        {
            //IcerikKutuphanesi Silinmesi 
            var icerikBilgiModel = _icerikKutuphanesiService.List(a => a.TabloID == id).Value.SingleOrDefault();
            icerikBilgiModel.AktifMi = 0;
            icerikBilgiModel.SilindiMi = 1;
            _icerikKutuphanesiService.Update(icerikBilgiModel);

            //IcerikKutuphanesiMeyalar Silinmesi
            var icerikMedyalarBilgiModel = _icerikKutuphanesiMedyalarService.List(a => a.TabloID == id).Value.SingleOrDefault();
            icerikMedyalarBilgiModel.AktifMi = 0;
            icerikMedyalarBilgiModel.SilindiMi = 1;
            _icerikKutuphanesiService.Update(icerikBilgiModel);

            return icerikBilgiModel.ToResult();

        }
 
    }
}