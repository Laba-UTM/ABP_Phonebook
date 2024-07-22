import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
  selector: 'app-filter-hello-world',
  templateUrl: './filter-hello-world.component.html',
  styleUrls: ['./filter-hello-world.component.css']
})
export class FilterHelloWorldComponent extends AppComponentBase {

  constructor(injector: Injector) {
    super(injector)
  }

  publishName(name: string): void {
    abp.event.trigger('app.dashboardFilters.helloFilter.onNameChange', name);
    console.log('publishName');
  }
}