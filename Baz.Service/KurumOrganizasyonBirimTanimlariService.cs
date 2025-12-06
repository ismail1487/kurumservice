using Baz.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Baz.Model.Entity;
using Baz.Model.Pattern;
using Baz.Repository.Pattern;
using Baz.Mapper.Pattern;
using Microsoft.Extensions.Logging;
using Baz.ProcessResult;
using Baz.Model.Entity.ViewModel;
using Baz.RequestManager.Abstracts;
using Baz.Model.Entity.Constants;


namespace Baz.Service
{
    /// <summary>
    /// Kurum Organizasyon Birim Tanımlarının ve ağaç yapısının belirlendiği servis sınıfıdır.
    /// </summary>
    public interface IKurumOrganizasyonBirimTanimlariService : IService<KurumOrganizasyonBirimTanimlari>
    {
        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> Add(KurumOrganizasyonBirimView item);

        /// <summary>
        /// Ilgili Kurum ID'ye göre Kurum Organizasyon Birim Ekleme Methodu (Pozisyon)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> AddPoz(KurumOrganizasyonBirimView item);


        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> List(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method(Pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public Result<List<KurumOrganizasyonBirimView>> List2(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public Result<bool> Update(KurumOrganizasyonBirimView item);

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method(Pozisyon)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public Result<bool> Update2(KurumOrganizasyonBirimView item);

        /// <summary>
        /// Kurum Organizasyon Birimlerininin TipId lerine göre seviyelerini belirleyen method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> ListForLevel(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// KurumId ve Name e göre listeleme yapan method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public Result<List<KurumOrganizasyonBirimView>> ListTip(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// ListForView
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamBirimTanimView>> ListForView();

        /// <summary>
        /// Kurum organizasyon biriminin adını değiştiren method
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>

        public Result<bool> UpdateName(KurumOrganizasyonBirimView item);

        /// <summary>
        /// TiptId ve Ilgili KurumId'ye göre Ağaç pozisyon yapısını getiren method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Result<List<KurumOrganizasyonBirimView>> GetTree2(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// Kurum Organizasyon Birimlerininin  seviyelerini belirleyen method(pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public Result<List<KurumOrganizasyonBirimView>> ListForLevel2(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// Kişi Rol Admin mi Kontrolü.
        /// </summary>
        /// <returns></returns>
        public Result<bool> AdminMi();

        /// <summary>
        /// Kurum ID'ye göre OrganizasyonBirimTipiId 1 olan kayıtları listeler
        /// </summary>
        /// <param name="kurumId">İlgili Kurum ID</param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> GetByKurumIdAndTipId(int kurumId);
    }

    /// <summary>
    /// Kurum Organizasyon Birim Tanımları işlemlerini yöneten servis sınıfı
    /// </summary>
    public class KurumOrganizasyonBirimTanimlariService : Service<KurumOrganizasyonBirimTanimlari>, IKurumOrganizasyonBirimTanimlariService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly ILoginUser _loginUser;

        /// <summary>
        /// Kurum Organizasyon Birim Tanımları işlemlerini yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="requestHelper"></param>
        /// <param name="loginUser"></param>
        public KurumOrganizasyonBirimTanimlariService(IRepository<KurumOrganizasyonBirimTanimlari> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumOrganizasyonBirimTanimlariService> logger, IRequestHelper requestHelper, ILoginUser loginUser) : base(repository, dataMapper, serviceProvider, logger)
        {
            _requestHelper = requestHelper;
            _loginUser = loginUser;
        }

        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> Add(KurumOrganizasyonBirimView item)
        {
            var list = List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.OrganizasyonBirimTipiId == item.TipId && o.KurumID == _loginUser.KurumID).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı kayıt bulunmaktadır.", ResultStatusCode.CreateError);
            }
            var entity = new KurumOrganizasyonBirimTanimlari()
            {
                KurumID = _loginUser.KurumID,
                IlgiliKurumId = item.KurumId,
                BirimTanim = item.Tanim,
                OrganizasyonBirimTipiId = item.TipId,
                UstId = item.UstId,
                GuncellenmeTarihi = DateTime.Now,
                KayitTarihi = DateTime.Now,
                AktifMi = 1
            };
            if (item.TipId == (int)OrganizasyonBirimTipi.Lokasyon)//4
            {
                entity.Koordinat = item.Koordinat;
            }
            if (item.UstId == 0)
                entity.BirimKisaTanim = Guid.NewGuid().ToString() + "level1";//Hiyerarşik yapıda en üstte bulunması için "level1" verilmiştir.
            else
            {
                var ustItem = base.SingleOrDefault(item.UstId).Value;
                int level = Convert.ToInt32(ustItem.BirimKisaTanim.Split("level")[1]);
                var group = ustItem.BirimKisaTanim.Split("level")[0];
                entity.BirimKisaTanim = group + "level" + (level + 1);
            }
            var result = base.Add(entity);
            return result.Value.TabloID.ToResult();
        }

