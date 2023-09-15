import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { StatusesServiceProxy, CreateOrEditStatusesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditStatusesModal',
    templateUrl: './create-or-edit-statuses-modal.component.html'
})
export class CreateOrEditStatusesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    statuses: CreateOrEditStatusesDto = new CreateOrEditStatusesDto();



    constructor(
        injector: Injector,
        private _statusesServiceProxy: StatusesServiceProxy
    ) {
        super(injector);
    }

    show(statusesId?: number): void {

        if (!statusesId) {
            this.statuses = new CreateOrEditStatusesDto();
            this.statuses.id = statusesId;

            this.active = true;
            this.modal.show();
        } else {
            this._statusesServiceProxy.getStatusesForEdit(statusesId).subscribe(result => {
                this.statuses = result.statuses;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._statusesServiceProxy.createOrEdit(this.statuses)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
