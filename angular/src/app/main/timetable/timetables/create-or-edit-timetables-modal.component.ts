import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimetablesServiceProxy, CreateOrEditTimetablesDto, GetTimetablesForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';


import { TimetablesPayPeriodsLookupTableModalComponent } from './timetables-payPeriods-lookup-table-modal.component';
import { TimetablesResourcesLookupTableModalComponent } from './timetables-resources-lookup-table-modal.component';
import { TimetablesUnionPayRatesLookupTableModalComponent } from './timetables-unionPayRates-lookup-table-modal.component';
import { TimetablesUnionsLookupTableModalComponent } from './timetables-unions-lookup-table-modal.component';
import { TimetablesAddressesLookupTableModalComponent } from './timetables-addresses-lookup-table-modal.component';
import { TimetablesExpenseTypesLookupTableModalComponent } from './timetables-expenseTypes-lookup-table-modal.component';
import { TimetablesCostTypesLookupTableModalComponent } from './timetables-costTypes-lookup-table-modal.component';
import { TimetablesAccountsLookupTableModalComponent } from './timetables-accounts-lookup-table-modal.component';
import { TimetablesUserLookupTableModalComponent } from './timetables-user-lookup-table-modal.component';
import { TimetablesPayTypesLookupTableModalComponent } from './timetables-payTypes-lookup-table-modal.component';
import { TimetablesWorkerClaseesLookupTableModalComponent } from './timetables-workerClasees-lookup-table-modal.component';


