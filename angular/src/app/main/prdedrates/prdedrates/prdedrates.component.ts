import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PRDEDRATESServiceProxy, PRDEDRATEDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPRDEDRATEModalComponent } from './create-or-edit-prdedrate-modal.component';
import { ViewPRDEDRATEModalComponent } from './view-prdedrate-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './prdedrates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PRDEDRATESComponent extends AppComponentBase {

    @ViewChild('createOrEditPRDEDRATEModal', { static: true }) createOrEditPRDEDRATEModal: CreateOrEditPRDEDRATEModalComponent;
    @ViewChild('viewPRDEDRATEModalComponent', { static: true }) viewPRDEDRATEModal: ViewPRDEDRATEModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    unionlocalFilter = '';
    classFilter = '';
    maxDEDTYPEFilter : number;
		maxDEDTYPEFilterEmpty : number;
		minDEDTYPEFilter : number;
		minDEDTYPEFilterEmpty : number;
    maxPERHRFilter : number;
		maxPERHRFilterEmpty : number;
		minPERHRFilter : number;
		minPERHRFilterEmpty : number;
        prclassUNIONNUMFilter = '';


    _entityTypeFullName = 'Nucleus.PRDEDRATES.PRDEDRATE';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _prdedratesServiceProxy: PRDEDRATESServiceProxy,
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

    getPRDEDRATES(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._prdedratesServiceProxy.getAll(
            this.filterText,
            this.unionlocalFilter,
            this.classFilter,
            this.maxDEDTYPEFilter == null ? this.maxDEDTYPEFilterEmpty: this.maxDEDTYPEFilter,
            this.minDEDTYPEFilter == null ? this.minDEDTYPEFilterEmpty: this.minDEDTYPEFilter,
            this.maxPERHRFilter == null ? this.maxPERHRFilterEmpty: this.maxPERHRFilter,
            this.minPERHRFilter == null ? this.minPERHRFilterEmpty: this.minPERHRFilter,
            this.prclassUNIONNUMFilter,
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

    createPRDEDRATE(): void {
        this.createOrEditPRDEDRATEModal.show();
    }

    showHistory(prdedrate: PRDEDRATEDto): void {
        this.entityTypeHistoryModal.show({
            entityId: prdedrate.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deletePRDEDRATE(prdedrate: PRDEDRATEDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._prdedratesServiceProxy.delete(prdedrate.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._prdedratesServiceProxy.getPRDEDRATESToExcel(
        this.filterText,
            this.unionlocalFilter,
            this.classFilter,
            this.maxDEDTYPEFilter == null ? this.maxDEDTYPEFilterEmpty: this.maxDEDTYPEFilter,
            this.minDEDTYPEFilter == null ? this.minDEDTYPEFilterEmpty: this.minDEDTYPEFilter,
            this.maxPERHRFilter == null ? this.maxPERHRFilterEmpty: this.maxPERHRFilter,
            this.minPERHRFilter == null ? this.minPERHRFilterEmpty: this.minPERHRFilter,
            this.prclassUNIONNUMFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
