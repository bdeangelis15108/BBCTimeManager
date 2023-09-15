import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftsServiceProxy, CreateOrEditShiftsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ShiftsJobsLookupTableModalComponent } from './shifts-jobs-lookup-table-modal.component';

@Component({
    selector: 'createOrEditShiftsModal',
    templateUrl: './create-or-edit-shifts-modal.component.html'
})
export class CreateOrEditShiftsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('shiftsJobsLookupTableModal', { static: true }) shiftsJobsLookupTableModal: ShiftsJobsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shifts: CreateOrEditShiftsDto = new CreateOrEditShiftsDto();

    jobsName = '';


    constructor(
        injector: Injector,
        private _shiftsServiceProxy: ShiftsServiceProxy
    ) {
        super(injector);
    }

    show(shiftsId?: number): void {

        if (!shiftsId) {
            this.shifts = new CreateOrEditShiftsDto();
            this.shifts.id = shiftsId;
            this.shifts.scheduledStart = moment().startOf('day');
            this.shifts.scheduledEnd = moment().startOf('day');
            this.jobsName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._shiftsServiceProxy.getShiftsForEdit(shiftsId).subscribe(result => {
                this.shifts = result.shifts;

                this.jobsName = result.jobsName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._shiftsServiceProxy.createOrEdit(this.shifts)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJobsModal() {
        this.shiftsJobsLookupTableModal.id = this.shifts.jobsId;
        this.shiftsJobsLookupTableModal.displayName = this.jobsName;
        this.shiftsJobsLookupTableModal.show();
    }


    setJobsIdNull() {
        this.shifts.jobsId = null;
        this.jobsName = '';
    }


    getNewJobsId() {
        this.shifts.jobsId = this.shiftsJobsLookupTableModal.id;
        this.jobsName = this.shiftsJobsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
