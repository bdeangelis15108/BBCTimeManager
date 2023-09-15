import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PRDEDRATESServiceProxy, CreateOrEditPRDEDRATEDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { PRDEDRATEPRCLASSLookupTableModalComponent } from './prdedrate-prclass-lookup-table-modal.component';

@Component({
    selector: 'createOrEditPRDEDRATEModal',
    templateUrl: './create-or-edit-prdedrate-modal.component.html'
})
export class CreateOrEditPRDEDRATEModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('prdedratePRCLASSLookupTableModal', { static: true }) prdedratePRCLASSLookupTableModal: PRDEDRATEPRCLASSLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    prdedrate: CreateOrEditPRDEDRATEDto = new CreateOrEditPRDEDRATEDto();

    prclassUNIONNUM = '';


    constructor(
        injector: Injector,
        private _prdedratesServiceProxy: PRDEDRATESServiceProxy
    ) {
        super(injector);
    }

    show(prdedrateId?: number): void {

        if (!prdedrateId) {
            this.prdedrate = new CreateOrEditPRDEDRATEDto();
            this.prdedrate.id = prdedrateId;
            this.prclassUNIONNUM = '';

            this.active = true;
            this.modal.show();
        } else {
            this._prdedratesServiceProxy.getPRDEDRATEForEdit(prdedrateId).subscribe(result => {
                this.prdedrate = result.prdedrate;

                this.prclassUNIONNUM = result.prclassunionnum;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._prdedratesServiceProxy.createOrEdit(this.prdedrate)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectPRCLASSModal() {
        this.prdedratePRCLASSLookupTableModal.id = this.prdedrate.unionnum;
        this.prdedratePRCLASSLookupTableModal.displayName = this.prclassUNIONNUM;
        this.prdedratePRCLASSLookupTableModal.show();
    }


    setUNIONNUMNull() {
        this.prdedrate.unionnum = null;
        this.prclassUNIONNUM = '';
    }


    getNewUNIONNUM() {
        this.prdedrate.unionnum = this.prdedratePRCLASSLookupTableModal.id;
        this.prclassUNIONNUM = this.prdedratePRCLASSLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
