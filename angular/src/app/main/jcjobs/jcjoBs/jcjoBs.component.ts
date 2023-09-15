import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JCJOBsServiceProxy, JCJOBDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJCJOBModalComponent } from './create-or-edit-jcjob-modal.component';
import { ViewJCJOBModalComponent } from './view-jcjob-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './jcjoBs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class JCJOBsComponent extends AppComponentBase {

    @ViewChild('createOrEditJCJOBModal', { static: true }) createOrEditJCJOBModal: CreateOrEditJCJOBModalComponent;
    @ViewChild('viewJCJOBModalComponent', { static: true }) viewJCJOBModal: ViewJCJOBModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    stateFilter = '';
    localityFilter = '';
    classFilter = '';
    maxCLOSEDFilter : number;
		maxCLOSEDFilterEmpty : number;
		minCLOSEDFilter : number;
		minCLOSEDFilterEmpty : number;
        jaccatJOBNUMFilter = '';


    _entityTypeFullName = 'Nucleus.JCJOBS.JCJOB';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _jcjoBsServiceProxy: JCJOBsServiceProxy,
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

    getJCJOBs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._jcjoBsServiceProxy.getAll(
            this.filterText,
            this.stateFilter,
            this.localityFilter,
            this.classFilter,
            this.maxCLOSEDFilter == null ? this.maxCLOSEDFilterEmpty: this.maxCLOSEDFilter,
            this.minCLOSEDFilter == null ? this.minCLOSEDFilterEmpty: this.minCLOSEDFilter,
            this.jaccatJOBNUMFilter,
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

    createJCJOB(): void {
        this.createOrEditJCJOBModal.show();
    }

    showHistory(jcjob: JCJOBDto): void {
        this.entityTypeHistoryModal.show({
            entityId: jcjob.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteJCJOB(jcjob: JCJOBDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._jcjoBsServiceProxy.delete(jcjob.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._jcjoBsServiceProxy.getJCJOBsToExcel(
        this.filterText,
            this.stateFilter,
            this.localityFilter,
            this.classFilter,
            this.maxCLOSEDFilter == null ? this.maxCLOSEDFilterEmpty: this.maxCLOSEDFilter,
            this.minCLOSEDFilter == null ? this.minCLOSEDFilterEmpty: this.minCLOSEDFilter,
            this.jaccatJOBNUMFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
