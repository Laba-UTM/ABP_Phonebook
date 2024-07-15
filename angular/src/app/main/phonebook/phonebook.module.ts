import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {PhoneBookRoutingModule} from './phonebook-routing.module';
import {PhoneBookComponent} from './phonebook.component';
import { CreatePersonModalComponent } from './create-person-modal.component';
import { EditPersonModalComponent } from './edit-person-modal.component';
import { EditPhonePersonModalComponent } from './edit-phone-person-modal.component';
import { MainModule } from '../main.module';


@NgModule({
    declarations: [PhoneBookComponent, CreatePersonModalComponent, EditPersonModalComponent,EditPhonePersonModalComponent],
    imports: [AppSharedModule, PhoneBookRoutingModule, MainModule]
})
export class PhoneBookModule {}
