import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetJCJOBForViewDto, JCJOBDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJCJOBModal',
    templateUrl: './view-jcjob-modal.component.html'
})
export class ViewJCJOBModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJCJOBForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJCJOBForViewDto();
        this.item.jcjob = new JCJOBDto();
    }

    show(item: GetJCJOBForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
