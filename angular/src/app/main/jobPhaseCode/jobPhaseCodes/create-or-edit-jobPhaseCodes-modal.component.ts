import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JobPhaseCodesServiceProxy, CreateOrEditJobPhaseCodesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { JobPhaseCodesJobsLookupTableModalComponent } from './jobPhaseCodes-jobs-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobPhaseCodesModal',
    templateUrl: './create-or-edit-jobPhaseCodes-modal.component.html'
})
export class CreateOrEditJobPhaseCodesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobPhaseCodesJobsLookupTableModal', { static: true }) jobPhaseCodesJobsLookupTableModal: JobPhaseCodesJobsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobPhaseCodes: CreateOrEditJobPhaseCodesDto = new CreateOrEditJobPhaseCodesDto();

    jobsName = '';


    constructor(
        injector: Injector,
        private _jobPhaseCodesServiceProxy: JobPhaseCodesServiceProxy
    ) {
        super(injector);
    }

    show(jobPhaseCodesId?: number): void {

        if (!jobPhaseCodesId) {
            this.jobPhaseCodes = new CreateOrEditJobPhaseCodesDto();
            this.jobPhaseCodes.id = jobPhaseCodesId;
            this.jobsName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobPhaseCodesServiceProxy.getJobPhaseCodesForEdit(jobPhaseCodesId).subscribe(result => {
                this.jobPhaseCodes = result.jobPhaseCodes;

                this.jobsName = result.jobsName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jobPhaseCodesServiceProxy.createOrEdit(this.jobPhaseCodes)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJobsModal() {
        this.jobPhaseCodesJobsLookupTableModal.id = this.jobPhaseCodes.jobsId;
        this.jobPhaseCodesJobsLookupTableModal.displayName = this.jobsName;
        this.jobPhaseCodesJobsLookupTableModal.show();
    }


    setJobsIdNull() {
        this.jobPhaseCodes.jobsId = null;
        this.jobsName = '';
    }


    getNewJobsId() {
        this.jobPhaseCodes.jobsId = this.jobPhaseCodesJobsLookupTableModal.id;
        this.jobsName = this.jobPhaseCodesJobsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
