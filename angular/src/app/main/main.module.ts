import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { EquipTimetablesComponent } from './equipTimetable/equipTimetables/equipTimetables.component';
import { ViewEquipTimetablesModalComponent } from './equipTimetable/equipTimetables/view-equipTimetables-modal.component';
import { CreateOrEditEquipTimetablesModalComponent } from './equipTimetable/equipTimetables/create-or-edit-equipTimetables-modal.component';
import { EquipTimetablesPayPeriodsLookupTableModalComponent } from './equipTimetable/equipTimetables/equipTimetables-payPeriods-lookup-table-modal.component';
import { EquipTimetablesResourcesLookupTableModalComponent } from './equipTimetable/equipTimetables/equipTimetables-resources-lookup-table-modal.component';
import { EquipTimetablesJobPhaseCodesLookupTableModalComponent } from './equipTimetable/equipTimetables/equipTimetables-jobPhaseCodes-lookup-table-modal.component';
import { EquipTimetablesJobCategoriesLookupTableModalComponent } from './equipTimetable/equipTimetables/equipTimetables-jobCategories-lookup-table-modal.component';
import { EquipTimetablesJobsLookupTableModalComponent } from './equipTimetable/equipTimetables/equipTimetables-jobs-lookup-table-modal.component';

import { TimetablesWorkerClaseesLookupTableModalComponent } from './timetable/timetables/timetables-workerClasees-lookup-table-modal.component';

import { TimetablesPayTypesLookupTableModalComponent } from './timetable/timetables/timetables-payTypes-lookup-table-modal.component';

import { ShiftResourcesWorkerClaseesLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-workerClasees-lookup-table-modal.component';

import { ResourceWorkerInfosUserLookupTableModalComponent } from './resourceWorkerInfo/resourceWorkerInfoses/resourceWorkerInfos-user-lookup-table-modal.component';

import { TimetablesComponent } from './timetable/timetables/timetables.component';
import { ViewTimetablesModalComponent } from './timetable/timetables/view-timetables-modal.component';
import { CreateOrEditTimetablesModalComponent } from './timetable/timetables/create-or-edit-timetables-modal.component';
import { TimetablesPayPeriodsLookupTableModalComponent } from './timetable/timetables/timetables-payPeriods-lookup-table-modal.component';
import { TimetablesResourcesLookupTableModalComponent } from './timetable/timetables/timetables-resources-lookup-table-modal.component';
import { TimetablesUnionPayRatesLookupTableModalComponent } from './timetable/timetables/timetables-unionPayRates-lookup-table-modal.component';
import { TimetablesUnionsLookupTableModalComponent } from './timetable/timetables/timetables-unions-lookup-table-modal.component';
import { TimetablesAddressesLookupTableModalComponent } from './timetable/timetables/timetables-addresses-lookup-table-modal.component';
import { TimetablesExpenseTypesLookupTableModalComponent } from './timetable/timetables/timetables-expenseTypes-lookup-table-modal.component';
import { TimetablesCostTypesLookupTableModalComponent } from './timetable/timetables/timetables-costTypes-lookup-table-modal.component';
import { TimetablesAccountsLookupTableModalComponent } from './timetable/timetables/timetables-accounts-lookup-table-modal.component';
import { TimetablesUserLookupTableModalComponent } from './timetable/timetables/timetables-user-lookup-table-modal.component';


import { CostTypeseComponent } from './costType/costTypese/costTypese.component';
import { ViewCostTypesModalComponent } from './costType/costTypese/view-costTypes-modal.component';
import { CreateOrEditCostTypesModalComponent } from './costType/costTypese/create-or-edit-costTypes-modal.component';

import { AccountsComponent } from './account/accounts/accounts.component';
import { ViewAccountsModalComponent } from './account/accounts/view-accounts-modal.component';
import { CreateOrEditAccountsModalComponent } from './account/accounts/create-or-edit-accounts-modal.component';


import { UnionPayRatesComponent } from './unionPayRate/unionPayRates/unionPayRates.component';
import { ViewUnionPayRatesModalComponent } from './unionPayRate/unionPayRates/view-unionPayRates-modal.component';
import { CreateOrEditUnionPayRatesModalComponent } from './unionPayRate/unionPayRates/create-or-edit-unionPayRates-modal.component';
import { UnionPayRatesUnionsLookupTableModalComponent } from './unionPayRate/unionPayRates/unionPayRates-unions-lookup-table-modal.component';

import { PayperiodHistoriesComponent } from './payperiodHistory/payperiodHistories/payperiodHistories.component';
import { ViewPayperiodHistoriesModalComponent } from './payperiodHistory/payperiodHistories/view-payperiodHistories-modal.component';
import { CreateOrEditPayperiodHistoriesModalComponent } from './payperiodHistory/payperiodHistories/create-or-edit-payperiodHistories-modal.component';
import { PayperiodHistoriesPayPeriodsLookupTableModalComponent } from './payperiodHistory/payperiodHistories/payperiodHistories-payPeriods-lookup-table-modal.component';

import { StatusUpdatesComponent } from './statusUpdate/statusUpdates/statusUpdates.component';
import { ViewStatusUpdatesModalComponent } from './statusUpdate/statusUpdates/view-statusUpdates-modal.component';
import { CreateOrEditStatusUpdatesModalComponent } from './statusUpdate/statusUpdates/create-or-edit-statusUpdates-modal.component';
import { StatusUpdatesTimesheetsLookupTableModalComponent } from './statusUpdate/statusUpdates/statusUpdates-timesheets-lookup-table-modal.component';
import { StatusUpdatesStatusesLookupTableModalComponent } from './statusUpdate/statusUpdates/statusUpdates-statuses-lookup-table-modal.component';
import { StatusUpdatesJobsLookupTableModalComponent } from './statusUpdate/statusUpdates/statusUpdates-jobs-lookup-table-modal.component';
import { StatusUpdatesUserLookupTableModalComponent } from './statusUpdate/statusUpdates/statusUpdates-user-lookup-table-modal.component';

import { ShiftExpensesComponent } from './shiftExpense/shiftExpenses/shiftExpenses.component';
import { ViewShiftExpensesModalComponent } from './shiftExpense/shiftExpenses/view-shiftExpenses-modal.component';
import { CreateOrEditShiftExpensesModalComponent } from './shiftExpense/shiftExpenses/create-or-edit-shiftExpenses-modal.component';
import { ShiftExpensesShiftResourcesLookupTableModalComponent } from './shiftExpense/shiftExpenses/shiftExpenses-shiftResources-lookup-table-modal.component';
import { ShiftExpensesExpenseTypesLookupTableModalComponent } from './shiftExpense/shiftExpenses/shiftExpenses-expenseTypes-lookup-table-modal.component';

import { EmployeeUnionsComponent } from './employeeUnion/employeeUnions/employeeUnions.component';
import { ViewEmployeeUnionsModalComponent } from './employeeUnion/employeeUnions/view-employeeUnions-modal.component';
import { CreateOrEditEmployeeUnionsModalComponent } from './employeeUnion/employeeUnions/create-or-edit-employeeUnions-modal.component';
import { EmployeeUnionsUnionsLookupTableModalComponent } from './employeeUnion/employeeUnions/employeeUnions-unions-lookup-table-modal.component';
import { EmployeeUnionsResourcesLookupTableModalComponent } from './employeeUnion/employeeUnions/employeeUnions-resources-lookup-table-modal.component';

import { JobUnionsComponent } from './jobUnion/jobUnions/jobUnions.component';
import { ViewJobUnionsModalComponent } from './jobUnion/jobUnions/view-jobUnions-modal.component';
import { CreateOrEditJobUnionsModalComponent } from './jobUnion/jobUnions/create-or-edit-jobUnions-modal.component';
import { JobUnionsJobsLookupTableModalComponent } from './jobUnion/jobUnions/jobUnions-jobs-lookup-table-modal.component';
import { JobUnionsUnionsLookupTableModalComponent } from './jobUnion/jobUnions/jobUnions-unions-lookup-table-modal.component';

import { UnionsComponent } from './union/unions/unions.component';
import { ViewUnionsModalComponent } from './union/unions/view-unions-modal.component';
import { CreateOrEditUnionsModalComponent } from './union/unions/create-or-edit-unions-modal.component';

import { JobCategoriesComponent } from './jobCategory/jobCategories/jobCategories.component';
import { ViewJobCategoriesModalComponent } from './jobCategory/jobCategories/view-jobCategories-modal.component';
import { CreateOrEditJobCategoriesModalComponent } from './jobCategory/jobCategories/create-or-edit-jobCategories-modal.component';
import { JobCategoriesJobPhaseCodesLookupTableModalComponent } from './jobCategory/jobCategories/jobCategories-jobPhaseCodes-lookup-table-modal.component';

import { ShiftResourcesComponent } from './shiftResource/shiftResources/shiftResources.component';
import { ViewShiftResourcesModalComponent } from './shiftResource/shiftResources/view-shiftResources-modal.component';
import { CreateOrEditShiftResourcesModalComponent } from './shiftResource/shiftResources/create-or-edit-shiftResources-modal.component';
import { ShiftResourcesResourcesLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-resources-lookup-table-modal.component';
import { ShiftResourcesPayTypesLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-payTypes-lookup-table-modal.component';
import { ShiftResourcesJobPhaseCodesLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-jobPhaseCodes-lookup-table-modal.component';
import { ShiftResourcesJobCategoriesLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-jobCategories-lookup-table-modal.component';
import { ShiftResourcesTimesheetsLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-timesheets-lookup-table-modal.component';
import { ShiftResourcesShiftsLookupTableModalComponent } from './shiftResource/shiftResources/shiftResources-shifts-lookup-table-modal.component';


import { JobPhaseCodesComponent } from './jobPhaseCode/jobPhaseCodes/jobPhaseCodes.component';
import { ViewJobPhaseCodesModalComponent } from './jobPhaseCode/jobPhaseCodes/view-jobPhaseCodes-modal.component';
import { CreateOrEditJobPhaseCodesModalComponent } from './jobPhaseCode/jobPhaseCodes/create-or-edit-jobPhaseCodes-modal.component';
import { JobPhaseCodesJobsLookupTableModalComponent } from './jobPhaseCode/jobPhaseCodes/jobPhaseCodes-jobs-lookup-table-modal.component';

import { ShiftsComponent } from './shift/shifts/shifts.component';
import { ViewShiftsModalComponent } from './shift/shifts/view-shifts-modal.component';
import { CreateOrEditShiftsModalComponent } from './shift/shifts/create-or-edit-shifts-modal.component';
import { ShiftsJobsLookupTableModalComponent } from './shift/shifts/shifts-jobs-lookup-table-modal.component';

import { PayPeriodsComponent } from './payPeriod/payPeriods/payPeriods.component';
import { ViewPayPeriodsModalComponent } from './payPeriod/payPeriods/view-payPeriods-modal.component';
import { CreateOrEditPayPeriodsModalComponent } from './payPeriod/payPeriods/create-or-edit-payPeriods-modal.component';

import { TimesheetsComponent } from './timesheet/timesheets/timesheets.component';
import { ViewTimesheetsModalComponent } from './timesheet/timesheets/view-timesheets-modal.component';
import { CreateOrEditTimesheetsModalComponent } from './timesheet/timesheets/create-or-edit-timesheets-modal.component';
import { TimesheetsStatusesLookupTableModalComponent } from './timesheet/timesheets/timesheets-statuses-lookup-table-modal.component';

import { StatusesComponent } from './status/statuses/statuses.component';
import { ViewStatusesModalComponent } from './status/statuses/view-statuses-modal.component';
import { CreateOrEditStatusesModalComponent } from './status/statuses/create-or-edit-statuses-modal.component';

import { ExpenseTypesesComponent } from './expenseType/expenseTypeses/expenseTypeses.component';
import { ViewExpenseTypesModalComponent } from './expenseType/expenseTypeses/view-expenseTypes-modal.component';
import { CreateOrEditExpenseTypesModalComponent } from './expenseType/expenseTypeses/create-or-edit-expenseTypes-modal.component';

import { PayTypesesComponent } from './payType/payTypeses/payTypeses.component';
import { ViewPayTypesModalComponent } from './payType/payTypeses/view-payTypes-modal.component';
import { CreateOrEditPayTypesModalComponent } from './payType/payTypeses/create-or-edit-payTypes-modal.component';

import { ResourceWorkerInfosResourcesLookupTableModalComponent } from './resourceWorkerInfo/resourceWorkerInfoses/resourceWorkerInfos-resources-lookup-table-modal.component';

import { ResourceReservationsesComponent } from './resourceReservation/resourceReservationses/resourceReservationses.component';
import { ViewResourceReservationsModalComponent } from './resourceReservation/resourceReservationses/view-resourceReservations-modal.component';
import { CreateOrEditResourceReservationsModalComponent } from './resourceReservation/resourceReservationses/create-or-edit-resourceReservations-modal.component';
import { ResourceReservationsUserLookupTableModalComponent } from './resourceReservation/resourceReservationses/resourceReservations-user-lookup-table-modal.component';
import { ResourceReservationsResourcesLookupTableModalComponent } from './resourceReservation/resourceReservationses/resourceReservations-resources-lookup-table-modal.component';

import { JobsesComponent } from './job/jobses/jobses.component';
import { ViewJobsModalComponent } from './job/jobses/view-jobs-modal.component';
import { CreateOrEditJobsModalComponent } from './job/jobses/create-or-edit-jobs-modal.component';
import { JobsAddressesLookupTableModalComponent } from './job/jobses/jobs-addresses-lookup-table-modal.component';
import { JobsJobClassesLookupTableModalComponent } from './job/jobses/jobs-jobClasses-lookup-table-modal.component';

import { JobClassesesComponent } from './jobClass/jobClasseses/jobClasseses.component';
import { ViewJobClassesModalComponent } from './jobClass/jobClasseses/view-jobClasses-modal.component';
import { CreateOrEditJobClassesModalComponent } from './jobClass/jobClasseses/create-or-edit-jobClasses-modal.component';

import { AddressesesComponent } from './address/addresseses/addresseses.component';
import { ViewAddressesModalComponent } from './address/addresseses/view-addresses-modal.component';
import { CreateOrEditAddressesModalComponent } from './address/addresseses/create-or-edit-addresses-modal.component';

import { ResourceEquipmentInfosesComponent } from './resourceEquipmentInfo/resourceEquipmentInfoses/resourceEquipmentInfoses.component';
import { ViewResourceEquipmentInfosModalComponent } from './resourceEquipmentInfo/resourceEquipmentInfoses/view-resourceEquipmentInfos-modal.component';
import { CreateOrEditResourceEquipmentInfosModalComponent } from './resourceEquipmentInfo/resourceEquipmentInfoses/create-or-edit-resourceEquipmentInfos-modal.component';

import { ResourceWorkerInfosesComponent } from './resourceWorkerInfo/resourceWorkerInfoses/resourceWorkerInfoses.component';
import { ViewResourceWorkerInfosModalComponent } from './resourceWorkerInfo/resourceWorkerInfoses/view-resourceWorkerInfos-modal.component';
import { CreateOrEditResourceWorkerInfosModalComponent } from './resourceWorkerInfo/resourceWorkerInfoses/create-or-edit-resourceWorkerInfos-modal.component';
import { ResourceWorkerInfosWorkerClaseesLookupTableModalComponent } from './resourceWorkerInfo/resourceWorkerInfoses/resourceWorkerInfos-workerClasees-lookup-table-modal.component';

import { WorkerClaseesesComponent } from './workerClasee/workerClaseeses/workerClaseeses.component';
import { ViewWorkerClaseesModalComponent } from './workerClasee/workerClaseeses/view-workerClasees-modal.component';
import { CreateOrEditWorkerClaseesModalComponent } from './workerClasee/workerClaseeses/create-or-edit-workerClasees-modal.component';

import { ResourcesesComponent } from './resource/resourceses/resourceses.component';
import { ViewResourcesModalComponent } from './resource/resourceses/view-resources-modal.component';
import { CreateOrEditResourcesModalComponent } from './resource/resourceses/create-or-edit-resources-modal.component';

import { ECCOSTSComponent } from './eccosts/eccosts/eccosts.component';
import { ViewECCOSTModalComponent } from './eccosts/eccosts/view-eccost-modal.component';
import { CreateOrEditECCOSTModalComponent } from './eccosts/eccosts/create-or-edit-eccost-modal.component';
import { ECCOSTEQUIPMENTLookupTableModalComponent } from './eccosts/eccosts/eccost-equipment-lookup-table-modal.component';

import { EQUIPMENTSComponent } from './equipments/equipments/equipments.component';
import { ViewEQUIPMENTModalComponent } from './equipments/equipments/view-equipment-modal.component';
import { CreateOrEditEQUIPMENTModalComponent } from './equipments/equipments/create-or-edit-equipment-modal.component';

import { PRDEDRATESComponent } from './prdedrates/prdedrates/prdedrates.component';
import { ViewPRDEDRATEModalComponent } from './prdedrates/prdedrates/view-prdedrate-modal.component';
import { CreateOrEditPRDEDRATEModalComponent } from './prdedrates/prdedrates/create-or-edit-prdedrate-modal.component';
import { PRDEDRATEPRCLASSLookupTableModalComponent } from './prdedrates/prdedrates/prdedrate-prclass-lookup-table-modal.component';

import { JCJOBsComponent } from './jcjobs/jcjoBs/jcjoBs.component';
import { ViewJCJOBModalComponent } from './jcjobs/jcjoBs/view-jcjob-modal.component';
import { CreateOrEditJCJOBModalComponent } from './jcjobs/jcjoBs/create-or-edit-jcjob-modal.component';
import { JCJOBJACCATLookupTableModalComponent } from './jcjobs/jcjoBs/jcjob-jaccat-lookup-table-modal.component';

import { PRCLASSsComponent } from './prclasses/prclasSs/prclasSs.component';
import { ViewPRCLASSModalComponent } from './prclasses/prclasSs/view-prclass-modal.component';
import { CreateOrEditPRCLASSModalComponent } from './prclasses/prclasSs/create-or-edit-prclass-modal.component';
import { PRCLASSJCUNIONLookupTableModalComponent } from './prclasses/prclasSs/prclass-jcunion-lookup-table-modal.component';
import { PRCLASSPREMPLOYEELookupTableModalComponent } from './prclasses/prclasSs/prclass-premployee-lookup-table-modal.component';

import { JCUNIONsComponent } from './jcunions/jcunioNs/jcunioNs.component';
import { ViewJCUNIONModalComponent } from './jcunions/jcunioNs/view-jcunion-modal.component';
import { CreateOrEditJCUNIONModalComponent } from './jcunions/jcunioNs/create-or-edit-jcunion-modal.component';
import { JCUNIONJACCATLookupTableModalComponent } from './jcunions/jcunioNs/jcunion-jaccat-lookup-table-modal.component';

import { JACCATsComponent } from './jccat/jaccaTs/jaccaTs.component';
import { ViewJACCATModalComponent } from './jccat/jaccaTs/view-jaccat-modal.component';
import { CreateOrEditJACCATModalComponent } from './jccat/jaccaTs/create-or-edit-jaccat-modal.component';

import { PREMPLOYEESComponent } from './premployees/premployees/premployees.component';
import { ViewPREMPLOYEEModalComponent } from './premployees/premployees/view-premployee-modal.component';
import { CreateOrEditPREMPLOYEEModalComponent } from './premployees/premployees/create-or-edit-premployee-modal.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PaginatorModule } from 'primeng/paginator';
import { EditorModule } from 'primeng/editor';
import { InputMaskModule } from 'primeng/inputmask';import { FileUploadModule } from 'primeng/fileupload';
import { TableModule } from 'primeng/table';

import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,		TableModule,

        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot()
    ],
    declarations: [
		EquipTimetablesComponent,

		ViewEquipTimetablesModalComponent,
		CreateOrEditEquipTimetablesModalComponent,
    EquipTimetablesPayPeriodsLookupTableModalComponent,
    EquipTimetablesResourcesLookupTableModalComponent,
    EquipTimetablesJobPhaseCodesLookupTableModalComponent,
    EquipTimetablesJobCategoriesLookupTableModalComponent,
    EquipTimetablesJobsLookupTableModalComponent,
    TimetablesWorkerClaseesLookupTableModalComponent,
    TimetablesPayTypesLookupTableModalComponent,
    ShiftResourcesWorkerClaseesLookupTableModalComponent,
    ResourceWorkerInfosUserLookupTableModalComponent,
		TimetablesComponent,

		ViewTimetablesModalComponent,
		CreateOrEditTimetablesModalComponent,
    TimetablesPayPeriodsLookupTableModalComponent,
    TimetablesResourcesLookupTableModalComponent,
    TimetablesUnionPayRatesLookupTableModalComponent,
    TimetablesUnionsLookupTableModalComponent,
    TimetablesAddressesLookupTableModalComponent,
    TimetablesExpenseTypesLookupTableModalComponent,
    TimetablesCostTypesLookupTableModalComponent,
    TimetablesAccountsLookupTableModalComponent,
    TimetablesUserLookupTableModalComponent,
		CostTypeseComponent,
		ViewCostTypesModalComponent,
        CreateOrEditCostTypesModalComponent,
        AccountsComponent,
		ViewAccountsModalComponent,
		CreateOrEditAccountsModalComponent,
		UnionPayRatesComponent,

		ViewUnionPayRatesModalComponent,
		CreateOrEditUnionPayRatesModalComponent,
    UnionPayRatesUnionsLookupTableModalComponent,
		PayperiodHistoriesComponent,

		ViewPayperiodHistoriesModalComponent,
		CreateOrEditPayperiodHistoriesModalComponent,
    PayperiodHistoriesPayPeriodsLookupTableModalComponent,
		StatusUpdatesComponent,

		ViewStatusUpdatesModalComponent,
		CreateOrEditStatusUpdatesModalComponent,
    StatusUpdatesTimesheetsLookupTableModalComponent,
    StatusUpdatesStatusesLookupTableModalComponent,
    StatusUpdatesJobsLookupTableModalComponent,
    StatusUpdatesUserLookupTableModalComponent,
		ShiftExpensesComponent,
		ViewShiftExpensesModalComponent,		CreateOrEditShiftExpensesModalComponent,
    ShiftExpensesShiftResourcesLookupTableModalComponent,
    ShiftExpensesExpenseTypesLookupTableModalComponent,
		EmployeeUnionsComponent,
		ViewEmployeeUnionsModalComponent,		CreateOrEditEmployeeUnionsModalComponent,
    EmployeeUnionsUnionsLookupTableModalComponent,
    EmployeeUnionsResourcesLookupTableModalComponent,
		JobUnionsComponent,
		ViewJobUnionsModalComponent,		CreateOrEditJobUnionsModalComponent,
    JobUnionsJobsLookupTableModalComponent,
    JobUnionsUnionsLookupTableModalComponent,
		UnionsComponent,
		ViewUnionsModalComponent,		CreateOrEditUnionsModalComponent,
		JobCategoriesComponent,
		ViewJobCategoriesModalComponent,		CreateOrEditJobCategoriesModalComponent,
    JobCategoriesJobPhaseCodesLookupTableModalComponent,
		ShiftResourcesComponent,
		ViewShiftResourcesModalComponent,		CreateOrEditShiftResourcesModalComponent,
    ShiftResourcesResourcesLookupTableModalComponent,
    ShiftResourcesPayTypesLookupTableModalComponent,
    ShiftResourcesJobPhaseCodesLookupTableModalComponent,
    ShiftResourcesJobCategoriesLookupTableModalComponent,
    ShiftResourcesTimesheetsLookupTableModalComponent,
    ShiftResourcesShiftsLookupTableModalComponent,
		JobCategoriesComponent,
		ViewJobCategoriesModalComponent,		CreateOrEditJobCategoriesModalComponent,
		JobPhaseCodesComponent,
		ViewJobPhaseCodesModalComponent,		CreateOrEditJobPhaseCodesModalComponent,
    JobPhaseCodesJobsLookupTableModalComponent,
		ShiftsComponent,
		ViewShiftsModalComponent,		CreateOrEditShiftsModalComponent,
    ShiftsJobsLookupTableModalComponent,
		PayPeriodsComponent,
		ViewPayPeriodsModalComponent,		CreateOrEditPayPeriodsModalComponent,
		TimesheetsComponent,
		ViewTimesheetsModalComponent,		CreateOrEditTimesheetsModalComponent,
    TimesheetsStatusesLookupTableModalComponent,
		StatusesComponent,
		ViewStatusesModalComponent,		CreateOrEditStatusesModalComponent,
		ExpenseTypesesComponent,
		ViewExpenseTypesModalComponent,		CreateOrEditExpenseTypesModalComponent,
		PayTypesesComponent,
		ViewPayTypesModalComponent,		CreateOrEditPayTypesModalComponent,
		ResourceWorkerInfosesComponent,
		ViewResourceWorkerInfosModalComponent,		CreateOrEditResourceWorkerInfosModalComponent,
    ResourceWorkerInfosResourcesLookupTableModalComponent,
		ResourceReservationsesComponent,
		ViewResourceReservationsModalComponent,		CreateOrEditResourceReservationsModalComponent,
    ResourceReservationsUserLookupTableModalComponent,
    ResourceReservationsResourcesLookupTableModalComponent,
		JobsesComponent,
		ViewJobsModalComponent,		CreateOrEditJobsModalComponent,
    JobsAddressesLookupTableModalComponent,
    JobsJobClassesLookupTableModalComponent,
		JobClassesesComponent,
		ViewJobClassesModalComponent,		CreateOrEditJobClassesModalComponent,
		AddressesesComponent,
		ViewAddressesModalComponent,		CreateOrEditAddressesModalComponent,
		ResourceEquipmentInfosesComponent,
		ViewResourceEquipmentInfosModalComponent,		CreateOrEditResourceEquipmentInfosModalComponent,
		ResourceWorkerInfosesComponent,
		ViewResourceWorkerInfosModalComponent,		CreateOrEditResourceWorkerInfosModalComponent,
    ResourceWorkerInfosWorkerClaseesLookupTableModalComponent,
		WorkerClaseesesComponent,
		ViewWorkerClaseesModalComponent,		CreateOrEditWorkerClaseesModalComponent,
		ResourcesesComponent,
		ViewResourcesModalComponent,		CreateOrEditResourcesModalComponent,
		ECCOSTSComponent,
		ViewECCOSTModalComponent,		CreateOrEditECCOSTModalComponent,
    ECCOSTEQUIPMENTLookupTableModalComponent,
		EQUIPMENTSComponent,
		ViewEQUIPMENTModalComponent,		CreateOrEditEQUIPMENTModalComponent,
		PRDEDRATESComponent,
		ViewPRDEDRATEModalComponent,		CreateOrEditPRDEDRATEModalComponent,
    PRDEDRATEPRCLASSLookupTableModalComponent,
		JCJOBsComponent,
		ViewJCJOBModalComponent,		CreateOrEditJCJOBModalComponent,
    JCJOBJACCATLookupTableModalComponent,
		PRCLASSsComponent,
		ViewPRCLASSModalComponent,		CreateOrEditPRCLASSModalComponent,
    PRCLASSJCUNIONLookupTableModalComponent,
    PRCLASSPREMPLOYEELookupTableModalComponent,
		JCUNIONsComponent,
		ViewJCUNIONModalComponent,		CreateOrEditJCUNIONModalComponent,
    JCUNIONJACCATLookupTableModalComponent,
		JACCATsComponent,
		ViewJACCATModalComponent,		CreateOrEditJACCATModalComponent,
		PREMPLOYEESComponent,
		ViewPREMPLOYEEModalComponent,		CreateOrEditPREMPLOYEEModalComponent,
        DashboardComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
