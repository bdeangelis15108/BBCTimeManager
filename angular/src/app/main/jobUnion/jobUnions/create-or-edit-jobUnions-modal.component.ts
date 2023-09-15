import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JobUnionsServiceProxy, CreateOrEditJobUnionsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { JobUnionsJobsLookupTableModalComponent } from './jobUnions-jobs-lookup-table-modal.component';
import { JobUnionsUnionsLookupTableModalComponent } from './jobUnions-unions-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobUnionsModal',
    templateUrl: './create-or-edit-jobUnions-modal.component.html'
})
export class CreateOrEditJobUnionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobUnionsJobsLookupTableModal', { static: true }) jobUnionsJobsLookupTableModal: JobUnionsJobsLookupTableModalComponent;
    @ViewChild('jobUnionsUnionsLookupTableModal', { static: true }) jobUnionsUnionsLookupTableModal: JobUnionsUnionsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobUnions: CreateOrEditJobUnionsDto = new CreateOrEditJobUnionsDto();

    jobsName = '';
    unionsNumber = '';


    constructor(
        injector: Injector,
        private _jobUnionsServiceProxy: JobUnionsServiceProxy
    ) {
        super(injector);
    }

    show(jobUnionsId?: number): void {

        if (!jobUnionsId) {
            this.jobUnions = new CreateOrEditJobUnionsDto();
            this.jobUnions.id = jobUnionsId;
            this.jobsName = '';
            this.unionsNumber = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobUnionsServiceProxy.getJobUnionsForEdit(jobUnionsId).subscribe(result => {
                this.jobUnions = result.jobUnions;

                this.jobsName = result.jobsName;
                this.unionsNumber = result.unionsNumber;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jobUnionsServiceProxy.createOrEdit(this.jobUnions)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJobsModal() {
        this.jobUnionsJobsLookupTableModal.id = this.jobUnions.jobsId;
        this.jobUnionsJobsLookupTableModal.displayName = this.jobsName;
        this.jobUnionsJobsLookupTableModal.show();
    }
    openSelectUnionsModal() {
        this.jobUnionsUnionsLookupTableModal.id = this.jobUnions.unionsId;
        this.jobUnionsUnionsLookupTableModal.displayName = this.unionsNumber;
        this.jobUnionsUnionsLookupTableModal.show();
    }


    setJobsIdNull() {
        this.jobUnions.jobsId = null;
        this.jobsName = '';
    }
    setUnionsIdNull() {
        this.jobUnions.unionsId = null;
        this.unionsNumber = '';
    }


    getNewJobsId() {
        this.jobUnions.jobsId = this.jobUnionsJobsLookupTableModal.id;
        this.jobsName = this.jobUnionsJobsLookupTableModal.displayName;
    }
    getNewUnionsId() {
        this.jobUnions.unionsId = this.jobUnionsUnionsLookupTableModal.id;
        this.unionsNumber = this.jobUnionsUnionsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
