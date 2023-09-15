import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEQUIPMENTForViewDto, EQUIPMENTDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEQUIPMENTModal',
    templateUrl: './view-equipment-modal.component.html'
})
export class ViewEQUIPMENTModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEQUIPMENTForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEQUIPMENTForViewDto();
        this.item.equipment = new EQUIPMENTDto();
    }

    show(item: GetEQUIPMENTForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
