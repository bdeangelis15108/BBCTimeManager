import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EQUIPMENTSServiceProxy, CreateOrEditEQUIPMENTDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditEQUIPMENTModal',
    templateUrl: './create-or-edit-equipment-modal.component.html'
})
export class CreateOrEditEQUIPMENTModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    equipment: CreateOrEditEQUIPMENTDto = new CreateOrEditEQUIPMENTDto();



    constructor(
        injector: Injector,
        private _equipmentsServiceProxy: EQUIPMENTSServiceProxy
    ) {
        super(injector);
    }

    show(equipmentId?: number): void {

        if (!equipmentId) {
            this.equipment = new CreateOrEditEQUIPMENTDto();
            this.equipment.id = equipmentId;

            this.active = true;
            this.modal.show();
        } else {
            this._equipmentsServiceProxy.getEQUIPMENTForEdit(equipmentId).subscribe(result => {
                this.equipment = result.equipment;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._equipmentsServiceProxy.createOrEdit(this.equipment)
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
