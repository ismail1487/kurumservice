using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Baz.Service.Helper.ExpressionUtils;

namespace KurumServiceTest
{
    /// <summary>
    /// Hedef kitle test classı
    /// </summary>
    [TestClass()]
    public class HedefKitleTests
    {
        private readonly IRequestHelper _helper;
        private readonly IRequestHelper _helper2;
        private readonly IRequestHelper _helper3;

        /// <summary>
        /// Hedef kitle test classı yapıcı metodu
        /// </summary>
        public HedefKitleTests()
        {
            _helper = TestServerRequestHelper.CreateHelper();
            _helper2 = TestServerRequestHelper2.CreateHelper();
            _helper3 = TestServerRequestHelper3.CreateHelper();
        }

        /// <summary>
        /// Hedef Kitle Tasarım Kurum Crud Testleridir.
        /// </summary>
        [TestMethod()]
        public void HedefKitleCrudTest() //public void HedefKitleCrudTest()
        {
            //Assert-5 GetListTest3
            var getList3 = _helper3.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetList");
            Assert.AreEqual(getList3.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getList3.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getList3.Result);

            //Assert-5 GetListTest2
            var getList2 = _helper2.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetList");
            Assert.AreEqual(getList2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getList2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getList2.Result);

            //Assert-1 Add kurum2
            var add2 = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                Filters = new List<HedefKitleFilter>()
                {
                    new()
                    {
                        LocigalOperator="OR",Operator=null,MemberName=null,Value=null,FilterType="group",FieldType=null,InputType=null,ParametreTableName=null,EkParametreId=0,FieldName=null,TableName=null
                    },
                    new()
                    {
                       LocigalOperator="AND",Operator="Contains",MemberName="Kurum Kısa Adı",Value="Orsa",FilterType="expression",FieldType="System.String",InputType="text",ParametreTableName=null,EkParametreId=0,FieldName="KurumKisaUnvan",TableName=null
                    },
                    new()
                    {
                       LocigalOperator="AND",Operator="DoesNotContain",MemberName="Kurum Kısa Adı",Value="Maya",FilterType="expression",FieldType="System.String",InputType="text",ParametreTableName=null,EkParametreId=0,FieldName="KurumKisaUnvan",TableName=null
                    }
                },
                Tanim = "Kurum Servis Unit Test",
                KisiId = 130,
                KurumId = 82,
                HedefKitleTipi = "kurum"
            });
            Assert.AreEqual(add2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(add2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(add2.Result);

            //Assert-1 Add kurum
            var add = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                Filters = new List<HedefKitleFilter>()
                {
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = null,
                        MemberName = null,
                        Value = null,
                        FilterType = "group",
                        FieldType = null,
                        InputType = null,
                        ParametreTableName = null,
                        EkParametreId = 0,
                        FieldName = null
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsGreaterThanOrEqualTo",
                        MemberName = "Şube Sayısı",
                        Value = "3",
                        FilterType = "expression",
                        FieldType = "System.Int32",
                        InputType = "number",
                        ParametreTableName = null,
                        TableName = "KurumEkParametreGerceklesenler",
                        EkParametreId = 44,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsGreaterThan",
                        MemberName = "Şube Sayısı",
                        Value = "0",
                        FilterType = "expression",
                        FieldType = "System.Int32",
                        InputType = "number",
                        ParametreTableName = null,
                        TableName = "KurumEkParametreGerceklesenler",
                        EkParametreId = 44,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsNotEqualTo",
                        MemberName = "Şube Sayısı",
                        Value = "0",
                        FilterType = "expression",
                        FieldType = "System.Int32",
                        InputType = "number",
                        ParametreTableName = null,
                        TableName = "KurumEkParametreGerceklesenler",
                        EkParametreId = 44,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsLessThanOrEqualTo",
                        MemberName = "Şube Sayısı",
                        Value = "150",
                        FilterType = "expression",
                        FieldType = "System.Int32",
                        InputType = "number",
                        ParametreTableName = null,
                        TableName = "KurumEkParametreGerceklesenler",
                        EkParametreId = 44,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "Contains",
                        MemberName = "Kurum Kısa Adı",
                        Value = "Orsa",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "text",
                        ParametreTableName = null,
                        TableName = "KurumTemelBilgiler",
                        EkParametreId = 0,
                        FieldName = "KurumKisaUnvan"
                    }
                },
                Tanim = "Kurum Servis Unit Test",
                KisiId = 130,
                KurumId = 82,
                HedefKitleTipi = "kurum"
            });
            Assert.AreEqual(add.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(add.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(add.Result);

            //Assert-1.2 Add kisi
            var addKisi = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                Filters = new List<HedefKitleFilter>()
                {
                    new()
                    {
                        LocigalOperator = "OR",
                        Operator = null,
                        MemberName = null,
                        Value = null,
                        FilterType = "group",
                        FieldType = null,
                        InputType = null,
                        ParametreTableName = null,
                        EkParametreId = 0,
                        FieldName = null
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "Contains",
                        MemberName = "Adı",
                        Value = "Bilal",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "text",
                        ParametreTableName = null,
                        TableName = null,
                        EkParametreId = 0,
                        FieldName = "KisiAdi"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "DoesNotContain",
                        MemberName = "Adı",
                        Value = "X",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "text",
                        ParametreTableName = null,
                        TableName = null,
                        EkParametreId = 0,
                        FieldName = "KisiAdi"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsEqualTo",
                        MemberName = "Kan Grubu",
                        Value = "35",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "select",
                        ParametreTableName = null,
                        TableName = null,
                        EkParametreId =19,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "IsGreaterThanOrEqualTo",
                        MemberName= "Pasaport Bitiş Tarihi",
                        Value= "2021-06-25T00:00:00+03:00",
                        FilterType= "expression",
                        FieldType= "System.DateTime",
                        InputType= "date",
                        ParametreTableName= null,
                        EkParametreId= 21,
                        FieldName= "Deger",
                        TableName= "KisiEkParametreGerceklesenler"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "IsLessThan",
                        MemberName= "Pasaport Bitiş Tarihi",
                        Value= "2121-06-25T00:00:00+03:00",
                        FilterType= "expression",
                        FieldType= "System.DateTime",
                        InputType= "date",
                        ParametreTableName= null,
                        EkParametreId= 21,
                        FieldName= "Deger",
                        TableName= "KisiEkParametreGerceklesenler"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "Contains",
                        MemberName= "Kimlik No",
                        Value= "317",
                        FilterType= "expression",
                        FieldType= "System.String",
                        InputType= "text",
                        ParametreTableName= null,
                        EkParametreId= 0,
                        FieldName= "KisiKimlikNo",
                        TableName= "KisiHassasBilgiler"
                    }
                },
                Tanim = "Kurum Servis Unit Test kisi",
                KisiId = 129,
                KurumId = 82,
                HedefKitleTipi = "Kişi"
            });
            Assert.AreEqual(addKisi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(addKisi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(addKisi.Result.Value);
            ////Assert-1.2 Add kisi2
            //var addKisi2 = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            //{
            //    Filters = new List<HedefKitleFilter>()
            //    {
            //        new()
            //        {
            //            LocigalOperator = "OR",
            //            Operator = null,
            //            MemberName = null,
            //            Value = null,
            //            FilterType = "group",
            //            FieldType = null,
            //            InputType = null,
            //            ParametreTableName = "KisiTemelBilgiler",
            //            EkParametreId = 0,
            //            FieldName = null
            //        },
            //        new()
            //        {
            //            LocigalOperator = "OR",
            //            Operator = "Contains",
            //            MemberName = "Adı",
            //            Value = "Ah",
            //            FilterType = "expression",
            //            FieldType = "System.String",
            //            InputType = "text",
            //            ParametreTableName = null,
            //            TableName = "KisiTemelBilgiler",
            //            EkParametreId = 0,
            //            FieldName = "KisiAdi"
            //        },

            //        new()
            //        {
            //            LocigalOperator = "OR",
            //            Operator = "Contains",
            //            MemberName = "Adı",
            //            Value = "kö",
            //            FilterType = "expression",
            //            FieldType = "System.String",
            //            InputType = "text",
            //            ParametreTableName = null,
            //            TableName = "KisiTemelBilgiler",
            //            EkParametreId = 0,
            //            FieldName = "KisiSoyadi"
            //        },
            //    },
            //    Tanim = "Kurum Servis Unit Test kisi2323",
            //    KisiId = 129,
            //    KurumId = 82,
            //    HedefKitleTipi = "Kişi"
            //});
            //Assert.AreEqual(addKisi2.StatusCode, HttpStatusCode.OK);
            //Assert.AreEqual(addKisi2.Result.StatusCode, (int)ResultStatusCode.Success);
            //Assert.IsNotNull(addKisi2.Result.Value);

            //Assert-5 GetListTest
            var getList = _helper.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetList");
            Assert.AreEqual(getList.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getList.Result);

            var lastItem = getList.Result.Value.LastOrDefault(a => a.HedefKitleTipi == "kurum");
            var listItemKisi = getList.Result.Value.LastOrDefault(a => a.HedefKitleTipi == "Kişi");
            var sonDegil = (lastItem.TabloID - 1);

            //Assert- 8 RunExpressionTest
            var runExpressionTest1 = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpression/" + lastItem.TabloID);
            Assert.AreEqual(runExpressionTest1.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(runExpressionTest1.Result);

            ////Assert- 8 RunExpressionTest
            //var runExpressionTest2 = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpression/" + listItemKisi.TabloID);
            //Assert.AreEqual(runExpressionTest2.StatusCode, HttpStatusCode.OK);
            //Assert.IsNotNull(runExpressionTest2.Result);

            //Assert- 8 RunExpressionTest
            var runExpressionReturnUserTest1 = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpressionReturnUser/" + lastItem.TabloID);
            Assert.AreEqual(runExpressionReturnUserTest1.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(runExpressionReturnUserTest1.Result);

            ////Assert- 8 RunExpressionTest
            //var runExpressionReturnUserTest2 = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpressionReturnUser/" + listItemKisi.TabloID);
            //Assert.AreEqual(runExpressionReturnUserTest2.StatusCode, HttpStatusCode.OK);
            //Assert.IsNotNull(runExpressionReturnUserTest2.Result);

            //Assert-5.1 getListByKurum
            var getListByKurum = _helper.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetListByKurum/" + 82);
            Assert.AreEqual(getListByKurum.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getListByKurum.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getListByKurum.Result);

            //Assert-5.2 GetKurumListByKurum
            var getKurumListByKurum = _helper.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetKurumListByKurum/" + 82);
            Assert.AreEqual(getKurumListByKurum.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getKurumListByKurum.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getKurumListByKurum.Result);

            //Assert-2 Update kurum
            //Assert-1 Add
            var update = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                Filters = new List<HedefKitleFilter>()
                {
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = null,
                        MemberName = null,
                        Value = null,
                        FilterType = "group",
                        FieldType = null,
                        InputType = null,
                        ParametreTableName = null,
                        EkParametreId = 0,
                        FieldName = null
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsGreaterThanOrEqualTo",
                        MemberName = "Şube Sayısı",
                        Value = "3",
                        FilterType = "expression",
                        FieldType = "System.Int32",
                        InputType = "number",
                        ParametreTableName = null,
                        TableName = "KurumEkParametreGerceklesenler",
                        EkParametreId = 44,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "Contains",
                        MemberName = "Kurum Kısa Adı",
                        Value = "Orsa",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "text",
                        ParametreTableName = null,
                        TableName = "KurumTemelBilgiler",
                        EkParametreId = 0,
                        FieldName = "KurumKisaUnvan"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "IsLessThan",
                        MemberName= "Kuruluş Tarihi",
                        Value= "2121-06-25T00:00:00+03:00",
                        FilterType= "expression",
                        FieldType= "System.DateTime",
                        InputType= "date",
                        ParametreTableName= null,
                        EkParametreId= 0,
                        FieldName= "KurulusTarihi",
                        TableName= "KurumTemelBilgiler"
                    },
                },
                Tanim = "Kurum Servis Unit Test",
                KisiId = 130,
                KurumId = 82,
                HedefKitleTipi = "kurum",
                TabloID = lastItem.TabloID
            });
            Assert.AreEqual(update.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(update.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(update.Result);

            //Assert-2 Update kisi
            var updatekisi = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                Filters = new List<HedefKitleFilter>()
                {
                    new()
                    {
                        LocigalOperator = "OR",
                        Operator = null,
                        MemberName = null,
                        Value = null,
                        FilterType = "group",
                        FieldType = null,
                        InputType = null,
                        ParametreTableName = null,
                        EkParametreId = 0,
                        FieldName = null
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "Contains",
                        MemberName = "Adı",
                        Value = "Bilal",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "text",
                        ParametreTableName = null,
                        TableName = null,
                        EkParametreId = 0,
                        FieldName = "KisiAdi"
                    },
                    new()
                    {
                        LocigalOperator = "AND",
                        Operator = "IsEqualTo",
                        MemberName = "Kan Grubu",
                        Value = "3",
                        FilterType = "expression",
                        FieldType = "System.String",
                        InputType = "select",
                        ParametreTableName = null,
                        TableName = null,
                        EkParametreId =19,
                        FieldName = "Deger"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "IsGreaterThanOrEqualTo",
                        MemberName= "Pasaport Bitiş Tarihi",
                        Value= "2021-06-25T00:00:00+03:00",
                        FilterType= "expression",
                        FieldType= "System.DateTime",
                        InputType= "date",
                        ParametreTableName= null,
                        EkParametreId= 21,
                        FieldName= "Deger",
                        TableName= "KisiEkParametreGerceklesenler"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "IsLessThan",
                        MemberName= "Pasaport Bitiş Tarihi",
                        Value= "2121-06-25T00:00:00+03:00",
                        FilterType= "expression",
                        FieldType= "System.DateTime",
                        InputType= "date",
                        ParametreTableName= null,
                        EkParametreId= 21,
                        FieldName= "Deger",
                        TableName= "KisiEkParametreGerceklesenler"
                    },
                    new()
                    {
                        LocigalOperator= "AND",
                        Operator= "Contains",
                        MemberName= "Kimlik No",
                        Value= "317",
                        FilterType= "expression",
                        FieldType= "System.String",
                        InputType= "text",
                        ParametreTableName= null,
                        EkParametreId= 0,
                        FieldName= "KisiKimlikNo",
                        TableName= "KisiHassasBilgiler"
                    }
                },
                Tanim = "Kurum Servis Unit Test kisi update",
                KisiId = 130,
                KurumId = 82,
                HedefKitleTipi = "Kişi",
                TabloID = listItemKisi.TabloID
            });
            Assert.AreEqual(updatekisi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(updatekisi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(updatekisi.Result);

            //Assert- 6 GetFieldsTest
            var getFields = _helper.Get<Result<List<HedefKitleField>>>("/api/HedefKitle/GetFields/" + lastItem.KurumID);
            Assert.AreEqual(getFields.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getFields.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getFields.Result);

            //Assert-6.1 GetKisiListByKurum
            var getKisiListByKurum = _helper.Get<Result<List<HedefKitleTanimlamalar>>>("/api/HedefKitle/GetKisiListByKurum/" + lastItem.KisiID);
            Assert.AreEqual(getKisiListByKurum.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getKisiListByKurum.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getKisiListByKurum.Result);

            //Assert- 7 SingleOrDefaultForViewTest
            var singleOrDefaultForViewTest = _helper.Get<Result<HedefKitle>>("/api/HedefKitle/SingleOrDefaultForView/" + lastItem.TabloID);
            Assert.AreEqual(singleOrDefaultForViewTest.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(singleOrDefaultForViewTest.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(singleOrDefaultForViewTest.Result);

            //Assert- 8 RunExpressionTest
            var runExpressionTest = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpression/" + lastItem.TabloID);
            Assert.AreEqual(runExpressionTest.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(runExpressionTest.Result);

            //Assert- 8 RunExpressionTest
            var runExpressionTest2 = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpression/" + sonDegil);
            Assert.AreEqual(runExpressionTest2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(runExpressionTest2.Result);

            //Assert- 8 runExpressionReturnUserTest
            var runExpressionReturnUserTest = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpressionReturnUser/" + lastItem.TabloID);
            Assert.AreEqual(runExpressionReturnUserTest.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(runExpressionReturnUserTest.Result);

            //Assert- 9 DeleteTest
            var delete = _helper.Get<Result<bool>>("/api/HedefKitle/hedefkitlesil/" + lastItem.TabloID);
            Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(delete.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(delete.Result);

            //Assert- 9 DeleteTest Kişi
            var deletekisi = _helper.Get<Result<bool>>("/api/HedefKitle/hedefkitlesil/" + listItemKisi.TabloID);
            Assert.AreEqual(deletekisi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(deletekisi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(deletekisi.Result);

            //Assert-10 AddNegative
            var addnegative = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
            });
            Assert.IsFalse(addnegative.Result.IsSuccess);
            Assert.IsFalse(addnegative.Result.Value);

            //Assert- 12 Update Negative
            var updatenegative = _helper.Post<Result<bool>>("/api/HedefKitle/AddOrUpdate", new HedefKitle
            {
                TabloID = -1
            });
            Assert.IsFalse(updatenegative.Result.IsSuccess);
            Assert.IsFalse(updatenegative.Result.Value);

            //Assert- 12 GetFieldsTest negative
            var getFieldsnegative = _helper.Get<Result<List<HedefKitleField>>>("/api/HedefKitle/GetFields/" + 0);
            Assert.IsFalse(getFieldsnegative.Result.IsSuccess);

            //Assert - 13 SingleOrDefaultForViewTest negative
            var singleOrDefaultForViewTestnegative = _helper.Get<Result<HedefKitle>>("/api/HedefKitle/SingleOrDefaultForView/" + 0);
            Assert.IsFalse(singleOrDefaultForViewTestnegative.Result.IsSuccess);

            //Assert- 14 RunExpressionTest negative
            var runExpressionTestnegative = _helper.Get<Result<List<KeyValueModel>>>("/api/HedefKitle/RunExpression/" + 0);
            Assert.IsFalse(runExpressionTestnegative.Result.IsSuccess);

            //Assert- 15 DeleteTest negative
            var deletenegative = _helper.Get<Result<bool>>("/api/HedefKitle/hedefkitlesil/" + 0);
            Assert.IsFalse(deletenegative.Result.IsSuccess);

            //HedefKitleHardDelete
            var hardDelete = _helper.Get<Result<bool>>("/api/HedefKitle/Delete/" + lastItem.TabloID);
            Assert.AreEqual(hardDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(hardDelete.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(hardDelete.Result);

            //HedefKitleHardDelete2
            var hardDelete2 = _helper.Get<Result<bool>>("/api/HedefKitle/Delete/" + sonDegil);
            Assert.AreEqual(hardDelete2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(hardDelete2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(hardDelete2.Result);

            //HedefKitleHardDeletekisi
            var hardDeletekisi = _helper.Get<Result<bool>>("/api/HedefKitle/Delete/" + listItemKisi.TabloID);
            Assert.AreEqual(hardDeletekisi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(hardDeletekisi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(hardDeletekisi.Result);

            //Assert- 15 HedefKitleHardDelete negative
            var hardDeleteNegative = _helper.Get<Result<bool>>("/api/HedefKitle/Delete/" + 0);
            Assert.IsFalse(hardDeleteNegative.Result.IsSuccess);
        }

        // [TestMethod()]
        // public void HK()
        // {
        //     var lista = new List<KurumEkParametreGerceklesenler>
        //      {
        //          new KurumEkParametreGerceklesenler{ KurumEkParametreID = 44 ,Deger ="11" },
        //          new KurumEkParametreGerceklesenler{ KurumEkParametreID = 44 ,Deger ="33" },
        //          new KurumEkParametreGerceklesenler{KurumEkParametreID=43, Deger="22" },
        //          new KurumEkParametreGerceklesenler{KurumEkParametreID=43, Deger="55" }
        //      };
        //     /*"IsLessThan", "IsLessThanOrEqualTo", "IsGreaterThanOrEqualTo", "IsGreaterThan", "IsEqualTo", "IsNotEqualTo", "StartsWith", "EndsWith", "Contains", "IsContainedIn", "DoesNotContain", "IsNull", "IsNotNull", "IsEmpty", "IsNotEmpty", "IsNullOrEmpty"*/

        //     var resulta = lista.AsQueryable().WhereBuilder(ComparisonType.AND,
        //                          new List<BuildPredicateModel> {
        //                          new BuildPredicateModel
        //                             {
        //                                 PropertyName = "KurumEkParametreID",
        //                                 Comparison = "IsEqualTo",
        //                                 Value = 44,
        //                                 ValueType=typeof(int)
        //                             },
        //                          new BuildPredicateModel
        //                             {
        //                                 PropertyName = "Deger",
        //                                 Comparison = "IsGreaterThan",
        //                                 Value = 22,
        //                                 ValueType=typeof(int)
        //                             }
        //                          })
        //                          .ToList();

        //     var resultb = lista.AsQueryable().WhereBuilder(ComparisonType.AND,
        //                          new List<BuildPredicateModel> {
        //                          new BuildPredicateModel
        //                             {
        //                                 PropertyName = "KurumEkParametreID",
        //                                 Comparison = "IsEqualTo",
        //                                 Value = 44,
        //                                 ValueType=typeof(int)
        //                             },
        //                          new BuildPredicateModel
        //                             {
        //                                 PropertyName = "Deger",
        //                                 Comparison = "IsLessThan",
        //                                 Value = 22,
        //                                 ValueType=typeof(int)
        //                             }
        //                          })
        //                          .ToList();

        //     var resultc = lista.AsQueryable().WhereBuilder(ComparisonType.AND,
        //                          new List<BuildPredicateModelForDynamic> {
        //                               new BuildPredicateModelForDynamic
        //                                  {
        //                                    ForEkParametre= new BuildPredicateModel()
        //                                    {   PropertyName = "KurumEkParametreID",
        //                                      Comparison = "IsEqualTo",
        //                                      Value = 43,
        //                                      ValueType=typeof(int)
        //                                   },
        //                                    ForDeger= new BuildPredicateModel()
        //                                    {PropertyName = "Deger",
        //                                      Comparison = "IsGreaterThan",
        //                                      Value = 30,
        //                                      ValueType=typeof(int)
        //                                    }
        //                                  },
        //                               new BuildPredicateModelForDynamic
        //                                  {
        //                                   ForDeger= new BuildPredicateModel()
        //                                   {PropertyName = "Deger",
        //                                      Comparison = "IsGreaterThan",
        //                                      Value = 22,
        //                                      ValueType=typeof(int)
        //                                   },
        //                                   ForEkParametre= new BuildPredicateModel()
        //                                   { PropertyName = "KurumEkParametreID",
        //                                      Comparison = "IsEqualTo",
        //                                      Value = 44,
        //                                      ValueType=typeof(int)
        //                                   }
        //                                  }
        //                          })
        //                          .ToList();

        //     var list = new List<KurumTemelBilgiler>
        //      {
        //          new KurumTemelBilgiler{ KurumKisaUnvan = "Kurum1",KurumVergiNo = "1234",WebSitesi="kurum1.com.tr" },
        //          new KurumTemelBilgiler{ KurumKisaUnvan = "Kurum2",KurumVergiNo = "5678",WebSitesi="kurum2.com.tr" }
        //      };

        //     var result = list.AsQueryable().WhereBuilder(new BuildPredicateModel
        //     {
        //         PropertyName = "KurumKisaUnvan",
        //         Comparison = "==",
        //         Value = "Kurum2",
        //         ValueType = typeof(string)
        //     }).ToList();

        //     var result2 = list.AsQueryable().WhereBuilder(ComparisonType.OR,
        //     new List<BuildPredicateModel> {
        //          new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumKisaUnvan",
        //                  Comparison = "==",
        //                  Value = "Kurum2"
        //              },
        //           new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumVergiNo",
        //                  Comparison = "==",
        //                  Value = "5678"
        //              }
        //     })
        //     .ToList();

        //     var result3 = list.AsQueryable().WhereBuilder(ComparisonType.OR,
        //    new List<BuildPredicateModel> {
        //          new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumKisaUnvan",
        //                  Comparison = "==",
        //                  Value = "Kurum2"
        //              },
        //           new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumVergiNo",
        //                  Comparison = "==",
        //                  Value = "1234"
        //              }
        //    })
        //    .ToList();

        //     var result4 = list.AsQueryable().WhereBuilder(ComparisonType.AND,
        //new List<BuildPredicateModel> {
        //          new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumKisaUnvan",
        //                  Comparison = "==",
        //                  Value = "Kurum2"
        //              },
        //           new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumVergiNo",
        //                  Comparison = "==",
        //                  Value = "5678"
        //              }
        //})
        //.ToList();

        //     var result5 = list.AsQueryable().WhereBuilder(ComparisonType.AND,
        //    new List<BuildPredicateModel> {
        //          new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumKisaUnvan",
        //                  Comparison = "==",
        //                  Value = "Kurum2"
        //              },
        //           new BuildPredicateModel
        //              {
        //                  PropertyName = "KurumVergiNo",
        //                  Comparison = "==",
        //                  Value = "1234"
        //              }
        //    })
        //    .ToList();
        // }
    }
}