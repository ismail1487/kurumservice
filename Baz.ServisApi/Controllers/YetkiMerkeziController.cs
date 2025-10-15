using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baz.KurumServiceApi.Controllers
{
    /// <summary>
    /// Menü kontrolü için gerekli methodların bulunduğu sınıftır.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class YetkiMerkeziController : ControllerBase
    {
        private readonly IYetkiMerkeziService _yetkiMerkeziService;

        /// <summary>
        ///  Menü kontrolü için gerekli methodların bulunduğu sınıfının yapıcı methodudur.
        /// </summary>
        /// <param name="yetkiMerkeziService"></param>
        public YetkiMerkeziController(IYetkiMerkeziService yetkiMerkeziService)
        {
            _yetkiMerkeziService = yetkiMerkeziService;
        }

        /// <summary>
        /// Erişim yetkilendirme tanımlarını kaydeden method.
        /// </summary>
        /// <param name="list">kaydedilecek tanımlar listesi.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ErisimYetkilendirmeTanimlariKaydet")]
        public Result<List<ErisimYetkilendirmeTanimlari>> ErisimYetkilendirmeTanimlariKaydet([FromBody] List<ErisimYetkilendirmeTanimlari> list)
        {
            var result = _yetkiMerkeziService.ErisimYetkilendirmeTanimlariKaydet(list);
            return result;
        }

        /// <summary>
        /// KişiID değeri ile ilgili kişinin yetkilendirildiği sayfa tanımlarını getiren method.
        /// </summary>
        /// <param name="kisiID">ilgili kişiID değeri</param>
        /// <returns>yetkilendirilen sayfa tanımları listesiin döndürür.</returns>
        [HttpGet]
        [Route("KisiYetkilerListGetir/{kisiID}")]
        public Result<List<string>> KisiYetkilerListGetir(int kisiID)
        {
            var result = _yetkiMerkeziService.KisiYetkilerListGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Erişim yetkilendirme tanimlari listesinin tanımlarını getiren method.
        /// </summary>
        /// <returns>erişim yetki tanımları view model listesi döndürür.</returns>
        [HttpGet]
        [Route("ErisimYetkiTanimListGetir")]
        public Result<List<ErisimYetkilendirmeTanimlariListView>> ErisimYetkiTanimListGetir()
        {
            var result = _yetkiMerkeziService.ErisimYetkiTanimListGetir();
            return result;
        }

        /// <summary>
        /// Erişim yetki tanımı kaydını silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>sonuca göre true veya false döndürür.</returns>
        [HttpGet]
        [Route("ErisimYetkiTanimiSil/{id}")]
        public Result<bool> ErisimYetkiTanimiSil(int id)
        {
            var result = _yetkiMerkeziService.ErisimYetkiTanimiSil(id);
            return result;
        }
        [Route("SistemSayfalariGetir")]
        [HttpGet]
        [AllowAnonymous]
        public Result<List<SistemSayfalari>> SistemSayfalariGetir()
        {
            var result = _yetkiMerkeziService.SistemSayfalariGetir();
            return result;
        }
    }
}