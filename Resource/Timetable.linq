<Query Kind="Expression">
  <Connection>
    <ID>41a82e84-a9c7-419f-8115-8cea2bb901ac</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>3PT-W350B-HEMAL\MSSQLSERVER01</Server>
    <Database>BBCNucleus</Database>
  </Connection>
</Query>

					Timesheets.ToList()  
                    .OrderBy(x => x.CreatedDate)
                    .SelectMany(timesheet => ShiftResources
                        .Where(shiftResource => shiftResource.TimesheetsId == timesheet.Id)
                        ,
                        (timesheet, shiftResource) => new
                        {
                            Timesheet = timesheet,
                            ShiftResource = shiftResource
                        }
                    )
                    
                    .OrderBy(x => x.ShiftResource.TimesheetsId)
                        .ThenBy(x => x.Timesheet.CreatedDate)
                    .ToList()
                    .GroupBy(item => new
                    {
                        item.Timesheet.Id,
						//item.Timesheet.CreatedDate,
                        item.ShiftResource.ResourcesId,
                        item.ShiftResource.JobCategoriesId,
                        item.ShiftResource.JobPhaseCodesId,
                        item.ShiftResource.PayTypesId,
                        item.ShiftResource.ShiftsId,
                        item.ShiftResource.WorkerClaseesId,
                        AddressId = item.ShiftResource.Shifts.Jobs.Addresses?.Id
                    })
                    .Select(group => new
                    {
                        group.Key,
						
						EmployeeName = group.First().ShiftResource.Resources.Name,
						PayTypesIds = string.Join(",", group.Select(x=>x.ShiftResource.PayTypesId)),
						TimesheetsIds = string.Join(",", group.Select(x=>x.Timesheet.Id)),
                        TotalHours = group.Sum(item => (item.ShiftResource.HoursWorked.HasValue) ? item.ShiftResource.HoursWorked.Value : 0),
                        ShiftResource = group.First().ShiftResource,
                        Timesheet = group.First().Timesheet,
                       
                    }).Where(x => x.Timesheet.CreatedDate == new DateTime(2020, 02, 23))