﻿import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetPRCLASSForViewDto, PRCLASSDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPRCLASSModal',
    templateUrl: './view-prclass-modal.component.html'
})
export class ViewPRCLASSModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPRCLASSForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPRCLASSForViewDto();
        this.item.prclass = new PRCLASSDto();
    }

    show(item: GetPRCLASSForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
