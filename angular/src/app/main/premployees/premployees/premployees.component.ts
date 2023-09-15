import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PREMPLOYEESServiceProxy, PREMPLOYEEDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPREMPLOYEEModalComponent } from './create-or-edit-premployee-modal.component';
import { ViewPREMPLOYEEModalComponent } from './view-premployee-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './premployees.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PREMPLOYEESComponent extends AppComponentBase {

    @ViewChild('createOrEditPREMPLOYEEModal', { static: true }) createOrEditPREMPLOYEEModal: CreateOrEditPREMPLOYEEModalComponent;
    @ViewChild('viewPREMPLOYEEModalComponent', { static: true }) viewPREMPLOYEEModal: ViewPREMPLOYEEModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    empnumFilter = '';
    nameFilter = '';
    unionnumFilter = '';
    unionlocalFilter = '';
    classFilter = '';
    wcompnuM1Filter = '';
    lastnameFilter = '';
    firstnameFilter = '';
    statusFilter = '';
    maxPAYRATEFilter : number;
		maxPAYRATEFilterEmpty : number;
		minPAYRATEFilter : number;
		minPAYRATEFilterEmpty : number;


    _entityTypeFullName = 'Nucleus.PREMPLOYEES.PREMPLOYEE';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _premployeesServiceProxy: PREMPLOYEESServiceProxy,
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

    getPREMPLOYEES(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._premployeesServiceProxy.getAll(
            this.filterText,
            this.empnumFilter,
            this.nameFilter,
            this.unionnumFilter,
            this.unionlocalFilter,
            this.classFilter,
            this.wcompnuM1Filter,
            this.lastnameFilter,
            this.firstnameFilter,
            this.statusFilter,
            this.maxPAYRATEFilter == null ? this.maxPAYRATEFilterEmpty: this.maxPAYRATEFilter,
            this.minPAYRATEFilter == null ? this.minPAYRATEFilterEmpty: this.minPAYRATEFilter,
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

    createPREMPLOYEE(): void {
        this.createOrEditPREMPLOYEEModal.show();
    }

    showHistory(premployee: PREMPLOYEEDto): void {
        this.entityTypeHistoryModal.show({
            entityId: premployee.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deletePREMPLOYEE(premployee: PREMPLOYEEDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._premployeesServiceProxy.delete(premployee.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._premployeesServiceProxy.getPREMPLOYEESToExcel(
        this.filterText,
            this.empnumFilter,
            this.nameFilter,
            this.unionnumFilter,
            this.unionlocalFilter,
            this.classFilter,
            this.wcompnuM1Filter,
            this.lastnameFilter,
            this.firstnameFilter,
            this.statusFilter,
            this.maxPAYRATEFilter == null ? this.maxPAYRATEFilterEmpty: this.maxPAYRATEFilter,
            this.minPAYRATEFilter == null ? this.minPAYRATEFilterEmpty: this.minPAYRATEFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
