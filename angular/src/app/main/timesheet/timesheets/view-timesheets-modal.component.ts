import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTimesheetsForViewDto, TimesheetsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTimesheetsModal',
    templateUrl: './view-timesheets-modal.component.html'
})
export class ViewTimesheetsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTimesheetsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTimesheetsForViewDto();
        this.item.timesheets = new TimesheetsDto();
    }

    show(item: GetTimesheetsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
