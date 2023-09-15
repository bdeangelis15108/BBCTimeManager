import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetPREMPLOYEEForViewDto, PREMPLOYEEDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPREMPLOYEEModal',
    templateUrl: './view-premployee-modal.component.html'
})
export class ViewPREMPLOYEEModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPREMPLOYEEForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPREMPLOYEEForViewDto();
        this.item.premployee = new PREMPLOYEEDto();
    }

    show(item: GetPREMPLOYEEForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
