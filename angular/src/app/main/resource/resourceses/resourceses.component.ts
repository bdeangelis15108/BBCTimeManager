import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ResourcesesServiceProxy, ResourcesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditResourcesModalComponent } from './create-or-edit-resources-modal.component';
import { ViewResourcesModalComponent } from './view-resources-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './resourceses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ResourcesesComponent extends AppComponentBase {

    @ViewChild('createOrEditResourcesModal', { static: true }) createOrEditResourcesModal: CreateOrEditResourcesModalComponent;
    @ViewChild('viewResourcesModalComponent', { static: true }) viewResourcesModal: ViewResourcesModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    typeFilter = '';
    maxCostPerHourFilter : number;
		maxCostPerHourFilterEmpty : number;
		minCostPerHourFilter : number;
		minCostPerHourFilterEmpty : number;
    maxCostPerUserFilter : number;
		maxCostPerUserFilterEmpty : number;
		minCostPerUserFilter : number;
		minCostPerUserFilterEmpty : number;
    maxCostPerDayFilter : number;
		maxCostPerDayFilterEmpty : number;
		minCostPerDayFilter : number;
		minCostPerDayFilterEmpty : number;
    resourceNumberFilter = '';


    _entityTypeFullName = 'Nucleus.Resource.Resources';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _resourcesesServiceProxy: ResourcesesServiceProxy,
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

    getResourceses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._resourcesesServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.typeFilter,
            this.maxCostPerHourFilter == null ? this.maxCostPerHourFilterEmpty: this.maxCostPerHourFilter,
            this.minCostPerHourFilter == null ? this.minCostPerHourFilterEmpty: this.minCostPerHourFilter,
            this.maxCostPerUserFilter == null ? this.maxCostPerUserFilterEmpty: this.maxCostPerUserFilter,
            this.minCostPerUserFilter == null ? this.minCostPerUserFilterEmpty: this.minCostPerUserFilter,
            this.maxCostPerDayFilter == null ? this.maxCostPerDayFilterEmpty: this.maxCostPerDayFilter,
            this.minCostPerDayFilter == null ? this.minCostPerDayFilterEmpty: this.minCostPerDayFilter,
            this.resourceNumberFilter,
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

    createResources(): void {
        this.createOrEditResourcesModal.show();
    }

    showHistory(resources: ResourcesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: resources.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteResources(resources: ResourcesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._resourcesesServiceProxy.delete(resources.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._resourcesesServiceProxy.getResourcesesToExcel(
        this.filterText,
            this.nameFilter,
            this.typeFilter,
            this.maxCostPerHourFilter == null ? this.maxCostPerHourFilterEmpty: this.maxCostPerHourFilter,
            this.minCostPerHourFilter == null ? this.minCostPerHourFilterEmpty: this.minCostPerHourFilter,
            this.maxCostPerUserFilter == null ? this.maxCostPerUserFilterEmpty: this.maxCostPerUserFilter,
            this.minCostPerUserFilter == null ? this.minCostPerUserFilterEmpty: this.minCostPerUserFilter,
            this.maxCostPerDayFilter == null ? this.maxCostPerDayFilterEmpty: this.maxCostPerDayFilter,
            this.minCostPerDayFilter == null ? this.minCostPerDayFilterEmpty: this.minCostPerDayFilter,
            this.resourceNumberFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
