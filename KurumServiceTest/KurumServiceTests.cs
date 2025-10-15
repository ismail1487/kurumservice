using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baz.KurumServiceApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.RequestManager;
using Baz.RequestManager.Abstracts;
using Castle.Components.DictionaryAdapter;
using KurumServiceTest.Helper;
using System.Threading.Tasks;

namespace KurumServiceTest
{
    /// <summary>
    /// Kurum Servisinin test methodlarının olduğu sınıftır.
    /// </summary>
    [TestClass()]
    public class KurumServiceTests
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// KurumServiceController testleri
        /// </summary>
        public KurumServiceTests()
        {
            _helper = TestServerRequestHelper.CreateHelper();
        }

        #region KurumTemelTests

        /// <summary>
        /// Kurum Crud Test Methods
        /// </summary>
        [TestMethod()]
        public void CrudTestsKurum()//public void CrudTestsKurum()
        {
            //var id1 = 0;
            //try
            //{
            //Add Kurum
            var kurumkaydet = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriKaydet", new KurumTemelKayitModel
            {
                KurumAdi = "Kurum Servis Unit Test " + Guid.NewGuid().ToString().Substring(0, 10),
                KurulusTarihi = DateTime.Now,
                TicaretSicilNo = "",
                VergiDairesiAdi = "",
                KurumLogo = 1,
                EpostaAdresi = "bilalas@bilal.com",
                WebSitesi = "",
                VergiNo = "",
                FaxNo = "",
                KurumResimUrl = "",
                KurumId = 82,
                KurumVergiDairesiId = "12345df",
                AdresListesi = new EditableList<KurumAdresModel>
                {
                    new KurumAdresModel
                    {
                        Adres = "",
                        LokasyonAdi = "",
                        LokasyonTipi = 1,
                        Sehir = 1,
                        SehirAdi = "",
                        Ulke = 1,
                        UlkeAdi = ""
                    }
                },
                BankaListesi = new List<KurumBankaModel>
                {
                    new KurumBankaModel
                    {
                        BankaAdi="Kurum Servis test bank",
                        BankaId = 1,
                        SubeAdi ="test sube",
                        HesapNo ="123456789",
                        Iban="TR0000000000000000000000",
                        IlgiliKurumId= 0,
                        SubeId = 1
                    }
                },
                KurumTips = new List<KurumTipi>
                {
                    new KurumTipi
                    {
                        TabloID=8,
                        Tanim="Müşteri"
                    }
                },
                MusteriTemsilciIds = new List<MusteriTemsilcisi>
                {
                    new MusteriTemsilcisi
                    {
                        AdSoyad = "",
                        TabloID = 129
                    }
                }
            });
            Assert.AreEqual(kurumkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumkaydet.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumkaydet.Result);
            //id1 = kurumkaydet.Result.Value.TabloID;
            // Add Kurum Negative
            var kurumkaydetnegative = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriKaydet", new KurumTemelKayitModel
            {
                KurumAdi=kurumkaydet.Result.Value.KurumAdi,
                KurumId=kurumkaydet.Result.Value.TabloID
            });
            Assert.IsFalse(kurumkaydetnegative.Result.IsSuccess);

            var kurumguncelle = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriGuncelle", new KurumTemelKayitModel
            {
                KurumAdi = "Kurum Servis Unit Test1" + Guid.NewGuid().ToString().Substring(0, 10),
                KurulusTarihi = DateTime.Now,
                TicaretSicilNo = "",
                VergiDairesiAdi = "",
                KurumLogo = 1,
                EpostaAdresi = "bilal@bilal.com",
                WebSitesi = "",
                VergiNo = "",
                FaxNo = "",
                KurumResimUrl = "",
                KurumVergiDairesiId = "12345df",
                TabloID = kurumkaydet.Result.Value.TabloID,
                AdresListesi = new EditableList<KurumAdresModel>
                {
                    new KurumAdresModel
                    {
                        Adres = "",
                        LokasyonAdi = "",
                        LokasyonTipi = 1,
                        Sehir = 1,
                        SehirAdi = "",
                        Ulke = 1,
                        UlkeAdi = ""
                    }
                },
                BankaListesi = new List<KurumBankaModel>
                {
                    new KurumBankaModel
                    {
                        BankaAdi="Kurum Servis test bank",
                        BankaId = 1,
                        SubeAdi ="test sube",
                        HesapNo ="123456789",
                        Iban="TR0000000000000000000000",
                        IlgiliKurumId= kurumkaydet.Result.Value.TabloID,
                        SubeId = 2
                    }
                },
                KurumTips = new List<KurumTipi>
                {
                    new KurumTipi
                    {
                        TabloID=8,
                        Tanim="Müşteri"
                    }
                },
                MusteriTemsilciIds = new List<MusteriTemsilcisi>
                {
                    new MusteriTemsilcisi
                    {
                        AdSoyad = "",
                        TabloID = 129
                    }
                }
            });
            Assert.AreEqual(kurumguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumguncelle.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumguncelle.Result);

            var kurumguncellenegative = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriGuncelle", new KurumTemelKayitModel
            {
                KurumAdi = "Kurum Servis Test Bilal1 A.Ş",
                KurulusTarihi = DateTime.Now,
                TicaretSicilNo = "",
                VergiDairesiAdi = "",
                KurumLogo = 1,
                EpostaAdresi = "bilal@bilal.com",
                WebSitesi = "",
                VergiNo = "",
                FaxNo = "",
                KurumResimUrl = "",
                KurumVergiDairesiId = "testvergi",
                //TabloID = kurumkaydet.Result.Value.TabloID
            });
            Assert.IsFalse(kurumguncellenegative.Result.IsSuccess);

            //// Get Kurum
            var kurumveri = _helper.Get<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriGetir/" + kurumkaydet.Result.Value.TabloID);
            Assert.AreEqual(kurumveri.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumveri.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumveri.Result);

            // Get Kurum Negative
            var kurumverinegative = _helper.Get<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriGetir/" + 0);
            Assert.IsFalse(kurumverinegative.Result.IsSuccess);

            // Get KurumGetir - TemelBilgiler
            var kurumtemelveri = _helper.Get<Result<KurumTemelBilgiler>>("/api/KurumService/KurumGetir/" + kurumkaydet.Result.Value.TabloID);
            Assert.AreEqual(kurumtemelveri.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumtemelveri.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumtemelveri.Result);

            // kisi musteri temsilcisi mi
            var musteritemsilcisiMi = _helper.Get<Result<bool>>("/api/KurumService/KisiMusteriTemsilcisiMi/" + kurumtemelveri.Result.Value.KisiID);
            Assert.AreEqual(musteritemsilcisiMi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(musteritemsilcisiMi.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(musteritemsilcisiMi.Result);

            //KurumMusteriTemsilcisiGetir
            var kurumMusteriTemsilcisiGetir = _helper.Get<Result<List<GenelViewModel>>>("/api/KurumService/KurumMusteriTemsilcisiGetir");
            Assert.AreEqual(kurumMusteriTemsilcisiGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumMusteriTemsilcisiGetir.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumMusteriTemsilcisiGetir.Result);

            //MusteriTemsilcisiBagliKurumlarList
            var musteriTemsilcisiBagliKurumlar = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/MusteriTemsilcisiBagliKurumlarList/" + kurumtemelveri.Result.Value.KisiID);
            Assert.AreEqual(musteriTemsilcisiBagliKurumlar.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(musteriTemsilcisiBagliKurumlar.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(musteriTemsilcisiBagliKurumlar.Result);

            //AmirlereAstMusteriTemsilcisiKurumlariniGetir
            var amirlereAstMusteriTemsilcisiKurumlari = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/AmirlereAstMusteriTemsilcisiKurumlariniGetir/" + kurumtemelveri.Result.Value.KisiID);
            Assert.AreEqual(amirlereAstMusteriTemsilcisiKurumlari.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(amirlereAstMusteriTemsilcisiKurumlari.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(amirlereAstMusteriTemsilcisiKurumlari.Result);

            // HiyerarsiDisiKisilerKurumListesi
            var hiyerarsiDisiKisilerKurumListesi = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/HiyerarsiDisiKisilerKurumListesi");
            Assert.AreEqual(hiyerarsiDisiKisilerKurumListesi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(hiyerarsiDisiKisilerKurumListesi.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(hiyerarsiDisiKisilerKurumListesi.Result);

            // Get Kurum
            var kurumgetId = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/List/" + kurumkaydet.Result.Value.KurumId);
            Assert.AreEqual(kurumgetId.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumgetId.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumgetId.Result);

            // Get Kurum Negative
            var kurumgetIdnegative = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/List/" + 0);
            Assert.IsFalse(kurumgetIdnegative.Result.IsSuccess);

            // List
            var kurumlist = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/List");
            Assert.AreEqual(kurumlist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumlist.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumlist.Result);

            // ListKendisiIle
            var kurumlistKendisiIle = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/ListKendisiIle/" + kurumkaydet.Result.Value.TabloID);
            Assert.AreEqual(kurumlistKendisiIle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumlistKendisiIle.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumlistKendisiIle.Result);

            // ListKendisiIle2
            var ListKendisiIle2 = _helper.Get<Result<List<KurumTemelBilgiler>>>("/api/KurumService/ListKendisiIle/" + 0);
            Assert.IsNull(ListKendisiIle2.Result.Value);

            // List Ticari
            var kurumlist1 = _helper.Post<Result<List<KurumTemelBilgiler>>>("/api/KurumService/KurumList", kurumguncelle.Result.Value.KurumAdi);
            Assert.AreEqual(kurumlist1.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumlist1.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumlist1.Result.Value);

            // List Ticari Negative
            var kurumlist1negative = _helper.Post<Result<List<KurumTemelBilgiler>>>("/api/KurumService/KurumList", 0
            );
            Assert.IsFalse(kurumlist1negative.Result.IsSuccess);

            // Kurum Idari verileri getir.
            var sistemIdari = _helper.Get<Result<KurumIdariProfilModel>>("/api/KurumService/KurumIdariVerileriGetir/" + kurumguncelle.Result.Value.TabloID);
            Assert.AreEqual(sistemIdari.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(sistemIdari.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(sistemIdari.Result.Value);

            // Kurum Idari verileri getir.
            var sistemIdariNegative = _helper.Get<Result<KurumIdariProfilModel>>("/api/KurumService/KurumIdariVerileriGetir/" + 0);
            Assert.IsFalse(sistemIdariNegative.Result.IsSuccess);

            // Kurum Silindi
            var kurumsilindi = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + kurumguncelle.Result.Value.TabloID);
            Assert.AreEqual(kurumsilindi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumsilindi.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumsilindi.Result);

            // Kurum silindi negatif
            var kurumsilindinegative = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + 0);
            Assert.IsFalse(kurumsilindinegative.Result.IsSuccess);

            // Kurum Sil
            var harddelete = _helper.Get<Result<bool>>("/api/KurumService/KurumDelete/" + kurumguncelle.Result.Value.TabloID);
            Assert.AreEqual(harddelete.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(harddelete.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(harddelete.Result);
            //}
            //catch (Exception e)
            //{
            //    var harddelete = _helper.Get<Result<bool>>("/api/KurumService/KurumDelete/" + id1);
            //    throw;
            //}
        }

        #endregion KurumTemelTests

        #region Kurum İlişki

        /// <summary>
        /// Kurum İlişki Crud Test Methods
        /// </summary>
        [TestMethod()]
        public void KurumIliskiCrudTests() //public void KurumIliskiCrudTests()
        {
            //var kurumID1 = 0;
            //var kurumID2 = 0;
            //try
            //{
            //Add Bu Kurum
            var kurumkaydet = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriKaydet", new KurumTemelKayitModel
            {
                KurumAdi = "Kurum Servis Test B A.Ş" + Guid.NewGuid().ToString().Substring(0, 10),
                KurulusTarihi = DateTime.Now,
                TicaretSicilNo = "",
                VergiDairesiAdi = "",
                KurumLogo = 1,
                EpostaAdresi = "test@bilal.com",
                WebSitesi = "",
                VergiNo = "",
                FaxNo = "",
                KurumId = 82,
                KurumResimUrl = "",
                KurumVergiDairesiId = "testvergi 123",
                AdresListesi = new EditableList<KurumAdresModel>
                {
                    new KurumAdresModel
                    {
                        Adres = "",
                        LokasyonAdi = "",
                        LokasyonTipi = 1,
                        Sehir = 1,
                        SehirAdi = "",
                        Ulke = 1,
                        UlkeAdi = ""
                    }
                }
            });
            //kurumID1 = kurumkaydet.Result.Value.TabloID;
            //Add Bunun Kurum
            var kurumkaydet1 = _helper.Post<Result<KurumTemelKayitModel>>("/api/KurumService/KurumTemelVerileriKaydet", new KurumTemelKayitModel
            {
                KurumAdi = "Kurum Servis Test1 B A.Ş" + Guid.NewGuid().ToString().Substring(0, 10),
                KurulusTarihi = DateTime.Now,
                TicaretSicilNo = "",
                VergiDairesiAdi = "",
                KurumLogo = 1,
                EpostaAdresi = "test1@bilal.com",
                WebSitesi = "",
                VergiNo = "",
                FaxNo = "",
                KurumId = 82,
                KurumResimUrl = "",
                KurumVergiDairesiId = "testvergi 1234",
                AdresListesi = new EditableList<KurumAdresModel>
                {
                    new KurumAdresModel
                    {
                        Adres = "",
                        LokasyonAdi = "",
                        LokasyonTipi = 1,
                        Sehir = 1,
                        SehirAdi = "",
                        Ulke = 1,
                        UlkeAdi = ""
                    }
                }
            });
            //kurumID2 = kurumkaydet1.Result.Value.TabloID;
            // Add
            var kurumiliskikaydet = _helper.Post<Result<Iliskiler>>("/api/KurumService/KurumIliskiKaydet", new KurumIliskiKayitModel
            {
                GuncelleyenKisiID = 129,
                KurumID = kurumkaydet.Result.Value.TabloID,
                BuKurumID = kurumkaydet.Result.Value.TabloID,
                IliskiTuruID = 5,
                BununKurumID = kurumkaydet1.Result.Value.TabloID
            });
            Assert.AreEqual(kurumiliskikaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumiliskikaydet.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumiliskikaydet.Result.Value);

            // Add Olumsuz
            var kurumiliskikaydetOlumsuz = _helper.Post<Result<Iliskiler>>("/api/KurumService/KurumIliskiKaydet", new KurumIliskiKayitModel
            {
                GuncelleyenKisiID = 129,
                KurumID = 0,
                BuKurumID = kurumkaydet.Result.Value.TabloID,
                IliskiTuruID = 5,
                BununKurumID = 0
            });
            Assert.IsNull(kurumiliskikaydetOlumsuz.Result.Value);

            // Add Negative
            var kurumiliskikaydetnegative = _helper.Post<Result<Iliskiler>>("/api/KurumService/KurumIliskiKaydet", new KurumIliskiKayitModel
            {
            });
            Assert.IsFalse(kurumiliskikaydetnegative.Result.IsSuccess);

            // Update
            var iliskiguncelle = _helper.Post<Result<Iliskiler>>("/api/KurumService/KurumIliskiGuncelle", new KurumIliskiKayitModel
            {
                TabloID = kurumiliskikaydet.Result.Value.TabloID,
                KayitEdenID = 129,
                GuncelleyenKisiID = 129,
                KurumID = kurumkaydet1.Result.Value.TabloID,
                BuKurumID = kurumkaydet1.Result.Value.TabloID,
                IliskiTuruID = 4,
                BununKurumID = kurumkaydet.Result.Value.TabloID
            });
            Assert.AreEqual(iliskiguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(iliskiguncelle.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(iliskiguncelle.Result.Value);

            // Update negative
            var iliskiguncellenegative = _helper.Post<Result<Iliskiler>>("/api/KurumService/KurumIliskiGuncelle", new KurumIliskiKayitModel
            {
                //TabloID = kurumiliskikaydet.Result.Value.TabloID,
                KayitEdenID = 129,
                GuncelleyenKisiID = 129,
                KurumID = kurumkaydet1.Result.Value.TabloID,
                BuKurumID = kurumkaydet1.Result.Value.TabloID,
                IliskiTuruID = 4,
                BununKurumID = kurumkaydet.Result.Value.TabloID
            });
            Assert.IsFalse(iliskiguncellenegative.Result.IsSuccess);

            // List for kurumID
            var iliskilist = _helper.Get<Result<List<Iliskiler>>>("/api/KurumService/KurumIliskiList/" + kurumkaydet.Result.Value.KurumId);
            Assert.AreEqual(iliskilist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(iliskilist.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(iliskilist.Result);

            // List for kurumID Negative
            var iliskilistnegative = _helper.Get<Result<List<Iliskiler>>>("/api/KurumService/KurumIliskiList/" + 0);
            Assert.IsFalse(iliskilistnegative.Result.IsSuccess);

            // Delete
            var iliskisil = _helper.Get<Result<bool>>("/api/KurumService/KurumIliskiSil/" + kurumiliskikaydet.Result.Value.TabloID);
            Assert.AreEqual(iliskisil.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(iliskisil.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(iliskisil.Result.Value);

            // Delete Negative
            var iliskisilnegative = _helper.Get<Result<bool>>("/api/KurumService/KurumIliskiSil/" + 0);
            Assert.IsFalse(iliskisilnegative.Result.IsSuccess);

            // Kurum Silindi
            var kurumsil = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + kurumkaydet.Result.Value.TabloID);
            Assert.AreEqual(kurumsil.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumsil.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumsil.Result);

            // Kurum Silindi
            var kurumsil2 = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + kurumkaydet1.Result.Value.TabloID);
            Assert.AreEqual(kurumsil2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kurumsil2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kurumsil2.Result);
            //}
            //catch (Exception e)
            //{
            //    var rs1 = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + kurumID1);
            //    var rs2 = _helper.Get<Result<bool>>("/api/KurumService/TemelKurumSilindiYap/" + kurumID2);
            //}
        }

        #endregion Kurum İlişki
    }
}