using Baz.Attributes;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baz.KurumServiceApi.Controllers
{
    /// <summary>
    /// Kurum Organizasyon Birim Tanımlarının ve ağaç yapısının belirlendiği api kontrol sınıfıdır.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizasyonBirimController : ControllerBase
    {
        private readonly IKurumOrganizasyonBirimTanimlariService _service;
        private readonly IKurumlarKisilerService _kurumlarKisilerService;

        /// <summary>
        ///  Kurum Organizasyon Birim Tanımlarının ve ağaç yapısının belirlendiği api kontrol sınıfının yapıcı methodudur.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="kurumlarKisilerService"></param>
        public OrganizasyonBirimController(IKurumOrganizasyonBirimTanimlariService service, IKurumlarKisilerService kurumlarKisilerService) { _service = service; _kurumlarKisilerService = kurumlarKisilerService; }

        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [ProcessName(Name = "Add")]
        public Result<int> Add(KurumOrganizasyonBirimView item)
        {
            return _service.Add(item);
        }

        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPoz")]
        [ProcessName(Name = "AddPoz")]
        public Result<int> AddPoz(KurumOrganizasyonBirimView item)
        {
            return _service.AddPoz(item);
        }

        

        /// <summary>
        ///  TipId ve Ilgili Kurum ID'ye göre pozisyon Ağaç yapısını getiren method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTree2")]
        [ProcessName(Name = "GetTree2")]
        public Result<List<KurumOrganizasyonBirimView>> GetTree2(KurumOrganizasyonBirimRequest request)
        {
            if (request.IlgiliKurumID == 0)
            {
                return Results.Fail("İşleminiz gerçekleşmemiştir!", ResultStatusCode.ReadError);
            }
            var rs = _service.GetTree2(request);

            return rs;
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProcessName(Name = "ListForKurum")]
        [Route("ListForKurum")]
        public Result<List<KurumOrganizasyonBirimView>> ListForKurum(KurumOrganizasyonBirimRequest request)
        {
            return _service.List(request);
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Listeleyen Method(Pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProcessName(Name = "ListForKurum2")]
        [Route("ListForKurum2")]
        public Result<List<KurumOrganizasyonBirimView>> ListForKurum2(KurumOrganizasyonBirimRequest request)
        {
            return _service.List2(request);
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerininin TiptId lerine göre seviyelerini belirleyen method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProcessName(Name = "ListForLevel")]
        [Route("ListForLevel")]
        public Result<List<KurumOrganizasyonBirimView>> ListForLevel(KurumOrganizasyonBirimRequest request)
        {
            return _service.ListForLevel(request);
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerininin seviyelerini belirleyen method(Pozisyon)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProcessName(Name = "ListForLevel2")]
        [Route("ListForLevel2")]
        public Result<List<KurumOrganizasyonBirimView>> ListForLevel2(KurumOrganizasyonBirimRequest request)
        {
            return _service.ListForLevel2(request);
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateForKurum")]
        [ProcessName(Name = "UpdateForKurum")]
        [HttpPost]
        public Result<bool> UpdateForKurum(KurumOrganizasyonBirimView item)
        {
            return _service.Update(item);
        }

        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method(Pozisyon)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateForKurum2")]
        [ProcessName(Name = "UpdateForKurum2")]
        [HttpPost]
        public Result<bool> UpdateForKurum2(KurumOrganizasyonBirimView item)
        {
            return _service.Update2(item);
        }

        /// <summary>
        /// Id'ye gönre Kurum Organizasyon Birimlerini Silen method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [ProcessName(Name = "Delete")]
        [HttpGet]
        public Result<bool> Delete(int id)
        {
            var result = _service.Delete(id);
            return result.IsSuccess.ToResult();
        }

        /// <summary>
        /// KurumId ve Name e göre listeleme yapan method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProcessName(Name = "Organizasyon birim tipi listesi")]
        [Route("ListTip")]
        public Result<List<KurumOrganizasyonBirimView>> ListTip(KurumOrganizasyonBirimRequest request)
        {
            return _service.ListTip(request);
        }

        /// <summary>
        /// ListForView
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProcessName(Name = "Organizasyon birim tipi listesi")]
        [Route("OrganizasyonBirim")]
        public Result<List<ParamBirimTanimView>> ListForView()
        {
            return _service.ListForView();
        }

        


        /// <summary>
        /// Kurum Organizasyon Birimlerini Güncelleyen Method
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateName")]
        [ProcessName(Name = "UpdateName")]
        [HttpPost]
        public Result<bool> UpdateName(KurumOrganizasyonBirimView item)
        {
            return _service.UpdateName(item);
        }

        /// <summary>
        /// organizasyonBirimIdye organizasyona bağlı kişileri getiren method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("ListKisiId/{id}")]
        [HttpGet]
        public Result<List<KisiTemelBilgiler>> ListKisiId(int id)
        {
            return _kurumlarKisilerService.ListKisiId(id);
        }

        /// <summary>
        /// Test amaçlı yazılan get mmethodu idye göre organizasyonbirimini getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("TestGet/{id}")]
        [HttpGet]
        public Result<KurumOrganizasyonBirimTanimlari> TestGet(int id)
        {
            return _service.SingleOrDefault(id);
        }

        /// <summary>
        /// Kişi Rol Admin mi Kontrolü.
        /// </summary>
        /// <returns></returns>
        [Route("AdminMi")]
        [HttpGet]
        public Result<bool> AdminMi()
        {
            return _service.AdminMi();
        }
    }
}