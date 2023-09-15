import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetStatusesForViewDto, StatusesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStatusesModal',
    templateUrl: './view-statuses-modal.component.html'
})
export class ViewStatusesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStatusesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetStatusesForViewDto();
        this.item.statuses = new StatusesDto();
    }

    show(item: GetStatusesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
