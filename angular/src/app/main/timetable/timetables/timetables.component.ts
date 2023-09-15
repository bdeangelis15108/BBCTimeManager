import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TimetablesServiceProxy, TimetablesDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTimetablesModalComponent } from './create-or-edit-timetables-modal.component';

import { ViewTimetablesModalComponent } from './view-timetables-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './timetables.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TimetablesComponent extends AppComponentBase {


    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditTimetablesModal', { static: true }) createOrEditTimetablesModal: CreateOrEditTimetablesModalComponent;
    @ViewChild('viewTimetablesModalComponent', { static: true }) viewTimetablesModal: ViewTimetablesModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDay1Filter: number;
    maxDay1FilterEmpty: number;
    minDay1Filter: number;
    minDay1FilterEmpty: number;
    maxDay2Filter: number;
    maxDay2FilterEmpty: number;
    minDay2Filter: number;
    minDay2FilterEmpty: number;
    maxDay3Filter: number;
    maxDay3FilterEmpty: number;
    minDay3Filter: number;
    minDay3FilterEmpty: number;
    maxDay4Filter: number;
    maxDay4FilterEmpty: number;
    minDay4Filter: number;
    minDay4FilterEmpty: number;
    maxDay5Filter: number;
    maxDay5FilterEmpty: number;
    minDay5Filter: number;
    minDay5FilterEmpty: number;
    maxDay6Filter: number;
    maxDay6FilterEmpty: number;
    minDay6Filter: number;
    minDay6FilterEmpty: number;
    maxDay7Filter: number;
    maxDay7FilterEmpty: number;
    minDay7Filter: number;
    minDay7FilterEmpty: number;
    maxAmountFilter: number;
    maxAmountFilterEmpty: number;
    minAmountFilter: number;
    minAmountFilterEmpty: number;
    maxCreatedOnFilter: moment.Moment;
    minCreatedOnFilter: moment.Moment;
    isActiveFilter = -1;
    costCodeFilter = '';
        payPeriodsNameFilter = '';
        resourcesNameFilter = '';
        unionPayRatesClassFilter = '';
        unionsNumberFilter = '';
        addressesStateFilter = '';
        expenseTypesDescriptionFilter = '';
        costTypesNameFilter = '';
        accountsNameFilter = '';
        userNameFilter = '';
        payTypesCodeFilter = '';
        workerClaseesNameFilter = '';

    wcomp1Filter:string;
    _entityTypeFullName = 'Nucleus.Timetable.Timetables';
    entityHistoryEnabled = false;



    constructor(
        injector: Injector,
        private _timetablesServiceProxy: TimetablesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _fileDownloadService: FileDownloadService
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

    getTimetables(event?: LazyLoadEvent) {

        console.log('this.route.snapshot.queryParams.data', this._activatedRoute.snapshot.queryParams)
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._timetablesServiceProxy.getAll(
            this.filterText,
            this.wcomp1Filter,
            this._activatedRoute.snapshot.queryParams.selectedCode,
            this._activatedRoute.snapshot.queryParams.selectedPayWeek,
            this.maxDay1Filter == null ? this.maxDay1FilterEmpty : this.maxDay1Filter,
            this.minDay1Filter == null ? this.minDay1FilterEmpty : this.minDay1Filter,
            this.maxDay2Filter == null ? this.maxDay2FilterEmpty : this.maxDay2Filter,
            this.minDay2Filter == null ? this.minDay2FilterEmpty : this.minDay2Filter,
            this.maxDay3Filter == null ? this.maxDay3FilterEmpty : this.maxDay3Filter,
            this.minDay3Filter == null ? this.minDay3FilterEmpty : this.minDay3Filter,
            this.maxDay4Filter == null ? this.maxDay4FilterEmpty : this.maxDay4Filter,
            this.minDay4Filter == null ? this.minDay4FilterEmpty : this.minDay4Filter,
            this.maxDay5Filter == null ? this.maxDay5FilterEmpty : this.maxDay5Filter,
            this.minDay5Filter == null ? this.minDay5FilterEmpty : this.minDay5Filter,
            this.maxDay6Filter == null ? this.maxDay6FilterEmpty : this.maxDay6Filter,
            this.minDay6Filter == null ? this.minDay6FilterEmpty : this.minDay6Filter,
            this.maxDay7Filter == null ? this.maxDay7FilterEmpty : this.maxDay7Filter,
            this.minDay7Filter == null ? this.minDay7FilterEmpty : this.minDay7Filter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
            this.maxCreatedOnFilter === undefined ? this.maxCreatedOnFilter : moment(this.maxCreatedOnFilter).endOf('day'),
            this.minCreatedOnFilter === undefined ? this.minCreatedOnFilter : moment(this.minCreatedOnFilter).startOf('day'),
            this.isActiveFilter,
            this.costCodeFilter,
            this.payPeriodsNameFilter,
            this.resourcesNameFilter,
            this.unionPayRatesClassFilter,
            this.unionsNumberFilter,
            this.addressesStateFilter,
            this.expenseTypesDescriptionFilter,
            this.costTypesNameFilter,
            this.accountsNameFilter,
            this.userNameFilter,
            this.payTypesCodeFilter,
            this.workerClaseesNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            //console.log('result.items',result.items)
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            
            this.primengTableHelper.hideLoadingIndicator();
        });
    }
    goBackToJobSelection(): void {
        this._router.navigate(['/app/main/job/jobses']);
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTimetables(): void {
        this.createOrEditTimetablesModal.show();
    }



    showHistory(timetables: TimetablesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: timetables.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteTimetables(timetables: TimetablesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._timetablesServiceProxy.delete(timetables.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._timetablesServiceProxy.getTimetablesToExcel(
            this.filterText,
            this._activatedRoute.snapshot.queryParams.selectedCode,
            this._activatedRoute.snapshot.queryParams.selectedPayWeek,
            this.maxDay1Filter == null ? this.maxDay1FilterEmpty : this.maxDay1Filter,
            this.minDay1Filter == null ? this.minDay1FilterEmpty : this.minDay1Filter,
            this.maxDay2Filter == null ? this.maxDay2FilterEmpty : this.maxDay2Filter,
            this.minDay2Filter == null ? this.minDay2FilterEmpty : this.minDay2Filter,
            this.maxDay3Filter == null ? this.maxDay3FilterEmpty : this.maxDay3Filter,
            this.minDay3Filter == null ? this.minDay3FilterEmpty : this.minDay3Filter,
            this.maxDay4Filter == null ? this.maxDay4FilterEmpty : this.maxDay4Filter,
            this.minDay4Filter == null ? this.minDay4FilterEmpty : this.minDay4Filter,
            this.maxDay5Filter == null ? this.maxDay5FilterEmpty : this.maxDay5Filter,
            this.minDay5Filter == null ? this.minDay5FilterEmpty : this.minDay5Filter,
            this.maxDay6Filter == null ? this.maxDay6FilterEmpty : this.maxDay6Filter,
            this.minDay6Filter == null ? this.minDay6FilterEmpty : this.minDay6Filter,
            this.maxDay7Filter == null ? this.maxDay7FilterEmpty : this.maxDay7Filter,
            this.minDay7Filter == null ? this.minDay7FilterEmpty : this.minDay7Filter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty : this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty : this.minAmountFilter,
            this.maxCreatedOnFilter === undefined ? this.maxCreatedOnFilter : moment(this.maxCreatedOnFilter).endOf('day'),
            this.minCreatedOnFilter === undefined ? this.minCreatedOnFilter : moment(this.minCreatedOnFilter).startOf('day'),
            this.isActiveFilter,
            this.costCodeFilter,
            this.payPeriodsNameFilter,
            this.resourcesNameFilter,
            this.unionPayRatesClassFilter,
            this.unionsNumberFilter,
            this.addressesStateFilter,
            this.expenseTypesDescriptionFilter,
            this.costTypesNameFilter,
            this.accountsNameFilter,
            this.userNameFilter,
            this.payTypesCodeFilter,
            this.workerClaseesNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }


    getReadableDay(day) {
        var inital = moment(this._activatedRoute.snapshot.queryParams.startDate).toDate();
        var date = new Date(inital).setDate(new Date(inital).getDate() + day);
        return moment(date).format('DD ddd')
    }

    formateDays(value) {
        return value === null ? 0 : value;
    }



}
