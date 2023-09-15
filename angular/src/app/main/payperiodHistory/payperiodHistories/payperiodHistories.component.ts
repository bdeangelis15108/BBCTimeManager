import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { PayperiodHistoriesServiceProxy, PayperiodHistoriesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPayperiodHistoriesModalComponent } from './create-or-edit-payperiodHistories-modal.component';

import { ViewPayperiodHistoriesModalComponent } from './view-payperiodHistories-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './payperiodHistories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PayperiodHistoriesComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditPayperiodHistoriesModal', { static: true }) createOrEditPayperiodHistoriesModal: CreateOrEditPayperiodHistoriesModalComponent;
    @ViewChild('viewPayperiodHistoriesModalComponent', { static: true }) viewPayperiodHistoriesModal: ViewPayperiodHistoriesModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    periodFilter = '';
    activeFilter = -1;
        payPeriodsNameFilter = '';


    _entityTypeFullName = 'Nucleus.PayperiodHistory.PayperiodHistories';
    entityHistoryEnabled = false;



    constructor(
        injector: Injector,
        private _payperiodHistoriesServiceProxy: PayperiodHistoriesServiceProxy,
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

    getPayperiodHistories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._payperiodHistoriesServiceProxy.getAll(
            this.filterText,
            this.periodFilter,
            this.activeFilter,
            this.payPeriodsNameFilter,
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

    createPayperiodHistories(): void {
        this.createOrEditPayperiodHistoriesModal.show();        
    }


    showHistory(payperiodHistories: PayperiodHistoriesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: payperiodHistories.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deletePayperiodHistories(payperiodHistories: PayperiodHistoriesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._payperiodHistoriesServiceProxy.delete(payperiodHistories.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._payperiodHistoriesServiceProxy.getPayperiodHistoriesToExcel(
        this.filterText,
            this.periodFilter,
            this.activeFilter,
            this.payPeriodsNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
}
