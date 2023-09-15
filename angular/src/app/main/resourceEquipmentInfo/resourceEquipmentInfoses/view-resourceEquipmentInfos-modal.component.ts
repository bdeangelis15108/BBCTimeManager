import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetResourceEquipmentInfosForViewDto, ResourceEquipmentInfosDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewResourceEquipmentInfosModal',
    templateUrl: './view-resourceEquipmentInfos-modal.component.html'
})
export class ViewResourceEquipmentInfosModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetResourceEquipmentInfosForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetResourceEquipmentInfosForViewDto();
        this.item.resourceEquipmentInfos = new ResourceEquipmentInfosDto();
    }

    show(item: GetResourceEquipmentInfosForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
