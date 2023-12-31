﻿import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { TimesheetsServiceProxy, TimesheetsDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTimesheetsModalComponent } from './create-or-edit-timesheets-modal.component';

import { ViewTimesheetsModalComponent } from './view-timesheets-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './timesheets.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TimesheetsComponent extends AppComponentBase {
    
    @ViewChild('createOrEditTimesheetsModal', { static: true }) createOrEditTimesheetsModal: CreateOrEditTimesheetsModalComponent;
    @ViewChild('viewTimesheetsModalComponent', { static: true }) viewTimesheetsModal: ViewTimesheetsModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxCreatedDateFilter : moment.Moment;
		minCreatedDateFilter : moment.Moment;
    maxSubmitedDateFilter : moment.Moment;
		minSubmitedDateFilter : moment.Moment;
    nameFilter = '';
        statusesNameFilter = '';


    _entityTypeFullName = 'Nucleus.Timesheet.Timesheets';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _timesheetsServiceProxy: TimesheetsServiceProxy,
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

    getTimesheets(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._timesheetsServiceProxy.getAll(
            this.filterText,
            this.maxCreatedDateFilter,
            this.minCreatedDateFilter,
            this.maxSubmitedDateFilter,
            this.minSubmitedDateFilter,
            this.nameFilter,
            this.statusesNameFilter,
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

    createTimesheets(): void {
        this.createOrEditTimesheetsModal.show();        
    }


    showHistory(timesheets: TimesheetsDto): void {
        this.entityTypeHistoryModal.show({
            entityId: timesheets.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteTimesheets(timesheets: TimesheetsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._timesheetsServiceProxy.delete(timesheets.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._timesheetsServiceProxy.getTimesheetsToExcel(
        this.filterText,
            this.maxCreatedDateFilter,
            this.minCreatedDateFilter,
            this.maxSubmitedDateFilter,
            this.minSubmitedDateFilter,
            this.nameFilter,
            this.statusesNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
