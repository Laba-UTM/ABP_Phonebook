import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PersonServiceProxy, EditPersonInput, PhoneInPersonListDto, PhoneType, AddPhoneInput } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { remove as _remove } from 'lodash-es';
import { Table } from 'primeng/table';

@Component({
  selector: 'editPhonePersonModal',
  templateUrl: './edit-phone-person-modal.component.html'
})
export class EditPhonePersonModalComponent extends AppComponentBase {

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('modal') modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;

  person: EditPersonInput = new EditPersonInput();
  phones: PhoneInPersonListDto[] = [];

  active: boolean = false;
  saving: boolean = false;

  newPhone: AddPhoneInput = null;

  constructor(
    injector: Injector,
    private _personService: PersonServiceProxy
  ) {
    super(injector);
  }

  onShown(): void {
    
}

show(personId,personName,personSurname,personEmailAdress): void { 
    this._personService.getPersonPhones(personId).subscribe((result)=> {
        this.phones = result;
        this.newPhone = new AddPhoneInput();
        this.newPhone.type = PhoneType.Mobile;
        this.newPhone.personId = personId;
        this.person.id = personId;
        this.person.name = personName;
        this.person.surname = personSurname;
        this.person.emailAddress = personEmailAdress;
        this.active = true;
        this.modal.show();
      });
  }

  save(): void {

    if (!this.newPhone.number) {
        this.message.warn('Please enter a number!');
        return;
    }

    this.saving = true;
    this._personService.addPhone(this.newPhone).subscribe(result => {
        this.phones.push(result);
        this.newPhone.number = '';
        this.notify.success(this.l('SavedSuccessfully'));
    });
    this.saving = false;
  }

  close(): void {
    this.modal.hide();
    this.active = false;
  }

  getPhoneTypeAsString(phoneType: PhoneType): string {
    switch (phoneType) {
        case PhoneType.Mobile:
            return this.l('PhoneType_Mobile');
        case PhoneType.Home:
            return this.l('PhoneType_Home');
        case PhoneType.Business:
            return this.l('PhoneType_Business');
        default:
            return '?';
    }
};

deletePhone(phone): void {
    this._personService.deletePhone(phone.id).subscribe(() => {
        this.notify.success(this.l('SuccessfullyDeleted'));
        _remove(this.phones, phone);
    });
};

}
