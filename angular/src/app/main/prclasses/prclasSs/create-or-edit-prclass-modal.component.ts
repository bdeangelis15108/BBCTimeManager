import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PRCLASSsServiceProxy, CreateOrEditPRCLASSDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { PRCLASSJCUNIONLookupTableModalComponent } from './prclass-jcunion-lookup-table-modal.component';
import { PRCLASSPREMPLOYEELookupTableModalComponent } from './prclass-premployee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditPRCLASSModal',
    templateUrl: './create-or-edit-prclass-modal.component.html'
})
export class CreateOrEditPRCLASSModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('prclassJCUNIONLookupTableModal', { static: true }) prclassJCUNIONLookupTableModal: PRCLASSJCUNIONLookupTableModalComponent;
    @ViewChild('prclassPREMPLOYEELookupTableModal', { static: true }) prclassPREMPLOYEELookupTableModal: PRCLASSPREMPLOYEELookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    prclass: CreateOrEditPRCLASSDto = new CreateOrEditPRCLASSDto();

    jcunionUNIONNUM = '';
    premployeeCLASS = '';


    constructor(
        injector: Injector,
        private _prclasSsServiceProxy: PRCLASSsServiceProxy
    ) {
        super(injector);
    }

    show(prclassId?: number): void {

        if (!prclassId) {
            this.prclass = new CreateOrEditPRCLASSDto();
            this.prclass.id = prclassId;
            this.jcunionUNIONNUM = '';
            this.premployeeCLASS = '';

            this.active = true;
            this.modal.show();
        } else {
            this._prclasSsServiceProxy.getPRCLASSForEdit(prclassId).subscribe(result => {
                this.prclass = result.prclass;

                this.jcunionUNIONNUM = result.jcunionunionnum;
                this.premployeeCLASS = result.premployeeclass;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._prclasSsServiceProxy.createOrEdit(this.prclass)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJCUNIONModal() {
        this.prclassJCUNIONLookupTableModal.id = this.prclass.unionnum;
        this.prclassJCUNIONLookupTableModal.displayName = this.jcunionUNIONNUM;
        this.prclassJCUNIONLookupTableModal.show();
    }
    openSelectPREMPLOYEEModal() {
        this.prclassPREMPLOYEELookupTableModal.id = this.prclass.class;
        this.prclassPREMPLOYEELookupTableModal.displayName = this.premployeeCLASS;
        this.prclassPREMPLOYEELookupTableModal.show();
    }


    setUNIONNUMNull() {
        this.prclass.unionnum = null;
        this.jcunionUNIONNUM = '';
    }
    setCLASSNull() {
        this.prclass.class = null;
        this.premployeeCLASS = '';
    }


    getNewUNIONNUM() {
        this.prclass.unionnum = this.prclassJCUNIONLookupTableModal.id;
        this.jcunionUNIONNUM = this.prclassJCUNIONLookupTableModal.displayName;
    }
    getNewCLASS() {
        this.prclass.class = this.prclassPREMPLOYEELookupTableModal.id;
        this.premployeeCLASS = this.prclassPREMPLOYEELookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
