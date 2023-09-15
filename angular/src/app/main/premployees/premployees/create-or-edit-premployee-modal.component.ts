import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PREMPLOYEESServiceProxy, CreateOrEditPREMPLOYEEDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditPREMPLOYEEModal',
    templateUrl: './create-or-edit-premployee-modal.component.html'
})
export class CreateOrEditPREMPLOYEEModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    premployee: CreateOrEditPREMPLOYEEDto = new CreateOrEditPREMPLOYEEDto();



    constructor(
        injector: Injector,
        private _premployeesServiceProxy: PREMPLOYEESServiceProxy
    ) {
        super(injector);
    }

    show(premployeeId?: number): void {

        if (!premployeeId) {
            this.premployee = new CreateOrEditPREMPLOYEEDto();
            this.premployee.id = premployeeId;

            this.active = true;
            this.modal.show();
        } else {
            this._premployeesServiceProxy.getPREMPLOYEEForEdit(premployeeId).subscribe(result => {
                this.premployee = result.premployee;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._premployeesServiceProxy.createOrEdit(this.premployee)
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
