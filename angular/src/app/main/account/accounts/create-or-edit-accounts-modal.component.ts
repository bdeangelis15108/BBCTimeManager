import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AccountsServiceProxy, CreateOrEditAccountsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditAccountsModal',
    templateUrl: './create-or-edit-accounts-modal.component.html'
})
export class CreateOrEditAccountsModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    accounts: CreateOrEditAccountsDto = new CreateOrEditAccountsDto();



    constructor(
        injector: Injector,
        private _accountsServiceProxy: AccountsServiceProxy
    ) {
        super(injector);
    }
    
    show(accountsId?: number): void {
    

        if (!accountsId) {
            this.accounts = new CreateOrEditAccountsDto();
            this.accounts.id = accountsId;

            this.active = true;
            this.modal.show();
        } else {
            this._accountsServiceProxy.getAccountsForEdit(accountsId).subscribe(result => {
                this.accounts = result.accounts;


                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._accountsServiceProxy.createOrEdit(this.accounts)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
