import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetShiftsForViewDto, ShiftsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewShiftsModal',
    templateUrl: './view-shifts-modal.component.html'
})
export class ViewShiftsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetShiftsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetShiftsForViewDto();
        this.item.shifts = new ShiftsDto();
    }

    show(item: GetShiftsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
