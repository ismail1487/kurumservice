using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Decor;
using Microsoft.AspNetCore.Http;
using Baz.AOP.Logger.ExceptionLog;
using Baz.AOP.Logger.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Baz.RequestManager.Abstracts;
using Baz.RequestManager;
using Baz.Service;
using Baz.Model.Pattern;
using Baz.KurumServiceApi.Handlers;
using Baz.KurumServiceApi;
using System.Diagnostics.CodeAnalysis;
using Baz.Model.Entity.Constants;



using Baz.AletKutusu;

namespace Baz.KurumServiceApi
{
    /// <summary>
    /// Uygulamayý ayaða kalkarken kullanýlacak servislerin belirlendiði middleware sýnýfý
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Uygulamayý ayaða kalkarken kullanýlacak servislerin belirlendiði middleware sýnýfýnýn yapýcý methodu
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables().Build();
        }

        /// <summary>
        /// Uygulamayý yapýlandýran özellik
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            SetCoreURL(Configuration.GetValue<string>("CoreUrl"));
            services.AddHttpContextAccessor();
            services.AddControllers(c => { c.Filters.Add(typeof(ModelValidationFilter), int.MinValue); });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Baz.KurumServiceApi", Version = "v1" });
                c.OperationFilter<DefaultHeaderParameter>();
            });
            services.AddDbContext<Repository.Pattern.IDataContext, Repository.Pattern.Entity.DataContext>(conf => conf.UseSqlServer(Configuration.GetConnectionString("Connection")));
            services.AddSingleton<Baz.Mapper.Pattern.IDataMapper>(new Baz.Mapper.Pattern.Entity.DataMapper(GenerateConfiguratedMapper()));
            //////////////////////////////////////////SESSION SERVER AYARLARI/////////////////////////////////////////////////
            //Distributed session i?lemleri i?in session server?n network ba?lant?lar?n? yap?land?r?r.
            services.AddDistributedSqlServerCache(p =>
            {
                p.ConnectionString = Configuration.GetConnectionString("SessionConnection");
                p.SchemaName = "dbo";
                p.TableName = "SQLSessions";
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Path = "/";
                options.Cookie.Name = "Test.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddSession();
            //Http deste?i olmadan payla??ml? session i?lemleri yapan servisi kay?t eder.
            services.AddTransient<Baz.SharedSession.ISharedSession, Baz.SharedSession.BaseSharedSession>();
            //Http deste?i olan i?lemler i?in payla??ml? session nesnesinin kayd?n? yapar.
            //BaseSharedSessionForHttpRequest i?lemleri i?in ?ncelikle BaseSharedSession servisi kay?t edilmelidir.
            services.AddTransient<ILoginUser, Baz.SharedSession.LoginUserManager>();
            services.AddTransient<Baz.SharedSession.ISharedSessionForHttpRequest, Baz.SharedSession.BaseSharedSessionForHttpRequest>();
            //////////////////////////////////////////////////////////////////////////////////////
            services.AddScoped<ISistemSayfalariService, SistemSayfalariService>();
            services.AddScoped<ISistemMenuTanimlariAyrintilarService, SistemMenuTanimlariAyrintilarService>();
            services.AddScoped<IParamOrganizasyonBirimleriService, ParamOrganizasyonBirimleriService>();
            services.AddScoped<IParamIcerikTipleriService, ParamIcerikTipleriService>();
            services.AddScoped<IParamIcerikKategorileriService, ParamIcerikKategorileriService>();
            services.AddScoped<IParamParaBirimleriService, ParamParaBirimleriService>();
            services.AddScoped<IParamDisPlatformlarService, ParamDisPlatformlarService>();
            services.AddScoped<IParamDillerService, ParamDillerService>();
            services.AddScoped<IMedyaKutuphanesiService, MedyaKutuphanesiService>();
            services.AddScoped<IKurumIliskiService, KurumIliskiService>();
            services.AddScoped<IKurumlarKisilerService, KurumlarKisilerService>();
            services.AddScoped<IKurumService, KurumService>();
            services.AddScoped<IParamBankalarService, ParamBankalarService>();
            services.AddScoped<IKurumOrganizasyonBirimTanimlariService, KurumOrganizasyonBirimTanimlariService>();
            services.AddScoped<IKurumBankaBilgileriService, KurumBankaBilgileriService>();
            services.AddScoped<IKurumAdresBilgileriService, KurumAdresBilgileriService>();
            services.AddScoped<IKisiService, KisiService>();
            services.AddScoped<IKisiIliskiService, KisiIliskiService>();
            services.AddScoped<IIcerikKutuphanesiService, IcerikKutuphanesiService>();
            services.AddScoped<IIcerikKutuphanesiMedyalarService, IcerikKutuphanesiMedyalarService>();
            services.AddScoped<IIcerikGrubuEssizBilgilerService, IcerikGrubuEssizBilgilerService>();
            services.AddScoped<IErisimYetkilendirmeTanimlariService, ErisimYetkilendirmeTanimlariService>();
            services.AddScoped<IParamUrunKategorilerService, ParamUrunKategorilerService> ();
            services.AddScoped<IUrunKutuphanesiService, UrunKutuphanesiService>();
            services.AddScoped<IUrunKutuphanesiMedyalarService, UrunKutuphanesiMedyalarService>();
            services.AddScoped<IUrunParametrelerService, UrunParametrelerService>();
            services.AddScoped<IUrunIcerikBloklariMedyalarService, UrunIcerikBloklariMedyalarService>();
            services.AddScoped<IUrunIcerikBloklariService, UrunIcerikBloklariService>();
            services.AddScoped<IParamUrunMarkalarService, ParamUrunMarkalarService>();
            services.AddScoped<IParamIcerikBlokKategorileriService, ParamIcerikBlokKategorileriService>();
            services.AddScoped<IKaynakRezerveTanimlariService, KaynakRezerveTanimlariService>();
            services.AddScoped<IKaynakTanimlariService, KaynakTanimlariService>();
            services.AddScoped<IParamKaynakTipiService, ParamKaynakTipiService>();
            services.AddScoped<IKaynakGunIciIstisnaTanimlariService, KaynakGunIciIstisnaTanimlariService>();

            //////////////////////////////////////////////////////////////////////////////////////
            services.AddScoped<Repository.Pattern.IUnitOfWork, Repository.Pattern.Entity.UnitOfWork>();
            services.AddScoped(typeof(Repository.Pattern.IRepository<>), typeof(Repository.Pattern.Entity.Repository<>));
            services.AddScoped(typeof(Service.Base.IService<>), typeof(Service.Base.Service<>));
            services.AddScoped<IYetkiMerkeziService, YetkiMerkeziService>();
            services.AddTransient<IRequestHelper, RequestHelper>(provider =>
            {
                return new RequestHelper("", new RequestManagerHeaderHelperForHttp(provider).SetDefaultHeader());
            });
            var types = typeof(Service.Base.IService<>).Assembly.GetTypes();
            var interfaces = types.Where(p => p.IsInterface && p.GetInterface("IService`1") != null).ToList();

            //Exception loglar?n? i?leyen Baz.AOP.Logger.ExceptionLog servisinin kayd?n? yapar
            services.AddAOPExceptionLogging();
            //Http i?lemleri i?in loglama yapan BaseHttpLogger servisinin kayd?n? yapar.
            services.AddControllers();
            foreach (var item in interfaces)
            {
                var serviceTypes = types.Where(p => p.GetInterface(item.Name) != null && !p.IsInterface).ToList();
                serviceTypes.ForEach(p => services.AddScoped(item, p).Decorated());
            }
            services.AddResponseCompression();
            //BaseHttpLogger nesnesini b?t?n controllerlar i?in filter servisi olarak tan?mlan?r.

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(
                         "http://172.16.31.10:51307",
                            "https://digiforceweb.mayaict.com.tr",
                            "https://digiforceweb.mayaict.com.tr:51309",
                            "http://172.16.31.20:51305",
                            "http://172.16.31.20:51309",
                            "http://localhost:51307"
                        )
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((x) => true)
                        .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// Bu method çalýþma zamaný tarafýndan çaðrýlýr. HTTP istek ardýþýk düzenini yapýlandýrmak için bu yöntemi kullanýn.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        /// <param name="cache"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime,
            IDistributedCache cache)
        {
            // Configure the Localization middleware
            app.UseRequestLocalization();
            ////////////////////////////////// SESSION SERVER AYARLARI/////////////////////////////////////
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSession();
            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });
            /////////////////////////////////////////////////////////////////////////////////////

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Baz.KurumServiceApi v1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<AuthMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors();
        }

        private Profile GenerateConfiguratedMapper()
        {
            var mapper = Baz.Mapper.Pattern.Entity.DataMapperProfile.GenerateProfile();

            return mapper;
        }

        private static void SetCoreURL(string url)
        {
            LocalPortlar.CoreUrl = url;
        }
    }
}