import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeUnionsServiceProxy, CreateOrEditEmployeeUnionsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeUnionsUnionsLookupTableModalComponent } from './employeeUnions-unions-lookup-table-modal.component';
import { EmployeeUnionsResourcesLookupTableModalComponent } from './employeeUnions-resources-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeUnionsModal',
    templateUrl: './create-or-edit-employeeUnions-modal.component.html'
})
export class CreateOrEditEmployeeUnionsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeUnionsUnionsLookupTableModal', { static: true }) employeeUnionsUnionsLookupTableModal: EmployeeUnionsUnionsLookupTableModalComponent;
    @ViewChild('employeeUnionsResourcesLookupTableModal', { static: true }) employeeUnionsResourcesLookupTableModal: EmployeeUnionsResourcesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeUnions: CreateOrEditEmployeeUnionsDto = new CreateOrEditEmployeeUnionsDto();

    unionsNumber = '';
    resourcesName = '';


    constructor(
        injector: Injector,
        private _employeeUnionsServiceProxy: EmployeeUnionsServiceProxy
    ) {
        super(injector);
    }

    show(employeeUnionsId?: number): void {

        if (!employeeUnionsId) {
            this.employeeUnions = new CreateOrEditEmployeeUnionsDto();
            this.employeeUnions.id = employeeUnionsId;
            this.unionsNumber = '';
            this.resourcesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeUnionsServiceProxy.getEmployeeUnionsForEdit(employeeUnionsId).subscribe(result => {
                this.employeeUnions = result.employeeUnions;

                this.unionsNumber = result.unionsNumber;
                this.resourcesName = result.resourcesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._employeeUnionsServiceProxy.createOrEdit(this.employeeUnions)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUnionsModal() {
        this.employeeUnionsUnionsLookupTableModal.id = this.employeeUnions.unionsId;
        this.employeeUnionsUnionsLookupTableModal.displayName = this.unionsNumber;
        this.employeeUnionsUnionsLookupTableModal.show();
    }
    openSelectResourcesModal() {
        this.employeeUnionsResourcesLookupTableModal.id = this.employeeUnions.resourcesId;
        this.employeeUnionsResourcesLookupTableModal.displayName = this.resourcesName;
        this.employeeUnionsResourcesLookupTableModal.show();
    }


    setUnionsIdNull() {
        this.employeeUnions.unionsId = null;
        this.unionsNumber = '';
    }
    setResourcesIdNull() {
        this.employeeUnions.resourcesId = null;
        this.resourcesName = '';
    }


    getNewUnionsId() {
        this.employeeUnions.unionsId = this.employeeUnionsUnionsLookupTableModal.id;
        this.unionsNumber = this.employeeUnionsUnionsLookupTableModal.displayName;
    }
    getNewResourcesId() {
        this.employeeUnions.resourcesId = this.employeeUnionsResourcesLookupTableModal.id;
        this.resourcesName = this.employeeUnionsResourcesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
