//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Baz.KurumServiceApi.Controllers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Baz.Model.Entity;
//using Baz.Model.Entity.Constants;
//using Baz.Model.Entity.ViewModel;
//using Baz.ProcessResult;
//using Baz.RequestManager;
//using Baz.RequestManager.Abstracts;
//using KurumServiceTest.Helper;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace KurumServiceTest
//{
//    /// <summary>
//    /// ParametreTipi
//    /// 1-Metin
//    /// 2-Seçim
//    /// 3-Dosya
//    ///
//    /// ParametreGosterimTipi
//    /// 1-Textbox
//    /// 2-TextArea
//    /// 3-NumericText
//    /// 4-Datepicker
//    /// 5-Dropdown
//    /// 6-Listbox
//    /// 7-Radiobox
//    /// 8-Checkbox
//    /// 9-Filepicker
//    ///
//    /// İlgili Tablolar
//    /// KurumEkParametreler
//    /// KurumEkParametreOlasiDegerler
//    /// KurumEkParametreGerceklesenler
//    ///
//    /// Kayıt edilecek dinamik değerler KurumEkParametreler tablosuna kayıt edilir.
//    /// Burada hangi  formatta gösterileceği seçilir.
//    /// Eğer seçim parametresi ise KurumEkParametreOlasiDegerler tablosuna seçilebilir değerler ilgili parametre ile kayıt edilir.
//    /// Kurum kayıt sayfasında belirlenen değerler KurumEkParametreGerceklesenler tablosuna kayıt edilir.
//    ///
//    /// </summary>

//    [TestClass()]
//    public class KurumEkParametreTests
//    {
//        private readonly IRequestHelper _helper;

//        /// <summary>
//        /// Ek parametre test classı
//        /// </summary>
//        public KurumEkParametreTests()
//        {
//            _helper = TestServerRequestHelper.CreateHelper();
//        }

//        #region EkParametrelerTests

//        /// <summary>
//        /// Ek parametreler test metodu
//        /// </summary>
//        [TestMethod]
//        public void EkParametrelerTests() //public void EkParametrelerTests()
//        {
//            //Assert
//            //var kurumekparamkaydet = _helper.Post<Result<KurumEkParametreler>>("/api/KurumEkParametre/KurumEkParametreKaydet", new KurumEkParametreViewModel
//            //{
//            //    ParametreAdi = "Kurum Servis Unit Test" + Guid.NewGuid().ToString().Substring(0, 10),
//            //    ParametreGosterimTipi = 2,
//            //    ParametreTipi = 1,
//            //});
//            //Assert.AreEqual(kurumekparamkaydet.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(kurumekparamkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(kurumekparamkaydet.Result);

//            //Assert EkParametreListesi
//            var ekparamlist = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesi");
//            Assert.AreEqual(ekparamlist.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(ekparamlist.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(ekparamlist.Result);

//            //Assert ekparamlistID
//            var ekparamlistID = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesiID/" + kurumekparamkaydet.Result.Value.TabloID);
//            Assert.AreEqual(ekparamlistID.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(ekparamlistID.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(ekparamlistID.Result);

//            //Assert ekparamlistID negative
//            var ekparamlistIDnegative = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesiID/" + 0);
//            Assert.IsNull(ekparamlistIDnegative.Result.Value);

//            //Assert
//            var ekparamlistKurumID = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesi/" + 82);
//            Assert.AreEqual(ekparamlistKurumID.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(ekparamlistKurumID.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(ekparamlistKurumID.Result);

//            //Assert EkParametreListesi negative
//            var ekparamlistnegative = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesi/" + 0);
//            Assert.IsNull(ekparamlistnegative.Result.Value);

//            //Assert
//            //var kurumekparamkaydetolumsuz = _helper.Post<Result<KurumEkParametreler>>("/api/KurumEkParametre/KurumEkParametreKaydet", new KurumEkParametreViewModel
//            //{
//            //    ParametreAdi = "YeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeni",
//            //    ParametreGosterimTipi = 2,
//            //    ParametreTipi = 1
//            //}
//            //);
//            //Assert.IsNull(kurumekparamkaydetolumsuz.Result.Value);

//            ////Assert
//            //var kurumekparamlist = _helper.Get<Result<List<KurumEkParametreler>>>("/api/KurumEkParametre/KurumEkParametreList");
//            //Assert.AreEqual(kurumekparamlist.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(kurumekparamlist.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(kurumekparamlist.Result);

