import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftExpensesServiceProxy, CreateOrEditShiftExpensesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ShiftExpensesShiftResourcesLookupTableModalComponent } from './shiftExpenses-shiftResources-lookup-table-modal.component';
import { ShiftExpensesExpenseTypesLookupTableModalComponent } from './shiftExpenses-expenseTypes-lookup-table-modal.component';

@Component({
    selector: 'createOrEditShiftExpensesModal',
    templateUrl: './create-or-edit-shiftExpenses-modal.component.html'
})
export class CreateOrEditShiftExpensesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('shiftExpensesShiftResourcesLookupTableModal', { static: true }) shiftExpensesShiftResourcesLookupTableModal: ShiftExpensesShiftResourcesLookupTableModalComponent;
    @ViewChild('shiftExpensesExpenseTypesLookupTableModal', { static: true }) shiftExpensesExpenseTypesLookupTableModal: ShiftExpensesExpenseTypesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shiftExpenses: CreateOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();

    shiftResourcesName = '';
    expenseTypesName = '';


    constructor(
        injector: Injector,
        private _shiftExpensesServiceProxy: ShiftExpensesServiceProxy
    ) {
        super(injector);
    }

    show(shiftExpensesId?: number): void {

        if (!shiftExpensesId) {
            this.shiftExpenses = new CreateOrEditShiftExpensesDto();
            this.shiftExpenses.id = shiftExpensesId;
            this.shiftResourcesName = '';
            this.expenseTypesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._shiftExpensesServiceProxy.getShiftExpensesForEdit(shiftExpensesId).subscribe(result => {
                this.shiftExpenses = result.shiftExpenses;

                this.shiftResourcesName = result.shiftResourcesName;
                this.expenseTypesName = result.expenseTypesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._shiftExpensesServiceProxy.createOrEdit(this.shiftExpenses)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectShiftResourcesModal() {
        this.shiftExpensesShiftResourcesLookupTableModal.id = this.shiftExpenses.shiftResourcesId;
        this.shiftExpensesShiftResourcesLookupTableModal.displayName = this.shiftResourcesName;
        this.shiftExpensesShiftResourcesLookupTableModal.show();
    }
    openSelectExpenseTypesModal() {
        this.shiftExpensesExpenseTypesLookupTableModal.id = this.shiftExpenses.expenseTypesId;
        this.shiftExpensesExpenseTypesLookupTableModal.displayName = this.expenseTypesName;
        this.shiftExpensesExpenseTypesLookupTableModal.show();
    }


    setShiftResourcesIdNull() {
        this.shiftExpenses.shiftResourcesId = null;
        this.shiftResourcesName = '';
    }
    setExpenseTypesIdNull() {
        this.shiftExpenses.expenseTypesId = null;
        this.expenseTypesName = '';
    }


    getNewShiftResourcesId() {
        this.shiftExpenses.shiftResourcesId = this.shiftExpensesShiftResourcesLookupTableModal.id;
        this.shiftResourcesName = this.shiftExpensesShiftResourcesLookupTableModal.displayName;
    }
    getNewExpenseTypesId() {
        this.shiftExpenses.expenseTypesId = this.shiftExpensesExpenseTypesLookupTableModal.id;
        this.expenseTypesName = this.shiftExpensesExpenseTypesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
