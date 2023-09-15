import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetResourceReservationsForViewDto, ResourceReservationsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewResourceReservationsModal',
    templateUrl: './view-resourceReservations-modal.component.html'
})
export class ViewResourceReservationsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetResourceReservationsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetResourceReservationsForViewDto();
        this.item.resourceReservations = new ResourceReservationsDto();
    }

    show(item: GetResourceReservationsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
