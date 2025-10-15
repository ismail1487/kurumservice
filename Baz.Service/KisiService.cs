using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.AOP.Logger.ExceptionLog;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.RequestManager.Abstracts;
//using BazWebApp.Services;
using Decor;
using Microsoft.Extensions.DependencyInjection;

namespace Baz.Service
{
    /// <summary>
    /// KisiService ile kişilere dair işlemlerin yönetileceği interface.
    /// </summary>
    public interface IKisiService : IService<KisiTemelBilgiler>
    {
        /// <summary>
        /// Listeleme methodu
        /// </summary>
        /// <returns></returns>
        IQueryable<KisiTemelBilgiler> ListForQery();



        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <returns>kişinin astları listesi.</returns>
        Result<List<KisiOrganizasyonBirimView>> KisiAstlarListGetir(int kisiID);

        /// <summary>
        /// kisi Id ile müsteri temsilcileri getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmireBagliMusteriTemsilcileriList(int kisiId);

        /// <summary>
        /// kisi Id mile amir veya müsteri temsilcisi listeleme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmirveyaMusteriTemsilcisiKurumlariIDGetir(int kisiId);
    }

    /// <summary>
    /// KisiService ile kişilere dair işlemlerin yönetileceği servis classı.
    /// IKisiService interface'ini ve Service class'ını baz alır.
    /// </summary>
    public class KisiService : Service<KisiTemelBilgiler>, IKisiService
    {
        private readonly ILoginUser _loginUser;
        private readonly IRequestHelper _helper;

        /// <summary>
        /// KisiService ile kişilere dair işlemlerin yönetileceği servis classının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="helper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="loginUser"></param>
        public KisiService(IRepository<KisiTemelBilgiler> repository, IDataMapper dataMapper, ILoginUser loginUser, IRequestHelper helper, IServiceProvider serviceProvider, ILogger<KisiService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _loginUser = loginUser;
            _helper = helper;
        }

        /// <summary>
        /// Listeleme methodu
        /// </summary>
        /// <returns></returns>
        public IQueryable<KisiTemelBilgiler> ListForQery()
        {
            return _repository.List();
        }

 

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <returns>kişinin astları listesi.</returns>
        public Result<List<KisiOrganizasyonBirimView>> KisiAstlarListGetir(int kisiID)
        {
            var request = _helper.Get<Result<List<KisiOrganizasyonBirimView>>>(LocalPortlar.KisiServis + "/api/KisiService/KisiAstlarListGetir/" + kisiID);
            return request.Result;
        }

        /// <summary>
        /// kisi Id ile müsteri temsilcileri getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmireBagliMusteriTemsilcileriList(int kisiId)
        {
            List<int> result = new();
            var _kurumlarKisilerService = _serviceProvider.GetService<IKurumlarKisilerService>();
            var astlar = KisiAstlarListGetir(kisiId);
            if (astlar.Value != null)
            {
                foreach (var ast in astlar.Value)
                {
                    var kontrol = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(ast.KisiId);
                    if (kontrol.Value)
                        result.Add(ast.KisiId);
                }
            }
            return result.ToResult();
        }

        /// <summary>
        /// kisi Id mile amir veya müsteri temsilcisi listeleme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmirveyaMusteriTemsilcisiKurumlariIDGetir(int kisiId)
        {
            var _kurumService = _serviceProvider.GetService<IKurumService>();
            var _kurumlarKisilerService = _serviceProvider.GetService<IKurumlarKisilerService>();
            var amirMi = AmireBagliMusteriTemsilcileriList(kisiId).Value.Count > 0;
            var musteriTemsilcisiMi = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(kisiId).Value;
            if (amirMi)
            {
                var result = _kurumService.AmirlereAstMusteriTemsilcisiKurumlariniGetir(kisiId);
                return result.Value.Select(a => a.TabloID).ToList().ToResult();
            }

            if (musteriTemsilcisiMi)
            {
                var result = _kurumService.MusteriTemsilcisiBagliKurumlarList(kisiId);
                return result.Value.Select(a => a.TabloID).ToList().ToResult();
            }
            var kurum = base.SingleOrDefault(kisiId).Value.KurumID;
            var list = _kurumService.List(a => a.AktifMi == 1 && a.KurumID == kurum).Value.Select(prop => prop.TabloID).ToList();
            return list.ToResult();//new List<int>() { kurum }.ToResult();
        }
    
    }
}