//            var updateModel = new KurumEkParametreViewModel
//            {
//                TabloID = kurumekparamkaydet.Result.Value.TabloID,
//                ParametreAdi = "Kurum Servis Unit Test Güncel" + Guid.NewGuid().ToString().Substring(0, 10),
//                ParametreGosterimTipi = 2,
//                ParametreTipi = 1
//            };
//            //Assert
//            //var kurumekparamguncelle = _helper.Post<Result<KurumEkParametreler>>("/api/KurumEkParametre/KurumEkParametreGuncelle", updateModel);
//            //Assert.AreEqual(kurumekparamguncelle.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(kurumekparamguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(kurumekparamguncelle.Result);

//            ////Assert
//            //var kurumekparamguncelleolumsuz = _helper.Post<Result<KurumEkParametreler>>("/api/KurumEkParametre/KurumEkParametreGuncelle", new KurumEkParametreViewModel
//            //{
//            //    TabloID = kurumekparamkaydet.Result.Value.TabloID,
//            //    ParametreAdi = "YeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeni",
//            //    ParametreGosterimTipi = 2,
//            //    ParametreTipi = 1
//            //}
//            //);
//            //Assert.IsNull(kurumekparamguncelleolumsuz.Result.Value);

//            //Assert
//            // Kurum Ek Parametre Gerçekleşenlerde kaydı yok ise silme işlemi gerçekleştirilir.
//            var kurumekparamsil2 = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParametreSil/" + kurumekparamkaydet.Result.Value.TabloID);
//            Assert.AreEqual(kurumekparamsil2.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(kurumekparamsil2.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsTrue(kurumekparamsil2.Result.Value);

//            //Assert
//            //var kurumekparamkaydet2 = _helper.Post<Result<KurumEkParametreler>>("/api/KurumEkParametre/KurumEkParametreKaydet", new KurumEkParametreViewModel
//            //{
//            //    ParametreAdi = "Kurum Servis Unit Test Gerçekleşen" + Guid.NewGuid().ToString().Substring(0, 10),
//            //    ParametreGosterimTipi = 2,
//            //    ParametreTipi = 1
//            //});
//            //Assert.AreEqual(kurumekparamkaydet2.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(kurumekparamkaydet2.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(kurumekparamkaydet2.Result);

//            //Assert
//            //var ekparamDegerkayit = _helper.Post<Result<bool>>("/api/KurumEkParametre/EkParametreKayit", new KurumEkParametreKayitViewModel
//            //{
//            //    IlgiliKurumID = 82,
//            //    KisiID = 129,
//            //    KurumID = 82,
//            //    Degerler = new List<KurumEkParametreKayitParametreViewModel>
//            //    {
//            //        new KurumEkParametreKayitParametreViewModel
//            //        {
//            //            Deger = "Kurum Servis Test Gerçekleşen Değer"  + Guid.NewGuid().ToString().Substring(0, 10),
//            //            ParametreID = kurumekparamkaydet2.Result.Value.TabloID
//            //        }
//            //    }
//            //});
//            //Assert.AreEqual(ekparamDegerkayit.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(ekparamDegerkayit.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsTrue(ekparamDegerkayit.Result.Value);

//            //Assert Parametre Id'ye göre kişi ek parametre gerçekleşenleri listeleyen test metodu
//            //var gerceklesenList = _helper.Get<Result<List<KisiEkParametreGerceklesenler>>>("/api/KurumEkParametre/GerceklesenList/" + kurumekparamkaydet2.Result.Value.TabloID);
//            //Assert.AreEqual(gerceklesenList.StatusCode, HttpStatusCode.OK);
//            //Assert.IsNotNull(gerceklesenList.Result.Value);

//            ////Assert Parametre Id'ye göre kişi ek parametre gerçekleşenleri listeleyen test metodu olumsuz
//            //var gerceklesenListOlumsuz = _helper.Get<Result<List<KisiEkParametreGerceklesenler>>>("/api/KurumEkParametre/GerceklesenList/" + 0);
//            //Assert.IsNull(gerceklesenListOlumsuz.Result.Value);

//            //Assert
//            // Kurum Ek Parametre Gerçekleşenlerde kaydı var ise silme işlemi gerçekleştirilmez!.
//            //var kurumekparamsil = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParametreSil/" + kurumekparamkaydet2.Result.Value.TabloID);
//            //Assert.IsFalse(kurumekparamsil.Result.Value);

