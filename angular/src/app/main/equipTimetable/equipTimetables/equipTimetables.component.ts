import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { EquipTimetablesServiceProxy, EquipTimetablesDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEquipTimetablesModalComponent } from './create-or-edit-equipTimetables-modal.component';

import { ViewEquipTimetablesModalComponent } from './view-equipTimetables-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './equipTimetables.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EquipTimetablesComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditEquipTimetablesModal', { static: true }) createOrEditEquipTimetablesModal: CreateOrEditEquipTimetablesModalComponent;
    @ViewChild('viewEquipTimetablesModalComponent', { static: true }) viewEquipTimetablesModal: ViewEquipTimetablesModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDay1Filter : number;
		maxDay1FilterEmpty : number;
		minDay1Filter : number;
		minDay1FilterEmpty : number;
    maxDay2Filter : number;
		maxDay2FilterEmpty : number;
		minDay2Filter : number;
		minDay2FilterEmpty : number;
    maxDay3Filter : number;
		maxDay3FilterEmpty : number;
		minDay3Filter : number;
		minDay3FilterEmpty : number;
    maxDay4Filter : number;
		maxDay4FilterEmpty : number;
		minDay4Filter : number;
		minDay4FilterEmpty : number;
    maxDay5Filter : number;
		maxDay5FilterEmpty : number;
		minDay5Filter : number;
		minDay5FilterEmpty : number;
    maxDay6Filter : number;
		maxDay6FilterEmpty : number;
		minDay6Filter : number;
		minDay6FilterEmpty : number;
    maxDay7Filter : number;
		maxDay7FilterEmpty : number;
		minDay7Filter : number;
		minDay7FilterEmpty : number;
    maxAmountFilter : number;
		maxAmountFilterEmpty : number;
		minAmountFilter : number;
		minAmountFilterEmpty : number;
    maxCreatedOnFilter : moment.Moment;
		minCreatedOnFilter : moment.Moment;
    isActiveFilter = -1;
        payPeriodsNameFilter = '';
        resourcesResourceNumberFilter = '';
        jobPhaseCodesNameFilter = '';
        jobCategoriesNameFilter = '';
        jobsNameFilter = '';






    constructor(
        injector: Injector,
        private _equipTimetablesServiceProxy: EquipTimetablesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEquipTimetables(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._equipTimetablesServiceProxy.getAll(
            this.filterText,
            this._activatedRoute.snapshot.queryParams.selectedCode,
            this._activatedRoute.snapshot.queryParams.selectedPayWeek,
            this.maxDay1Filter == null ? this.maxDay1FilterEmpty: this.maxDay1Filter,
            this.minDay1Filter == null ? this.minDay1FilterEmpty: this.minDay1Filter,
            this.maxDay2Filter == null ? this.maxDay2FilterEmpty: this.maxDay2Filter,
            this.minDay2Filter == null ? this.minDay2FilterEmpty: this.minDay2Filter,
            this.maxDay3Filter == null ? this.maxDay3FilterEmpty: this.maxDay3Filter,
            this.minDay3Filter == null ? this.minDay3FilterEmpty: this.minDay3Filter,
            this.maxDay4Filter == null ? this.maxDay4FilterEmpty: this.maxDay4Filter,
            this.minDay4Filter == null ? this.minDay4FilterEmpty: this.minDay4Filter,
            this.maxDay5Filter == null ? this.maxDay5FilterEmpty: this.maxDay5Filter,
            this.minDay5Filter == null ? this.minDay5FilterEmpty: this.minDay5Filter,
            this.maxDay6Filter == null ? this.maxDay6FilterEmpty: this.maxDay6Filter,
            this.minDay6Filter == null ? this.minDay6FilterEmpty: this.minDay6Filter,
            this.maxDay7Filter == null ? this.maxDay7FilterEmpty: this.maxDay7Filter,
            this.minDay7Filter == null ? this.minDay7FilterEmpty: this.minDay7Filter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.maxCreatedOnFilter === undefined ? this.maxCreatedOnFilter : moment(this.maxCreatedOnFilter).endOf('day'),
            this.minCreatedOnFilter === undefined ? this.minCreatedOnFilter : moment(this.minCreatedOnFilter).startOf('day'),
            this.isActiveFilter,
            this.payPeriodsNameFilter,
            this.resourcesResourceNumberFilter,
            this.jobPhaseCodesNameFilter,
            this.jobCategoriesNameFilter,
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

    createEquipTimetables(): void {
        this.createOrEditEquipTimetablesModal.show();        
    }


    deleteEquipTimetables(equipTimetables: EquipTimetablesDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._equipTimetablesServiceProxy.delete(equipTimetables.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }
    getReadableDay(day) {
      var inital = moment(this._activatedRoute.snapshot.queryParams.startDate).toDate();
      var date = new Date(inital).setDate(new Date(inital).getDate() + day);
      return moment(date).format('DD ddd')
  }

  formateDays(value) {
      return value === null ? 0 : value;
  }
  goBackToJobSelection(): void {
    this._router.navigate(['/app/main/job/jobses']);
  }
    exportToExcel(): void {
            this._equipTimetablesServiceProxy.getEquipTimetablesToExcel(
            this.filterText,
            this._activatedRoute.snapshot.queryParams.selectedCode,
            this._activatedRoute.snapshot.queryParams.selectedPayWeek,
            this.maxDay1Filter == null ? this.maxDay1FilterEmpty: this.maxDay1Filter,
            this.minDay1Filter == null ? this.minDay1FilterEmpty: this.minDay1Filter,
            this.maxDay2Filter == null ? this.maxDay2FilterEmpty: this.maxDay2Filter,
            this.minDay2Filter == null ? this.minDay2FilterEmpty: this.minDay2Filter,
            this.maxDay3Filter == null ? this.maxDay3FilterEmpty: this.maxDay3Filter,
            this.minDay3Filter == null ? this.minDay3FilterEmpty: this.minDay3Filter,
            this.maxDay4Filter == null ? this.maxDay4FilterEmpty: this.maxDay4Filter,
            this.minDay4Filter == null ? this.minDay4FilterEmpty: this.minDay4Filter,
            this.maxDay5Filter == null ? this.maxDay5FilterEmpty: this.maxDay5Filter,
            this.minDay5Filter == null ? this.minDay5FilterEmpty: this.minDay5Filter,
            this.maxDay6Filter == null ? this.maxDay6FilterEmpty: this.maxDay6Filter,
            this.minDay6Filter == null ? this.minDay6FilterEmpty: this.minDay6Filter,
            this.maxDay7Filter == null ? this.maxDay7FilterEmpty: this.maxDay7Filter,
            this.minDay7Filter == null ? this.minDay7FilterEmpty: this.minDay7Filter,
            this.maxAmountFilter == null ? this.maxAmountFilterEmpty: this.maxAmountFilter,
            this.minAmountFilter == null ? this.minAmountFilterEmpty: this.minAmountFilter,
            this.maxCreatedOnFilter === undefined ? this.maxCreatedOnFilter : moment(this.maxCreatedOnFilter).endOf('day'),
            this.minCreatedOnFilter === undefined ? this.minCreatedOnFilter : moment(this.minCreatedOnFilter).startOf('day'),
            this.isActiveFilter,
            this.payPeriodsNameFilter,
            this.resourcesResourceNumberFilter,
            this.jobPhaseCodesNameFilter,
            this.jobCategoriesNameFilter,
            this.jobsNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
}
