import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { StatusUpdatesServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';

@Component({
    templateUrl: './host-dashboard.component.html',
    styleUrls: ['./host-dashboard.component.less'],
    encapsulation: ViewEncapsulation.None
})
export class HostDashboardComponent extends AppComponentBase {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultHostDashboard;

    constructor(injector: Injector,
        private _notifyService: NotifyService,
        private _statusUpdateServiceProxy: StatusUpdatesServiceProxy) {
        super(injector);
    }
    LoadNewPayPeriod(): void {
        this._statusUpdateServiceProxy.loadNewPayPeriod()
        .subscribe(result => {
            this.notify.info(this.l(result.success));
        });
    }
    CeDataRefresh(): void{
        this._statusUpdateServiceProxy.ceDataRefresh()
        .subscribe(result => {
            this.notify.info(this.l(result.status));
        });
    }
}
