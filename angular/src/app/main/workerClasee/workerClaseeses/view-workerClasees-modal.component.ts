import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetWorkerClaseesForViewDto, WorkerClaseesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewWorkerClaseesModal',
    templateUrl: './view-workerClasees-modal.component.html'
})
export class ViewWorkerClaseesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetWorkerClaseesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetWorkerClaseesForViewDto();
        this.item.workerClasees = new WorkerClaseesDto();
    }

    show(item: GetWorkerClaseesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
