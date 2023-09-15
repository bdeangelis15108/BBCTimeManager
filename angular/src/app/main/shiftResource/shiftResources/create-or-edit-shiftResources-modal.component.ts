import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftResourcesServiceProxy, CreateOrEditShiftResourcesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ShiftResourcesResourcesLookupTableModalComponent } from './shiftResources-resources-lookup-table-modal.component';
import { ShiftResourcesPayTypesLookupTableModalComponent } from './shiftResources-payTypes-lookup-table-modal.component';
import { ShiftResourcesJobPhaseCodesLookupTableModalComponent } from './shiftResources-jobPhaseCodes-lookup-table-modal.component';
import { ShiftResourcesJobCategoriesLookupTableModalComponent } from './shiftResources-jobCategories-lookup-table-modal.component';
import { ShiftResourcesTimesheetsLookupTableModalComponent } from './shiftResources-timesheets-lookup-table-modal.component';
import { ShiftResourcesShiftsLookupTableModalComponent } from './shiftResources-shifts-lookup-table-modal.component';
import { ShiftResourcesWorkerClaseesLookupTableModalComponent } from './shiftResources-workerClasees-lookup-table-modal.component';

@Component({
    selector: 'createOrEditShiftResourcesModal',
    templateUrl: './create-or-edit-shiftResources-modal.component.html'
})
export class CreateOrEditShiftResourcesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('shiftResourcesResourcesLookupTableModal', { static: true }) shiftResourcesResourcesLookupTableModal: ShiftResourcesResourcesLookupTableModalComponent;
    @ViewChild('shiftResourcesPayTypesLookupTableModal', { static: true }) shiftResourcesPayTypesLookupTableModal: ShiftResourcesPayTypesLookupTableModalComponent;
    @ViewChild('shiftResourcesJobPhaseCodesLookupTableModal', { static: true }) shiftResourcesJobPhaseCodesLookupTableModal: ShiftResourcesJobPhaseCodesLookupTableModalComponent;
    @ViewChild('shiftResourcesJobCategoriesLookupTableModal', { static: true }) shiftResourcesJobCategoriesLookupTableModal: ShiftResourcesJobCategoriesLookupTableModalComponent;
    @ViewChild('shiftResourcesTimesheetsLookupTableModal', { static: true }) shiftResourcesTimesheetsLookupTableModal: ShiftResourcesTimesheetsLookupTableModalComponent;
    @ViewChild('shiftResourcesShiftsLookupTableModal', { static: true }) shiftResourcesShiftsLookupTableModal: ShiftResourcesShiftsLookupTableModalComponent;
    @ViewChild('shiftResourcesWorkerClaseesLookupTableModal', { static: true }) shiftResourcesWorkerClaseesLookupTableModal: ShiftResourcesWorkerClaseesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shiftResources: CreateOrEditShiftResourcesDto = new CreateOrEditShiftResourcesDto();

    resourcesName = '';
    payTypesCode = '';
    jobPhaseCodesName = '';
    jobCategoriesName = '';
    timesheetsName = '';
    shiftsName = '';
    workerClaseesName = '';


    constructor(
        injector: Injector,
        private _shiftResourcesServiceProxy: ShiftResourcesServiceProxy
    ) {
        super(injector);
    }
    
    show(shiftResourcesId?: number): void {
    

        if (!shiftResourcesId) {
            this.shiftResources = new CreateOrEditShiftResourcesDto();
            this.shiftResources.id = shiftResourcesId;
            this.resourcesName = '';
            this.payTypesCode = '';
            this.jobPhaseCodesName = '';
            this.jobCategoriesName = '';
            this.timesheetsName = '';
            this.shiftsName = '';
            this.workerClaseesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._shiftResourcesServiceProxy.getShiftResourcesForEdit(shiftResourcesId).subscribe(result => {
                this.shiftResources = result.shiftResources;

                this.resourcesName = result.resourcesName;
                this.payTypesCode = result.payTypesCode;
                this.jobPhaseCodesName = result.jobPhaseCodesName;
                this.jobCategoriesName = result.jobCategoriesName;
                this.timesheetsName = result.timesheetsName;
                this.shiftsName = result.shiftsName;
                this.workerClaseesName = result.workerClaseesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._shiftResourcesServiceProxy.createOrEdit(this.shiftResources)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectResourcesModal() {
        this.shiftResourcesResourcesLookupTableModal.id = this.shiftResources.resourcesId;
        this.shiftResourcesResourcesLookupTableModal.displayName = this.resourcesName;
        this.shiftResourcesResourcesLookupTableModal.show();
    }
    openSelectPayTypesModal() {
        this.shiftResourcesPayTypesLookupTableModal.id = this.shiftResources.payTypesId;
        this.shiftResourcesPayTypesLookupTableModal.displayName = this.payTypesCode;
        this.shiftResourcesPayTypesLookupTableModal.show();
    }
    openSelectJobPhaseCodesModal() {
        this.shiftResourcesJobPhaseCodesLookupTableModal.id = this.shiftResources.jobPhaseCodesId;
        this.shiftResourcesJobPhaseCodesLookupTableModal.displayName = this.jobPhaseCodesName;
        this.shiftResourcesJobPhaseCodesLookupTableModal.show();
    }
    openSelectJobCategoriesModal() {
        this.shiftResourcesJobCategoriesLookupTableModal.id = this.shiftResources.jobCategoriesId;
        this.shiftResourcesJobCategoriesLookupTableModal.displayName = this.jobCategoriesName;
        this.shiftResourcesJobCategoriesLookupTableModal.show();
    }
    openSelectTimesheetsModal() {
        this.shiftResourcesTimesheetsLookupTableModal.id = this.shiftResources.timesheetsId;
        this.shiftResourcesTimesheetsLookupTableModal.displayName = this.timesheetsName;
        this.shiftResourcesTimesheetsLookupTableModal.show();
    }
    openSelectShiftsModal() {
        this.shiftResourcesShiftsLookupTableModal.id = this.shiftResources.shiftsId;
        this.shiftResourcesShiftsLookupTableModal.displayName = this.shiftsName;
        this.shiftResourcesShiftsLookupTableModal.show();
    }
    openSelectWorkerClaseesModal() {
        this.shiftResourcesWorkerClaseesLookupTableModal.id = this.shiftResources.workerClaseesId;
        this.shiftResourcesWorkerClaseesLookupTableModal.displayName = this.workerClaseesName;
        this.shiftResourcesWorkerClaseesLookupTableModal.show();
    }


    setResourcesIdNull() {
        this.shiftResources.resourcesId = null;
        this.resourcesName = '';
    }
    setPayTypesIdNull() {
        this.shiftResources.payTypesId = null;
        this.payTypesCode = '';
    }
    setJobPhaseCodesIdNull() {
        this.shiftResources.jobPhaseCodesId = null;
        this.jobPhaseCodesName = '';
    }
    setJobCategoriesIdNull() {
        this.shiftResources.jobCategoriesId = null;
        this.jobCategoriesName = '';
    }
    setTimesheetsIdNull() {
        this.shiftResources.timesheetsId = null;
        this.timesheetsName = '';
    }
    setShiftsIdNull() {
        this.shiftResources.shiftsId = null;
        this.shiftsName = '';
    }
    setWorkerClaseesIdNull() {
        this.shiftResources.workerClaseesId = null;
        this.workerClaseesName = '';
    }


    getNewResourcesId() {
        this.shiftResources.resourcesId = this.shiftResourcesResourcesLookupTableModal.id;
        this.resourcesName = this.shiftResourcesResourcesLookupTableModal.displayName;
    }
    getNewPayTypesId() {
        this.shiftResources.payTypesId = this.shiftResourcesPayTypesLookupTableModal.id;
        this.payTypesCode = this.shiftResourcesPayTypesLookupTableModal.displayName;
    }
    getNewJobPhaseCodesId() {
        this.shiftResources.jobPhaseCodesId = this.shiftResourcesJobPhaseCodesLookupTableModal.id;
        this.jobPhaseCodesName = this.shiftResourcesJobPhaseCodesLookupTableModal.displayName;
    }
    getNewJobCategoriesId() {
        this.shiftResources.jobCategoriesId = this.shiftResourcesJobCategoriesLookupTableModal.id;
        this.jobCategoriesName = this.shiftResourcesJobCategoriesLookupTableModal.displayName;
    }
    getNewTimesheetsId() {
        this.shiftResources.timesheetsId = this.shiftResourcesTimesheetsLookupTableModal.id;
        this.timesheetsName = this.shiftResourcesTimesheetsLookupTableModal.displayName;
    }
    getNewShiftsId() {
        this.shiftResources.shiftsId = this.shiftResourcesShiftsLookupTableModal.id;
        this.shiftsName = this.shiftResourcesShiftsLookupTableModal.displayName;
    }
    getNewWorkerClaseesId() {
        this.shiftResources.workerClaseesId = this.shiftResourcesWorkerClaseesLookupTableModal.id;
        this.workerClaseesName = this.shiftResourcesWorkerClaseesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
