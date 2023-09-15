import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJobsForViewDto, JobsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobsModal',
    templateUrl: './view-jobs-modal.component.html'
})
export class ViewJobsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobsForViewDto();
        this.item.jobs = new JobsDto();
    }

    show(item: GetJobsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
