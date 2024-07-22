using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Widget;

namespace MyCompanyName.AbpZeroTemplate
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AbpZeroTemplateApplicationSharedModule),
        typeof(AbpZeroTemplateCoreModule)
        )]
    public class AbpZeroTemplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

            // Adding backgroundWorker for Widget PhoneBook
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;
            IocManager.Register<BackgroungWorkerPhoneBook, BackgroungWorkerPhoneBook>(DependencyLifeStyle.Singleton);
            IocManager.Register<BackgroundWorkerHandleCSVFiles, BackgroundWorkerHandleCSVFiles>(DependencyLifeStyle.Singleton);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTemplateApplicationModule).GetAssembly());
        }
    }
}