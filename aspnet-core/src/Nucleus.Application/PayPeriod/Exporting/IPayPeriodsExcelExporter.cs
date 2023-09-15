using System.Collections.Generic;
using Nucleus.PayPeriod.Dtos;
using Nucleus.Dto;

namespace Nucleus.PayPeriod.Exporting
{
    public interface IPayPeriodsExcelExporter
    {
        FileDto ExportToFile(List<GetPayPeriodsForViewDto> payPeriods);
    }
}