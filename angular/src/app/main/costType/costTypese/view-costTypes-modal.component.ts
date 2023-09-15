import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetCostTypesForViewDto, CostTypesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCostTypesModal',
    templateUrl: './view-costTypes-modal.component.html'
})
export class ViewCostTypesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCostTypesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCostTypesForViewDto();
        this.item.costTypes = new CostTypesDto();
    }

    show(item: GetCostTypesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
