import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTimetablesForViewDto, TimetablesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTimetablesModal',
    templateUrl: './view-timetables-modal.component.html'
})
export class ViewTimetablesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTimetablesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTimetablesForViewDto();
        this.item.timetables = new TimetablesDto();
    }

    show(item: GetTimetablesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
        console.log(this.item);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
