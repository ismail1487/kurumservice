using Microsoft.VisualStudio.TestTools.UnitTesting;
using Baz.KurumServiceApi.Controllers;
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
    /// Kurum Organizasyon Birim Tanımlarının ve ağaç yapısının belirlendiği api kontrolün test sınıfıdır.
    /// </summary>
    [TestClass()]
    public class OrganizasyonBirimTests
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// Kurum Organizasyon Birim Tanımlarının ve ağaç yapısının belirlendiği api kontrolün test sınıfıdır.
        /// </summary>
        public OrganizasyonBirimTests()
        {
            _helper = TestServerRequestHelper.CreateHelper();
        }

        /// <summary>
        /// Kurum Organizasyon Birim Crud Test Methodu
        /// </summary>
        [TestMethod()]
        public void CrudTest()//public void CrudTest()
        {
            var birimTanım = "Kurum Servis Test Muhasebe1" + Guid.NewGuid().ToString().Substring(0, 10);
            // Add
            var add = _helper.Post<Result<int>>("/api/OrganizasyonBirim/Add", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                UstId = 0,
                Tanim = birimTanım,
                TipId = 1
            });
            Assert.AreEqual(add.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(add.Result);

            var addtekrar = _helper.Post<Result<int>>("/api/OrganizasyonBirim/Add", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                UstId = 0,
                Tanim = birimTanım,
                TipId = 1
            });
            Assert.IsFalse(addtekrar.Result.IsSuccess);

            var mod = new KurumOrganizasyonBirimView
            {
                TabloId = add.Result.Value,
                Tanim = "Kurum Servis Test xyz" + Guid.NewGuid().ToString().Substring(0, 10),
                TipId = 1,
                KurumId = 82
            };

            //id1 = add.Result.Value;
            // Update Name
            var updateName = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateName", mod);
            Assert.AreEqual(updateName.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(updateName.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(updateName.Result);

            // Update Name Negative
            var updateNamenegative = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateName", new KurumOrganizasyonBirimView
            {
                TabloId=0,
                Tanim="234"
            });
            Assert.IsFalse(updateNamenegative.Result.IsSuccess);


            mod.TabloId = -5;
              // Update Name Negative
            var updateNamenegative1 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateName",mod);
            Assert.IsFalse(updateNamenegative1.Result.IsSuccess);



            // Add Negative
            var addnegative = _helper.Post<Result<int>>("/api/OrganizasyonBirim/Add", new KurumOrganizasyonBirimView
            {
            });
            Assert.IsFalse(addnegative.Result.IsSuccess);

            // Add
            var add2 = _helper.Post<Result<int>>("/api/OrganizasyonBirim/Add", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                UstId = add.Result.Value,
                Tanim = "Test Muhasebe2" + Guid.NewGuid().ToString().Substring(0, 10),
                TipId = 1
            });
            Assert.AreEqual(add2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(add2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(add2.Result);
            //id2 = add2.Result.Value;
            // AddPoz
            var addPoz = _helper.Post<Result<int>>("/api/OrganizasyonBirim/AddPoz", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                IlgiliKurumID = 82,
                UstId = 0,
                Tanim = "Kurum Servis Test pozisyonu" + Guid.NewGuid().ToString().Substring(0, 10),
                TipId = 2
            });
            Assert.AreEqual(addPoz.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(addPoz.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(addPoz.Result);
            //id3 = addPoz.Result.Value;

            // AddPoz
            var AddPoz2Tanim = "Kurum Servis Test pozisyonu2" + Guid.NewGuid().ToString().Substring(0, 10);
            var addPoz2 = _helper.Post<Result<int>>("/api/OrganizasyonBirim/AddPoz", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                IlgiliKurumID = 82,
                UstId = addPoz.Result.Value,
                Tanim = AddPoz2Tanim,
                TipId = 2
            });
            //id4 = addPoz2.Result.Value;
            Assert.AreEqual(addPoz2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(addPoz2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(addPoz2.Result);
            // AddPoz negatice Negative
            var addPoznegative = _helper.Post<Result<int>>("/api/OrganizasyonBirim/AddPoz", new KurumOrganizasyonBirimView
            {
            });
            Assert.IsFalse(addPoznegative.Result.IsSuccess);

            // AddPoz negatice Negative
            var addPoznegative3 = _helper.Post<Result<int>>("/api/OrganizasyonBirim/AddPoz", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                IlgiliKurumID = 82,
                UstId = 0,
                Tanim = AddPoz2Tanim,
                TipId = 2
            });
            Assert.IsFalse(addPoznegative3.Result.IsSuccess);

            var getbyid =
                _helper.Get<Result<KurumOrganizasyonBirimTanimlari>>("/api/OrganizasyonBirim/TestGet/" +
                                                                     add.Result.Value);

            var getbyid2 =
                _helper.Get<Result<KurumOrganizasyonBirimTanimlari>>($"/api/OrganizasyonBirim/TestGet/" +
                                                                     add2.Result.Value);
            // List Kişi
            var Listkisi =
                _helper.Get<Result<List<KisiTemelBilgiler>>>("/api/OrganizasyonBirim/ListKisiId/" +
                                                             getbyid.Result.Value.OrganizasyonBirimTipiId);
            Assert.AreEqual(Listkisi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(Listkisi.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(Listkisi.Result);

            // List Kişi Negative
            var Listkisinegative =
                _helper.Get<Result<List<KisiTemelBilgiler>>>("/api/OrganizasyonBirim/ListKisiId/" +
                                                             0);
            Assert.IsFalse(Listkisinegative.Result.IsSuccess);

            var update1 = new KurumOrganizasyonBirimView
            {
                TabloId = add.Result.Value,
                KurumId = getbyid.Result.Value.KurumID,
                TipId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                UstId = getbyid.Result.Value.UstId.Value,
                Tanim = "Kurum Servis Root1" + Guid.NewGuid().ToString().Substring(0, 10)
            };

            //update for kurum
            var updateforkurum = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum", update1);
            Assert.AreEqual(updateforkurum.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(updateforkurum.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(updateforkurum.Result);


            var update2 = new KurumOrganizasyonBirimView
            {
                TabloId = -10,
                KurumId = getbyid.Result.Value.KurumID,
                TipId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                UstId = getbyid.Result.Value.UstId.Value,
                Tanim = update1.Tanim
            };

            var updateforkurum0 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum", update2);
            Assert.IsFalse(updateforkurum0.Result.IsSuccess);

            // Update Negative
            var updateforkurumnegative = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum", new KurumOrganizasyonBirimView
            {
                TabloId = 0
            });
            Assert.IsFalse(updateforkurumnegative.Result.IsSuccess);

            var updatekurum2 = new KurumOrganizasyonBirimView
            {
                TabloId = add.Result.Value,
                IlgiliKurumID = getbyid.Result.Value.IlgiliKurumId.Value,
                KurumId = getbyid.Result.Value.KurumID,
                TipId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                UstId = getbyid.Result.Value.UstId.Value,
                Tanim = "Kurum Servis Root1" + Guid.NewGuid().ToString().Substring(0, 10)
            };

            //updateforkurum2
            var updateforkurum2 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum2", updatekurum2) ;
            Assert.AreEqual(updateforkurum2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(updateforkurum2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(updateforkurum2.Result);

            var updatekurum3 = new KurumOrganizasyonBirimView
            {
                TabloId = -10,
                IlgiliKurumID = getbyid.Result.Value.IlgiliKurumId.Value,
                KurumId = getbyid.Result.Value.KurumID,
                TipId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                UstId = getbyid.Result.Value.UstId.Value,
                Tanim = updatekurum2.Tanim
            };

            //updateforkurum2
            var updateforkurum3 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum2", updatekurum3);
            Assert.IsFalse(updateforkurum3.Result.IsSuccess);

              var updateforkurum4 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum2", new KurumOrganizasyonBirimView
              {
                  TabloId = 0
              });
            Assert.IsFalse(updateforkurum4.Result.IsSuccess);

             var updateforkurum5 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum2", new KurumOrganizasyonBirimView
              {
                 TabloId = -1,
                 UstId = -1,
                 TipId=1,
                 KurumId=1,
                 Tanim="dsf"
             });
            Assert.IsFalse(updateforkurum5.Result.Value);


            // Update Negative
            var updateforkurumnegative2 = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum2", new KurumOrganizasyonBirimView
            {
                TabloId=0,
                Tanim=null
            });
            Assert.IsFalse(updateforkurumnegative2.Result.IsSuccess);

            //List Tip
            var listtip = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListTip", new KurumOrganizasyonBirimRequest
            {
                KurumId = getbyid.Result.Value.KurumID,
                Name = getbyid.Result.Value.BirimTanim,
                IlgiliKurumID = getbyid.Result.Value.KurumID
            });
            Assert.AreEqual(listtip.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(listtip.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(listtip.Result.Value);

            //List Tip Negative
            var listtipnegative = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListTip", new KurumOrganizasyonBirimRequest
            {
            });
            Assert.IsFalse(listtipnegative.Result.IsSuccess);

            // ListForKurum
            var listforkurum =
                _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForKurum", new KurumOrganizasyonBirimRequest
                {
                    KurumId = getbyid.Result.Value.KurumID,
                    IlgiliKurumID = getbyid.Result.Value.KurumID,
                    UstId = getbyid.Result.Value.UstId.Value
                });
            Assert.AreEqual(listforkurum.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(listforkurum.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(listforkurum.Result);
            // ListForKurum2
            var listforkurum2 =
                _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForKurum2", new KurumOrganizasyonBirimRequest
                {
                    KurumId = getbyid.Result.Value.KurumID,
                    IlgiliKurumID = getbyid.Result.Value.KurumID,
                    UstId = getbyid.Result.Value.UstId.Value
                });
            Assert.AreEqual(listforkurum2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(listforkurum2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(listforkurum2.Result);

            // ListForKurum2 olumsuz
            var listforkurum2olumsuz =
                _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForKurum2", new KurumOrganizasyonBirimRequest
                {
                    KurumId = getbyid.Result.Value.KurumID,
                    IlgiliKurumID = 0,
                    UstId = getbyid.Result.Value.UstId.Value
                });
            Assert.IsNull(listforkurum2olumsuz.Result.Value);

            //ListKurumOrganizasyonIdileKisiListele
            var listOrgIdileKisiId = _helper.Post<Result<List<int>>>("/api/KurumService/ListKurumOrganizasyonIdileKisiListele", listforkurum.Result.Value.Select(a => a.TabloId));
            Assert.AreEqual(listOrgIdileKisiId.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(listOrgIdileKisiId.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(listOrgIdileKisiId.Result);

            // ListForKurum Negative
            var listforkurumnegative =
                _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForKurum", new KurumOrganizasyonBirimRequest
                {
                });
            Assert.IsFalse(listforkurumnegative.Result.IsSuccess);

            ////Get Tree
            //var gettree = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree",
            //    new KurumOrganizasyonBirimRequest
            //    {
            //        KurumId = getbyid.Result.Value.KurumID,
            //        IlgiliKurumID = getbyid.Result.Value.KurumID,
            //        UstId = getbyid.Result.Value.UstId.Value
            //    });
            //Assert.AreEqual(gettree.Result.StatusCode, (int)ResultStatusCode.Success);
            //Assert.AreEqual(gettree.StatusCode, HttpStatusCode.OK);
            //Assert.IsNotNull(gettree.Result);

            ////Get Tree Negative
            //var gettreenegative = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree",
            //    new KurumOrganizasyonBirimRequest
            //    {
            //    });
            //Assert.IsFalse(gettreenegative.Result.IsSuccess);

            //Get Tree2
            //var gettree2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree",
            //    new KurumOrganizasyonBirimRequest
            //    {
            //        KurumId = getbyid.Result.Value.KurumID,
            //        IlgiliKurumID = getbyid.Result.Value.KurumID,
            //        UstId = getbyid.Result.Value.UstId.Value
            //    });
            //Assert.AreEqual(gettree2.Result.StatusCode, (int)ResultStatusCode.Success);
            //Assert.AreEqual(gettree2.StatusCode, HttpStatusCode.OK);
            //Assert.IsNotNull(gettree2.Result);

            ////Get Tree Negative
            //var gettreenegative2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree",
            //    new KurumOrganizasyonBirimRequest
            //    {
            //    });
            //Assert.IsFalse(gettreenegative2.Result.IsSuccess);

            //Get Tree 2
            var gettreeSec = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree2",
                new KurumOrganizasyonBirimRequest
                {
                    IlgiliKurumID = getbyid.Result.Value.IlgiliKurumId.Value,
                    UstId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                    KurumId = getbyid.Result.Value.KurumID
                });
            Assert.AreEqual(gettreeSec.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(gettreeSec.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(gettreeSec.Result);

            //Get Tree 2 Negative
            var gettreSecenegative = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree2",
                new KurumOrganizasyonBirimRequest
                {
                });
            Assert.IsFalse(gettreSecenegative.Result.IsSuccess);

            //Get Tree 2 2
            var gettreeSec2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree2",
                new KurumOrganizasyonBirimRequest
                {
                    IlgiliKurumID = getbyid.Result.Value.IlgiliKurumId.Value,
                    UstId = getbyid.Result.Value.OrganizasyonBirimTipiId.Value,
                    KurumId = getbyid.Result.Value.KurumID
                });
            Assert.AreEqual(gettreeSec2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(gettreeSec2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(gettreeSec2.Result);

            //Get Tree 2 Negative
            var gettreeSecnegative2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/GetTree2",
                new KurumOrganizasyonBirimRequest
                {
                });
            Assert.IsFalse(gettreeSecnegative2.Result.IsSuccess);

            // List For Level
            var listforlevel = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForLevel", new KurumOrganizasyonBirimRequest
            {
                KurumId = getbyid2.Result.Value.KurumID,
                UstId = getbyid2.Result.Value.UstId.Value,
                IlgiliKurumID = getbyid.Result.Value.KurumID
            });
            Assert.AreEqual(listforlevel.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(listforlevel.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(listforlevel.Result);

            // List For Level Negative
            var listforlevelnegative = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForLevel", new KurumOrganizasyonBirimRequest
            {
            });
            Assert.IsFalse(listforlevelnegative.Result.IsSuccess);

            // List For Level2
            var listforlevel2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForLevel2", new KurumOrganizasyonBirimRequest
            {
                KurumId = getbyid.Result.Value.KurumID,
                IlgiliKurumID = getbyid2.Result.Value.IlgiliKurumId.Value,
                UstId = getbyid2.Result.Value.UstId.Value
            });
            Assert.AreEqual(listforlevel2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(listforlevel2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(listforlevel2.Result);

            // List For Level Negative
            var listforlevelnegative2 = _helper.Post<Result<List<KurumOrganizasyonBirimView>>>("/api/OrganizasyonBirim/ListForLevel2", new KurumOrganizasyonBirimRequest
            {
            });
            Assert.IsFalse(listforlevelnegative2.Result.IsSuccess);

            //MesajmodülüIzinVerilmeyenPozisyonKaydi
            var templist = listforkurum.Result.Value.Select(a => a.TabloId).ToList();
            templist.Add(listforkurum.Result.Value.Select(a => a.TabloId).FirstOrDefault());
            var izinVerilmeyenPozisyonKaydi = _helper.Post<Result<bool>>("/api/KurumService/MessageModuleIzinVerilmeyenPozisyonKaydi", new KurumMesajlasmaModuluIzinIslemleriViewModel
            {
                IzinVerilmeyenBirimIDleri = templist
            });
            Assert.AreEqual(izinVerilmeyenPozisyonKaydi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(izinVerilmeyenPozisyonKaydi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(izinVerilmeyenPozisyonKaydi.Result);

            //MessageModuleListForView
            var messageModuleIzınVerilmeyenlist = _helper.Get<Result<List<KurumMesajlasmaModuluIzinIslemleriViewModel>>>("/api/KurumService/MessageModuleListForView");
            Assert.AreEqual(messageModuleIzınVerilmeyenlist.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(messageModuleIzınVerilmeyenlist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(messageModuleIzınVerilmeyenlist.Result);
            //idList1 = messageModuleIzınVerilmeyenlist.Result.Value.Select(a => a.TabloID).ToList();
            //IzinVerilmeyenPozisyonlarIdList
            var izinVerilmeyenPozisyonlarIdList = _helper.Get<Result<List<int>>>("/api/KurumService/IzinVerilmeyenPozisyonlarIdList");
            Assert.AreEqual(izinVerilmeyenPozisyonlarIdList.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(izinVerilmeyenPozisyonlarIdList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(izinVerilmeyenPozisyonlarIdList.Result);

            // Add lokasyon
            var addlokasyon = _helper.Post<Result<int>>("/api/OrganizasyonBirim/Add", new KurumOrganizasyonBirimView
            {
                KurumId = 82,
                UstId = 0,
                Tanim = "Kurum Servis lokasyon" + Guid.NewGuid().ToString().Substring(0, 10),
                TipId = 4,
                Koordinat = "34.454/54.345" + Guid.NewGuid().ToString().Substring(0, 10)
            });
            Assert.AreEqual(addlokasyon.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(addlokasyon.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(addlokasyon.Result);
            //id5 = addlokasyon.Result.Value;
            //update for kurum lokasyon
            var updateforkurumlokasyon = _helper.Post<Result<bool>>("/api/OrganizasyonBirim/UpdateForKurum", new KurumOrganizasyonBirimView
            {
                TabloId = addlokasyon.Result.Value,
                KurumId = getbyid.Result.Value.KurumID,
                Tanim = "Kurum Servis lokasyon update" + Guid.NewGuid().ToString().Substring(0, 10),
                Koordinat = "34.754/54.7457" + Guid.NewGuid().ToString().Substring(0, 10),
                TipId = 4
            });
            Assert.AreEqual(updateforkurumlokasyon.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(updateforkurumlokasyon.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(updateforkurumlokasyon.Result);

            // Delete lokasyon
            var deletelokasyon = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + addlokasyon.Result.Value);
            Assert.AreEqual(deletelokasyon.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(deletelokasyon.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(deletelokasyon.IsSuccess);

            // Delete
            var delete = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + add.Result.Value);
            Assert.AreEqual(delete.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(delete.IsSuccess);

            // Delete
            var delete2 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + add2.Result.Value);
            Assert.AreEqual(delete2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(delete2.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(delete2.IsSuccess);

            // Delete Negative
            var deletenegative = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + 0);
            Assert.IsTrue(deletenegative.IsSuccess);

            // Delete Poz
            var deletePoz = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + addPoz.Result.Value);
            Assert.AreEqual(deletePoz.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(deletePoz.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(deletePoz.IsSuccess);

            // Delete Poz2
            var deletePoz2 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + addPoz2.Result.Value);
            Assert.AreEqual(deletePoz2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(deletePoz2.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(deletePoz2.IsSuccess);

            //MessageModuleDeleteRecord
            foreach (var id in messageModuleIzınVerilmeyenlist.Result.Value.Select(a => a.TabloID))
            {
                var messageModuleDeleteIzinRecord = _helper.Get<Result<bool>>("/api/KurumService/MessageModuleDeleteRecord/" + id);
                Assert.AreEqual(messageModuleDeleteIzinRecord.StatusCode, HttpStatusCode.OK);
                Assert.AreEqual(messageModuleDeleteIzinRecord.Result.StatusCode, (int)ResultStatusCode.Success);
                Assert.IsTrue(messageModuleDeleteIzinRecord.Result.Value);
            }

            var messageModuleDeleteIzinRecordNegative = _helper.Get<Result<bool>>("/api/KurumService/MessageModuleDeleteRecord/" + 0);
            Assert.AreEqual(messageModuleDeleteIzinRecordNegative.StatusCode, HttpStatusCode.OK);
            Assert.IsFalse(messageModuleDeleteIzinRecordNegative.Result.Value);
            //}
            //catch (Exception e)
            //{
            //    var delete1 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + id1);
            //    var delete2 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + id2);
            //    var delete3 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + id3);
            //    var delete4 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + id4);
            //    var delete5 = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/Delete/" + id5);
            //    foreach (var id in idList1)
            //    {
            //        var delete6 = _helper.Get<Result<bool>>("/api/KurumService/MessageModuleDeleteRecord/" + id);
            //    }
            //}
        }

        /// <summary>
        /// Kurum Organizasyon Birim Crud Test Methodu
        /// </summary>
        [TestMethod()]
        public void AdminMiTest()//public void CrudTest()
        {
            //Admin Mi
            var adminMi = _helper.Get<Result<bool>>("/api/OrganizasyonBirim/AdminMi");
            Assert.IsNotNull(adminMi.Result);
        }
    }
}