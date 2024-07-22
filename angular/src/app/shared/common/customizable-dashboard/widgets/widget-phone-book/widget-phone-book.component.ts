import { Component, Injector, OnInit, OnDestroy} from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TenantDashboardServiceProxy, GetPhoneBookOutput, PhoneType, PersonServiceProxy} from '@shared/service-proxies/service-proxies';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { DateTime } from 'luxon';
import { result } from 'lodash-es';
import { threadId } from 'worker_threads';
import { Observable, interval } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Component({
  selector: 'app-widget-phone-book',
  templateUrl: './widget-phone-book.component.html',
  styleUrls: ['./widget-phone-book.component.css']
})
export class WidgetPhoneBookComponent extends WidgetComponentBaseComponent implements OnInit, OnDestroy {
  helloResponse: string;
  constructor(injector: Injector,
    private _tenantDashboardService: TenantDashboardServiceProxy,
    private _personServiceProxy: PersonServiceProxy) {
    super(injector);    
  }

  phoneBook: GetPhoneBookOutput = null;
  filter: string = '';
  changingValue: number=0;

  ngOnInit(): void {
    // this.subHelloWorldFilter();
    // this.runDelayed(()=>{
    //     this.getHelloWorld("First Attempt");  
    // });
    
    this.getChangingValue().subscribe(
      result => {
        this.changingValue = result;
        //console.log('NEW CHANGING VALUE ', this.changingValue);
      },
      error => {
        //console.error('Error fetching changing value:', error);
      }
    );

  }

  getChangingValue(): Observable<any> {
    return interval(3000) // 5 секунд
      .pipe(
        switchMap(() => this._personServiceProxy.getBackgroundWorkerValue())
      );
    }

  getPhoneBookPerson()
  {
    this._tenantDashboardService
      .getPhoneBookPerson(this.filter)
      .subscribe((result) => {
        this.phoneBook = result;
        console.log('GetPerson INFO ', this.phoneBook);
    });
  }

  getPhones(type: string)
  {
    if(this.phoneBook !=null && this.phoneBook.name != null)
    {
      switch(type)
      {
        case "business": return this.phoneBook?.businessPhones.filter(phone => phone.type == PhoneType.Business) || [];
        case "mobile": return this.phoneBook?.mobilePhones.filter(phone => phone.type == PhoneType.Mobile) || [];
        case "home": return this.phoneBook?.homePhones.filter(phone => phone.type == PhoneType.Home) || [];
      }
    }
    else
      return null;
  }

  // getChangingValue()
  // {
  //   this._personServiceProxy
  //     .getBackgroundWorkerValue()
  //     .subscribe((result) =>{
  //         this.changingValue=result;
  //         console.log('NEW CHANGING VALUE ', this.changingValue);
  //     });
  // }


  getHelloWorld = (name: string) => {
    this._tenantDashboardService
      .getHelloWorldData(name)
      .subscribe((data) => {
        this.helloResponse = data.outPutName;
        console.log('getHelloWorld ',data);
      });
  }
  
  onNameChange = (name) => {
   this.runDelayed(()=>{
    console.log('onNameChange');
        this.getHelloWorld(name);  
    });
  }
  
  subHelloWorldFilter() {
    abp.event.on('app.dashboardFilters.helloFilter.onNameChange', this.onNameChange);
  }

  unSubHelloWorldFilter() {
    abp.event.off('app.dashboardFilters.helloFilter.onNameChange', this.onNameChange);
  }

  ngOnDestroy(): void {
    this.unSubHelloWorldFilter();
  }
}