        /// <summary>
        /// Ilgili Kurum ID'ye göre Kurum Organizasyon Birim Ekleme Methodu (Pozisyon)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> AddPoz(KurumOrganizasyonBirimView item)
        {
            var list = this.List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.OrganizasyonBirimTipiId == item.TipId && o.IlgiliKurumId == item.IlgiliKurumID).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı kayıt bulunmaktadır.", ResultStatusCode.CreateError);
            }
            var entity = new KurumOrganizasyonBirimTanimlari()
            {
                KurumID = _loginUser.KurumID,
                KisiID = _loginUser.KisiID,
                IlgiliKurumId = item.KurumId,
                BirimTanim = item.Tanim,
                OrganizasyonBirimTipiId = item.TipId,
                UstId = item.UstId,
                GuncellenmeTarihi = DateTime.Now,
                KayitTarihi = DateTime.Now,
                AktifMi = 1
            };
            if (item.UstId == 0)
                entity.BirimKisaTanim = Guid.NewGuid().ToString() + "level1";//Hiyerarşik yapıda en üstte bulunması için "level1" verilmiştir.
            else
            {
                var ustItem = base.SingleOrDefault(item.UstId).Value;
                int level = Convert.ToInt32(ustItem.BirimKisaTanim.Split("level")[1]);
                var group = ustItem.BirimKisaTanim.Split("level")[0];
                entity.BirimKisaTanim = group + "level" + (level + 1);
            }
            var result = base.Add(entity);
            return result.Value.TabloID.ToResult();
        }

        

        /// <summary>
        /// TiptId ve Ilgili KurumId'ye göre Ağaç pozisyon yapısını getiren method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> GetTree2(KurumOrganizasyonBirimRequest request)
        {
            return TreeList2(request.UstId, request.IlgiliKurumID).ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> List(KurumOrganizasyonBirimRequest request)
        {
            if (request.KurumId == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            return _repository.List(p => p.IlgiliKurumId == request.KurumId && p.OrganizasyonBirimTipiId == request.UstId && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                TabloId = p.TabloID,
                UstId = p.UstId.Value,
                Tanim = p.BirimTanim,
                TipId = p.OrganizasyonBirimTipiId.Value,
                KurumId = request.KurumId
            }).ToList().ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method(Pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> List2(KurumOrganizasyonBirimRequest request)
        {
            if (request.IlgiliKurumID == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            return _repository.List(p => p.KurumID == _loginUser.KurumID && p.OrganizasyonBirimTipiId == request.UstId && p.IlgiliKurumId == request.IlgiliKurumID && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                TabloId = p.TabloID,
                UstId = p.UstId.Value,
                IlgiliKurumID = request.IlgiliKurumID,
                Tanim = p.BirimTanim,
                TipId = p.OrganizasyonBirimTipiId.Value,
                KurumId = _loginUser.KurumID
            }).OrderBy(x => x.Tanim).ToList().ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerininin TipId lerine göre seviyelerini belirleyen method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> ListForLevel(KurumOrganizasyonBirimRequest request)
        {
            if (request.KurumId == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            List<KurumOrganizasyonBirimView> result = new();
            var selectItem = base.SingleOrDefault(request.UstId).Value;
            var level = Convert.ToInt32(selectItem.BirimKisaTanim.Split("level")[1]);
            var group = selectItem.BirimKisaTanim.Split("level")[0];
            var items = _repository.List(p => p.KurumID == request.KurumId && p.OrganizasyonBirimTipiId == selectItem.OrganizasyonBirimTipiId && p.TabloID != selectItem.TabloID && p.AktifMi == 1 && p.SilindiMi == 0).ToList();
            foreach (var item in items)
            {
                if (!item.BirimKisaTanim.Contains(group))
                    result.Add(new KurumOrganizasyonBirimView() { KurumId = item.KurumID, TabloId = item.TabloID, Tanim = item.BirimTanim, Koordinat = item.Koordinat, TipId = item.OrganizasyonBirimTipiId.Value, UstId = item.UstId.Value });
                else
                {
                    var itemLevel = Convert.ToInt32(item.BirimKisaTanim.Split("level")[1]);
                    if (level >= itemLevel)
                        result.Add(new KurumOrganizasyonBirimView() { KurumId = item.KurumID, TabloId = item.TabloID, Tanim = item.BirimTanim, Koordinat = item.Koordinat, TipId = item.OrganizasyonBirimTipiId.Value, UstId = item.UstId.Value });
                }
            }
            return result.ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerininin  seviyelerini belirleyen method(pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> ListForLevel2(KurumOrganizasyonBirimRequest request)
        {
            if (request.IlgiliKurumID == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            List<KurumOrganizasyonBirimView> result = new();
            var selectItem = base.SingleOrDefault(request.UstId).Value;
            var level = Convert.ToInt32(selectItem.BirimKisaTanim.Split("level")[1]);
            var group = selectItem.BirimKisaTanim.Split("level")[0];
            var items = _repository.List(p => p.IlgiliKurumId == request.IlgiliKurumID && p.KurumID == _loginUser.KurumID && p.OrganizasyonBirimTipiId == selectItem.OrganizasyonBirimTipiId && p.TabloID != selectItem.TabloID && p.AktifMi == 1 && p.SilindiMi == 0).ToList();
            foreach (var item in items)
            {
                if (!item.BirimKisaTanim.Contains(group))
                    result.Add(new KurumOrganizasyonBirimView() { KurumId = _loginUser.KurumID, IlgiliKurumID = Convert.ToInt32(item.IlgiliKurumId), TabloId = item.TabloID, Tanim = item.BirimTanim, Koordinat = item.Koordinat, TipId = item.OrganizasyonBirimTipiId.Value, UstId = item.UstId.Value });
                else
                {
                    var itemLevel = Convert.ToInt32(item.BirimKisaTanim.Split("level")[1]);
                    if (level >= itemLevel)
                        result.Add(new KurumOrganizasyonBirimView() { KurumId = _loginUser.KurumID, IlgiliKurumID = Convert.ToInt32(item.IlgiliKurumId), TabloId = item.TabloID, Tanim = item.BirimTanim, Koordinat = item.Koordinat, TipId = item.OrganizasyonBirimTipiId.Value, UstId = item.UstId.Value });
                }
            }
            return result.ToResult();
        }

        /// <summary>
        /// Id'ye gönre Kurum Organizasyon Birimlerini Silen method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Result<KurumOrganizasyonBirimTanimlari> Delete(int id)
        {
            if (id == 0)
            {
                return Results.Fail("Silme işleminiz gerçekleşmemiştir!", ResultStatusCode.DeleteError);
            }

            _repository.DataContextConfiguration().AutoDetectChangesEnable();
            var entity = base.SingleOrDefault(id).Value;
            var childs = _repository.List(p => p.UstId == entity.TabloID).ToList();
            foreach (var item in childs)
            {
                var group = Guid.NewGuid().ToString();
                int level = 1;
                item.BirimKisaTanim = group + "level" + level;
                item.UstId = 0;
                UpdateGroup(item.TabloID, group, level);
            }

            entity.SilindiMi = 1;
            entity.AktifMi = 0;
            _repository.SaveChanges();
            _repository.DataContextConfiguration().AutoDetectChangesDisable();

            return entity.ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method..
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<bool> Update(KurumOrganizasyonBirimView item)
        {
            if (item.TabloId == 0)
            {
                return Results.Fail("Güncelleme başarısız.", ResultStatusCode.UpdateError);
            }
            var list = List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.TabloID != item.TabloId && o.OrganizasyonBirimTipiId == item.TipId && o.KurumID == item.KurumId).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı kayıt bulunmaktadır.", ResultStatusCode.UpdateError);
            }
            _repository.DataContextConfiguration().AutoDetectChangesEnable();
            if (item.TabloId == item.UstId)
                return false.ToResult();
            var selecttedItem = base.SingleOrDefault(item.TabloId).Value;
            var newUstItem = base.SingleOrDefault(item.UstId).Value;
            if (newUstItem != null && selecttedItem.BirimKisaTanim == newUstItem.BirimKisaTanim)
            {
                newUstItem.UstId = selecttedItem.UstId;
            }
            selecttedItem.UstId = item.UstId;
            selecttedItem.BirimTanim = item.Tanim;
            if (item.TipId == (int)OrganizasyonBirimTipi.Lokasyon)//4
            {
                selecttedItem.Koordinat = item.Koordinat;
            }
            if (item.UstId > 0)
            {
                var level = Convert.ToInt32(newUstItem?.BirimKisaTanim.Split("level")[1]);
                var group = newUstItem?.BirimKisaTanim.Split("level")[0];
                selecttedItem.BirimKisaTanim = group + "level" + (level + 1);
            }
            else
            {
                var group = Guid.NewGuid().ToString();
                int level = 1;
                selecttedItem.BirimKisaTanim = group + "level" + level;
                UpdateGroup(selecttedItem.TabloID, group, level);
            }
            if (_repository.SaveChanges() > 0)
            {
                _repository.DataContextConfiguration().AutoDetectChangesDisable();
                return true.ToResult();
            }
            return false.ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method(Pozisyon)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<bool> Update2(KurumOrganizasyonBirimView item)
        {
            item.IlgiliKurumID = item.KurumId;
            if (item.TabloId == 0)
            {
                return Results.Fail("Güncelleme başarısız.", ResultStatusCode.UpdateError);
            }
            var list = this.List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.TabloID != item.TabloId && o.OrganizasyonBirimTipiId == item.TipId && o.IlgiliKurumId == item.IlgiliKurumID).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı kayıt bulunmaktadır.", ResultStatusCode.UpdateError);
            }
            _repository.DataContextConfiguration().AutoDetectChangesEnable();
            if (item.TabloId == item.UstId)
                return false.ToResult();
            var selecttedItem = base.SingleOrDefault(item.TabloId).Value;
            var newUstItem = base.SingleOrDefault(item.UstId).Value;
            if (newUstItem != null && selecttedItem.BirimKisaTanim == newUstItem.BirimKisaTanim)
            {
                newUstItem.UstId = selecttedItem.UstId;
            }
            selecttedItem.UstId = item.UstId;
            selecttedItem.BirimTanim = item.Tanim;
            selecttedItem.IlgiliKurumId = item.IlgiliKurumID;

            if (item.UstId > 0)
            {
                var level = Convert.ToInt32(newUstItem?.BirimKisaTanim.Split("level")[1]);
                var group = newUstItem?.BirimKisaTanim.Split("level")[0];
                selecttedItem.BirimKisaTanim = group + "level" + (level + 1);
            }
            else
            {
                var group = Guid.NewGuid().ToString();
                int level = 1;
                selecttedItem.BirimKisaTanim = group + "level" + level;
                UpdateGroup(selecttedItem.TabloID, group, level);
            }
            if (_repository.SaveChanges() > 0)
            {
                _repository.DataContextConfiguration().AutoDetectChangesDisable();
                return true.ToResult();
            }
            return false.ToResult();
        }

        /// <summary>
        /// Kurum organizasyon biriminin adını değiştiren method
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Result<bool> UpdateName(KurumOrganizasyonBirimView item)
        {
            if (item.TabloId == 0 || item.Tanim == null)
            {
                return Results.Fail("Güncelleme işleminiz başarısız!", ResultStatusCode.UpdateError);
            }
            var list = this.List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.TabloID != item.TabloId).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı isimden kayıt bulunmaktadır.", ResultStatusCode.UpdateError);
            }
            var birim = this.SingleOrDefault(item.TabloId).Value;
            birim.BirimTanim = item.Tanim;
            this.Update(birim);

            return true.ToResult();
        }

        private void UpdateGroup(int ustId, string group, int level)
        {
            var items = _repository.List(p => p.UstId == ustId).ToList();
            foreach (var item in items)
            {
                item.BirimKisaTanim = group + "level" + (level + 1);
                UpdateGroup(item.TabloID, group, (level + 1));
            }
        }

        

        /// <summary>
        /// Ağaç yapısının çocuklarını UstId ve KurumId'ye belirleyen method
        /// </summary>
        /// <param name="ustId"></param>
        /// <param name="IlgiliKurumId"></param>
        /// <returns></returns>
        private List<KurumOrganizasyonBirimView> TreeChild2(int ustId, int IlgiliKurumId)
        {
            var data = _repository.List(p => p.UstId == ustId && p.KurumID == _loginUser.KurumID && p.IlgiliKurumId == IlgiliKurumId && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                KurumId = p.KurumID,
                IlgiliKurumID = Convert.ToInt32(p.IlgiliKurumId),
                TabloId = p.TabloID,
                Tanim = p.BirimTanim,
                UstId = p.UstId.Value,
                TipId = p.OrganizasyonBirimTipiId.Value,
                Koordinat = p.Koordinat,
                AltItems = new List<KurumOrganizasyonBirimView>()
            }).ToList();

            foreach (var item in data)
            {
                item.AltItems = TreeChild2(item.TabloId, IlgiliKurumId);
            }
            return data;
        }

        

        /// <summary>
        /// Ağaç yapısını TipId ve IlgiliKurumId'ye göre listeleyen method (Pozisyon)
        /// </summary>
        /// <param name="tipId"></param>
        /// <param name="IlgiliKurumId"></param>
        /// <returns></returns>
        private List<KurumOrganizasyonBirimView> TreeList2(int tipId, int IlgiliKurumId)
        {
            var data = _repository.List(p => p.OrganizasyonBirimTipiId == tipId && p.UstId == 0 && p.KurumID == _loginUser.KurumID && p.IlgiliKurumId == IlgiliKurumId && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                KurumId = p.KurumID,
                IlgiliKurumID = IlgiliKurumId,
                TabloId = p.TabloID,
                Tanim = p.BirimTanim,
                UstId = p.UstId.Value,
                TipId = p.OrganizasyonBirimTipiId.Value,
                Koordinat = p.Koordinat,
                AltItems = new List<KurumOrganizasyonBirimView>()
            }).ToList();

            foreach (var item in data)
            {
                item.AltItems = TreeChild2(item.TabloId, IlgiliKurumId);
            }
            return data;
        }

        /// <summary>
        /// KurumId ve Name e göre listeleme yapan method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> ListTip(KurumOrganizasyonBirimRequest request)
        {
            if (request.KurumId == 0 || request.Name == null)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            string url = LocalPortlar.IYSService + "/api/ParamOrganizasyonBirimTanim/GetTipId/" + request.Name;
            var r = _requestHelper.Get<Result<int>>(url);
            int departmanId = r.Result.Value;
            var result = _repository.List(p => p.IlgiliKurumId == request.KurumId && p.OrganizasyonBirimTipiId == departmanId && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                IlgiliKurumID = request.IlgiliKurumID,
                Tanim = p.BirimTanim,
                TabloId = p.TabloID,
                TipId = departmanId,
                KurumId = request.KurumId,
                UstId = p.UstId.Value,
                Koordinat = p.Koordinat
            }).ToList();

            var yeniList = result.ToResult().Value.OrderBy(x => x.Tanim).ToList();

            return yeniList.ToResult();
        }
        /// <summary>
        /// ListForView
        /// </summary>
        /// <returns></returns>
        public Result<List<ParamBirimTanimView>> ListForView()
        {
            var request = _requestHelper.Get<Result<List<ParamBirimTanimView>>>(LocalPortlar.IYSService + "/api/ParamOrganizasyonBirimTanim/ListForView");

            return request.Result;
        }
        
        /// <summary>
        /// Admin Kontrol
        /// </summary>
        /// <returns></returns>
        public Result<bool> AdminMi()
        {
            var rs = base.List(a => a.KurumID == _loginUser.KurumID && a.IlgiliKurumId == _loginUser.KurumID && a.AktifMi == 1 && a.BirimTanim == "Kurum Admin").Value.FirstOrDefault();
            var _kurumKisi = (IKurumlarKisilerService)_serviceProvider.GetService(typeof(IKurumlarKisilerService));
            if (rs==null)
            {
                return false.ToResult();
            }
            return _kurumKisi.List(a => a.AktifMi == 1 && a.KurumOrganizasyonBirimTanimId == rs.TabloID).Value.Any(b => b.IlgiliKisiId == _loginUser.KisiID).ToResult();
        }

        /// <summary>
        /// Kurum ID'ye göre OrganizasyonBirimTipiId 1 olan kayıtları listeler
        /// </summary>
        /// <param name="kurumId">İlgili Kurum ID</param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> GetByKurumIdAndTipId(int kurumId)
        {
            if (kurumId == 0)
            {
                return Results.Fail("Kurum ID geçersiz!", ResultStatusCode.ReadError);
            }

            var result = _repository.List(p => p.IlgiliKurumId == kurumId && p.OrganizasyonBirimTipiId == 1 && p.AktifMi == 1 && p.SilindiMi == 0)
                .Select(p => new KurumOrganizasyonBirimView()
                {
                    TabloId = p.TabloID,
                    KurumId = p.KurumID,
                    IlgiliKurumID = p.IlgiliKurumId.Value,
                    Tanim = p.BirimTanim,
                    TipId = p.OrganizasyonBirimTipiId.Value,
                    UstId = p.UstId.Value,
                    Koordinat = p.Koordinat
                })
                .OrderBy(x => x.Tanim)
                .ToList();

            return result.ToResult();
        }
    }
}