import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddressesesServiceProxy, AddressesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAddressesModalComponent } from './create-or-edit-addresses-modal.component';
import { ViewAddressesModalComponent } from './view-addresses-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './addresseses.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AddressesesComponent extends AppComponentBase {

    @ViewChild('createOrEditAddressesModal', { static: true }) createOrEditAddressesModal: CreateOrEditAddressesModalComponent;
    @ViewChild('viewAddressesModalComponent', { static: true }) viewAddressesModal: ViewAddressesModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    linne1Filter = '';
    line2Filter = '';
    cityFilter = '';
    stateFilter = '';
    zipFilter = '';
    lanFilter = '';
    latFilter = '';


    _entityTypeFullName = 'Nucleus.Address.Addresses';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _addressesesServiceProxy: AddressesesServiceProxy,
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

    getAddresseses(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._addressesesServiceProxy.getAll(
            this.filterText,
            this.linne1Filter,
            this.line2Filter,
            this.cityFilter,
            this.stateFilter,
            this.zipFilter,
            this.lanFilter,
            this.latFilter,
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

    createAddresses(): void {
        this.createOrEditAddressesModal.show();
    }

    showHistory(addresses: AddressesDto): void {
        this.entityTypeHistoryModal.show({
            entityId: addresses.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteAddresses(addresses: AddressesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._addressesesServiceProxy.delete(addresses.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._addressesesServiceProxy.getAddressesesToExcel(
        this.filterText,
            this.linne1Filter,
            this.line2Filter,
            this.cityFilter,
            this.stateFilter,
            this.zipFilter,
            this.lanFilter,
            this.latFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
