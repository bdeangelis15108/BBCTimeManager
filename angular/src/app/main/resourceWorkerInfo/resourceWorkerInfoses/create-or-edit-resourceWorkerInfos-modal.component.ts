import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ResourceWorkerInfosesServiceProxy, CreateOrEditResourceWorkerInfosDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ResourceWorkerInfosWorkerClaseesLookupTableModalComponent } from './resourceWorkerInfos-workerClasees-lookup-table-modal.component';
import { ResourceWorkerInfosResourcesLookupTableModalComponent } from './resourceWorkerInfos-resources-lookup-table-modal.component';

@Component({
    selector: 'createOrEditResourceWorkerInfosModal',
    templateUrl: './create-or-edit-resourceWorkerInfos-modal.component.html'
})
export class CreateOrEditResourceWorkerInfosModalComponent extends AppComponentBase {
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('resourceWorkerInfosWorkerClaseesLookupTableModal', { static: true }) resourceWorkerInfosWorkerClaseesLookupTableModal: ResourceWorkerInfosWorkerClaseesLookupTableModalComponent;
    @ViewChild('resourceWorkerInfosResourcesLookupTableModal', { static: true }) resourceWorkerInfosResourcesLookupTableModal: ResourceWorkerInfosResourcesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    resourceWorkerInfos: CreateOrEditResourceWorkerInfosDto = new CreateOrEditResourceWorkerInfosDto();

    workerClaseesName = '';
    resourcesName = '';


    constructor(
        injector: Injector,
        private _resourceWorkerInfosesServiceProxy: ResourceWorkerInfosesServiceProxy
    ) {
        super(injector);
    }
    
    show(resourceWorkerInfosId?: number): void {
    

        if (!resourceWorkerInfosId) {
            this.resourceWorkerInfos = new CreateOrEditResourceWorkerInfosDto();
            this.resourceWorkerInfos.id = resourceWorkerInfosId;
            this.workerClaseesName = '';
            this.resourcesName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._resourceWorkerInfosesServiceProxy.getResourceWorkerInfosForEdit(resourceWorkerInfosId).subscribe(result => {
                this.resourceWorkerInfos = result.resourceWorkerInfos;

                this.workerClaseesName = result.workerClaseesName;
                this.resourcesName = result.resourcesName;

                this.active = true;
                this.modal.show();
            });
        }
        
    }

    save(): void {
            this.saving = true;

			
			
            this._resourceWorkerInfosesServiceProxy.createOrEdit(this.resourceWorkerInfos)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectWorkerClaseesModal() {
        this.resourceWorkerInfosWorkerClaseesLookupTableModal.id = this.resourceWorkerInfos.workerClaseesId;
        this.resourceWorkerInfosWorkerClaseesLookupTableModal.displayName = this.workerClaseesName;
        this.resourceWorkerInfosWorkerClaseesLookupTableModal.show();
    }
    openSelectResourcesModal() {
        this.resourceWorkerInfosResourcesLookupTableModal.id = this.resourceWorkerInfos.resourcesId;
        this.resourceWorkerInfosResourcesLookupTableModal.displayName = this.resourcesName;
        this.resourceWorkerInfosResourcesLookupTableModal.show();
    }


    setWorkerClaseesIdNull() {
        this.resourceWorkerInfos.workerClaseesId = null;
        this.workerClaseesName = '';
    }
    setResourcesIdNull() {
        this.resourceWorkerInfos.resourcesId = null;
        this.resourcesName = '';
    }


    getNewWorkerClaseesId() {
        this.resourceWorkerInfos.workerClaseesId = this.resourceWorkerInfosWorkerClaseesLookupTableModal.id;
        this.workerClaseesName = this.resourceWorkerInfosWorkerClaseesLookupTableModal.displayName;
    }
    getNewResourcesId() {
        this.resourceWorkerInfos.resourcesId = this.resourceWorkerInfosResourcesLookupTableModal.id;
        this.resourcesName = this.resourceWorkerInfosResourcesLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
