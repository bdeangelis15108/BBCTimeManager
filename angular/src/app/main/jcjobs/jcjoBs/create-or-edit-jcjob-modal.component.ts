import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { JCJOBsServiceProxy, CreateOrEditJCJOBDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { JCJOBJACCATLookupTableModalComponent } from './jcjob-jaccat-lookup-table-modal.component';

@Component({
    selector: 'createOrEditJCJOBModal',
    templateUrl: './create-or-edit-jcjob-modal.component.html'
})
export class CreateOrEditJCJOBModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jcjobJACCATLookupTableModal', { static: true }) jcjobJACCATLookupTableModal: JCJOBJACCATLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jcjob: CreateOrEditJCJOBDto = new CreateOrEditJCJOBDto();

    jaccatJOBNUM = '';


    constructor(
        injector: Injector,
        private _jcjoBsServiceProxy: JCJOBsServiceProxy
    ) {
        super(injector);
    }

    show(jcjobId?: number): void {

        if (!jcjobId) {
            this.jcjob = new CreateOrEditJCJOBDto();
            this.jcjob.id = jcjobId;
            this.jaccatJOBNUM = '';

            this.active = true;
            this.modal.show();
        } else {
            this._jcjoBsServiceProxy.getJCJOBForEdit(jcjobId).subscribe(result => {
                this.jcjob = result.jcjob;

                this.jaccatJOBNUM = result.jaccatjobnum;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._jcjoBsServiceProxy.createOrEdit(this.jcjob)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJACCATModal() {
        this.jcjobJACCATLookupTableModal.id = this.jcjob.jobnum;
        this.jcjobJACCATLookupTableModal.displayName = this.jaccatJOBNUM;
        this.jcjobJACCATLookupTableModal.show();
    }


    setJOBNUMNull() {
        this.jcjob.jobnum = null;
        this.jaccatJOBNUM = '';
    }


    getNewJOBNUM() {
        this.jcjob.jobnum = this.jcjobJACCATLookupTableModal.id;
        this.jaccatJOBNUM = this.jcjobJACCATLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
