import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetStatusUpdatesForViewDto, StatusUpdatesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStatusUpdatesModal',
    templateUrl: './view-statusUpdates-modal.component.html'
})
export class ViewStatusUpdatesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStatusUpdatesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetStatusUpdatesForViewDto();
        this.item.statusUpdates = new StatusUpdatesDto();
    }

    show(item: GetStatusUpdatesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
