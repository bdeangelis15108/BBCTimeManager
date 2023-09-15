import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAccountsForViewDto, AccountsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAccountsModal',
    templateUrl: './view-accounts-modal.component.html'
})
export class ViewAccountsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAccountsForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAccountsForViewDto();
        this.item.accounts = new AccountsDto();
    }

    show(item: GetAccountsForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
