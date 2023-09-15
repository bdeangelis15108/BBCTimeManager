import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { UnionPayRatesServiceProxy, CreateOrEditUnionPayRatesDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UnionPayRatesUnionsLookupTableModalComponent } from './unionPayRates-unions-lookup-table-modal.component';

@Component({
    selector: 'createOrEditUnionPayRatesModal',
    templateUrl: './create-or-edit-unionPayRates-modal.component.html'
})
export class CreateOrEditUnionPayRatesModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('unionPayRatesUnionsLookupTableModal', { static: true }) unionPayRatesUnionsLookupTableModal: UnionPayRatesUnionsLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    unionPayRates: CreateOrEditUnionPayRatesDto = new CreateOrEditUnionPayRatesDto();

    unionsNumber = '';


    constructor(
        injector: Injector,
        private _unionPayRatesServiceProxy: UnionPayRatesServiceProxy
    ) {
        super(injector);
    }
    
    show(unionPayRatesId?: number): void {
    

        if (!unionPayRatesId) {
            this.unionPayRates = new CreateOrEditUnionPayRatesDto();
            this.unionPayRates.id = unionPayRatesId;
            this.unionsNumber = '';

            this.active = true;
            this.modal.show();
        } else {
            this._unionPayRatesServiceProxy.getUnionPayRatesForEdit(unionPayRatesId).subscribe(result => {
                this.unionPayRates = result.unionPayRates;

                this.unionsNumber = result.unionsNumber;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._unionPayRatesServiceProxy.createOrEdit(this.unionPayRates)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUnionsModal() {
        this.unionPayRatesUnionsLookupTableModal.id = this.unionPayRates.unionsId;
        this.unionPayRatesUnionsLookupTableModal.displayName = this.unionsNumber;
        this.unionPayRatesUnionsLookupTableModal.show();
    }


    setUnionsIdNull() {
        this.unionPayRates.unionsId = null;
        this.unionsNumber = '';
    }


    getNewUnionsId() {
        this.unionPayRates.unionsId = this.unionPayRatesUnionsLookupTableModal.id;
        this.unionsNumber = this.unionPayRatesUnionsLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
