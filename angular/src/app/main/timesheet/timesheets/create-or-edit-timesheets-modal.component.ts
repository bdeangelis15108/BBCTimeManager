import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimesheetsServiceProxy, CreateOrEditTimesheetsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TimesheetsStatusesLookupTableModalComponent } from './timesheets-statuses-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTimesheetsModal',
    templateUrl: './create-or-edit-timesheets-modal.component.html'
})
export class CreateOrEditTimesheetsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timesheetsStatusesLookupTableModal', { static: true }) timesheetsStatusesLookupTableModal: TimesheetsStatusesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timesheets: CreateOrEditTimesheetsDto = new CreateOrEditTimesheetsDto();

    statusesName = '';


    constructor(
        injector: Injector,
        private _timesheetsServiceProxy: TimesheetsServiceProxy
    ) {
        super(injector);
    }

    show(timesheetsId?: number): void {

        if (!timesheetsId) {
            this.timesheets = new CreateOrEditTimesheetsDto();
            this.timesheets.id = timesheetsId;
            this.timesheets.createdDate = moment().startOf('day');
            this.timesheets.submitedDate = moment().startOf('day');
            this.statusesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._timesheetsServiceProxy.getTimesheetsForEdit(timesheetsId).subscribe(result => {
                this.timesheets = result.timesheets;

                this.statusesName = result.statusesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._timesheetsServiceProxy.createOrEdit(this.timesheets)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectStatusesModal() {
        this.timesheetsStatusesLookupTableModal.id = this.timesheets.statusesId;
        this.timesheetsStatusesLookupTableModal.displayName = this.statusesName;
        this.timesheetsStatusesLookupTableModal.show();
    }


    setStatusesIdNull() {
        this.timesheets.statusesId = null;
        this.statusesName = '';
    }


    getNewStatusesId() {
        this.timesheets.statusesId = this.timesheetsStatusesLookupTableModal.id;
        this.statusesName = this.timesheetsStatusesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
