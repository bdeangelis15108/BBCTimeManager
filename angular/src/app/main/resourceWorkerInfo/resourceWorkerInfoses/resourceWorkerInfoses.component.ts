import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ResourceWorkerInfosesServiceProxy, ResourceWorkerInfosDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditResourceWorkerInfosModalComponent } from './create-or-edit-resourceWorkerInfos-modal.component';

import { ViewResourceWorkerInfosModalComponent } from './view-resourceWorkerInfos-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './resourceWorkerInfoses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ResourceWorkerInfosesComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditResourceWorkerInfosModal', { static: true }) createOrEditResourceWorkerInfosModal: CreateOrEditResourceWorkerInfosModalComponent;
    @ViewChild('viewResourceWorkerInfosModalComponent', { static: true }) viewResourceWorkerInfosModal: ViewResourceWorkerInfosModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    firstNameFilter = '';
    lastNameFilter = '';
        workerClaseesNameFilter = '';
        resourcesNameFilter = '';


    _entityTypeFullName = 'Nucleus.ResourceWorkerInfo.ResourceWorkerInfos';
    entityHistoryEnabled = false;



    constructor(
        injector: Injector,
        private _resourceWorkerInfosesServiceProxy: ResourceWorkerInfosesServiceProxy,
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

    getResourceWorkerInfoses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._resourceWorkerInfosesServiceProxy.getAll(
            this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            '',
            '',
            '',
            this.workerClaseesNameFilter,
            this.resourcesNameFilter,
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

    createResourceWorkerInfos(): void {
        this.createOrEditResourceWorkerInfosModal.show();        
    }


    showHistory(resourceWorkerInfos: ResourceWorkerInfosDto): void {
        this.entityTypeHistoryModal.show({
            entityId: resourceWorkerInfos.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteResourceWorkerInfos(resourceWorkerInfos: ResourceWorkerInfosDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._resourceWorkerInfosesServiceProxy.delete(resourceWorkerInfos.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._resourceWorkerInfosesServiceProxy.getResourceWorkerInfosesToExcel(
        this.filterText,
            this.firstNameFilter,
            this.lastNameFilter,
            this.workerClaseesNameFilter,
            this.resourcesNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
}
