import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ShiftResourcesServiceProxy, ShiftResourcesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditShiftResourcesModalComponent } from './create-or-edit-shiftResources-modal.component';

import { ViewShiftResourcesModalComponent } from './view-shiftResources-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './shiftResources.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ShiftResourcesComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditShiftResourcesModal', { static: true }) createOrEditShiftResourcesModal: CreateOrEditShiftResourcesModalComponent;
    @ViewChild('viewShiftResourcesModalComponent', { static: true }) viewShiftResourcesModal: ViewShiftResourcesModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxHoursWorkedFilter : number;
		maxHoursWorkedFilterEmpty : number;
		minHoursWorkedFilter : number;
		minHoursWorkedFilterEmpty : number;
    nameFilter = '';
        resourcesNameFilter = '';
        payTypesCodeFilter = '';
        jobPhaseCodesNameFilter = '';
        jobCategoriesNameFilter = '';
    timesheetsNameFilter = '';
    timesheetStatusFilter = '';
        shiftsNameFilter = '';
    workerClaseesNameFilter = '';
    timesheetIdFilter: number;
    maxCreatedDateFilter: moment.Moment;
    minCreatedDateFilter: moment.Moment;
    resourceType = '';

    _entityTypeFullName = 'Nucleus.ShiftResource.ShiftResources';
    entityHistoryEnabled = false;



    constructor(
        injector: Injector,
        private _shiftResourcesServiceProxy: ShiftResourcesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
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

    getShiftResources(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._shiftResourcesServiceProxy.getAll(
            this.filterText,
            this.maxHoursWorkedFilter == null ? this.maxHoursWorkedFilterEmpty: this.maxHoursWorkedFilter,
            this.minHoursWorkedFilter == null ? this.minHoursWorkedFilterEmpty: this.minHoursWorkedFilter,
            this.nameFilter,
            this.resourcesNameFilter,
            this.payTypesCodeFilter,
            this.jobPhaseCodesNameFilter,
            this.jobCategoriesNameFilter,
            this.timesheetsNameFilter,
            this.shiftsNameFilter,            
            this.workerClaseesNameFilter,
            this.timesheetIdFilter,
            this.maxCreatedDateFilter,
            this.minCreatedDateFilter,
            this.resourceType,
            this.timesheetStatusFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createShiftResources(): void {
        this.createOrEditShiftResourcesModal.show();        
    }


    showHistory(shiftResources: ShiftResourcesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: shiftResources.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteShiftResources(shiftResources: ShiftResourcesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._shiftResourcesServiceProxy.delete(shiftResources.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._shiftResourcesServiceProxy.getShiftResourcesToExcel(
        this.filterText,
            this.maxHoursWorkedFilter == null ? this.maxHoursWorkedFilterEmpty: this.maxHoursWorkedFilter,
            this.minHoursWorkedFilter == null ? this.minHoursWorkedFilterEmpty: this.minHoursWorkedFilter,
            this.nameFilter,
            this.resourcesNameFilter,
            this.payTypesCodeFilter,
            this.jobPhaseCodesNameFilter,
            this.jobCategoriesNameFilter,
            this.timesheetsNameFilter,
            this.shiftsNameFilter,
            this.workerClaseesNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
}
