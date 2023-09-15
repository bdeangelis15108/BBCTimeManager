import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JACCATsServiceProxy, CreateOrEditJACCATDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditJACCATModal',
    templateUrl: './create-or-edit-jaccat-modal.component.html'
})
export class CreateOrEditJACCATModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jaccat: CreateOrEditJACCATDto = new CreateOrEditJACCATDto();



    constructor(
        injector: Injector,
        private _jaccaTsServiceProxy: JACCATsServiceProxy
    ) {
        super(injector);
    }

    show(jaccatId?: number): void {

        if (!jaccatId) {
            this.jaccat = new CreateOrEditJACCATDto();
            this.jaccat.id = jaccatId;

            this.active = true;
            this.modal.show();
        } else {
            this._jaccaTsServiceProxy.getJACCATForEdit(jaccatId).subscribe(result => {
                this.jaccat = result.jaccat;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jaccaTsServiceProxy.createOrEdit(this.jaccat)
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
