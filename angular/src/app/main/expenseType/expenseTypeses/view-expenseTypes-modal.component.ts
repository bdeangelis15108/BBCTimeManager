import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetExpenseTypesForViewDto, ExpenseTypesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewExpenseTypesModal',
    templateUrl: './view-expenseTypes-modal.component.html'
})
export class ViewExpenseTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetExpenseTypesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetExpenseTypesForViewDto();
        this.item.expenseTypes = new ExpenseTypesDto();
    }

    show(item: GetExpenseTypesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
