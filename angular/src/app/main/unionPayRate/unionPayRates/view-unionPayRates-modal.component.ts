import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetUnionPayRatesForViewDto, UnionPayRatesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewUnionPayRatesModal',
    templateUrl: './view-unionPayRates-modal.component.html'
})
export class ViewUnionPayRatesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetUnionPayRatesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetUnionPayRatesForViewDto();
        this.item.unionPayRates = new UnionPayRatesDto();
    }

    show(item: GetUnionPayRatesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
