import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AddressesesServiceProxy, CreateOrEditAddressesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditAddressesModal',
    templateUrl: './create-or-edit-addresses-modal.component.html'
})
export class CreateOrEditAddressesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    addresses: CreateOrEditAddressesDto = new CreateOrEditAddressesDto();



    constructor(
        injector: Injector,
        private _addressesesServiceProxy: AddressesesServiceProxy
    ) {
        super(injector);
    }

    show(addressesId?: number): void {

        if (!addressesId) {
            this.addresses = new CreateOrEditAddressesDto();
            this.addresses.id = addressesId;

            this.active = true;
            this.modal.show();
        } else {
            this._addressesesServiceProxy.getAddressesForEdit(addressesId).subscribe(result => {
                this.addresses = result.addresses;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._addressesesServiceProxy.createOrEdit(this.addresses)
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
