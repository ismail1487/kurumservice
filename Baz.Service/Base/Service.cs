using Baz.Mapper.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Common;
using Baz.Repository.Pattern;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Baz.Model.Entity;
using Baz.Model.Pattern;
using Microsoft.Extensions.DependencyInjection;
using Baz.Service.Helper;
using Baz.AOP.Logger.ExceptionLog;

namespace Baz.Service.Base
{
    /// <summary>
    /// Ekleme,düzenleme,silme listeleme vb işlemlerin yer aldığı sınftır.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Baz.Service.Base.IService{TEntity}" />
    public class Service<TEntity> : IService<TEntity> where TEntity : class, Baz.Model.Pattern.IBaseModel
    {
        /// <summary>
        /// Reposiitory değişkeni
        /// </summary>
        protected readonly IRepository<TEntity> _repository;

        /// <summary>
        /// Model mapper
        /// </summary>
        protected IDataMapper _dataMapper;

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger _logger;

        /// <summary>
        /// Servis collector
        /// </summary>
        protected IServiceProvider _serviceProvider;

        private bool _disposed;
        private readonly ILoginUser _loginUser;

        /// <summary>
        /// Ekleme,düzenleme,silme listeleme vb işlemlerin yer aldığı sınfın yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public Service(IRepository<TEntity> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger logger)
        {
            _repository = repository;
            _dataMapper = dataMapper;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _loginUser = _serviceProvider.GetService<ILoginUser>();
        }

        /// <summary>
        /// Ekleme işleminin yapıldığı methodtur.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual Result<TEntity> Add(TEntity entity)
        {
            if (IslemYetkisiVarMi(entity))
            {
                var result = _repository.Add(entity);
                if (_repository.SaveChanges() > 0)
                    return result.ToResult();
                else
                    throw new OctapullException(OctapullExceptions.AddError);
            }
            throw new OctapullException(OctapullExceptions.AuthError);
        }

        /// <summary>
        /// Silme işleminin yapıldığı methodtur.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual Result<TEntity> Delete(int id)
        {
            var entity = _repository.SingleOrDefault(id).ToResult();
            if (IslemYetkisiVarMi(entity.Value))
            {
                var result = _repository.Delete(id);
                if (_repository.SaveChanges() > 0)
                    return result.ToResult();
            }
            return Results.Fail("Bu işleme yetkiniz bulunmamaktadır");
        }

        /// <summary>
        /// Alınan parametreye göre listelemenin yapıldığı methodtur.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual Result<List<TEntity>> List(Expression<Func<TEntity, bool>> expression)
        {
            return _repository.List(expression).ToList().ToResult();
        }

        /// <summary>
        /// Sorguların listesini veren methodtur..
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> ListForQuery()
        {
            return _repository.List();
        }

        /// <summary>
        /// Id'ye göre sonucun döndürüldüğü methodtur.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual Result<TEntity> SingleOrDefault(int id)
        {
            var entity = _repository.SingleOrDefault(id).ToResult();
            if (IslemYetkisiVarMi(entity.Value))
            {
                return entity;
            }
            return Results.Fail("Bu işleme yetkiniz bulunmamaktadır");
        }

        /// <summary>
        /// Düzenleme işleminin yapıldığı methodtur.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual Result<TEntity> Update(TEntity entity)
        {
            if (IslemYetkisiVarMi(entity))
            {
                TEntity result = null;
                int saveResult = 0;
                _repository.DataContextConfiguration().AutoDetectChangesEnable();
                var dbItem = _repository.SingleOrDefault(entity.TabloID);
                if (dbItem != null)
                {
                    result = _dataMapper.Map(dbItem, entity);
                    saveResult = _repository.SaveChanges();
                }
                _repository.DataContextConfiguration().AutoDetectChangesDisable();
                if (saveResult > 0)
                    return result.ToResult();
            }
            return Results.Fail("Bu işleme yetkiniz bulunmamaktadır");
        }

