import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ResourceReservationsesServiceProxy, CreateOrEditResourceReservationsDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ResourceReservationsUserLookupTableModalComponent } from './resourceReservations-user-lookup-table-modal.component';
import { ResourceReservationsResourcesLookupTableModalComponent } from './resourceReservations-resources-lookup-table-modal.component';

@Component({
    selector: 'createOrEditResourceReservationsModal',
    templateUrl: './create-or-edit-resourceReservations-modal.component.html'
})
export class CreateOrEditResourceReservationsModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('resourceReservationsUserLookupTableModal', { static: true }) resourceReservationsUserLookupTableModal: ResourceReservationsUserLookupTableModalComponent;
    @ViewChild('resourceReservationsResourcesLookupTableModal', { static: true }) resourceReservationsResourcesLookupTableModal: ResourceReservationsResourcesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    resourceReservations: CreateOrEditResourceReservationsDto = new CreateOrEditResourceReservationsDto();

    userName = '';
    resourcesName = '';


    constructor(
        injector: Injector,
        private _resourceReservationsesServiceProxy: ResourceReservationsesServiceProxy
    ) {
        super(injector);
    }

    show(resourceReservationsId?: number): void {

        if (!resourceReservationsId) {
            this.resourceReservations = new CreateOrEditResourceReservationsDto();
            this.resourceReservations.id = resourceReservationsId;
            this.resourceReservations.reservedFrom = moment().startOf('day');
            this.resourceReservations.reservedUntil = moment().startOf('day');
            this.userName = '';
            this.resourcesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._resourceReservationsesServiceProxy.getResourceReservationsForEdit(resourceReservationsId).subscribe(result => {
                this.resourceReservations = result.resourceReservations;

                this.userName = result.userName;
                this.resourcesName = result.resourcesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
            this._resourceReservationsesServiceProxy.createOrEdit(this.resourceReservations)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.resourceReservationsUserLookupTableModal.id = this.resourceReservations.userId;
        this.resourceReservationsUserLookupTableModal.displayName = this.userName;
        this.resourceReservationsUserLookupTableModal.show();
    }
    openSelectResourcesModal() {
        this.resourceReservationsResourcesLookupTableModal.id = this.resourceReservations.resourcesId;
        this.resourceReservationsResourcesLookupTableModal.displayName = this.resourcesName;
        this.resourceReservationsResourcesLookupTableModal.show();
    }


    setUserIdNull() {
        this.resourceReservations.userId = null;
        this.userName = '';
    }
    setResourcesIdNull() {
        this.resourceReservations.resourcesId = null;
        this.resourcesName = '';
    }


    getNewUserId() {
        this.resourceReservations.userId = this.resourceReservationsUserLookupTableModal.id;
        this.userName = this.resourceReservationsUserLookupTableModal.displayName;
    }
    getNewResourcesId() {
        this.resourceReservations.resourcesId = this.resourceReservationsResourcesLookupTableModal.id;
        this.resourcesName = this.resourceReservationsResourcesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
