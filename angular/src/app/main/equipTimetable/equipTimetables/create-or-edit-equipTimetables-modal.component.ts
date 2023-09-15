import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EquipTimetablesServiceProxy, CreateOrEditEquipTimetablesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { EquipTimetablesPayPeriodsLookupTableModalComponent } from './equipTimetables-payPeriods-lookup-table-modal.component';
import { EquipTimetablesResourcesLookupTableModalComponent } from './equipTimetables-resources-lookup-table-modal.component';
import { EquipTimetablesJobPhaseCodesLookupTableModalComponent } from './equipTimetables-jobPhaseCodes-lookup-table-modal.component';
import { EquipTimetablesJobCategoriesLookupTableModalComponent } from './equipTimetables-jobCategories-lookup-table-modal.component';
import { EquipTimetablesJobsLookupTableModalComponent } from './equipTimetables-jobs-lookup-table-modal.component';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'createOrEditEquipTimetablesModal',
    templateUrl: './create-or-edit-equipTimetables-modal.component.html'
})
export class CreateOrEditEquipTimetablesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('equipTimetablesPayPeriodsLookupTableModal', { static: true }) equipTimetablesPayPeriodsLookupTableModal: EquipTimetablesPayPeriodsLookupTableModalComponent;
    @ViewChild('equipTimetablesResourcesLookupTableModal', { static: true }) equipTimetablesResourcesLookupTableModal: EquipTimetablesResourcesLookupTableModalComponent;
    @ViewChild('equipTimetablesJobPhaseCodesLookupTableModal', { static: true }) equipTimetablesJobPhaseCodesLookupTableModal: EquipTimetablesJobPhaseCodesLookupTableModalComponent;
    @ViewChild('equipTimetablesJobCategoriesLookupTableModal', { static: true }) equipTimetablesJobCategoriesLookupTableModal: EquipTimetablesJobCategoriesLookupTableModalComponent;
    @ViewChild('equipTimetablesJobsLookupTableModal', { static: true }) equipTimetablesJobsLookupTableModal: EquipTimetablesJobsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    equipTimetables: CreateOrEditEquipTimetablesDto = new CreateOrEditEquipTimetablesDto();

    payPeriodsName = '';
    resourcesResourceNumber = '';
    jobPhaseCodesName = '';
    jobCategoriesName = '';
    jobsName = '';


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _equipTimetablesServiceProxy: EquipTimetablesServiceProxy
    ) {
        super(injector);
    }
    
    show(equipTimetablesId?: number): void {
    

        if (!equipTimetablesId) {
            this.equipTimetables = new CreateOrEditEquipTimetablesDto();
            this.equipTimetables.id = equipTimetablesId;
            this.equipTimetables.createdOn = moment().startOf('day');
            //this.payPeriodsName = '';
            this.resourcesResourceNumber = '';
            this.jobPhaseCodesName = '';
            this.jobCategoriesName = '';
            this.jobsName = this._activatedRoute.snapshot.queryParams.selectedCode;
            this.payPeriodsName = moment(this._activatedRoute.snapshot.queryParams.startDate).format('LL') + ' - ' + moment(this._activatedRoute.snapshot.queryParams.endDate).format('LL');
            this.active = true;
            this.modal.show();
        } else {
            this._equipTimetablesServiceProxy.getEquipTimetablesForEdit(equipTimetablesId).subscribe(result => {
                this.equipTimetables = result.equipTimetables;

                this.payPeriodsName = moment(this._activatedRoute.snapshot.queryParams.startDate).format('LL') + ' - ' + moment(this._activatedRoute.snapshot.queryParams.endDate).format('LL');
                this.resourcesResourceNumber = result.resourcesResourceNumber;
                this.jobPhaseCodesName = result.jobPhaseCodesName;
                this.jobCategoriesName = result.jobCategoriesName;
                this.jobsName = result.jobsName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			this.equipTimetables.periodDate = this._activatedRoute.snapshot.queryParams.selectedPayWeek;
            this.equipTimetables.jobCodeString = this._activatedRoute.snapshot.queryParams.selectedCode;
            this._equipTimetablesServiceProxy.createOrEdit(this.equipTimetables)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectPayPeriodsModal() {
        this.equipTimetablesPayPeriodsLookupTableModal.id = this.equipTimetables.periodDate;
        this.equipTimetablesPayPeriodsLookupTableModal.displayName = this.payPeriodsName;
        this.equipTimetablesPayPeriodsLookupTableModal.show();
    }
    openSelectResourcesModal() {
        this.equipTimetablesResourcesLookupTableModal.id = this.equipTimetables.resourcesCode;
        this.equipTimetablesResourcesLookupTableModal.displayName = this.resourcesResourceNumber;
        this.equipTimetablesResourcesLookupTableModal.show();
    }
    openSelectJobPhaseCodesModal() {
        this.equipTimetablesJobPhaseCodesLookupTableModal.id = this.equipTimetables.phaseCode;
        this.equipTimetablesJobPhaseCodesLookupTableModal.displayName = this.jobPhaseCodesName;
        this.equipTimetablesJobPhaseCodesLookupTableModal.show();
    }
    openSelectJobCategoriesModal() {
        this.equipTimetablesJobCategoriesLookupTableModal.id = this.equipTimetables.categoryCode;
        this.equipTimetablesJobCategoriesLookupTableModal.displayName = this.jobCategoriesName;
        this.equipTimetablesJobCategoriesLookupTableModal.show();
    }
    openSelectJobsModal() {
        this.equipTimetablesJobsLookupTableModal.id = this.equipTimetables.jobCode;
        this.equipTimetablesJobsLookupTableModal.displayName = this.jobsName;
        this.equipTimetablesJobsLookupTableModal.show();
    }


    setPeriodDateNull() {
        this.equipTimetables.periodDate = null;
        this.payPeriodsName = '';
    }
    setResourcesCodeNull() {
        this.equipTimetables.resourcesCode = null;
        this.resourcesResourceNumber = '';
    }
    setPhaseCodeNull() {
        this.equipTimetables.phaseCode = null;
        this.jobPhaseCodesName = '';
    }
    setCategoryCodeNull() {
        this.equipTimetables.categoryCode = null;
        this.jobCategoriesName = '';
    }
    setJobCodeNull() {
        this.equipTimetables.jobCode = null;
        this.jobsName = '';
    }


    getNewPeriodDate() {
        this.equipTimetables.periodDate = this.equipTimetablesPayPeriodsLookupTableModal.id;
        this.payPeriodsName = this.equipTimetablesPayPeriodsLookupTableModal.displayName;
    }
    getNewResourcesCode() {
        this.equipTimetables.resourcesCode = this.equipTimetablesResourcesLookupTableModal.id;
        this.resourcesResourceNumber = this.equipTimetablesResourcesLookupTableModal.displayName;
    }
    getNewPhaseCode() {
        this.equipTimetables.phaseCode = this.equipTimetablesJobPhaseCodesLookupTableModal.id;
        this.jobPhaseCodesName = this.equipTimetablesJobPhaseCodesLookupTableModal.displayName;
    }
    getNewCategoryCode() {
        this.equipTimetables.categoryCode = this.equipTimetablesJobCategoriesLookupTableModal.id;
        this.jobCategoriesName = this.equipTimetablesJobCategoriesLookupTableModal.displayName;
    }
    getNewJobCode() {
        this.equipTimetables.jobCode = this.equipTimetablesJobsLookupTableModal.id;
        this.jobsName = this.equipTimetablesJobsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
