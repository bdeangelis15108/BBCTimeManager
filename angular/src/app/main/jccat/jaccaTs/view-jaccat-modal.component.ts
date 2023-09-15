import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJACCATForViewDto, JACCATDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJACCATModal',
    templateUrl: './view-jaccat-modal.component.html'
})
export class ViewJACCATModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJACCATForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJACCATForViewDto();
        this.item.jaccat = new JACCATDto();
    }

    show(item: GetJACCATForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
