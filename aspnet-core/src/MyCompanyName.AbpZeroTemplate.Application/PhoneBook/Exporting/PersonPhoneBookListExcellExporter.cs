using Abp.Collections.Extensions;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;
using MyCompanyName.AbpZeroTemplate.DataExporting.Excel.NPOI;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using MyCompanyName.AbpZeroTemplate.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Exporting
{
    public class PersonPhoneBookListExcellExporter : NpoiExcelExporterBase, IPersonPhoneBookListExcellExporter
    {
        public PersonPhoneBookListExcellExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        { 
        }
        public FileDto ExportToFile(List<PersonListDto> personListDtos)
        {
            return CreateExcelPackage(
                "UserList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Users"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Surname"),
                        L("EmailAddress"),
                        L("BusinessPhoneNumber"),
                        L("MobilePhoneNumber"),
                        L("HomePhoneNumber")
                        );

                    AddObjects(
                        sheet, personListDtos,
                        _ => _.Name,
                        _ => _.Surname,
                        _ => _.EmailAddress,
                        _ => _.Phones.Where(t=> t.Type == PhoneType.Business).Select(n=>n.Number).JoinAsString(", "),
                        _ => _.Phones.Where(r => r.Type == PhoneType.Mobile).Select(n => n.Number).JoinAsString(", "),
                        _ => _.Phones.Where(r => r.Type == PhoneType.Home).Select(n => n.Number).JoinAsString(", ")
                        );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}