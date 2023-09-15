import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEquipTimetablesForViewDto, EquipTimetablesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEquipTimetablesModal',
    templateUrl: './view-equipTimetables-modal.component.html'
})
export class ViewEquipTimetablesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEquipTimetablesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEquipTimetablesForViewDto();
        this.item.equipTimetables = new EquipTimetablesDto();
    }

    show(item: GetEquipTimetablesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
