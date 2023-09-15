import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { UnionPayRatesServiceProxy, UnionPayRatesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditUnionPayRatesModalComponent } from './create-or-edit-unionPayRates-modal.component';

import { ViewUnionPayRatesModalComponent } from './view-unionPayRates-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './unionPayRates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class UnionPayRatesComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('createOrEditUnionPayRatesModal', { static: true }) createOrEditUnionPayRatesModal: CreateOrEditUnionPayRatesModalComponent;
    @ViewChild('viewUnionPayRatesModalComponent', { static: true }) viewUnionPayRatesModal: ViewUnionPayRatesModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    classFilter = '';
    dedtypeFilter = '';
    maxPerhourFilter : number;
		maxPerhourFilterEmpty : number;
		minPerhourFilter : number;
		minPerhourFilterEmpty : number;
        unionsNumberFilter = '';


    _entityTypeFullName = 'Nucleus.UnionPayRate.UnionPayRates';
    entityHistoryEnabled = false;



    constructor(
        injector: Injector,
        private _unionPayRatesServiceProxy: UnionPayRatesServiceProxy,
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

    getUnionPayRates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._unionPayRatesServiceProxy.getAll(
            this.filterText,
            this.classFilter,
            this.dedtypeFilter,
            this.maxPerhourFilter == null ? this.maxPerhourFilterEmpty: this.maxPerhourFilter,
            this.minPerhourFilter == null ? this.minPerhourFilterEmpty: this.minPerhourFilter,
            this.unionsNumberFilter,
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

    createUnionPayRates(): void {
        this.createOrEditUnionPayRatesModal.show();        
    }


    showHistory(unionPayRates: UnionPayRatesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: unionPayRates.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteUnionPayRates(unionPayRates: UnionPayRatesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._unionPayRatesServiceProxy.delete(unionPayRates.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._unionPayRatesServiceProxy.getUnionPayRatesToExcel(
        this.filterText,
            this.classFilter,
            this.dedtypeFilter,
            this.maxPerhourFilter == null ? this.maxPerhourFilterEmpty: this.maxPerhourFilter,
            this.minPerhourFilter == null ? this.minPerhourFilterEmpty: this.minPerhourFilter,
            this.unionsNumberFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
}
