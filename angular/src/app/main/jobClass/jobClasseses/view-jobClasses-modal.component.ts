import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJobClassesForViewDto, JobClassesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobClassesModal',
    templateUrl: './view-jobClasses-modal.component.html'
})
export class ViewJobClassesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobClassesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobClassesForViewDto();
        this.item.jobClasses = new JobClassesDto();
    }

    show(item: GetJobClassesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
