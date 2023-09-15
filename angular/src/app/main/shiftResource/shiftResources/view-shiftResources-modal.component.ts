import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetShiftResourcesForViewDto, ShiftResourcesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewShiftResourcesModal',
    templateUrl: './view-shiftResources-modal.component.html'
})
export class ViewShiftResourcesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetShiftResourcesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetShiftResourcesForViewDto();
        this.item.shiftResources = new ShiftResourcesDto();
    }

    show(item: GetShiftResourcesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
