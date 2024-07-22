using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Castle.Core.Logging;
using MyCompanyName.AbpZeroTemplate.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Widget
{
    public class BackgroungWorkerPhoneBook(ILogger logger)
    {
        private static int _latestNumber;
        private readonly Random _random = new();

        public void Execute()
        {
            _latestNumber = _random.Next(1, 101);
            logger.Debug($"Generated number: {_latestNumber}");
        }
        public int GetLatestNumber()
        {
            return _latestNumber;
        }
    }
}
