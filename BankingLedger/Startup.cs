using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BankingLedger.Startup))]
namespace BankingLedger
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
