import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ResourceReservationsesServiceProxy, ResourceReservationsDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditResourceReservationsModalComponent } from './create-or-edit-resourceReservations-modal.component';
import { ViewResourceReservationsModalComponent } from './view-resourceReservations-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './resourceReservationses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ResourceReservationsesComponent extends AppComponentBase {

    @ViewChild('createOrEditResourceReservationsModal', { static: true }) createOrEditResourceReservationsModal: CreateOrEditResourceReservationsModalComponent;
    @ViewChild('viewResourceReservationsModalComponent', { static: true }) viewResourceReservationsModal: ViewResourceReservationsModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxReservedFromFilter : moment.Moment;
		minReservedFromFilter : moment.Moment;
    maxReservedUntilFilter : moment.Moment;
		minReservedUntilFilter : moment.Moment;
        userNameFilter = '';
    resourcesNameFilter = '';
    resourcesTypeFilter = '';


    _entityTypeFullName = 'Nucleus.ResourceReservation.ResourceReservations';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _resourceReservationsesServiceProxy: ResourceReservationsesServiceProxy,
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

    getResourceReservationses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._resourceReservationsesServiceProxy.getAll(
            this.filterText,
            this.maxReservedFromFilter,
            this.minReservedFromFilter,
            this.maxReservedUntilFilter,
            this.minReservedUntilFilter,
            this.userNameFilter,
            this.resourcesNameFilter,
            this.resourcesTypeFilter,
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

    createResourceReservations(): void {
        this.createOrEditResourceReservationsModal.show();
    }

    showHistory(resourceReservations: ResourceReservationsDto): void {
        this.entityTypeHistoryModal.show({
            entityId: resourceReservations.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteResourceReservations(resourceReservations: ResourceReservationsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._resourceReservationsesServiceProxy.delete(resourceReservations.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._resourceReservationsesServiceProxy.getResourceReservationsesToExcel(
        this.filterText,
            this.maxReservedFromFilter,
            this.minReservedFromFilter,
            this.maxReservedUntilFilter,
            this.minReservedUntilFilter,
            this.userNameFilter,
            this.resourcesNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
