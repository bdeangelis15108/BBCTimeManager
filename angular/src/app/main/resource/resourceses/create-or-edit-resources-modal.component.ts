import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ResourcesesServiceProxy, CreateOrEditResourcesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditResourcesModal',
    templateUrl: './create-or-edit-resources-modal.component.html'
})
export class CreateOrEditResourcesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    resources: CreateOrEditResourcesDto = new CreateOrEditResourcesDto();



    constructor(
        injector: Injector,
        private _resourcesesServiceProxy: ResourcesesServiceProxy
    ) {
        super(injector);
    }

    show(resourcesId?: number): void {

        if (!resourcesId) {
            this.resources = new CreateOrEditResourcesDto();
            this.resources.id = resourcesId;

            this.active = true;
            this.modal.show();
        } else {
            this._resourcesesServiceProxy.getResourcesForEdit(resourcesId).subscribe(result => {
                this.resources = result.resources;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._resourcesesServiceProxy.createOrEdit(this.resources)
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
