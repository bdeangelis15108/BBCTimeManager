import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJCUNIONForViewDto, JCUNIONDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJCUNIONModal',
    templateUrl: './view-jcunion-modal.component.html'
})
export class ViewJCUNIONModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJCUNIONForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJCUNIONForViewDto();
        this.item.jcunion = new JCUNIONDto();
    }

    show(item: GetJCUNIONForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
