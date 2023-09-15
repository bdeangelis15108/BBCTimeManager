import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJobCategoriesForViewDto, JobCategoriesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobCategoriesModal',
    templateUrl: './view-jobCategories-modal.component.html'
})
export class ViewJobCategoriesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobCategoriesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobCategoriesForViewDto();
        this.item.jobCategories = new JobCategoriesDto();
    }

    show(item: GetJobCategoriesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