@Component({
    selector: 'createOrEditTimetablesModal',
    templateUrl: './create-or-edit-timetables-modal.component.html'
})
export class CreateOrEditTimetablesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timetablesPayPeriodsLookupTableModal', { static: true }) timetablesPayPeriodsLookupTableModal: TimetablesPayPeriodsLookupTableModalComponent;
    @ViewChild('timetablesResourcesLookupTableModal', { static: true }) timetablesResourcesLookupTableModal: TimetablesResourcesLookupTableModalComponent;
    @ViewChild('timetablesUnionPayRatesLookupTableModal', { static: true }) timetablesUnionPayRatesLookupTableModal: TimetablesUnionPayRatesLookupTableModalComponent;
    @ViewChild('timetablesUnionsLookupTableModal', { static: true }) timetablesUnionsLookupTableModal: TimetablesUnionsLookupTableModalComponent;
    @ViewChild('timetablesAddressesLookupTableModal', { static: true }) timetablesAddressesLookupTableModal: TimetablesAddressesLookupTableModalComponent;
    @ViewChild('timetablesExpenseTypesLookupTableModal', { static: true }) timetablesExpenseTypesLookupTableModal: TimetablesExpenseTypesLookupTableModalComponent;
    @ViewChild('timetablesCostTypesLookupTableModal', { static: true }) timetablesCostTypesLookupTableModal: TimetablesCostTypesLookupTableModalComponent;
    @ViewChild('timetablesAccountsLookupTableModal', { static: true }) timetablesAccountsLookupTableModal: TimetablesAccountsLookupTableModalComponent;
    @ViewChild('timetablesUserLookupTableModal', { static: true }) timetablesUserLookupTableModal: TimetablesUserLookupTableModalComponent;
    @ViewChild('timetablesPayTypesLookupTableModal', { static: true }) timetablesPayTypesLookupTableModal: TimetablesPayTypesLookupTableModalComponent;
    @ViewChild('timetablesWorkerClaseesLookupTableModal', { static: true }) timetablesWorkerClaseesLookupTableModal: TimetablesWorkerClaseesLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timetables: CreateOrEditTimetablesDto = new CreateOrEditTimetablesDto();
    cssForCreate = '{"display": "none"}';
    payPeriodsName = '';
    resourcesName = '';
    unionPayRatesClass = '';
    Category = '';
    phase = '';
    unionsNumber = '';
    addressesState = '';
    expenseTypesDescription = '';
    costTypesName = '';
    accountsName = '';
    userName = '';
    payTypesCode = '';
    workerClaseesName = '';
    job = '';

    constructor(
        injector: Injector,
        private _timetablesServiceProxy: TimetablesServiceProxy,
        private _activatedRoute: ActivatedRoute,
    ) {
        super(injector);
        

    }

    dayChange(elem){
        this.calculateAmount();
    }



    show(record?: GetTimetablesForViewDto): void {
        var timetablesId = undefined;
        if(record!=undefined){
            timetablesId = record.timetables.id;
        }
        //console.log('show', record);
        console.log(!timetablesId)
        if (!timetablesId) {
            
            this.timetables = new CreateOrEditTimetablesDto();
            this.timetables.id = timetablesId;
            this.timetables.multiplier = 0;
            this.timetables.costCode = this._activatedRoute.snapshot.queryParams.selectedCode;
            this.timetables.createdOn = moment().startOf('day');
            this.payPeriodsName = moment(this._activatedRoute.snapshot.queryParams.startDate).format('LL') + ' - ' + moment(this._activatedRoute.snapshot.queryParams.endDate).format('LL');
            this.resourcesName = '';
            this.unionPayRatesClass = '';
            this.unionsNumber = '';
            this.addressesState = '';
            this.expenseTypesDescription = '';
            this.costTypesName = '';
            this.accountsName = '';
            this.userName = '';
            //this.cssForCreate = ''
            this.payTypesCode = '';
            this.workerClaseesName = '';
            this.active = true;
            
            this.modal.show();
        } else {
            this._timetablesServiceProxy.getTimetablesForEdit(timetablesId).subscribe(result => {
                this.timetables = result.timetables;
                if (this.timetables.multiplier == undefined)
                    this.timetables.multiplier = 0;

                if(this.timetables.day1 == undefined){
                    this.timetables.day1 = 0;
                }
                if(this.timetables.day2 == undefined){
                    this.timetables.day2 = 0;
                }
                if(this.timetables.day3 == undefined){
                    this.timetables.day3 = 0;
                }
                if(this.timetables.day4 == undefined){
                    this.timetables.day4 = 0;
                }
                if(this.timetables.day5 == undefined){
                    this.timetables.day5 = 0;
                }
                if(this.timetables.day6 == undefined){
                    this.timetables.day6 = 0;
                }
                if(this.timetables.day7 == undefined){
                    this.timetables.day7 = 0;
                }
                var splittedCostCode = this.timetables.costCode.split('.');
                
                if (splittedCostCode.length > 0){
                    this.job = splittedCostCode[0];
                }
                if (splittedCostCode.length > 1){
                    this.phase = splittedCostCode[1];
                }
                if (splittedCostCode.length > 2){
                    this.Category = splittedCostCode[2];
                }
                
                // this.timetables.costCode = this.jobCode + "." + this.phase + "." + this.Category;//this._activatedRoute.snapshot.queryParams.selectedCode;
                this.payPeriodsName = moment(this._activatedRoute.snapshot.queryParams.startDate).format('LL') + ' - ' + moment(this._activatedRoute.snapshot.queryParams.endDate).format('LL');
                this.resourcesName = result.resourcesName;
                this.unionPayRatesClass = result.unionPayRatesClass;
                this.unionsNumber = result.unionsNumber;
                this.addressesState = result.addressesState;
                this.expenseTypesDescription = result.expenseTypesDescription;
                this.costTypesName = result.costTypesName;
                this.accountsName = result.accountsName;
                this.userName = result.userName;
                this.payTypesCode = result.payTypesCode;
                this.workerClaseesName = result.workerClaseesName;
                
                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        
        if(this.timetables.rate == -1){
             this.notify.error('Please select a valid pay rate.');
             this.saving = false;
             return;
        }
        this.calculateAmount();
        this.saving = true;
        this.timetables.periodDate = this._activatedRoute.snapshot.queryParams.selectedPayWeek;
        this.timetables.costCode = this.job + '.' + this.phase + '.' + this.Category;
        this._timetablesServiceProxy.createOrEdit(this.timetables)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectPayPeriodsModal() {
        
        this.timetablesPayPeriodsLookupTableModal.id = this.timetables.periodDate;
        this.timetablesPayPeriodsLookupTableModal.displayName = this.payPeriodsName;
        this.timetablesPayPeriodsLookupTableModal.show();
    }
    openSelectResourcesModal() {
        this.timetablesResourcesLookupTableModal.id = this.timetables.resourcesCode;
        this.timetablesResourcesLookupTableModal.displayName = this.resourcesName;
        this.timetablesResourcesLookupTableModal.show();
    }
    openSelectUnionPayRatesModal() {
        this.timetablesUnionPayRatesLookupTableModal.id = this.timetables.rate;
        this.timetablesUnionPayRatesLookupTableModal.displayName = this.unionPayRatesClass;
        this.timetablesUnionPayRatesLookupTableModal.show();
    }
    openSelectUnionsModal() {
        this.timetablesUnionsLookupTableModal.id = this.timetables.unionlocal;
        this.timetablesUnionsLookupTableModal.displayName = this.unionsNumber;
        this.timetablesUnionsLookupTableModal.show();
    }
    openSelectAddressesModal() {
        this.timetablesAddressesLookupTableModal.id = this.timetables.state;
        this.timetablesAddressesLookupTableModal.displayName = this.addressesState;
        this.timetablesAddressesLookupTableModal.show();
    }
    openSelectExpenseTypesModal() {
        this.timetablesExpenseTypesLookupTableModal.id = this.timetables.description;
        this.timetablesExpenseTypesLookupTableModal.displayName = this.expenseTypesDescription;
        this.timetablesExpenseTypesLookupTableModal.show();
    }
    openSelectCostTypesModal() {
        this.timetablesCostTypesLookupTableModal.id = this.timetables.costTypesId;
        this.timetablesCostTypesLookupTableModal.displayName = this.costTypesName;
        this.timetablesCostTypesLookupTableModal.show();
    }
    openSelectAccountsModal() {
        this.timetablesAccountsLookupTableModal.id = this.timetables.accountsId;
        this.timetablesAccountsLookupTableModal.displayName = this.accountsName;
        this.timetablesAccountsLookupTableModal.show();
    }
    openSelectUserModal() {
        this.timetablesUserLookupTableModal.id = this.timetables.createdBy;
        this.timetablesUserLookupTableModal.displayName = this.userName;
        this.timetablesUserLookupTableModal.show();
    }
    openSelectPayTypesModal() {
        this.timetablesPayTypesLookupTableModal.id = this.timetables.payTypesId;
        this.timetablesPayTypesLookupTableModal.displayName = this.payTypesCode;
        this.timetablesPayTypesLookupTableModal.show();
    }
    openSelectWorkerClaseesModal() {
        this.timetablesWorkerClaseesLookupTableModal.id = this.timetables.workerClaseesId;
        this.timetablesWorkerClaseesLookupTableModal.displayName = this.workerClaseesName;
        this.timetablesWorkerClaseesLookupTableModal.show();
    }


    setPeriodDateNull() {
        this.timetables.periodDate = null;
        this.payPeriodsName = '';
    }
    setResourcesCodeNull() {
        this.timetables.resourcesCode = null;
        this.resourcesName = '';
    }
    setRateNull() {
        this.timetables.rate = null;
        this.unionPayRatesClass = '';
    }
    setUnionlocalNull() {
        this.timetables.unionlocal = null;
        this.unionsNumber = '';
    }
    setStateNull() {
        this.timetables.state = null;
        this.addressesState = '';
    }
    setDescriptionNull() {
        this.timetables.description = null;
        this.expenseTypesDescription = '';
    }
    setCostTypesIdNull() {
        this.timetables.costTypesId = null;
        this.costTypesName = '';
    }
    setAccountsIdNull() {
        this.timetables.accountsId = null;
        this.accountsName = '';
    }
    setCreatedByNull() {
        this.timetables.createdBy = null;
        this.userName = '';
    }
    setPayTypesIdNull() {
        this.timetables.payTypesId = null;
        this.payTypesCode = '';
    }
    setWorkerClaseesIdNull() {
        this.timetables.workerClaseesId = null;
        this.workerClaseesName = '';
    }


    getNewPeriodDate() {
        this.timetables.periodDate = this.timetablesPayPeriodsLookupTableModal.id;
        this.payPeriodsName = this.timetablesPayPeriodsLookupTableModal.displayName;
    }
    getNewResourcesCode() {
        this.timetables.resourcesCode = this.timetablesResourcesLookupTableModal.id;
        this.resourcesName = this.timetablesResourcesLookupTableModal.displayName;
    }
    calculateAmount(){
        var totalDays = this.timetables.day1
                        + this.timetables.day2
                        + this.timetables.day3
                        + this.timetables.day4
                        + this.timetables.day5
                        + this.timetables.day6
                        + this.timetables.day7;

        if(this.unionPayRatesClass == ''){
            this.timetables.amount = '0';
        }
        var totalAmount = totalDays * Number(this.unionPayRatesClass) * this.timetables.multiplier;
        this.timetables.amount = totalAmount.toString();
    }
    getNewRate() {
        /*
        this.timetables.rate = this.timetablesUnionPayRatesLookupTableModal.id;
        this.unionPayRatesClass = this.timetablesUnionPayRatesLookupTableModal.displayName;
        */
        this._timetablesServiceProxy.refreshPayRate(this.timetables.unionlocal, this.timetables.workerClaseesId)
        .subscribe(result => {
            
            if(result == null || result == undefined || result.id == null || result.id == undefined){
                this.timetables.rate = -1;
                this.unionPayRatesClass = '';
                this.notify.error('No matching pay rate found!');
                this.calculateAmount();
                return;
            }

            this.timetables.rate = result.id;
            this.unionPayRatesClass = result.perhour.toString();
            this.calculateAmount();
            this.notify.success('Pay Rate Updated to: ' + this.unionPayRatesClass);
        });
    }
    getNewUnionlocal() {
        // Refresh PayRate
        this.timetables.unionlocal = this.timetablesUnionsLookupTableModal.id;
        this.unionsNumber = this.timetablesUnionsLookupTableModal.displayName;
        this.getNewRate();
    }
    getNewState() {
        this.timetables.state = this.timetablesAddressesLookupTableModal.id;
        this.addressesState = this.timetablesAddressesLookupTableModal.displayName;
    }
    getNewDescription() {
        this.timetables.description = this.timetablesExpenseTypesLookupTableModal.id;
        this.expenseTypesDescription = this.timetablesExpenseTypesLookupTableModal.displayName;
    }
    getNewCostTypesId() {
        this.timetables.costTypesId = this.timetablesCostTypesLookupTableModal.id;
        this.costTypesName = this.timetablesCostTypesLookupTableModal.displayName;
    }
    getNewAccountsId() {
        this.timetables.accountsId = this.timetablesAccountsLookupTableModal.id;
        this.accountsName = this.timetablesAccountsLookupTableModal.displayName;
    }
    getNewCreatedBy() {
        this.timetables.createdBy = this.timetablesUserLookupTableModal.id;
        this.userName = this.timetablesUserLookupTableModal.displayName;
    }
    getNewPayTypesId() {
        console.log(this.timetablesPayTypesLookupTableModal.displayName)
        var splittedPayType = this.timetablesPayTypesLookupTableModal.displayName.toString().split('|');
        
        this.timetables.payTypesId = this.timetablesPayTypesLookupTableModal.id;
        if(splittedPayType.length > 0)
            this.payTypesCode = splittedPayType[0].trim();
        if (splittedPayType.length > 1){
            this.timetables.multiplier = Number(splittedPayType[1].trim());
            this.calculateAmount();
        }
            
    }
    getNewWorkerClaseesId() {
        // Refresh PayRate
        this.timetables.workerClaseesId = this.timetablesWorkerClaseesLookupTableModal.id;
        this.workerClaseesName = this.timetablesWorkerClaseesLookupTableModal.displayName;
        this.getNewRate();
        
    }
   

    close(): void {
        this.active = false;
        this.modal.hide();
    }


}
