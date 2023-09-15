import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAddressesForViewDto, AddressesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAddressesModal',
    templateUrl: './view-addresses-modal.component.html'
})
export class ViewAddressesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAddressesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAddressesForViewDto();
        this.item.addresses = new AddressesDto();
    }

    show(item: GetAddressesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
