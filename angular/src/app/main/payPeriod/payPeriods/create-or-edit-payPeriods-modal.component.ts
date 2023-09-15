import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PayPeriodsServiceProxy, CreateOrEditPayPeriodsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditPayPeriodsModal',
    templateUrl: './create-or-edit-payPeriods-modal.component.html'
})
export class CreateOrEditPayPeriodsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    payPeriods: CreateOrEditPayPeriodsDto = new CreateOrEditPayPeriodsDto();



    constructor(
        injector: Injector,
        private _payPeriodsServiceProxy: PayPeriodsServiceProxy
    ) {
        super(injector);
    }

    show(payPeriodsId?: number): void {

        if (!payPeriodsId) {
            this.payPeriods = new CreateOrEditPayPeriodsDto();
            this.payPeriods.id = payPeriodsId;
            this.payPeriods.startDate = moment().startOf('day');
            this.payPeriods.endDate = moment().startOf('day');

            this.active = true;
            this.modal.show();
        } else {
            this._payPeriodsServiceProxy.getPayPeriodsForEdit(payPeriodsId).subscribe(result => {
                this.payPeriods = result.payPeriods;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._payPeriodsServiceProxy.createOrEdit(this.payPeriods)
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
