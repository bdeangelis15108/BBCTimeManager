import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetResourcesForViewDto, ResourcesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewResourcesModal',
    templateUrl: './view-resources-modal.component.html'
})
export class ViewResourcesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetResourcesForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetResourcesForViewDto();
        this.item.resources = new ResourcesDto();
    }

    show(item: GetResourcesForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
