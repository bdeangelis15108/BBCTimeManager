using System.Collections.Generic;
using Nucleus.UnionPayRate.Dtos;
using Nucleus.Dto;

namespace Nucleus.UnionPayRate.Exporting
{
    public interface IUnionPayRatesExcelExporter
    {
        FileDto ExportToFile(List<GetUnionPayRatesForViewDto> unionPayRates);
    }
}