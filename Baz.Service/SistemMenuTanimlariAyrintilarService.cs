using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Microsoft.Extensions.Logging;
using System;

namespace Baz.Service
{
    public interface ISistemMenuTanimlariAyrintilarService: Base.IService<SistemMenuTanimlariAyrintilar>
    {
    }
    public class SistemMenuTanimlariAyrintilarService : Base.Service<SistemMenuTanimlariAyrintilar>, ISistemMenuTanimlariAyrintilarService
    {

        public SistemMenuTanimlariAyrintilarService(IRepository<SistemMenuTanimlariAyrintilar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SistemMenuTanimlariAyrintilar> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }

}