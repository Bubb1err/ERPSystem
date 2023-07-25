
using ERPSystem.BLL.Services.LoggerManagerService;
using Microsoft.Extensions.DependencyInjection;

namespace ERPSystem.BLL.Extensions
{
    public static class RegisterServicesExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
