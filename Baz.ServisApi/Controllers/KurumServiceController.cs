using Baz.Attributes;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baz.KurumServiceApi.Controllers
{
    /// <summary>
    /// KurumService web API işlemlerini yöneten controller
    /// </summary>
    [Route("api/[Controller]")]
    public class KurumServiceController : Controller
    {
        private readonly IKurumService _kurumService;
        private readonly IKurumIliskiService _kurumIliskiService;
        private readonly IKurumlarKisilerService _kurumlarKisilerService;

        /// <summary>
        /// KurumService web API işlemlerini yöneten controller sınıfının yapıcı methdodu
        /// </summary>
        /// <param name="kurumService"></param>
        /// <param name="kurumIliskiService"></param>
        /// <param name="kurumlarKisilerService"></param>
        public KurumServiceController(IKurumService kurumService, IKurumIliskiService kurumIliskiService, IKurumlarKisilerService kurumlarKisilerService)
        {
            _kurumService = kurumService;
            _kurumIliskiService = kurumIliskiService;
            _kurumlarKisilerService = kurumlarKisilerService;
        }

        /// <summary>
        /// verilen string parametresini içeren kurumları listeleyip getiren method.
        /// </summary>
        /// <param name="kurumAdi">eşleşme için kullanılacak string parametresi</param>
        /// <returns>listeme sonucunu döndürür.</returns>
        [ProcessName(Name = "Kurum ismine göre yapılan aramadan listenin dönülmesi")]
        [Route("KurumList")]
        [HttpPost]
        public Result<List<KurumTemelBilgiler>> KurumList([FromBody] string kurumAdi)
        {
            var result = _kurumService.KurumlarList(kurumAdi);
            return result;
        }
        [HttpGet]
        [Route("SistemSayfaBasliklariGetir")]
        [AllowAnonymous]
        public Result<List<SistemMenuTanimlariWM>> SistemSayfaBasliklariGetir()
        {
            var result = _kurumService.SistemSayfaBasliklariGetir();
            return result;
        }
        /// <summary>
        /// Sistemde kayıtlı ve aktif durumdaki tüm kurumları listeleme methodu
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Kurum listesi")]
        [Route("List")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> List()
        {
            var result = _kurumService.List(p => p.AktifMi == 1 && p.SilindiMi == 0);
            return result;
        }

        /// <summary>
        /// Kurumları Id'ye göre listeleyen  method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum listesi")]
        [Route("List/{id}")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> List(int id)
        {
            var result = _kurumService.List(id);
            return result;
        }

        /// <summary>
        /// Kurum Organizasyon birimleri id si ile kişi id listesi getirir
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Kişi Id Listesi")]
        [Route("ListKurumOrganizasyonIdileKisiListele")]
        [HttpPost]
        public Result<List<int>> ListKurumOrganizasyonId([FromBody] List<int> model)
        {
            var result = _kurumlarKisilerService.KurumkisiGetirOrganizasyonKurumBirimIDYeGore(model);
            return result;
        }

        /// <summary>
        /// Kurumları Id'ye göre listeleyen  method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum listesi kendisi ile birlikte")]
        [Route("ListKendisiIle/{id}")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> ListKendisiIle(int id)
        {
            var result = _kurumService.ListKendisiIle(id);
            return result;
        }

        /// <summary>
        /// Id'ye göre kurum getiren method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum verilerinin getirilmesi")]
        [Route("KurumGetir/{id}")]
        [HttpGet]
        public Result<KurumTemelBilgiler> KurumGetir(int id)
        {
            var result = _kurumService.SingleOrDefault(id);
            return result;
        }

        /// <summary>
        /// Kurum kaydeden method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum temel verileri kaydetme")]
        [Route("KurumTemelVerileriKaydet")]
        [HttpPost]
        public Result<KurumTemelKayitModel> KurumTemelVerileriKaydet([FromBody] KurumTemelKayitModel model)
        {
            var result = _kurumService.KurumTemelVerileriKaydet(model);
            return result;
        }

        /// <summary>
        /// Kurum temel verileri getiren method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum temel verileri getirme")]
        [Route("KurumTemelVerileriGetir/{id}")]
        [HttpGet]
        public Result<KurumTemelKayitModel> KurumTemelVerileriGetir(int id)
        {
            var result = _kurumService.KurumTemelVerileriGetir(id);
            return result;
        }

        /// <summary>
        /// Kurum İdari verileri getiren method
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Kurum idari verileri getirme")]
        [Route("KurumIdariVerileriGetir/{kurumID}")]
        [HttpGet]
        public Result<KurumIdariProfilModel> KurumIdarilVerileriGetir(int kurumID)
        {
            var result = _kurumService.KurumIdarilVerileriGetir(kurumID);
            return result;
        }

        /// <summary>
        /// Kurum verilerini güncelleyen method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum temel verileri güncelleme")]
        [Route("KurumTemelVerileriGuncelle")]
        [HttpPost]
        public Result<KurumTemelKayitModel> KurumTemelVerileriGuncelle([FromBody] KurumTemelKayitModel model)
        {
            var result = _kurumService.KurumTemelVerileriGuncelle(model);
            return result;
        }

        /// <summary>
        /// Kurum bilgilerini silindi yapan method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum bilgilerini silindi yapma işlemi")]
        [Route("TemelKurumSilindiYap/{kurumID}")]
        [HttpGet]
        public Result<bool> TemelKurumSilindiYap(int kurumID)
        {
            var result = _kurumService.TemelKurumSilindiYap(kurumID);
            return result;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi kaydetme metotu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurumlar arası ilişki kaydetme")]
        [Route("KurumIliskiKaydet")]
        [HttpPost]
        public Result<Iliskiler> KurumIliskiKaydet([FromBody] KurumIliskiKayitModel model)
        {
            var result = _kurumIliskiService.KurumIliskiKaydet(model);
            return result;
        }

        /// <summary>
        /// Kurumlar arası ilişkileri listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurumlar arası ilişkiyi listeleme")]
        [Route("KurumIliskiList/{kurumID}")]
        [HttpGet]
        public Result<List<Iliskiler>> KurumIliskiList(int kurumID)
        {
            var result = _kurumIliskiService.KurumIliskiList(kurumID);
            return result;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum ilişki bilgilerini silindi yapma işlemi")]
        [Route("KurumIliskiSil/{tabloID}")]
        [HttpGet]
        public Result<bool> KurumIliskiSil(int tabloID)
        {
            var result = _kurumIliskiService.KurumIliskiSil(tabloID);
            return result;
        }

        /// <summary>
        /// Kurumlar arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurumlar arası ilişki güncelleme")]
        [Route("KurumIliskiGuncelle")]
        [HttpPost]
        public Result<Iliskiler> KurumIliskiGuncelle([FromBody] KurumIliskiKayitModel model)
        {
            var result = _kurumIliskiService.KurumIliskiGuncelle(model);
            return result;
        }

        /// <summary>
        /// Kişi müşteri temsilcisi mi kontrolü sağlayan metot.
        /// </summary>
        /// <param name="kisiId">kişi Id</param>
        /// <returns>sonucu true veya false olarak döndürür.</returns>
        [ProcessName(Name = "Kişi müşteri temsilcisi mi kontrolü sağlayan metot.")]
        [Route("KisiMusteriTemsilcisiMi/{kisiId}")]
        [HttpGet]
        public Result<bool> KisiMusteriTemsilcisiMi(int kisiId)
        {
            var result = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(kisiId);
            return result;
        }

        /// <summary>
        /// Müşteri temsilcisine bağlı kurumları listeleyen metot
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Müşteri temsilcisine atanmış kurumları listeleme")]
        [Route("MusteriTemsilcisiBagliKurumlarList/{musteriTemsilcisiId}")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> MusteriTemsilcisiBagliKurumlarList(int musteriTemsilcisiId)
        {
            var result = _kurumService.MusteriTemsilcisiBagliKurumlarList(musteriTemsilcisiId);
            return result;
        }

        /// <summary>
        /// Amirlerin astı Müşteri temsilerine bağlı kurumları listeleyen metot
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Amirlerin astı olan müşteri temsilcilerine atanmış kurumları listeleme")]
        [Route("AmirlereAstMusteriTemsilcisiKurumlariniGetir/{amirId}")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> AmirlereAstMusteriTemsilcisiKurumlariniGetir(int amirId)
        {
            var result = _kurumService.AmirlereAstMusteriTemsilcisiKurumlariniGetir(amirId);
            return result;
        }

        /// <summary>
        /// Kuruma ait Müşteri temsilcilerini getiren metod
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Kuruma ait müşteri temsilcileri")]
        [Route("KurumMusteriTemsilcisiGetir")]
        [HttpGet]
        public Result<List<GenelViewModel>> KurumMusteriTemsilcisiGetir()
        {
            var result = _kurumService.KurumMusteriTemsilcisiGetir();
            return result;
        }

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kurumlara erişmesi gereken kullanıcılar için kullanılacak kurum listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KurumTemelBilgiler"></see></returns>
        [Route("HiyerarsiDisiKisilerKurumListesi")]
        [HttpGet]
        public Result<List<KurumTemelBilgiler>> HiyerarsiDisiKisilerKurumListesi()
        {
            var result = _kurumService.HiyerarsiDisiKisilerKurumListesi();
            return result;
        }

     
        

       


        /// <summary>
        /// Kurum bilgilerini silen test için oluşturulmuş metod
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kurum bilgilerini silindi yapma işlemi")]
        [Route("KurumDelete/{kurumID}")]
        [HttpGet]
        public Result<bool> HardDeleteTest(int kurumID)
        {
            var result = _kurumService.Delete(kurumID);
            return true.ToResult();
        }
       
        /// <summary>
        /// İçerik Kategorileri Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("IcerikKategorileriGetir")]
        [HttpGet]
        public Result<List<ParamIcerikKategoriler>> IcerikKategorileriGetir()
        {
            var result = _kurumService.IcerikKategorileriGetir();
            return result;
        }
        /// <summary>
        /// İçerik Alt Kategorileri Listeleyen Metod
        /// </summary>
        /// <param name="kategoriId"></param>
        /// <returns></returns>
        //[Route("IcerikAltKategorileriGetir/{kategoriId}")]
        //[HttpGet]
        //public Result<List<ParamIcerikKategoriler>> IcerikAltKategorileriGetir(int kategoriId)
        //{
        //    var result = _kurumService.IcerikAltKategorileriGetir(kategoriId);
        //    return result;
        //}
        /// <summary>
        /// İçerik kayıt eden metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("IcerikKaydet")]
        [HttpPost]
        public Result<IcerikKutuphanesiMedyalarVM> IcerikKaydet([FromBody] IcerikKutuphanesiMedyalarVM model)
        {
            var result = _kurumService.IcerikKaydet(model);
            return result;
        }
        /// <summary>
        /// İçerik kayıt eden metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
 
        /// <summary>
        /// İçerik kayıt eden metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("MedyaKayit")]
        [HttpPost]
        public Result<MedyaKutuphanesi> MedyaKayit([FromBody] MedyaKutuphanesi model)
        {
            var result = _kurumService.MedyaKayit(model);
            return result;
        }

        /// <summary>
        /// İçerik Güncelleyen metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("IcerikGuncelle")]
        [HttpPost]
        public Result<IcerikKutuphanesiMedyalarVM> IcerikGuncelle([FromBody] IcerikKutuphanesiMedyalarVM model)
        {
            var result = _kurumService.IcerikGuncelle(model);
            return result;
        }
        /// <summary>
        /// İçerik silen meyod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("IcerikSil/{TabloId}")]
        [HttpPost]
        public Result<bool> IcerikSil(int TabloId)
        {
            var result = _kurumService.IcerikSil(TabloId);
            return true.ToResult();
        }
        /// <summary>
        /// İçerik Güncelleme için veri getiren metod
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        [Route("IcerikVeriGetir/{urlId}")]
        [HttpGet]
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikVeriGetir(int urlId)
        {
            var result = _kurumService.IcerikVeriGetir(urlId);
            return result;
        }
        /// <summary>
        /// İçerikKutuphanesi bağımsız bilgilerini listeleyen metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("IcerikListesi")]
        [HttpGet]
        public Result<List<IcerikKutuphanesiMedyalarVM>> IcerikListesi()
        {
            var result = _kurumService.IcerikListesi();
            return result;
        }
        /// <summary>
        /// icerik bilgilerinin getirildiği method
        /// </summary>
        /// <param name="icerikID"></param>
        /// <returns></returns>
        [ProcessName(Name = "İçerik kayıt bilgilerini getirme işlemi")]
        [Route("IcerikVerileriniGetir")]
        [HttpPost]
        public Result<IcerikKutuphanesiMedyalarVM> IcerikVerileriniGetir([FromBody] int icerikID)
        {
            var result = _kurumService.IcerikVerileriniGetir(icerikID);
            return result;
        }
        #region UrunGiris-KurumService
        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("ParamUrunMarkalarınıGetir")]
        [HttpGet]
        public Result<List<ParamUrunMarkalar>> ParamUrunMarkalarınıGetir()
        {
            var result = _kurumService.ParamUrunMarkalarınıGetir();
            return result;
        }
        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("UrunParametreDegerGetir")]
        [HttpGet]
        public Result<List<ParamOlcumBirimleri>> UrunParametreDegerGetir()
        {
            var result = _kurumService.UrunParametreDegerGetir();
            return result;
        }
        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("UrunOlcumBirimiData")]
        [HttpGet]
        public Result<List<ParamOlcumBirimleri>> UrunOlcumBirimiData()
        {
            var result = _kurumService.UrunOlcumBirimiData();
            return result;
        }

        /// <summary>
        /// Urun Kategorileri Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("UrunKategorileriGetir")]
        [HttpGet]
        public Result<List<ParamUrunKategoriler>> UrunKategorileriGetir()
        {
            var result = _kurumService.UrunKategorileriGetir();
            return result;
        }
        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("IcerikBlokKategorilerGetir")]
        [HttpGet]
        public Result<List<ParamIcerikBlokKategorileri >> IcerikBlokKategorilerGetir()
        {
            var result = _kurumService.IcerikBlokKategorilerGetir();
            return result;
        }

        /// <summary>
        /// Urun verilerini Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("IcerikKutuphanesiGetir")]
        [HttpGet]
        public Result<List<IcerikKutuphanesi>> IcerikKutuphanesiGetir()
        {
            var result = _kurumService.IcerikKutuphanesiGetir();
            return result;
        }
        /// <summary>
        /// Parabirimlerini Kategorileri Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("UrunParaBirimiGetir")]
        [HttpGet]
        public Result<List<ParamParaBirimleri>> UrunParaBirimiGetir()
        {
            var result = _kurumService.UrunParaBirimiGetir();
            return result;
        }
        /// <summary>
        /// Urunleri Kayıteden Metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UrunKaydet")]
        [HttpPost]
        public Result<UrunKutuphanesiMedyalarVM> UrunKaydet([FromBody]UrunKutuphanesiMedyalarVM model) {
            var result = _kurumService.UrunKaydet(model);
            return result;
        }
        /// <summary>
        /// Urunleri Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("UrunListesi")]
        [HttpGet]
        public Result<List<UrunKutuphanesiMedyalarVM>> UrunListesi()
        {
            var result = _kurumService.UrunListesi();
            return result;
        }

        /// <summary>
        /// Ürünlerin silen meyod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UrunSil/{TabloId}")]
        [HttpPost]
        public Result<bool> UrunSil(int TabloId)
        {
            var result = _kurumService.UrunSil(TabloId);
            return true.ToResult();
        }

        /// <summary>
        /// Ürün Güncelleme için veri getiren metod
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        [Route("UrunVeriGetir/{urlId}")]
        [HttpGet]
        public Result<List<UrunKutuphanesiMedyalarVM>> UrunVeriGetir(int urlId)
        {
            var result = _kurumService.UrunVeriGetir(urlId);
            return result;
        }

        /// <summary>
        /// Ürün Güncelleyen metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UrunGuncelle")]
        [HttpPost]
        public Result<UrunKutuphanesiMedyalarVM> UrunGuncelle([FromBody] UrunKutuphanesiMedyalarVM model)
        {
            var result = _kurumService.UrunGuncelle(model);
            return result;
        }
        #endregion


        #region Kaynak Rezerve
        /// <summary>
        ///  Veri getiren Kaynak Rezerve Methdları
        [Route("KaynakTipiGetir")]
        [HttpGet]
        public Result<List<ParamKaynakTipleri>> KaynakTipiGetir()
        {
            var result = _kurumService.KaynakTipiGetir();
            return result;
        }

        [Route("RezerveKaynakGuncelle")]
        [HttpPost]
        public Result<KaynakTanimlariRezerveVM> RezerveKaynakGuncelle([FromBody] KaynakTanimlariRezerveVM model)
        {
            var result = _kurumService.RezerveKaynakGuncelle(model);
            return result;
        }

        [Route("KaynakRezerveKayit")]
        [HttpPost]
        public Result<KaynakTanimlariRezerveVM> KaynakRezerveKayit([FromBody] KaynakTanimlariRezerveVM model)
        {
            var result = _kurumService.KaynakRezerveKayit(model);
            return result;
        }

        [Route("KaynakRezerveVeriGetir/{urlId}")]
        [HttpGet]
        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveVeriGetir(int urlId)
        {
            var result = _kurumService.KaynakRezerveVeriGetir(urlId);
            return result;
        }

        [Route("KaynakRezerveListele")]
        [HttpGet]
        public Result<List<KaynakTanimlariRezerveVM>> KaynakRezerveListele()
        {
            var result = _kurumService.KaynakRezerveListele();
            return result;
        }

        [Route("SilKaynak/{id}")]
        [HttpPost]
        public Result<bool> SilKaynak(int id)
        {
            var result = _kurumService.SilKaynak(id);
            return result;
        }
        /// </summary>
        /// <returns></returns>
        #endregion

        #region Slider
        /// <summary>
        /// Slider Kayıteden Metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SliderKaydet")]
        [HttpPost]
        public Result<SliderTemelBilgilerMedyalarVM> SliderKaydet([FromBody] SliderTemelBilgilerMedyalarVM model)
        {
            var result = _kurumService.SliderKaydet(model);
            return result;
        }
        /// <summary>
        /// Slider Listeleyen Metod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("SliderListesi")]
        [HttpGet]
        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderListesi()
        {
            var result = _kurumService.SliderListesi();
            return result;
        }

        /// <summary>
        /// Slider silen meyod
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("SliderSil/{TabloId}")]
        [HttpPost]
        public Result<bool> SliderSil(int TabloId)
        {
            var result = _kurumService.SliderSil(TabloId);
            return true.ToResult();
        }

        /// <summary>
        /// Slider Güncelleme için veri getiren metod
        /// </summary>
        /// <param name="urlId"></param>
        /// <returns></returns>
        [Route("SliderVeriGetir/{urlId}")]
        [HttpGet]
        public Result<List<SliderTemelBilgilerMedyalarVM>> SliderVeriGetir(int urlId)
        {
            var result = _kurumService.SliderVeriGetir(urlId);
            return result;
        }

        /// <summary>
        /// Slider Güncelleyen metod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SliderGuncelle")]
        [HttpPost]
        public Result<SliderTemelBilgilerMedyalarVM> SliderGuncelle([FromBody]SliderTemelBilgilerMedyalarVM model)
        {
            var result = _kurumService.SliderGuncelle(model);
            return result;
        }
        #endregion
    }
}