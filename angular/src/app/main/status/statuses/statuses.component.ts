import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { StatusesServiceProxy, StatusesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStatusesModalComponent } from './create-or-edit-statuses-modal.component';

import { ViewStatusesModalComponent } from './view-statuses-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './statuses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StatusesComponent extends AppComponentBase {
    
    @ViewChild('createOrEditStatusesModal', { static: true }) createOrEditStatusesModal: CreateOrEditStatusesModalComponent;
    @ViewChild('viewStatusesModalComponent', { static: true }) viewStatusesModal: ViewStatusesModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    isDefaultFilter = -1;
    forwardNameFilter = '';
    reverseNameFilter = '';
    maxForwardIdFilter : number;
		maxForwardIdFilterEmpty : number;
		minForwardIdFilter : number;
		minForwardIdFilterEmpty : number;
    maxReverseIdFilter : number;
		maxReverseIdFilterEmpty : number;
		minReverseIdFilter : number;
		minReverseIdFilterEmpty : number;


    _entityTypeFullName = 'Nucleus.Status.Statuses';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _statusesServiceProxy: StatusesServiceProxy,
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

    getStatuses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._statusesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.isDefaultFilter,
            this.forwardNameFilter,
            this.reverseNameFilter,
            this.maxForwardIdFilter == null ? this.maxForwardIdFilterEmpty: this.maxForwardIdFilter,
            this.minForwardIdFilter == null ? this.minForwardIdFilterEmpty: this.minForwardIdFilter,
            this.maxReverseIdFilter == null ? this.maxReverseIdFilterEmpty: this.maxReverseIdFilter,
            this.minReverseIdFilter == null ? this.minReverseIdFilterEmpty: this.minReverseIdFilter,
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

    createStatuses(): void {
        this.createOrEditStatusesModal.show();        
    }


    showHistory(statuses: StatusesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: statuses.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteStatuses(statuses: StatusesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._statusesServiceProxy.delete(statuses.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._statusesServiceProxy.getStatusesToExcel(
        this.filterText,
            this.nameFilter,
            this.isDefaultFilter,
            this.forwardNameFilter,
            this.reverseNameFilter,
            this.maxForwardIdFilter == null ? this.maxForwardIdFilterEmpty: this.maxForwardIdFilter,
            this.minForwardIdFilter == null ? this.minForwardIdFilterEmpty: this.minForwardIdFilter,
            this.maxReverseIdFilter == null ? this.maxReverseIdFilterEmpty: this.maxReverseIdFilter,
            this.minReverseIdFilter == null ? this.minReverseIdFilterEmpty: this.minReverseIdFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
