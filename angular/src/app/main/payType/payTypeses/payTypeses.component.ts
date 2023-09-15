import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { PayTypesesServiceProxy, PayTypesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPayTypesModalComponent } from './create-or-edit-payTypes-modal.component';

import { ViewPayTypesModalComponent } from './view-payTypes-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './payTypeses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PayTypesesComponent extends AppComponentBase {
    
    
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
       
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    codeFilter = '';
    descriptionFilter = '';
    maxMultiplierFilter : number;
		maxMultiplierFilterEmpty : number;
		minMultiplierFilter : number;
		minMultiplierFilterEmpty : number;
    section1Filter = -1;
    section2Filter = -1;
    section3Filter = -1;


    _entityTypeFullName = 'Nucleus.PayType.PayTypes';
    entityHistoryEnabled = false;
    createOrEditPayTypesModal: any;

    constructor(
        injector: Injector,
        private _payTypesesServiceProxy: PayTypesesServiceProxy,
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

    getPayTypeses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._payTypesesServiceProxy.getAll(
            this.filterText,
            this.codeFilter,
            this.descriptionFilter,
            this.maxMultiplierFilter == null ? this.maxMultiplierFilterEmpty: this.maxMultiplierFilter,
            this.minMultiplierFilter == null ? this.minMultiplierFilterEmpty: this.minMultiplierFilter,
            this.section1Filter,
            this.section2Filter,
            this.section3Filter,
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

    createPayTypes(): void {
        this.createOrEditPayTypesModal.show();        
    }


    showHistory(payTypes: PayTypesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: payTypes.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deletePayTypes(payTypes: PayTypesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._payTypesesServiceProxy.delete(payTypes.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._payTypesesServiceProxy.getPayTypesesToExcel(
        this.filterText,
            this.codeFilter,
            this.descriptionFilter,
            this.maxMultiplierFilter == null ? this.maxMultiplierFilterEmpty: this.maxMultiplierFilter,
            this.minMultiplierFilter == null ? this.minMultiplierFilterEmpty: this.minMultiplierFilter,
            this.section1Filter,
            this.section2Filter,
            this.section3Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
