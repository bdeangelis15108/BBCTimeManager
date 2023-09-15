import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PayperiodHistoriesServiceProxy, CreateOrEditPayperiodHistoriesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { PayperiodHistoriesPayPeriodsLookupTableModalComponent } from './payperiodHistories-payPeriods-lookup-table-modal.component';

@Component({
    selector: 'createOrEditPayperiodHistoriesModal',
    templateUrl: './create-or-edit-payperiodHistories-modal.component.html'
})
export class CreateOrEditPayperiodHistoriesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('payperiodHistoriesPayPeriodsLookupTableModal', { static: true }) payperiodHistoriesPayPeriodsLookupTableModal: PayperiodHistoriesPayPeriodsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    payperiodHistories: CreateOrEditPayperiodHistoriesDto = new CreateOrEditPayperiodHistoriesDto();

    payPeriodsName = '';


    constructor(
        injector: Injector,
        private _payperiodHistoriesServiceProxy: PayperiodHistoriesServiceProxy
    ) {
        super(injector);
    }
    
    show(payperiodHistoriesId?: number): void {
    

        if (!payperiodHistoriesId) {
            this.payperiodHistories = new CreateOrEditPayperiodHistoriesDto();
            this.payperiodHistories.id = payperiodHistoriesId;
            this.payPeriodsName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._payperiodHistoriesServiceProxy.getPayperiodHistoriesForEdit(payperiodHistoriesId).subscribe(result => {
                this.payperiodHistories = result.payperiodHistories;

                this.payPeriodsName = result.payPeriodsName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._payperiodHistoriesServiceProxy.createOrEdit(this.payperiodHistories)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectPayPeriodsModal() {
        this.payperiodHistoriesPayPeriodsLookupTableModal.id = this.payperiodHistories.payPeriodsId;
        this.payperiodHistoriesPayPeriodsLookupTableModal.displayName = this.payPeriodsName;
        this.payperiodHistoriesPayPeriodsLookupTableModal.show();
    }


    setPayPeriodsIdNull() {
        this.payperiodHistories.payPeriodsId = null;
        this.payPeriodsName = '';
    }


    getNewPayPeriodsId() {
        this.payperiodHistories.payPeriodsId = this.payperiodHistoriesPayPeriodsLookupTableModal.id;
        this.payPeriodsName = this.payperiodHistoriesPayPeriodsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
