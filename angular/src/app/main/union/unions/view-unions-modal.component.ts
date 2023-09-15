import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetUnionsForViewDto, UnionsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewUnionsModal',
    templateUrl: './view-unions-modal.component.html'
})
export class ViewUnionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetUnionsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetUnionsForViewDto();
        this.item.unions = new UnionsDto();
    }

    show(item: GetUnionsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
