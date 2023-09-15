import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetECCOSTForViewDto, ECCOSTDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewECCOSTModal',
    templateUrl: './view-eccost-modal.component.html'
})
export class ViewECCOSTModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetECCOSTForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetECCOSTForViewDto();
        this.item.eccost = new ECCOSTDto();
    }

    show(item: GetECCOSTForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
