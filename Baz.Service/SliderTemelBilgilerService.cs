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

namespace Baz.Service
{
    public interface ISliderTemelBilgilerService : IService<SliderTemelBilgiler> { 
    }

    /// <summary>
    /// SliderTemelBilgiler ile ilgili işlevleri barındıran, <see cref="ISliderTemelBilgilerService"/> interface'ini baz alan class.
    /// </summary>
    public class SliderTemelBilgilerService : Service<SliderTemelBilgiler>, ISliderTemelBilgilerService
    {
        /// <summary>
        /// SliderTemelBilgiler ile ilgili işlevleri barındıran servisin yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public SliderTemelBilgilerService(IRepository<SliderTemelBilgiler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SliderTemelBilgilerService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

    }
}
