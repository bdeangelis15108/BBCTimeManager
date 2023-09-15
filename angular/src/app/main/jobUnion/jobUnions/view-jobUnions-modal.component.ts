import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJobUnionsForViewDto, JobUnionsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobUnionsModal',
    templateUrl: './view-jobUnions-modal.component.html'
})
export class ViewJobUnionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobUnionsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobUnionsForViewDto();
        this.item.jobUnions = new JobUnionsDto();
    }

    show(item: GetJobUnionsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
