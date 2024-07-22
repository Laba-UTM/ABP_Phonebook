using Abp.Auditing;
using Abp.Authorization;
using Abp.Timing;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Authorization.Accounts.Dto;
using MyCompanyName.AbpZeroTemplate.PhoneBook;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using MyCompanyName.AbpZeroTemplate.Tenants.Dashboard.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace MyCompanyName.AbpZeroTemplate.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : AbpZeroTemplateAppServiceBase, ITenantDashboardAppService
    {
        private readonly PersonAppService _personAppService;
        public TenantDashboardAppService(PersonAppService personAppService)
        {
            _personAppService = personAppService;
        }

        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return new GetTopStatsOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500)
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod);
            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(0, 3000)
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }

        public GetHelloWorldOutput GetHelloWorldData(GetHelloWorldInput input)
        {
            return new GetHelloWorldOutput()
            {
                OutPutName = "Hello " + input.Name + " (" + Clock.Now.Millisecond + ")"
            };
        }

        public GetPhoneBookOutput GetPhoneBookPerson(GetPhoneBookInput input) 
        {
            var people = _personAppService.GetPersonsFilteredQuery(new GetPeopleInput() { Filter = input.Filter });
            Person person = people.FirstOrDefault();
            if (person != null)
            {
                List<PhoneInPersonListDto> business = new List<PhoneInPersonListDto>();
                List<PhoneInPersonListDto> mobile = new List<PhoneInPersonListDto>();
                List<PhoneInPersonListDto> home = new List<PhoneInPersonListDto>();

                foreach (var phone in person.Phones)
                {
                    switch (phone.Type)
                    {
                        case PhoneType.Business: 
                            business.Add(new PhoneInPersonListDto
                        {
                            Type = PhoneType.Business,
                            Number = phone.Number
                        }); 
                            break;
                        case PhoneType.Mobile:
                            mobile.Add(new PhoneInPersonListDto
                            {
                                Type = PhoneType.Mobile,
                                Number = phone.Number
                            }); 
                            break;
                        case PhoneType.Home:
                            home.Add(new PhoneInPersonListDto
                            {
                                Type = PhoneType.Home,
                                Number = phone.Number
                            }); 
                            break;
                    }
                }

                return new GetPhoneBookOutput()
                {
                    Name = person.Name,
                    BusinessPhones = business,
                    MobilePhones = mobile,  
                    HomePhones = home
                };
            }
            else return new GetPhoneBookOutput() { }; 
        }
    }
}