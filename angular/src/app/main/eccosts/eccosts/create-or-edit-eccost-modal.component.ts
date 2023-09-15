import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ECCOSTSServiceProxy, CreateOrEditECCOSTDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ECCOSTEQUIPMENTLookupTableModalComponent } from './eccost-equipment-lookup-table-modal.component';

@Component({
    selector: 'createOrEditECCOSTModal',
    templateUrl: './create-or-edit-eccost-modal.component.html'
})
export class CreateOrEditECCOSTModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('eccostEQUIPMENTLookupTableModal', { static: true }) eccostEQUIPMENTLookupTableModal: ECCOSTEQUIPMENTLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    eccost: CreateOrEditECCOSTDto = new CreateOrEditECCOSTDto();

    equipmentEQUIPNUM = '';


    constructor(
        injector: Injector,
        private _eccostsServiceProxy: ECCOSTSServiceProxy
    ) {
        super(injector);
    }

    show(eccostId?: number): void {

        if (!eccostId) {
            this.eccost = new CreateOrEditECCOSTDto();
            this.eccost.id = eccostId;
            this.equipmentEQUIPNUM = '';

            this.active = true;
            this.modal.show();
        } else {
            this._eccostsServiceProxy.getECCOSTForEdit(eccostId).subscribe(result => {
                this.eccost = result.eccost;

                this.equipmentEQUIPNUM = result.equipmentequipnum;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._eccostsServiceProxy.createOrEdit(this.eccost)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectEQUIPMENTModal() {
        this.eccostEQUIPMENTLookupTableModal.id = this.eccost.equipnum;
        this.eccostEQUIPMENTLookupTableModal.displayName = this.equipmentEQUIPNUM;
        this.eccostEQUIPMENTLookupTableModal.show();
    }


    setEQUIPNUMNull() {
        this.eccost.equipnum = null;
        this.equipmentEQUIPNUM = '';
    }


    getNewEQUIPNUM() {
        this.eccost.equipnum = this.eccostEQUIPMENTLookupTableModal.id;
        this.equipmentEQUIPNUM = this.eccostEQUIPMENTLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
