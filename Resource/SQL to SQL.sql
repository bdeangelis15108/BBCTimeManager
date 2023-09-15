CREATE TRIGGER t_AfterInsert ON [dbo].[JCCAT]
AFTER INSERT

AS BEGIN
	declare @jobNum nvarchar(100);
	declare @phaseNum nvarchar(50);
	declare @categoryNum nvarchar(100);

	SELECT @jobNum = JOBNUM FROM inserted;
	SELECT @phaseNum = PHASENUM from inserted;
	SELECT @categoryNum = CATNUM from inserted;

	if NOT EXISTS (select PHASENUM from JCCAT where PHASENUM=@phaseNum )
		/* insert new phase number into  */

	if NOT EXISTS (select * from JobCategories where Code=@categoryNum)
		/* insert new Category number to jobCategories Table's columns code and name */
	if NOT EXISTS ( SELECT * FROM [BBCNucleus].[dbo].Jobses WHERE Code=@jobNum
		/* Insert*/

	if NOT EXISTS (SELECT * FROM [BBCNucleus].[dbo].[JobPhaseCodes] as je join Jobses as j on j.Id= je.JobsId where j.Code = @jobNum)
		/**/
		
END