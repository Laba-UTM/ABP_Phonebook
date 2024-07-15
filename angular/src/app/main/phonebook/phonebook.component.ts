import { Component, Injector,ViewChild, ViewEncapsulation, AfterViewInit} from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PersonServiceProxy, PersonListDto, AddPhoneInput, PhoneType, GetPeopleInput } from '@shared/service-proxies/service-proxies';

import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { remove as _remove, result } from 'lodash-es';
import { log } from 'console';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
    templateUrl: './phonebook.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./phonebook.component.less'],
    animations: [appModuleAnimation()]
})
    export class PhoneBookComponent extends AppComponentBase implements AfterViewInit{
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    people: PersonListDto[] = [];
    filter: string = '';

    editingPerson: PersonListDto = null;

    uploadUrl: string;

    //Filters
    filterText = '';

    constructor(
        injector: Injector,
        private _personService: PersonServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        this.primengTableHelper.adjustScroll(this.dataTable);
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    getPeople(event?: LazyLoadEvent): void {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);

            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._personService
            .getPeople( new GetPeopleInput({
                    filter: this.filter,
                    sorting: this.primengTableHelper.getSorting(this.dataTable),
                    maxResultCount: this.primengTableHelper.getMaxResultCount(this.paginator, event),
                    skipCount: this.primengTableHelper.getSkipCount(this.paginator, event),
            })
            )
            .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.people = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    deletePerson(person: PersonListDto): void {
        this.message.confirm(
            this.l('AreYouSureToDeleteThePerson', person.name),
            undefined,
            isConfirmed => {
                if (isConfirmed) {
                    this._personService.deletePerson(person.id).subscribe(() => {
                        this.notify.info(this.l('SuccessfullyDeleted'));
                        _remove(this.people, person);
                    });
                }
            }
        );
    }

    exportToExcel(): void {

        this._personService
            .getPeoplePhoneToExcel(
                this.filterText,
                this.primengTableHelper.getSorting(this.dataTable)
            )
            .subscribe((result) => {
                console.log(result);
                this._fileDownloadService.downloadTempFile(result);
            })
    }
}
