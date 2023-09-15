import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EquipTimetablesComponent } from './equipTimetable/equipTimetables/equipTimetables.component';
import { TimetablesComponent } from './timetable/timetables/timetables.component';
import { CostTypeseComponent } from './costType/costTypese/costTypese.component';
import { AccountsComponent } from './account/accounts/accounts.component';
import { UnionPayRatesComponent } from './unionPayRate/unionPayRates/unionPayRates.component';
import { PayperiodHistoriesComponent } from './payperiodHistory/payperiodHistories/payperiodHistories.component';
import { StatusUpdatesComponent } from './statusUpdate/statusUpdates/statusUpdates.component';
import { ShiftExpensesComponent } from './shiftExpense/shiftExpenses/shiftExpenses.component';
import { EmployeeUnionsComponent } from './employeeUnion/employeeUnions/employeeUnions.component';
import { JobUnionsComponent } from './jobUnion/jobUnions/jobUnions.component';
import { UnionsComponent } from './union/unions/unions.component';
import { ShiftResourcesComponent } from './shiftResource/shiftResources/shiftResources.component';
import { JobCategoriesComponent } from './jobCategory/jobCategories/jobCategories.component';
import { JobPhaseCodesComponent } from './jobPhaseCode/jobPhaseCodes/jobPhaseCodes.component';
import { ShiftsComponent } from './shift/shifts/shifts.component';
import { PayPeriodsComponent } from './payPeriod/payPeriods/payPeriods.component';
import { TimesheetsComponent } from './timesheet/timesheets/timesheets.component';
import { StatusesComponent } from './status/statuses/statuses.component';
import { ExpenseTypesesComponent } from './expenseType/expenseTypeses/expenseTypeses.component';
import { PayTypesesComponent } from './payType/payTypeses/payTypeses.component';
import { ResourceReservationsesComponent } from './resourceReservation/resourceReservationses/resourceReservationses.component';
import { JobsesComponent } from './job/jobses/jobses.component';
import { JobClassesesComponent } from './jobClass/jobClasseses/jobClasseses.component';
import { AddressesesComponent } from './address/addresseses/addresseses.component';
import { ResourceEquipmentInfosesComponent } from './resourceEquipmentInfo/resourceEquipmentInfoses/resourceEquipmentInfoses.component';
import { ResourceWorkerInfosesComponent } from './resourceWorkerInfo/resourceWorkerInfoses/resourceWorkerInfoses.component';
import { WorkerClaseesesComponent } from './workerClasee/workerClaseeses/workerClaseeses.component';
import { ResourcesesComponent } from './resource/resourceses/resourceses.component';
import { ECCOSTSComponent } from './eccosts/eccosts/eccosts.component';
import { EQUIPMENTSComponent } from './equipments/equipments/equipments.component';
import { PRDEDRATESComponent } from './prdedrates/prdedrates/prdedrates.component';
import { JCJOBsComponent } from './jcjobs/jcjoBs/jcjoBs.component';
import { PRCLASSsComponent } from './prclasses/prclasSs/prclasSs.component';
import { JCUNIONsComponent } from './jcunions/jcunioNs/jcunioNs.component';
import { JACCATsComponent } from './jccat/jaccaTs/jaccaTs.component';
import { PREMPLOYEESComponent } from './premployees/premployees/premployees.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'equipTimetable/equipTimetables', component: EquipTimetablesComponent, data: { permission: 'Pages.EquipTimetables' }  },
                    { path: 'timetable/timetables', component: TimetablesComponent, data: { permission: 'Pages.Timetables' }  },
                    { path: 'costType/costTypese', component: CostTypeseComponent, data: { permission: 'Pages.CostTypese' }  },
                    { path: 'account/accounts', component: AccountsComponent, data: { permission: 'Pages.Accounts' }  },                    
                    { path: 'unionPayRate/unionPayRates', component: UnionPayRatesComponent, data: { permission: 'Pages.UnionPayRates' }  },
                    { path: 'payperiodHistory/payperiodHistories', component: PayperiodHistoriesComponent, data: { permission: 'Pages.PayperiodHistories' }  },
                    { path: 'statusUpdate/statusUpdates', component: StatusUpdatesComponent, data: { permission: 'Pages.StatusUpdates' }  },
                    { path: 'shiftExpense/shiftExpenses', component: ShiftExpensesComponent, data: { permission: 'Pages.ShiftExpenses' }  },
                    { path: 'employeeUnion/employeeUnions', component: EmployeeUnionsComponent, data: { permission: 'Pages.EmployeeUnions' }  },
                    { path: 'jobUnion/jobUnions', component: JobUnionsComponent, data: { permission: 'Pages.JobUnions' }  },
                    { path: 'union/unions', component: UnionsComponent, data: { permission: 'Pages.Unions' }  },
                    { path: 'shiftResource/shiftResources', component: ShiftResourcesComponent, data: { permission: 'Pages.ShiftResources' }  },
                    { path: 'jobCategory/jobCategories', component: JobCategoriesComponent, data: { permission: 'Pages.JobCategories' }  },
                    { path: 'jobPhaseCode/jobPhaseCodes', component: JobPhaseCodesComponent, data: { permission: 'Pages.JobPhaseCodes' }  },
                    { path: 'shift/shifts', component: ShiftsComponent, data: { permission: 'Pages.Shifts' }  },
                    { path: 'payPeriod/payPeriods', component: PayPeriodsComponent, data: { permission: 'Pages.PayPeriods' }  },
                    { path: 'timesheet/timesheets', component: TimesheetsComponent, data: { permission: 'Pages.Timesheets' }  },
                    { path: 'status/statuses', component: StatusesComponent, data: { permission: 'Pages.Statuses' }  },
                    { path: 'expenseType/expenseTypeses', component: ExpenseTypesesComponent, data: { permission: 'Pages.ExpenseTypeses' }  },
                    { path: 'payType/payTypeses', component: PayTypesesComponent, data: { permission: 'Pages.PayTypeses' }  },
                    { path: 'resourceReservation/resourceReservationses', component: ResourceReservationsesComponent, data: { permission: 'Pages.ResourceReservationses' }  },
                    { path: 'job/jobses', component: JobsesComponent, data: { permission: 'Pages.Jobses' }  },
                    { path: 'jobClass/jobClasseses', component: JobClassesesComponent, data: { permission: 'Pages.JobClasseses' }  },
                    { path: 'address/addresseses', component: AddressesesComponent, data: { permission: 'Pages.Addresseses' }  },
                    { path: 'resourceEquipmentInfo/resourceEquipmentInfoses', component: ResourceEquipmentInfosesComponent, data: { permission: 'Pages.ResourceEquipmentInfoses' }  },
                    { path: 'resourceWorkerInfo/resourceWorkerInfoses', component: ResourceWorkerInfosesComponent, data: { permission: 'Pages.ResourceWorkerInfoses' }  },
                    { path: 'workerClasee/workerClaseeses', component: WorkerClaseesesComponent, data: { permission: 'Pages.WorkerClaseeses' }  },
                    { path: 'resource/resourceses', component: ResourcesesComponent, data: { permission: 'Pages.Resourceses' }  },
                    { path: 'eccosts/eccosts', component: ECCOSTSComponent, data: { permission: 'Pages.ECCOSTS' }  },
                    { path: 'equipments/equipments', component: EQUIPMENTSComponent, data: { permission: 'Pages.EQUIPMENTS' }  },
                    { path: 'prdedrates/prdedrates', component: PRDEDRATESComponent, data: { permission: 'Pages.PRDEDRATES' }  },
                    { path: 'jcjobs/jcjoBs', component: JCJOBsComponent, data: { permission: 'Pages.JCJOBs' }  },
                    { path: 'prclasses/prclasSs', component: PRCLASSsComponent, data: { permission: 'Pages.PRCLASSs' }  },
                    { path: 'jcunions/jcunioNs', component: JCUNIONsComponent, data: { permission: 'Pages.JCUNIONs' }  },
                    { path: 'jccat/jaccaTs', component: JACCATsComponent, data: { permission: 'Pages.JACCATs' }  },
                    { path: 'premployees/premployees', component: PREMPLOYEESComponent, data: { permission: 'Pages.PREMPLOYEES' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
