import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { WorkerClaseesesServiceProxy, CreateOrEditWorkerClaseesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditWorkerClaseesModal',
    templateUrl: './create-or-edit-workerClasees-modal.component.html'
})
export class CreateOrEditWorkerClaseesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    workerClasees: CreateOrEditWorkerClaseesDto = new CreateOrEditWorkerClaseesDto();



    constructor(
        injector: Injector,
        private _workerClaseesesServiceProxy: WorkerClaseesesServiceProxy
    ) {
        super(injector);
    }

    show(workerClaseesId?: number): void {

        if (!workerClaseesId) {
            this.workerClasees = new CreateOrEditWorkerClaseesDto();
            this.workerClasees.id = workerClaseesId;

            this.active = true;
            this.modal.show();
        } else {
            this._workerClaseesesServiceProxy.getWorkerClaseesForEdit(workerClaseesId).subscribe(result => {
                this.workerClasees = result.workerClasees;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._workerClaseesesServiceProxy.createOrEdit(this.workerClasees)
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
