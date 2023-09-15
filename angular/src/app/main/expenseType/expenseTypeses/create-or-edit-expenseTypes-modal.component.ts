import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ExpenseTypesesServiceProxy, CreateOrEditExpenseTypesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditExpenseTypesModal',
    templateUrl: './create-or-edit-expenseTypes-modal.component.html'
})
export class CreateOrEditExpenseTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    expenseTypes: CreateOrEditExpenseTypesDto = new CreateOrEditExpenseTypesDto();



    constructor(
        injector: Injector,
        private _expenseTypesesServiceProxy: ExpenseTypesesServiceProxy
    ) {
        super(injector);
    }

    show(expenseTypesId?: number): void {

        if (!expenseTypesId) {
            this.expenseTypes = new CreateOrEditExpenseTypesDto();
            this.expenseTypes.id = expenseTypesId;

            this.active = true;
            this.modal.show();
        } else {
            this._expenseTypesesServiceProxy.getExpenseTypesForEdit(expenseTypesId).subscribe(result => {
                this.expenseTypes = result.expenseTypes;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._expenseTypesesServiceProxy.createOrEdit(this.expenseTypes)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
