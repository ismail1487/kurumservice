using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.KurumServiceApi;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.RequestManager;
using Baz.RequestManager.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KurumServiceTest.Helper
{
    /// <summary>
    /// Test servisi için kullanılan helper
    /// </summary>
    public class TestServerRequestHelper2
    {  /// Test servisi için kullanılan helper yapıcı metodu
       ///  </summary>
        protected TestServerRequestHelper2()
        {
        }

        /// <summary>
        /// Test için RequestHelper oluşturma
        /// </summary>
        /// <returns></returns>
        public static IRequestHelper CreateHelper(TestLoginUserManager2 manager = null)
        {
            var _server = new TestServer(new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.json")
                    .Build()
                )
                .ConfigureTestServices(services =>
                {
                    if (manager == null)
                    {
                        services.RemoveAll(typeof(ILoginUser));
                        services.AddTransient<ILoginUser, TestLoginUserManager2>();
                    }
                })
                .UseStartup<Startup>()

            );
            var _client = _server.CreateClient();

            var _helper = new RequestHelper(_client);
            if (_helper._headers == null)
            {
                var x = new RequestHelper().Post<Result<string>>(LocalPortlar.UserLoginregisterService + "/api/LoginRegister/Login", new LoginModel
                {
                    EmailOrUserName = "e@mail.com",
                    Password = "12345Aa!"
                });
                _helper._headers = new RequestHelperHeader();
                _helper._headers.Add("sessionId", x.Result.Value);
            }

            return _helper;
        }

        /// <summary>
        /// Test servisi için default test kullanıcısı
        /// </summary>
        public class TestLoginUserManager2 : ILoginUser
        {
            /// <summary>
            /// Test servisi için default test kullanıcısı yapıcı metodu
            /// </summary>
            public TestLoginUserManager2()
            {
            }

            /// <summary>
            /// Test servisi için default test kullanıcısı yapıcı metodu
            /// </summary>
            public TestLoginUserManager2(int KurumID, int KisiID)
            {
                this.KurumID = KurumID;
                this.KisiID = KisiID;
            }

            /// <summary>
            /// Test için login olan kişlinin default değerleri
            /// </summary>
            public int KurumID { get; set; } = 82;

            /// <summary>
            /// Test için login olan kişlinin default değerleri
            /// </summary>
            public int KisiID { get; set; } = 395;

            public List<int> YetkiliKisiIdleri { get; set; } = new()
            {
                129,
                130,
                210,
                216,
                218,
                236,
                239,
                240,
                242,
                243,
                389,
                392,
                393,
                394,
                395,
                396,
                397,
                398,
                399,
                401,
                403,
                404,
                405,
                406,
                442,
                444,
                445,
                446,
                447,
                488,
                1488,
                1489,
                1490,
                1491,
                1492,
                1493,
                3232,
                3603,
                3942,
                3943,
                4350,
                4450
            };
            public List<int> YetkiliKurumIdleri { get; set; } = new()
            {
                82,
                85,
                2497,
                3448,
                3593,
                3771,
                3792
            };
            public int LisansId { get; set; } = 1031;
        }
    }
}