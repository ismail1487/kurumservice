using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Service;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baz.Service
{
    public interface ISliderResimlerMedyaService : IService<SliderResimlerMedya>
    { 
    }

/// <summary>
/// SliderResimlerMedya ile ilgili işlevleri barındıran, <see cref="ISliderResimlerMedyaService"/> interface'ini baz alan class.
/// </summary>
public class SliderResimlerMedyaService : Service<SliderResimlerMedya>, ISliderResimlerMedyaService
{
    /// <summary>
    /// SliderResimlerMedya ile ilgili işlevleri barındıran servisin yapıcı metodu
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="dataMapper"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    public SliderResimlerMedyaService(IRepository<SliderResimlerMedya> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SliderResimlerMedyaService> logger) : base(repository, dataMapper, serviceProvider, logger)
    {
    }

}
}
