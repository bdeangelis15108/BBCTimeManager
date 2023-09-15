using System.Collections.Generic;
using Nucleus.Chat.Dto;
using Nucleus.Dto;

namespace Nucleus.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
