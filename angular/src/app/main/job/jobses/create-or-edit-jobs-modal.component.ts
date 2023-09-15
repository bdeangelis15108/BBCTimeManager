import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JobsesServiceProxy, CreateOrEditJobsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { JobsAddressesLookupTableModalComponent } from './jobs-addresses-lookup-table-modal.component';
import { JobsJobClassesLookupTableModalComponent } from './jobs-jobClasses-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJobsModal',
    templateUrl: './create-or-edit-jobs-modal.component.html'
})
export class CreateOrEditJobsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobsAddressesLookupTableModal', { static: true }) jobsAddressesLookupTableModal: JobsAddressesLookupTableModalComponent;
    @ViewChild('jobsJobClassesLookupTableModal', { static: true }) jobsJobClassesLookupTableModal: JobsJobClassesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobs: CreateOrEditJobsDto = new CreateOrEditJobsDto();

    addressesLinne1 = '';
    jobClassesName = '';


    constructor(
        injector: Injector,
        private _jobsesServiceProxy: JobsesServiceProxy
    ) {
        super(injector);
    }

    show(jobsId?: number): void {

        if (!jobsId) {
            this.jobs = new CreateOrEditJobsDto();
            this.jobs.id = jobsId;
            this.jobs.startDate = moment().startOf('day');
            this.jobs.endDate = moment().startOf('day');
            this.addressesLinne1 = '';
            this.jobClassesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jobsesServiceProxy.getJobsForEdit(jobsId).subscribe(result => {
                this.jobs = result.jobs;

                this.addressesLinne1 = result.addressesLinne1;
                this.jobClassesName = result.jobClassesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jobsesServiceProxy.createOrEdit(this.jobs)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectAddressesModal() {
        this.jobsAddressesLookupTableModal.id = this.jobs.addressesId;
        this.jobsAddressesLookupTableModal.displayName = this.addressesLinne1;
        this.jobsAddressesLookupTableModal.show();
    }
    openSelectJobClassesModal() {
        this.jobsJobClassesLookupTableModal.id = this.jobs.jobClassesId;
        this.jobsJobClassesLookupTableModal.displayName = this.jobClassesName;
        this.jobsJobClassesLookupTableModal.show();
    }


    setAddressesIdNull() {
        this.jobs.addressesId = null;
        this.addressesLinne1 = '';
    }
    setJobClassesIdNull() {
        this.jobs.jobClassesId = null;
        this.jobClassesName = '';
    }


    getNewAddressesId() {
        this.jobs.addressesId = this.jobsAddressesLookupTableModal.id;
        this.addressesLinne1 = this.jobsAddressesLookupTableModal.displayName;
    }
    getNewJobClassesId() {
        this.jobs.jobClassesId = this.jobsJobClassesLookupTableModal.id;
        this.jobClassesName = this.jobsJobClassesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
