import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JCUNIONsServiceProxy, CreateOrEditJCUNIONDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { JCUNIONJACCATLookupTableModalComponent } from './jcunion-jaccat-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJCUNIONModal',
    templateUrl: './create-or-edit-jcunion-modal.component.html'
})
export class CreateOrEditJCUNIONModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jcunionJACCATLookupTableModal', { static: true }) jcunionJACCATLookupTableModal: JCUNIONJACCATLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jcunion: CreateOrEditJCUNIONDto = new CreateOrEditJCUNIONDto();

    jaccatJOBNUM = '';


    constructor(
        injector: Injector,
        private _jcunioNsServiceProxy: JCUNIONsServiceProxy
    ) {
        super(injector);
    }

    show(jcunionId?: number): void {

        if (!jcunionId) {
            this.jcunion = new CreateOrEditJCUNIONDto();
            this.jcunion.id = jcunionId;
            this.jaccatJOBNUM = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jcunioNsServiceProxy.getJCUNIONForEdit(jcunionId).subscribe(result => {
                this.jcunion = result.jcunion;

                this.jaccatJOBNUM = result.jaccatjobnum;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jcunioNsServiceProxy.createOrEdit(this.jcunion)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJACCATModal() {
        this.jcunionJACCATLookupTableModal.id = this.jcunion.jobnum;
        this.jcunionJACCATLookupTableModal.displayName = this.jaccatJOBNUM;
        this.jcunionJACCATLookupTableModal.show();
    }


    setJOBNUMNull() {
        this.jcunion.jobnum = null;
        this.jaccatJOBNUM = '';
    }


    getNewJOBNUM() {
        this.jcunion.jobnum = this.jcunionJACCATLookupTableModal.id;
        this.jaccatJOBNUM = this.jcunionJACCATLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
