﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Timing;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Json;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;
using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using Castle.MicroKernel.Registration;
using MailKit.Security;
using MyCompanyName.AbpZeroTemplate.Authorization.Delegation;
using MyCompanyName.AbpZeroTemplate.Authorization.Ldap;
using MyCompanyName.AbpZeroTemplate.Authorization.Roles;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using MyCompanyName.AbpZeroTemplate.Chat;
using MyCompanyName.AbpZeroTemplate.Configuration;
using MyCompanyName.AbpZeroTemplate.DashboardCustomization.Definitions;
using MyCompanyName.AbpZeroTemplate.Debugging;
using MyCompanyName.AbpZeroTemplate.DynamicEntityProperties;
using MyCompanyName.AbpZeroTemplate.Features;
using MyCompanyName.AbpZeroTemplate.Friendships;
using MyCompanyName.AbpZeroTemplate.Friendships.Cache;
using MyCompanyName.AbpZeroTemplate.Localization;
using MyCompanyName.AbpZeroTemplate.MultiTenancy;
using MyCompanyName.AbpZeroTemplate.Net.Emailing;
using MyCompanyName.AbpZeroTemplate.Notifications;
using MyCompanyName.AbpZeroTemplate.WebHooks;
using Newtonsoft.Json;

namespace MyCompanyName.AbpZeroTemplate
{
    [DependsOn(
        typeof(AbpZeroTemplateCoreSharedModule),
        typeof(AbpZeroCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetZeroCoreModule),
        typeof(AbpMailKitModule))]
    public class AbpZeroTemplateCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //workaround for issue: https://github.com/aspnet/EntityFrameworkCore/issues/9825
            //related github issue: https://github.com/aspnet/EntityFrameworkCore/issues/10407
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            AbpZeroTemplateLocalizationConfigurer.Configure(Configuration.Localization);

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Adding webhook definition providers
            Configuration.Webhooks.Providers.Add<AppWebhookDefinitionProvider>();
            Configuration.Webhooks.TimeoutDuration = TimeSpan.FromMinutes(1);
            Configuration.Webhooks.IsAutomaticSubscriptionDeactivationEnabled = false;

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = AbpZeroTemplateConsts.MultiTenancyEnabled;

            //Enable LDAP authentication 
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Twilio - Enable this line to activate Twilio SMS integration
            //Configuration.ReplaceService<ISmsSender,TwilioSmsSender>();

            //Adding DynamicEntityParameters definition providers
            Configuration.DynamicEntityProperties.Providers.Add<AppDynamicEntityPropertyDefinitionProvider>();

            // MailKit configuration
            Configuration.Modules.AbpMailKit().SecureSocketOption = SecureSocketOptions.Auto;
            Configuration.ReplaceService<IMailKitSmtpBuilder, AbpZeroTemplateMailKitSmtpBuilder>(DependencyLifeStyle.Transient);

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            if (DebugHelper.IsDebug)
            {
                //Disabling email sending in debug mode
                Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            }

            Configuration.ReplaceService(typeof(IEmailSenderConfiguration), () =>
            {
                Configuration.IocManager.IocContainer.Register(
                    Component.For<IEmailSenderConfiguration, ISmtpEmailSenderConfiguration>()
                             .ImplementedBy<AbpZeroTemplateSmtpEmailSenderConfiguration>()
                             .LifestyleTransient()
                );
            });

            Configuration.Caching.Configure(FriendCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30);
            });

            IocManager.Register<DashboardConfiguration>();
            
            Configuration.Notifications.Notifiers.Add<SmsRealTimeNotifier>();
            Configuration.Notifications.Notifiers.Add<EmailRealTimeNotifier>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTemplateCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IChatCommunicator, NullChatCommunicator>();
            IocManager.Register<IUserDelegationConfiguration, UserDelegationConfiguration>();

            IocManager.Resolve<ChatUserStateWatcher>().Initialize();
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