//            ////HardDelete
//            //var kurumekparamsil3 = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParamHardDelete/" + kurumekparamkaydet2.Result.Value.TabloID);
//            //Assert.AreEqual(kurumekparamsil3.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsTrue(kurumekparamsil3.Result.Value);

//            //HardDelete Olumsuz
//            var kurumekparamsil3olumsuz = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParamHardDelete/" + 0);
//            Assert.IsFalse(kurumekparamsil3olumsuz.Result.Value);
//        }

//        #endregion EkParametrelerTests

//        #region OlasiDegerlerTests

//        /// <summary>
//        /// Olası değerler test metodu
//        /// </summary>
//        [TestMethod()]
//        public void KurumEkParametreOlasiDegerlerKaydetTest()
//        {
//            var saveModel = new KurumEkParametreViewModel
//            {
//                ParametreAdi = "Kurum Servis Unit Test Dropdown" + Guid.NewGuid().ToString().Substring(0, 10),
//                ParametreGosterimTipi = 5,
//                ParametreTipi = 2,
//            };
//            //Olası degerler icin öncellikle bir dropdown Ek Paramter Tipi tanımlanması gerekmektedir.
//            var kurumekparamkaydet = _helper.Post<Result<KurumEkParametreViewModel>>("/api/KurumEkParametre/KurumEkParametreKaydet", saveModel);
//            Assert.AreEqual(kurumekparamkaydet.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(kurumekparamkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(kurumekparamkaydet.Result);

//            //Olası degerler icin öncellikle bir dropdown Ek Paramter Tipi tanımlanması gerekmektedir.
//            var kurumekparamkaydet2 = _helper.Post<Result<KurumEkParametreViewModel>>("/api/KurumEkParametre/KurumEkParametreKaydet", saveModel);
//            Assert.IsNull(kurumekparamkaydet2.Result.Value);

//            //Assert
//            var kurumekparamolasidegerkaydet = _helper.Post<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerKaydet", new KurumEkParametreViewModel
//            {
//                TabloID = kurumekparamkaydet.Result.Value.TabloID,
//                ParametreAdi = kurumekparamkaydet.Result.Value.ParametreAdi,
//                ParametreGosterimTipi = kurumekparamkaydet.Result.Value.ParametreGosterimTipi,
//                ParametreTipi = kurumekparamkaydet.Result.Value.ParametreTipi,
//                OlasiDegerler = new List<KurumEkParametreOlasiDegerViewModel>
//                {
//                    new KurumEkParametreOlasiDegerViewModel
//                    {
//                        OlasiDegerAdi = "Kurum Servis Olasi Test Deger"  + Guid.NewGuid().ToString().Substring(0, 10)
//                    }
//                }
//            });
//            Assert.AreEqual(kurumekparamolasidegerkaydet.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(kurumekparamolasidegerkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(kurumekparamolasidegerkaydet.Result);

//            //Assert ekparamlistID
//            var ekparamlistID = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesiID/" + kurumekparamkaydet.Result.Value.TabloID);
//            Assert.AreEqual(ekparamlistID.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(ekparamlistID.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(ekparamlistID.Result);

//            //Assert
//            var ekparamlistKurumID = _helper.Get<Result<List<KurumEkParametreViewModel>>>("/api/KurumEkParametre/EkParametreListesi/" + 82);
//            Assert.AreEqual(ekparamlistKurumID.StatusCode, HttpStatusCode.OK);
//            Assert.AreEqual(ekparamlistKurumID.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsNotNull(ekparamlistKurumID.Result);

//            //Assert
//            //var kurumekparamolasidegerkaydetolumsuz = _helper.Post<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerKaydet", new KurumEkParametreViewModel
//            //{
//            //    TabloID = kurumekparamkaydet.Result.Value.TabloID,
//            //    ParametreAdi = kurumekparamkaydet.Result.Value.ParametreAdi,
//            //    ParametreGosterimTipi = kurumekparamkaydet.Result.Value.ParametreGosterimTipi,
//            //    ParametreTipi = kurumekparamkaydet.Result.Value.ParametreTipi,
//            //    OlasiDegerler = new List<KurumEkParametreOlasiDegerViewModel>
//            //    {
//            //        new KurumEkParametreOlasiDegerViewModel
//            //        {
//            //            OlasiDegerAdi = "YeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeni"
//            //        }
//            //    }
//            ////});
//            ////Assert.IsNull(kurumekparamolasidegerkaydetolumsuz.Result.Value);