        /// <summary>
        /// Kullanılmayan kaynakları boşa çıkardıktan sonra sonucu true veya false döndüren methodtur.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                _repository.Dispose();
            }
            this._disposed = true;
        }

        /// <summary>
        /// Kullanılmayan kaynakları boşa çıkaran methodtur.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Uygulamamız arasında sorgulama, güncelleme, silme gibi işlemleri yapmamız için olanak sağlar.
        /// </summary>
        /// <returns></returns>
        public DataContextConfiguration DataContextConfiguration()
        {
            return _repository.DataContextConfiguration();
        }

        /// <summary>
        /// Gelen veriye göre işleme yetkili olup olmadığını kontrol eder
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private /*async*/ bool IslemYetkisiVarMi(TEntity entity)
        {
            if (entity != null)
            {
                
                var gecicikisiList = _loginUser.YetkiliKisiIdleri;
                var gecicikurumList = _loginUser.YetkiliKurumIdleri;
                switch (entity.GetType().Name)
                {
                    case nameof(ErisimYetkilendirmeTanimlari):
                        {
                            var propIlgiliKurum = entity.GetType().GetProperty("IlgiliKurumId");
                            if (_loginUser.KisiID == 0) // Kisi login değilse login işlemi için yetki kontrolü
                                return true;
                            if (gecicikurumList.Any(a => a == entity.KurumID) && gecicikurumList.Any(a => a == (int?)propIlgiliKurum.GetValue(entity)) && gecicikisiList.Any(a => a == entity.KisiID))
                            {
                                return true;
                            }
                            return false;
                        }
                    case nameof(HedefKitleTanimlamalar):
                        {
                            if (gecicikurumList.Any(a => a == entity.KurumID) && gecicikisiList.Any(a => a == entity.KisiID))
                            {
                                return true;
                            }
                            return false;
                        }
                    
                    case nameof(KisiTemelBilgiler):
                        {
                            var propKurum = entity.GetType().GetProperty("KisiBagliOlduguKurumId");
                            if (gecicikurumList.Any(a => a == entity.KurumID)
                                && gecicikurumList.Any(a => a == (int?)propKurum.GetValue(entity)))
                            {
                                return true;
                            }
                            return false;
                        }
                    case nameof(KurumTemelBilgiler):
                        {
                            if (gecicikurumList.Any(a => a == entity.KurumID))
                            {
                                return true;
                            }
                            return false;
                        }
                    case nameof(KurumAdresBilgileri):
                    case nameof(KurumBankaBilgileri):
                        {
                            var prop = entity.GetType().GetProperty("IlgiliKurumId");
                            if (gecicikurumList.Any(a => a == entity.KurumID))
                            {
                                return true;
                            }
                            return false;
                        }
                    
                    case nameof(Iliskiler):
                        {
                            var propBununKisi = entity.GetType().GetProperty("BununKisiId");
                            var propBuKisi = entity.GetType().GetProperty("BuKisiId");
                            var propBununKurum = entity.GetType().GetProperty("BununKurumId");
                            var propBuKurum = entity.GetType().GetProperty("BuKurumId");

                            //kişiler arası ilişki kontrolü
                            if (gecicikurumList.Any(a => a == entity.KurumID) && gecicikisiList.Any(a => a == (int?)propBuKisi.GetValue(entity) && a == (int?)propBununKisi.GetValue(entity)))
                            {
                                return true;
                            }
                            //kurumlar arası ilişki kontrolü
                            if (gecicikurumList.Any(a => a == entity.KurumID) /*&& gecicikurumList.Any(a => a == (int?)propBuKurum.GetValue(entity)) && gecicikurumList.Any(a => a == (int?)propBununKurum.GetValue(entity))*/)
                            {
                                return true;
                            }
                            //kurum - kişi arası ilişki kontrolü
                            if (gecicikurumList.Any(a => a == entity.KurumID)/* && gecicikurumList.Any(a => a == (int?)propBuKurum.GetValue(entity)) && gecicikisiList.Any(a => a == (int?)propBuKisi.GetValue(entity))*/)
                            {
                                return true;
                            }
                            return false;
                        }
                    case nameof(KurumlarKisiler):
                        {
                            if (gecicikurumList.Any(a => a == entity.KurumID))
                            {
                                return true;
                            }
                            return false;
                        }
                    
                    
                    case nameof(KurumOrganizasyonBirimTanimlari):
                        {
                            if (gecicikurumList.Any(a => a == entity.KurumID))
                            {
                                return true;
                            }
                            return false;
                        }
                    //case nameof(MedyaKutuphanesi):
                    //    {
                    //        if (gecicikurumList.Any(a => a == entity.KurumID) && gecicikisiList.Any(a => a == entity.KisiID))
                    //        {
                    //            return true;
                    //        }
                    //        return false;
                    //    }
                    //case nameof(ParamOrganizasyonBirimleri):
                    //    {
                    //        if (lisansId == 1031)
                    //        {
                    //            return true;
                    //        }
                    //        return false;
                    //    }
                
                    default:
                        return true;
                }
            }
            return true;
        }
    }
}