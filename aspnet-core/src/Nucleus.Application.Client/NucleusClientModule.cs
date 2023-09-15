using Abp.Modules;
using Abp.Reflection.Extensions;
using Nucleus.EmployeeUnion;
using Nucleus.ExpenseType;
using Nucleus.ExpenseTypeses;
using Nucleus.Job;
using Nucleus.JobCategory;
using Nucleus.JobPhaseCode;
using Nucleus.JobPhaseCodes;
using Nucleus.Jobses;
using Nucleus.JobUnion;
using Nucleus.PayPeriod;
using Nucleus.PayperiodHistory;
using Nucleus.PayPeriods;
using Nucleus.PayType;
using Nucleus.PayTypes;
using Nucleus.Resource;
using Nucleus.ResourceEquipmentInfo;
using Nucleus.ResourceEquipmentsInfo;
using Nucleus.ResourceReservation;
using Nucleus.Resourceses;
using Nucleus.ResourceWorkerInfo;
using Nucleus.Shift;
using Nucleus.ShiftExpense;
using Nucleus.ShiftResource;
using Nucleus.Shifts;
using Nucleus.StatusUpdate;
using Nucleus.Timesheet;
using Nucleus.Timesheets;
using Nucleus.WorkerClasee;

namespace Nucleus
{
    public class NucleusClientModule : AbpModule
    {
        public override void Initialize()
        {
            //Register dbo.Resources 
            IocManager.Register<IResourcesesAppService, ProxyResourcesesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.ExpensesTypes 
            IocManager.Register<IExpenseTypesesAppService, ProxyExpenseTypesesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.ResourcesReservations
            IocManager.Register<IResourceReservationsesAppService, ProxyResourceReservationsesAppService>(Abp.Dependency.DependencyLifeStyle.Transient);
            //Register dbo.PayTypes
            IocManager.Register<IPayTypesesAppService, ProxyPayTypesesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.Payperiod History
            IocManager.Register<IPayperiodHistoriesAppService, ProxyPayperiodHistoriesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.Resources Worker Information (Employees)
            IocManager.Register<IResourceWorkerInfosesAppService, ProxyResourceWorkerInfosesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.Resources Equipment 
            IocManager.Register<IResourceEquipmentInfosesAppService, ProxyResourceEquipmentInfosesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Register dbo.Jobs
            IocManager.Register<IJobsesAppService, ProxyJobsesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //PayPeriods
            IocManager.Register<IPayPeriodsAppService, ProxyPayPeriodsAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //JobPhaseCode
            IocManager.Register<IJobPhaseCodesAppService, ProxyJobPhaseCodesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //JobCategoryCodes
            IocManager.Register<IJobCategoriesAppService, ProxyJobCategoriesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            // (Timesheet )
            IocManager.Register<ITimesheetsAppService, ProxyTimesheetsAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Shifts
            IocManager.Register<IShiftsAppService, ProxyShiftsAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Shift Resource 
            IocManager.Register<IShiftResourcesAppService, ProxyShiftResourcesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            // Employees Unions
            IocManager.Register<IEmployeeUnionsAppService, ProxyEmployeeUnionsAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Jobs Union
            IocManager.Register<IJobUnionsAppService, ProxyJobUnionsAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Shift Expense 
            IocManager.Register<IShiftExpensesAppService, ProxyShiftExpensesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //StatusUpdate
            IocManager.Register<IStatusUpdatesAppService, ProxyStatusUpdatesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //StatusUpdate
            IocManager.Register<IWorkerClaseesesAppService, ProxyWorkerClaseesesAppService>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //Default services Registered
            IocManager.RegisterAssemblyByConvention(typeof(NucleusClientModule).GetAssembly());
        }
    }
}
