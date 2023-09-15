import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { StatusUpdatesServiceProxy, CreateOrEditStatusUpdatesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { StatusUpdatesTimesheetsLookupTableModalComponent } from './statusUpdates-timesheets-lookup-table-modal.component';
import { StatusUpdatesStatusesLookupTableModalComponent } from './statusUpdates-statuses-lookup-table-modal.component';
import { StatusUpdatesJobsLookupTableModalComponent } from './statusUpdates-jobs-lookup-table-modal.component';
import { StatusUpdatesUserLookupTableModalComponent } from './statusUpdates-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStatusUpdatesModal',
    templateUrl: './create-or-edit-statusUpdates-modal.component.html'
})
export class CreateOrEditStatusUpdatesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('statusUpdatesTimesheetsLookupTableModal', { static: true }) statusUpdatesTimesheetsLookupTableModal: StatusUpdatesTimesheetsLookupTableModalComponent;
    @ViewChild('statusUpdatesStatusesLookupTableModal', { static: true }) statusUpdatesStatusesLookupTableModal: StatusUpdatesStatusesLookupTableModalComponent;
    @ViewChild('statusUpdatesJobsLookupTableModal', { static: true }) statusUpdatesJobsLookupTableModal: StatusUpdatesJobsLookupTableModalComponent;
    @ViewChild('statusUpdatesUserLookupTableModal', { static: true }) statusUpdatesUserLookupTableModal: StatusUpdatesUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    statusUpdates: CreateOrEditStatusUpdatesDto = new CreateOrEditStatusUpdatesDto();

    timesheetsName = '';
    statusesName = '';
    jobsName = '';
    userName = '';


    constructor(
        injector: Injector,
        private _statusUpdatesServiceProxy: StatusUpdatesServiceProxy
    ) {
        super(injector);
    }
    
    show(statusUpdatesId?: number): void {
    

        if (!statusUpdatesId) {
            this.statusUpdates = new CreateOrEditStatusUpdatesDto();
            this.statusUpdates.id = statusUpdatesId;
            this.statusUpdates.modifiedOn = moment().startOf('day');
            this.statusUpdates.actualCreateDateTime = moment().startOf('day');
            this.timesheetsName = '';
            this.statusesName = '';
            this.jobsName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._statusUpdatesServiceProxy.getStatusUpdatesForEdit(statusUpdatesId).subscribe(result => {
                this.statusUpdates = result.statusUpdates;

                this.timesheetsName = result.timesheetsName;
                this.statusesName = result.statusesName;
                this.jobsName = result.jobsName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._statusUpdatesServiceProxy.createOrEdit(this.statusUpdates)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTimesheetsModal() {
        this.statusUpdatesTimesheetsLookupTableModal.id = this.statusUpdates.timesheetsId;
        this.statusUpdatesTimesheetsLookupTableModal.displayName = this.timesheetsName;
        this.statusUpdatesTimesheetsLookupTableModal.show();
    }
    openSelectStatusesModal() {
        this.statusUpdatesStatusesLookupTableModal.id = this.statusUpdates.newStatusesId;
        this.statusUpdatesStatusesLookupTableModal.displayName = this.statusesName;
        this.statusUpdatesStatusesLookupTableModal.show();
    }
    openSelectJobsModal() {
        this.statusUpdatesJobsLookupTableModal.id = this.statusUpdates.jobsId;
        this.statusUpdatesJobsLookupTableModal.displayName = this.jobsName;
        this.statusUpdatesJobsLookupTableModal.show();
    }
    openSelectUserModal() {
        this.statusUpdatesUserLookupTableModal.id = this.statusUpdates.modifiedBy;
        this.statusUpdatesUserLookupTableModal.displayName = this.userName;
        this.statusUpdatesUserLookupTableModal.show();
    }


    setTimesheetsIdNull() {
        this.statusUpdates.timesheetsId = null;
        this.timesheetsName = '';
    }
    setNewStatusesIdNull() {
        this.statusUpdates.newStatusesId = null;
        this.statusesName = '';
    }
    setJobsIdNull() {
        this.statusUpdates.jobsId = null;
        this.jobsName = '';
    }
    setModifiedByNull() {
        this.statusUpdates.modifiedBy = null;
        this.userName = '';
    }


    getNewTimesheetsId() {
        this.statusUpdates.timesheetsId = this.statusUpdatesTimesheetsLookupTableModal.id;
        this.timesheetsName = this.statusUpdatesTimesheetsLookupTableModal.displayName;
    }
    getNewNewStatusesId() {
        this.statusUpdates.newStatusesId = this.statusUpdatesStatusesLookupTableModal.id;
        this.statusesName = this.statusUpdatesStatusesLookupTableModal.displayName;
    }
    getNewJobsId() {
        this.statusUpdates.jobsId = this.statusUpdatesJobsLookupTableModal.id;
        this.jobsName = this.statusUpdatesJobsLookupTableModal.displayName;
    }
    getNewModifiedBy() {
        this.statusUpdates.modifiedBy = this.statusUpdatesUserLookupTableModal.id;
        this.userName = this.statusUpdatesUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
