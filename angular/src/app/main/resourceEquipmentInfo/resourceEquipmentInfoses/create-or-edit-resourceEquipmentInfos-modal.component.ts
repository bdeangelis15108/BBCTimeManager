import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ResourceEquipmentInfosesServiceProxy, CreateOrEditResourceEquipmentInfosDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditResourceEquipmentInfosModal',
    templateUrl: './create-or-edit-resourceEquipmentInfos-modal.component.html'
})
export class CreateOrEditResourceEquipmentInfosModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    resourceEquipmentInfos: CreateOrEditResourceEquipmentInfosDto = new CreateOrEditResourceEquipmentInfosDto();



    constructor(
        injector: Injector,
        private _resourceEquipmentInfosesServiceProxy: ResourceEquipmentInfosesServiceProxy
    ) {
        super(injector);
    }

    show(resourceEquipmentInfosId?: number): void {

        if (!resourceEquipmentInfosId) {
            this.resourceEquipmentInfos = new CreateOrEditResourceEquipmentInfosDto();
            this.resourceEquipmentInfos.id = resourceEquipmentInfosId;

            this.active = true;
            this.modal.show();
        } else {
            this._resourceEquipmentInfosesServiceProxy.getResourceEquipmentInfosForEdit(resourceEquipmentInfosId).subscribe(result => {
                this.resourceEquipmentInfos = result.resourceEquipmentInfos;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._resourceEquipmentInfosesServiceProxy.createOrEdit(this.resourceEquipmentInfos)
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
