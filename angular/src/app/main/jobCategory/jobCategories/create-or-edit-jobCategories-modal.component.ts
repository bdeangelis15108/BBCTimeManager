import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JobCategoriesServiceProxy, CreateOrEditJobCategoriesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditJobCategoriesModal',
    templateUrl: './create-or-edit-jobCategories-modal.component.html'
})
export class CreateOrEditJobCategoriesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobCategories: CreateOrEditJobCategoriesDto = new CreateOrEditJobCategoriesDto();



    constructor(
        injector: Injector,
        private _jobCategoriesServiceProxy: JobCategoriesServiceProxy
    ) {
        super(injector);
    }

    show(jobCategoriesId?: number): void {

        if (!jobCategoriesId) {
            this.jobCategories = new CreateOrEditJobCategoriesDto();
            this.jobCategories.id = jobCategoriesId;

            this.active = true;
            this.modal.show();
        } else {
            this._jobCategoriesServiceProxy.getJobCategoriesForEdit(jobCategoriesId).subscribe(result => {
                this.jobCategories = result.jobCategories;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jobCategoriesServiceProxy.createOrEdit(this.jobCategories)
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
