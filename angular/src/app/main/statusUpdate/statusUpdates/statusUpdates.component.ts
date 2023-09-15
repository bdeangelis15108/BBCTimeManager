import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { StatusUpdatesServiceProxy, StatusUpdatesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStatusUpdatesModalComponent } from './create-or-edit-statusUpdates-modal.component';

import { ViewStatusUpdatesModalComponent } from './view-statusUpdates-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './statusUpdates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StatusUpdatesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditStatusUpdatesModal', { static: true }) createOrEditStatusUpdatesModal: CreateOrEditStatusUpdatesModalComponent;
    @ViewChild('viewStatusUpdatesModalComponent', { static: true }) viewStatusUpdatesModal: ViewStatusUpdatesModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxModifiedOnFilter : moment.Moment;
		minModifiedOnFilter : moment.Moment;
    nameFilter = 'Submit';
    maxOriginalstatusIdFilter : number;
		maxOriginalstatusIdFilterEmpty : number;
		minOriginalstatusIdFilter : number;
		minOriginalstatusIdFilterEmpty : number;
    maxActualCreateDateTimeFilter : moment.Moment;
		minActualCreateDateTimeFilter : moment.Moment;
    timeshetIdsFilter = '';
        timesheetsNameFilter = '';
        statusesNameFilter = '';
        jobsNameFilter = '';
        userNameFilter = '';
    





    constructor(
        injector: Injector,
        private _statusUpdatesServiceProxy: StatusUpdatesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router
    ) {
        super(injector);
    }

    getStatusUpdates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._statusUpdatesServiceProxy.getAll(
            this.filterText,
            this.maxModifiedOnFilter === undefined ? this.maxModifiedOnFilter : moment(this.maxModifiedOnFilter).endOf('day'),
            this.minModifiedOnFilter === undefined ? this.minModifiedOnFilter : moment(this.minModifiedOnFilter).startOf('day'),
            this.nameFilter,
            this.maxOriginalstatusIdFilter == null ? this.maxOriginalstatusIdFilterEmpty: this.maxOriginalstatusIdFilter,
            this.minOriginalstatusIdFilter == null ? this.minOriginalstatusIdFilterEmpty: this.minOriginalstatusIdFilter,
            this.maxActualCreateDateTimeFilter === undefined ? this.maxActualCreateDateTimeFilter : moment(this.maxActualCreateDateTimeFilter).endOf('day'),
            this.minActualCreateDateTimeFilter === undefined ? this.minActualCreateDateTimeFilter : moment(this.minActualCreateDateTimeFilter).startOf('day'),
            this.timeshetIdsFilter,
            this.timesheetsNameFilter,
            this.statusesNameFilter,
            this.jobsNameFilter,
            this.userNameFilter,
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

    createStatusUpdates(): void {
        this.createOrEditStatusUpdatesModal.show();        
    }


    deleteStatusUpdates(statusUpdates: StatusUpdatesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._statusUpdatesServiceProxy.delete(statusUpdates.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._statusUpdatesServiceProxy.getStatusUpdatesToExcel(
        this.filterText,
            this.maxModifiedOnFilter === undefined ? this.maxModifiedOnFilter : moment(this.maxModifiedOnFilter).endOf('day'),
            this.minModifiedOnFilter === undefined ? this.minModifiedOnFilter : moment(this.minModifiedOnFilter).startOf('day'),
            this.nameFilter,
            this.maxOriginalstatusIdFilter == null ? this.maxOriginalstatusIdFilterEmpty: this.maxOriginalstatusIdFilter,
            this.minOriginalstatusIdFilter == null ? this.minOriginalstatusIdFilterEmpty: this.minOriginalstatusIdFilter,
            this.maxActualCreateDateTimeFilter === undefined ? this.maxActualCreateDateTimeFilter : moment(this.maxActualCreateDateTimeFilter).endOf('day'),
            this.minActualCreateDateTimeFilter === undefined ? this.minActualCreateDateTimeFilter : moment(this.minActualCreateDateTimeFilter).startOf('day'),
            this.timeshetIdsFilter,
            this.timesheetsNameFilter,
            this.statusesNameFilter,
            this.jobsNameFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    viewAllSubmission(): void {
        this._router.navigate(['/app/main/job/jobses']);
    }
}
