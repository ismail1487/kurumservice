using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gelf.Extensions.Logging;
using System.Reflection;
using Baz.AOP.Logger.ExceptionLog;
using System.Diagnostics.CodeAnalysis;

namespace Baz.KurumServiceApi
{
    /// <summary>
    /// API'ýn çalýþmasý için gereken Main() methodunu barýndýran class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Servisin yapýcý methodu.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Apiyi Host olarak oluþturan method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).BazConfigureLogging(); // Graylog a log yazýlabilmesi için Baz.AOP.Logger.ExceptionLog paketi eklenip BazConfigureLogging() fonksiyonu çaðrýlýr.

                                          // BazConfigureLogging() fonksiyonu graylog için gerekli netwok ayarlarýný yapar. network ayarlarýný appsetting.json dan alýr.
    }
}