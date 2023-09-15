import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { UnionsServiceProxy, CreateOrEditUnionsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditUnionsModal',
    templateUrl: './create-or-edit-unions-modal.component.html'
})
export class CreateOrEditUnionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    unions: CreateOrEditUnionsDto = new CreateOrEditUnionsDto();



    constructor(
        injector: Injector,
        private _unionsServiceProxy: UnionsServiceProxy
    ) {
        super(injector);
    }

    show(unionsId?: number): void {

        if (!unionsId) {
            this.unions = new CreateOrEditUnionsDto();
            this.unions.id = unionsId;

            this.active = true;
            this.modal.show();
        } else {
            this._unionsServiceProxy.getUnionsForEdit(unionsId).subscribe(result => {
                this.unions = result.unions;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._unionsServiceProxy.createOrEdit(this.unions)
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
