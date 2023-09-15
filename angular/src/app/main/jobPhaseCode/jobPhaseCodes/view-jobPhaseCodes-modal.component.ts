import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJobPhaseCodesForViewDto, JobPhaseCodesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobPhaseCodesModal',
    templateUrl: './view-jobPhaseCodes-modal.component.html'
})
export class ViewJobPhaseCodesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobPhaseCodesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobPhaseCodesForViewDto();
        this.item.jobPhaseCodes = new JobPhaseCodesDto();
    }

    show(item: GetJobPhaseCodesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
