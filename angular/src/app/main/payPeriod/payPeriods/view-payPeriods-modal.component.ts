import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetPayPeriodsForViewDto, PayPeriodsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPayPeriodsModal',
    templateUrl: './view-payPeriods-modal.component.html'
})
export class ViewPayPeriodsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPayPeriodsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPayPeriodsForViewDto();
        this.item.payPeriods = new PayPeriodsDto();
    }

    show(item: GetPayPeriodsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
