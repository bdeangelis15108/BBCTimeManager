import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ShiftsServiceProxy, ShiftsDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditShiftsModalComponent } from './create-or-edit-shifts-modal.component';

import { ViewShiftsModalComponent } from './view-shifts-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';
import { ViewShiftResourcesModalComponent } from '../../shiftResource/shiftResources/view-shiftResources-modal.component';

@Component({
    templateUrl: './shifts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ShiftsComponent extends AppComponentBase {
    
    @ViewChild('createOrEditShiftsModal', { static: true }) createOrEditShiftsModal: CreateOrEditShiftsModalComponent;
   // @ViewChild('viewShiftsModalComponent', { static: true }) viewShiftsModal: ViewShiftsModalComponent;
    @ViewChild('viewShiftResourcesModalComponent', { static: true }) viewShiftResourcesModal: ViewShiftResourcesModalComponent;   
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxScheduledStartFilter : moment.Moment;
		minScheduledStartFilter : moment.Moment;
    maxScheduledEndFilter : moment.Moment;
		minScheduledEndFilter : moment.Moment;
    nameFilter = '';
        jobsNameFilter = '';


    _entityTypeFullName = 'Nucleus.Shift.Shifts';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _shiftsServiceProxy: ShiftsServiceProxy,
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

    getShifts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._shiftsServiceProxy.getAll(
            this.filterText,
            this.maxScheduledStartFilter,
            this.minScheduledStartFilter,
            this.maxScheduledEndFilter,
            this.minScheduledEndFilter,
            this.nameFilter,
            this.jobsNameFilter,
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

    createShifts(): void {
        this.createOrEditShiftsModal.show();        
    }


    showHistory(shifts: ShiftsDto): void {
        this.entityTypeHistoryModal.show({
            entityId: shifts.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteShifts(shifts: ShiftsDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._shiftsServiceProxy.delete(shifts.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._shiftsServiceProxy.getShiftsToExcel(
        this.filterText,
            this.maxScheduledStartFilter,
            this.minScheduledStartFilter,
            this.maxScheduledEndFilter,
            this.minScheduledEndFilter,
            this.nameFilter,
            this.jobsNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
