using System.Collections.Generic;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.Dto;

namespace Nucleus.ResourceReservation.Exporting
{
    public interface IResourceReservationsesExcelExporter
    {
        FileDto ExportToFile(List<GetResourceReservationsForViewDto> resourceReservationses);
    }
}