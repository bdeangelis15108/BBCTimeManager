import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetPayperiodHistoriesForViewDto, PayperiodHistoriesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPayperiodHistoriesModal',
    templateUrl: './view-payperiodHistories-modal.component.html'
})
export class ViewPayperiodHistoriesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPayperiodHistoriesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPayperiodHistoriesForViewDto();
        this.item.payperiodHistories = new PayperiodHistoriesDto();
    }

    show(item: GetPayperiodHistoriesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
