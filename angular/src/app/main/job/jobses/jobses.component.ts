import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobsesServiceProxy, JobsDto, GetJobsForViewDto, PayPeriodsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJobsModalComponent } from './create-or-edit-jobs-modal.component';
import { ViewJobsModalComponent } from './view-jobs-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';
import { retry, max } from 'rxjs/operators';

@Component({
    templateUrl: './jobses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class JobsesComponent extends AppComponentBase {

    @ViewChild('createOrEditJobsModal', { static: true }) createOrEditJobsModal: CreateOrEditJobsModalComponent;
    @ViewChild('viewJobsModalComponent', { static: true }) viewJobsModal: ViewJobsModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    codeNames: GetJobsForViewDto[] = [];
    payPerionds = [];
    advancedFiltersAreShown = false;
    filterText = '';
    codeFilter = '';
    nameFilter = '';
    maxStartDateFilter: moment.Moment;
    minStartDateFilter: moment.Moment;
    maxEndDateFilter: moment.Moment;
    minEndDateFilter: moment.Moment;
    maxStatusFilter: number;
    maxStatusFilterEmpty: number;
    minStatusFilter: number;
    minStatusFilterEmpty: number;
    addressesLinne1Filter = '';
    jobClassesNameFilter = '';


    _entityTypeFullName = 'Nucleus.Job.Jobs';
    entityHistoryEnabled = false;
    http: any;
    selectedCode: string;
    selectedPayWeek: string;
    constructor(
        injector: Injector,
        private _jobsesServiceProxy: JobsesServiceProxy,
        private _payperiodProxy: PayPeriodsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();

    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return this.isGrantedAny('Pages.Administration.AuditLogs') && customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getJobses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jobsesServiceProxy.getAll(
            this.filterText,
            this.codeFilter,
            this.nameFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.maxStatusFilter == null ? this.maxStatusFilterEmpty : this.maxStatusFilter,
            this.minStatusFilter == null ? this.minStatusFilterEmpty : this.minStatusFilter,
            this.addressesLinne1Filter,
            this.jobClassesNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
            this.codeNames = result.items;
        });

        this._jobsesServiceProxy.getAllPayPeriods().subscribe(result => {
            this.primengTableHelper.hideLoadingIndicator();
            console.log(result)
            this.payPerionds = result.items;
        });
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createJobs(): void {
        this.createOrEditJobsModal.show();
    }

    showHistory(jobs: JobsDto): void {
        this.entityTypeHistoryModal.show({
            entityId: jobs.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteJobs(jobs: JobsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jobsesServiceProxy.delete(jobs.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jobsesServiceProxy.getJobsesToExcel(
            this.filterText,
            this.codeFilter,
            this.nameFilter,
            this.maxStartDateFilter,
            this.minStartDateFilter,
            this.maxEndDateFilter,
            this.minEndDateFilter,
            this.maxStatusFilter == null ? this.maxStatusFilterEmpty : this.maxStatusFilter,
            this.minStatusFilter == null ? this.minStatusFilterEmpty : this.minStatusFilter,
            this.addressesLinne1Filter,
            this.jobClassesNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    isDisabled() {
        return !(this.selectedCode && this.selectedPayWeek && this.selectedCode !== '' && this.selectedPayWeek !== '');
    }


    viewAllsubmission(): void {
        var startDate = null;
        var endDate = null;
        for (let k in this.payPerionds) {
            if (this.selectedPayWeek + '' === this.payPerionds[k].payPeriods.id + '') {
                startDate = this.payPerionds[k].payPeriods.startDate;
                endDate = this.payPerionds[k].payPeriods.endDate;
            }
        }
        console.log({ selectedCode: this.selectedCode, selectedPayWeek: this.selectedPayWeek, startDate: startDate, endDate: endDate })
        this._router.navigate(['/app/main/timetable/timetables'], { queryParams: { selectedCode: this.selectedCode, selectedPayWeek: this.selectedPayWeek, startDate: startDate, endDate: endDate } });
    }
    viewAllsubmissionEquipment(): void{
        var startDate = null;
        var endDate = null;
        for (let k in this.payPerionds) {
            if (this.selectedPayWeek + '' === this.payPerionds[k].payPeriods.id + '') {
                startDate = this.payPerionds[k].payPeriods.startDate;
                endDate = this.payPerionds[k].payPeriods.endDate;
            }
        }
        console.log({ selectedCode: this.selectedCode, selectedPayWeek: this.selectedPayWeek, startDate: startDate, endDate: endDate })
        this._router.navigate(['/app/main//equipTimetable/equipTimetables'], { queryParams: { selectedCode: this.selectedCode, selectedPayWeek: this.selectedPayWeek, startDate: startDate, endDate: endDate } });
    }
    formateDate(period) {
        return moment(period.startDate).format('LL') + ' - ' + moment(period.endDate).format('LL')
    }
}
