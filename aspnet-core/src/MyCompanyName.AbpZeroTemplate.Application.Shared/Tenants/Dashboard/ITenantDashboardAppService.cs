﻿using Abp.Application.Services;
using MyCompanyName.AbpZeroTemplate.Tenants.Dashboard.Dto;

namespace MyCompanyName.AbpZeroTemplate.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();

        GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

        GetDailySalesOutput GetDailySales();

        GetProfitShareOutput GetProfitShare();

        GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

        GetTopStatsOutput GetTopStats();

        GetRegionalStatsOutput GetRegionalStats();

        GetGeneralStatsOutput GetGeneralStats();

        GetHelloWorldOutput GetHelloWorldData(GetHelloWorldInput input);

        GetPhoneBookOutput GetPhoneBookPerson(GetPhoneBookInput input);
    }
}
