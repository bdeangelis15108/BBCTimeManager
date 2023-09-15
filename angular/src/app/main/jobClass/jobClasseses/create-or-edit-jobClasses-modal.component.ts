import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JobClassesesServiceProxy, CreateOrEditJobClassesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditJobClassesModal',
    templateUrl: './create-or-edit-jobClasses-modal.component.html'
})
export class CreateOrEditJobClassesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobClasses: CreateOrEditJobClassesDto = new CreateOrEditJobClassesDto();



    constructor(
        injector: Injector,
        private _jobClassesesServiceProxy: JobClassesesServiceProxy
    ) {
        super(injector);
    }

    show(jobClassesId?: number): void {

        if (!jobClassesId) {
            this.jobClasses = new CreateOrEditJobClassesDto();
            this.jobClasses.id = jobClassesId;

            this.active = true;
            this.modal.show();
        } else {
            this._jobClassesesServiceProxy.getJobClassesForEdit(jobClassesId).subscribe(result => {
                this.jobClasses = result.jobClasses;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jobClassesesServiceProxy.createOrEdit(this.jobClasses)
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
