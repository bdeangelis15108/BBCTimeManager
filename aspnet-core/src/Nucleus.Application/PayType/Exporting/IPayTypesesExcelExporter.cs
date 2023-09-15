using System.Collections.Generic;
using Nucleus.PayType.Dtos;
using Nucleus.Dto;

namespace Nucleus.PayType.Exporting
{
    public interface IPayTypesesExcelExporter
    {
        FileDto ExportToFile(List<GetPayTypesForViewDto> payTypeses);
    }
}