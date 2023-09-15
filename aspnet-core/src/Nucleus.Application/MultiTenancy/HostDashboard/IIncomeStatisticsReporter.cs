using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nucleus.MultiTenancy.HostDashboard.Dto;

namespace Nucleus.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}