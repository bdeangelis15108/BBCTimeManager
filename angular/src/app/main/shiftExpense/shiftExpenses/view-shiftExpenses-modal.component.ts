import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetShiftExpensesForViewDto, ShiftExpensesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewShiftExpensesModal',
    templateUrl: './view-shiftExpenses-modal.component.html'
})
export class ViewShiftExpensesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetShiftExpensesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetShiftExpensesForViewDto();
        this.item.shiftExpenses = new ShiftExpensesDto();
    }

    show(item: GetShiftExpensesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
