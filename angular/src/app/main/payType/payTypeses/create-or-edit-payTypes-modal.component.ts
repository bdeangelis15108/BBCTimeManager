import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PayTypesesServiceProxy, CreateOrEditPayTypesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditPayTypesModal',
    templateUrl: './create-or-edit-payTypes-modal.component.html'
})
export class CreateOrEditPayTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    payTypes: CreateOrEditPayTypesDto = new CreateOrEditPayTypesDto();



    constructor(
        injector: Injector,
        private _payTypesesServiceProxy: PayTypesesServiceProxy
    ) {
        super(injector);
    }

    show(payTypesId?: number): void {

        if (!payTypesId) {
            this.payTypes = new CreateOrEditPayTypesDto();
            this.payTypes.id = payTypesId;

            this.active = true;
            this.modal.show();
        } else {
            this._payTypesesServiceProxy.getPayTypesForEdit(payTypesId).subscribe(result => {
                this.payTypes = result.payTypes;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._payTypesesServiceProxy.createOrEdit(this.payTypes)
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
