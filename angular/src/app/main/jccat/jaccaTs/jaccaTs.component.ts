import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JACCATsServiceProxy, JACCATDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJACCATModalComponent } from './create-or-edit-jaccat-modal.component';
import { ViewJACCATModalComponent } from './view-jaccat-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './jaccaTs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JACCATsComponent extends AppComponentBase {

    @ViewChild('createOrEditJACCATModal', { static: true }) createOrEditJACCATModal: CreateOrEditJACCATModalComponent;
    @ViewChild('viewJACCATModalComponent', { static: true }) viewJACCATModal: ViewJACCATModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxSEQUENCEFilter : number;
		maxSEQUENCEFilterEmpty : number;
		minSEQUENCEFilter : number;
		minSEQUENCEFilterEmpty : number;
    jobnumFilter = '';
    phasenumFilter = '';
    catnumFilter = '';
    nameFilter = '';


    _entityTypeFullName = 'Nucleus.JCCAT.JACCAT';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _jaccaTsServiceProxy: JACCATsServiceProxy,
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

    getJACCATs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jaccaTsServiceProxy.getAll(
            this.filterText,
            this.maxSEQUENCEFilter == null ? this.maxSEQUENCEFilterEmpty: this.maxSEQUENCEFilter,
            this.minSEQUENCEFilter == null ? this.minSEQUENCEFilterEmpty: this.minSEQUENCEFilter,
            this.jobnumFilter,
            this.phasenumFilter,
            this.catnumFilter,
            this.nameFilter,
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

    createJACCAT(): void {
        this.createOrEditJACCATModal.show();
    }

    showHistory(jaccat: JACCATDto): void {
        this.entityTypeHistoryModal.show({
            entityId: jaccat.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteJACCAT(jaccat: JACCATDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jaccaTsServiceProxy.delete(jaccat.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jaccaTsServiceProxy.getJACCATsToExcel(
        this.filterText,
            this.maxSEQUENCEFilter == null ? this.maxSEQUENCEFilterEmpty: this.maxSEQUENCEFilter,
            this.minSEQUENCEFilter == null ? this.minSEQUENCEFilterEmpty: this.minSEQUENCEFilter,
            this.jobnumFilter,
            this.phasenumFilter,
            this.catnumFilter,
            this.nameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
