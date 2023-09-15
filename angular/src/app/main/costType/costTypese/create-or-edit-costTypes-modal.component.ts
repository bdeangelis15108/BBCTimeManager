import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { CostTypeseServiceProxy, CreateOrEditCostTypesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditCostTypesModal',
    templateUrl: './create-or-edit-costTypes-modal.component.html'
})
export class CreateOrEditCostTypesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    costTypes: CreateOrEditCostTypesDto = new CreateOrEditCostTypesDto();



    constructor(
        injector: Injector,
        private _costTypeseServiceProxy: CostTypeseServiceProxy
    ) {
        super(injector);
    }
    
    show(costTypesId?: number): void {
    

        if (!costTypesId) {
            this.costTypes = new CreateOrEditCostTypesDto();
            this.costTypes.id = costTypesId;

            this.active = true;
            this.modal.show();
        } else {
            this._costTypeseServiceProxy.getCostTypesForEdit(costTypesId).subscribe(result => {
                this.costTypes = result.costTypes;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._costTypeseServiceProxy.createOrEdit(this.costTypes)
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