//            //////Assert
//            ////var kurumekparamolasidegerguncelle = _helper.Post<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerGuncelle", new KurumEkParametreViewModel
//            ////{
//            ////    TabloID = kurumekparamkaydet.Result.Value.TabloID,
//            ////    ParametreAdi = kurumekparamkaydet.Result.Value.ParametreAdi,
//            ////    ParametreGosterimTipi = kurumekparamkaydet.Result.Value.ParametreGosterimTipi,
//            ////    ParametreTipi = kurumekparamkaydet.Result.Value.ParametreTipi,
//            ////    OlasiDegerler = new List<KurumEkParametreOlasiDegerViewModel>
//            ////    {
//            ////        new KurumEkParametreOlasiDegerViewModel
//            ////        {
//            ////            TabloID =  kurumekparamolasidegerkaydet.Result.Value.ToArray()[0].TabloID,
//            ////            OlasiDegerAdi = "Kurum Servis Güncel Değer"  + Guid.NewGuid().ToString().Substring(0, 10)
//            ////        },
//            ////        new KurumEkParametreOlasiDegerViewModel
//            ////        {
//            ////            OlasiDegerAdi = "Kurum Servis Güncel Değer"  + Guid.NewGuid().ToString().Substring(0, 10)
//            ////        }
//            ////    }
//            ////});
//            //Assert.AreEqual(kurumekparamolasidegerguncelle.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(kurumekparamolasidegerguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(kurumekparamolasidegerguncelle.Result);

//            //Assert olasi Degerler List
//            //var olasidegerlerList = _helper.Get<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/olasidegerler/" + kurumekparamkaydet.Result.Value.TabloID);
//            //Assert.AreEqual(olasidegerlerList.StatusCode, HttpStatusCode.OK);
//            //Assert.AreEqual(olasidegerlerList.Result.StatusCode, (int)ResultStatusCode.Success);
//            //Assert.IsNotNull(olasidegerlerList.Result.Value);
//            ////Assert olasi Degerler List
//            //var olasidegerlerListNegative = _helper.Get<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/olasidegerler/" + 0);
//            //Assert.IsNull(olasidegerlerListNegative.Result.Value);

//            ////Assert
//            //var kurumekparamolasidegerguncelleolumsuz = _helper.Post<Result<List<KurumEkParametreOlasiDegerler>>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerGuncelle", new KurumEkParametreViewModel
//            //{
//            //    TabloID = kurumekparamkaydet.Result.Value.TabloID,
//            //    ParametreAdi = kurumekparamkaydet.Result.Value.ParametreAdi,
//            //    ParametreGosterimTipi = kurumekparamkaydet.Result.Value.ParametreGosterimTipi,
//            //    ParametreTipi = kurumekparamkaydet.Result.Value.ParametreTipi,
//            //    OlasiDegerler = new List<KurumEkParametreOlasiDegerViewModel>
//            //    {
//            //        new KurumEkParametreOlasiDegerViewModel
//            //        {
//            //            TabloID =  kurumekparamolasidegerkaydet.Result.Value.ToArray()[0].KurumEkParametreID,
//            //            OlasiDegerAdi = "YeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeniYeni"
//            //        }
//            //    }
//            //});
//            //Assert.IsNull(kurumekparamolasidegerguncelleolumsuz.Result.Value);

//            //HardDelete
//            var kurumekparamsil = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParamHardDelete/" + kurumekparamkaydet.Result.Value.TabloID);
//            Assert.AreEqual(kurumekparamsil.Result.StatusCode, (int)ResultStatusCode.Success);
//            Assert.IsTrue(kurumekparamsil.Result.Value);
//            //Assert
//            // Var olan Olasi degerleride silmektedir.
//            var kurumekparamolasidegersil = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerSil/" + kurumekparamolasidegerkaydet.Result.Value.FirstOrDefault().TabloID);
//            Assert.AreEqual(kurumekparamolasidegersil.StatusCode, HttpStatusCode.OK);
//            Assert.IsTrue(kurumekparamolasidegersil.Result.Value);

//            //Assert
//            // Var olan Olasi degerleride silmektedir.
//            var kurumekparamolasidegersilNegative = _helper.Get<Result<bool>>("/api/KurumEkParametre/KurumEkParametreOlasiDegerlerSil/" + 0);
//            Assert.AreEqual(kurumekparamolasidegersilNegative.StatusCode, HttpStatusCode.OK);
//            Assert.IsFalse(kurumekparamolasidegersilNegative.Result.Value);
//        }

//        #endregion OlasiDegerlerTests
//    }
//}