using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.RequestManager;
using Baz.RequestManager.Abstracts;
using KurumServiceTest.Helper;

namespace KurumServiceTest
{
    /// <summary>
    /// Menü kontrolü için gerekli methodların bulunduğu test sınıfıdır.
    /// </summary>
    [TestClass()]
    public class YetkiMerkeziTests
    {
        private readonly IRequestHelper _helper;
        private readonly IRequestHelper _noHeader;

        /// <summary>
        /// Menü kontrolü için gerekli methodların bulunduğu test sınıfıdır.
        /// </summary>
        public YetkiMerkeziTests()
        {
            _noHeader = TestServerRequestHelperNoHeader.CreateHelper();
            _helper = TestServerRequestHelper.CreateHelper();
        }

        /// <summary>
        /// Erişim yetkilendirme tanımlarını kaydeden test metotu.
        /// </summary>
        [TestMethod()]
        public void ErisimYetkilendirmeTanimlariCrudTest()//public void ErisimYetkilendirmeTanimlariCrudTest()
        {
            // Act-8 Add Negative
            var tanimkaydetnegative = _helper.Post<Result<List<ErisimYetkilendirmeTanimlari>>>("/api/YetkiMerkezi/ErisimYetkilendirmeTanimlariKaydet", new List<ErisimYetkilendirmeTanimlari>
            {
            });
            Assert.IsFalse(tanimkaydetnegative.Result.IsSuccess);
            // Act-1 Add
            var tanimkaydet = _helper.Post<Result<List<ErisimYetkilendirmeTanimlari>>>("/api/YetkiMerkezi/ErisimYetkilendirmeTanimlariKaydet", new List<ErisimYetkilendirmeTanimlari>
            {
                new ErisimYetkilendirmeTanimlari
                {
                    IlgiliKurumOrganizasyonBirimTanimiId = 441,
                    ErisimYetkisiVerilenSayfaId = 40,
                    KayitTarihi = DateTime.Now,
                    ErisimYetkisiVerilmeTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    AktifMi = 1,
                    SilindiMi = 0,
                    KayitEdenID = 129,
                    KisiID =129,
                    ErisimYetkisiVerenKisiId = 129,
                    IlgiliKurumId = 129,
                    KurumID = 129
                }
            });
            Assert.AreEqual(tanimkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(tanimkaydet.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(tanimkaydet.Result);
            // Act-1 Add Repeat
            var tanimkaydetTekrar = _helper.Post<Result<List<ErisimYetkilendirmeTanimlari>>>("/api/YetkiMerkezi/ErisimYetkilendirmeTanimlariKaydet", new List<ErisimYetkilendirmeTanimlari>
            {
                new ErisimYetkilendirmeTanimlari
                {
                    IlgiliKurumOrganizasyonBirimTanimiId = 441,
                    ErisimYetkisiVerilenSayfaId = 40,
                    KayitTarihi = DateTime.Now,
                    ErisimYetkisiVerilmeTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    AktifMi = 1,
                    SilindiMi = 0,
                    KayitEdenID = 129,
                    KisiID =129,
                    ErisimYetkisiVerenKisiId = 129,
                    IlgiliKurumId = 129,
                    KurumID = 129
                }
            });
            Assert.AreEqual(tanimkaydetTekrar.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(tanimkaydetTekrar.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(tanimkaydetTekrar.Result);
            // Act-1 Add Repeat 2
            var tanimkaydetTekrar2 = _helper.Post<Result<List<ErisimYetkilendirmeTanimlari>>>("/api/YetkiMerkezi/ErisimYetkilendirmeTanimlariKaydet", new List<ErisimYetkilendirmeTanimlari>
            {
                new ErisimYetkilendirmeTanimlari
                {
                    IlgiliKurumOrganizasyonBirimTanimiId = 441,
                    ErisimYetkisiVerilenSayfaId = 40,
                    KayitTarihi = DateTime.Now,
                    ErisimYetkisiVerilmeTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    AktifMi = 1,
                    SilindiMi = 0,
                    KayitEdenID = 129,
                    KisiID =129,
                    ErisimYetkisiVerenKisiId = 129,
                    IlgiliKurumId = 129,
                    KurumID = 129
                },
                new ErisimYetkilendirmeTanimlari
                {
                    IlgiliKurumOrganizasyonBirimTanimiId = 441,
                    ErisimYetkisiVerilenSayfaId = 41,
                    KayitTarihi = DateTime.Now,
                    ErisimYetkisiVerilmeTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    AktifMi = 1,
                    SilindiMi = 0,
                    KayitEdenID = 129,
                    KisiID =129,
                    ErisimYetkisiVerenKisiId = 129,
                    IlgiliKurumId = 129,
                    KurumID = 129
                },
            });
            Assert.AreEqual(tanimkaydetTekrar2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(tanimkaydetTekrar2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(tanimkaydetTekrar2.Result);

            // Act-2 Kişi Yetki Listesi Getir
            var yetkilistgetir = _helper.Get<Result<List<string>>>("/api/YetkiMerkezi/KisiYetkilerListGetir/" + 129);
            Assert.AreEqual(yetkilistgetir.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(yetkilistgetir.Result);

            // Act-3 Erişim Yetki Tanım Listesi
            var yetkitanimlist = _helper.Get<Result<List<ErisimYetkilendirmeTanimlariListView>>>("/api/YetkiMerkezi/ErisimYetkiTanimListGetir");
            Assert.AreEqual(yetkitanimlist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(yetkitanimlist.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(yetkitanimlist.Result);

            // Act-4 Yetki Tanımı Sil
            foreach (var tk in tanimkaydet.Result.Value)
            {
                var yetkitanimisil = _helper.Get<Result<bool>>("/api/YetkiMerkezi/ErisimYetkiTanimiSil/" + tk.TabloID);
                Assert.AreEqual(yetkitanimisil.Result.StatusCode, (int)ResultStatusCode.Success);
                Assert.AreEqual(yetkitanimisil.StatusCode, HttpStatusCode.OK);
                Assert.IsTrue(yetkitanimisil.Result.Value);
            }

            // Act-6 Aksiyon yetkileri listesi
            var aksiyonyetkilerilist = _helper.Get<Result<List<ErisimYetkilendirmeTanimlariListView>>>("/api/YetkiMerkezi/AksiyonYetkileriList");
            Assert.AreEqual(aksiyonyetkilerilist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(aksiyonyetkilerilist.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(aksiyonyetkilerilist.Result);

            var son = aksiyonyetkilerilist.Result.Value.LastOrDefault();

            // Act-7 Aksiyon yetkilendirme sil
            var yetkitanimisil2 = _noHeader.Get<Result<bool>>("/api/YetkiMerkezi/ErisimYetkiTanimiSil/" + son.TabloID);
            Assert.AreEqual(yetkitanimisil2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(yetkitanimisil2.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(yetkitanimisil2.Result.Value);

            // Act-9 Kişi Yetki Listesi Getir Negative
            var yetkilistgetirnegative = _noHeader.Get<Result<List<string>>>("/api/YetkiMerkezi/KisiYetkilerListGetir/" + 0);
            Assert.AreEqual(yetkilistgetirnegative.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(yetkilistgetirnegative.Result.Value);

            // Act-10 Yetki Tanımı Sil Negative
            var yetkitanimisilnegative = _noHeader.Get<Result<bool>>("/api/YetkiMerkezi/ErisimYetkiTanimiSil/" + 0);
            Assert.IsFalse(yetkitanimisilnegative.Result.IsSuccess);
        }
    }
}