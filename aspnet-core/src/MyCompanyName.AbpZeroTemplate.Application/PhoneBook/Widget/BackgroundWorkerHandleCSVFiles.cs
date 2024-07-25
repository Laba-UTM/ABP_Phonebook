using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.Core.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using MyCompanyName.AbpZeroTemplate.MultiTenancy;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using NPOI.POIFS.Storage;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Widget
{
    public class BackgroundWorkerHandleCSVFiles(ILogger logger, IDepartmentAppService departmentAppService,
        IUnitOfWorkManager unitOfWorkManager, IRepository<Tenant> _tenantRepository)
    {
        public void Execute()
        {
            IQueryable<Tenant> tenants;
            List<int> tenantsId = new List<int>();
            using (var uow1 = unitOfWorkManager.Begin())
            {
                tenants = _tenantRepository.GetAll().Where(t => t.IsActive);
                
                foreach (var tenant in tenants)
                    tenantsId.Add(tenant.Id);

                uow1.Complete();
            }
            foreach (var tenant in tenantsId)
                ExecuteHelper(tenant);
        }

        public void ExecuteHelper(int tenantId)
        {
            string folderPath="";
            string completedPath = "";
            string errorsPath = "";

            if (tenantId == 1)  
            {
                folderPath = @"C:\ProjFiles\Tenant1";
                completedPath = @"C:\ProjFiles\Tenant1\Completed";
                errorsPath = @"C:\ProjFiles\Tenant1\Errors";
            }
            else if(tenantId == 2)
            {
                folderPath = @"C:\ProjFiles\Tenant2";
                completedPath = @"C:\ProjFiles\Tenant2\Completed";
                errorsPath = @"C:\ProjFiles\Tenant2\Errors";
            }
            
            // Supporting formats
            string[] arrayFormat = { "dd.M.yyyy", "MMMM.dd.yyyy", "dddd.dd.MMM.yyyy"};
            //string[] arrayFormat = { "dd.M.yyyy", "MMMM.dd.yyyy", "dddd.dd.MMM.yyyy", "dddd.M.yyyy" };

            if (!Directory.Exists(completedPath))
            {
                Directory.CreateDirectory(completedPath);
            }
            if (!Directory.Exists(errorsPath))
            {
                Directory.CreateDirectory(errorsPath);
            }

            if (Directory.Exists(folderPath))
            {
                string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
                string[] allFiles = csvFiles.Concat(txtFiles).ToArray();

                bool notCopyToCompletePath = false;

                List<string> propertyNames = new List<string>
                {
                    nameof(DepartmentDtoCSVReader.DeptNumber),
                    nameof(DepartmentDtoCSVReader.DeptName),
                    nameof(DepartmentDtoCSVReader.CreatedDate)
                };

                var sortedCsvFiles = allFiles.OrderBy(f => File.GetCreationTime(f)).ToArray();

                foreach (string file in sortedCsvFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        var lineCount = File.ReadLines(file).Count();
                        if (lineCount != 0)
                            lineCount--;

                        using (var reader = new StreamReader(file, Encoding.UTF8, true))
                        {
                            string delimiter="";

                            // CHECK IF IS EMPTY
                            if (fileInfo.Length != 0)
                            {
                                List<string> headerForCheck = new List<string>();
                                // CHECK IF HAS HEADERS
                                for (int i = 0; i < 5; i++)
                                {
                                    string line = reader.ReadLine();
                                    if (line != null)
                                        headerForCheck.Add(line);
                                    else continue; 
                                }
                                int index_header;
                                delimiter = checkHeaders(propertyNames,headerForCheck, out index_header);
                                if (delimiter == "")
                                    notCopyToCompletePath = true;
                                else 
                                {
                                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                                    reader.DiscardBufferedData();
                                    for (int i = 0; i < index_header; i++)
                                    {
                                        reader.ReadLine();
                                    }
                                }

                                if(!notCopyToCompletePath)
                                {
                                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                                    {
                                        HasHeaderRecord = true,
                                        Delimiter = delimiter
                                    }))
                                    {
                                        // GET all departments from Db
                                        ConcurrentBag<DepartmentDto> allDepartments = [];
                                        using (var uow1 = unitOfWorkManager.Begin())
                                        {
                                            unitOfWorkManager.Current.SetTenantId(tenantId);
                                            List<DepartmentDto> departmentsList = departmentAppService.GetDepartments();
                                            uow1.Complete();

                                            foreach (var department in departmentsList)
                                            {
                                                allDepartments.Add(department);
                                            }
                                        }
                                        ConcurrentBag<DepartmentDto> needToAdd = [];
                                        ConcurrentBag<DepartmentDto> needToUpdate = [];

                                        var recordsCSV = csv.GetRecords<DepartmentDtoCSVReader>().ToList();
                                        var records = departmentAppService.MapDepCSVToDep(recordsCSV);

                                        // Filling the needToAdd and needToUpdate containers

                                        DateTime parsedDate;
                                        bool checkedDateType = false;
                                        if (allDepartments != null && !allDepartments.IsEmpty)
                                        {
                                            foreach (var record in records)
                                            {
                                                foreach (string format in arrayFormat)
                                                {
                                                    if (DateTime.TryParseExact(record.CreatedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                                                    {
                                                        record.CreatedDateFinal = parsedDate;
                                                        checkedDateType = true;
                                                        break;
                                                    }
                                                }
                                                if (checkedDateType)
                                                {
                                                    if (allDepartments.Any(d => d.DeptNumber == record.DeptNumber))
                                                    {
                                                        needToUpdate.Add(record);
                                                    }
                                                    else
                                                    {
                                                        needToAdd.Add(record);
                                                    }
                                                    checkedDateType = false;
                                                }
                                                else continue;
                                            }
                                        }
                                        else if (allDepartments == null || allDepartments.Count == 0)
                                        {
                                            foreach (var record in records)
                                            {
                                                foreach (string format in arrayFormat)
                                                {
                                                    if (DateTime.TryParseExact(record.CreatedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                                                    {
                                                        record.CreatedDateFinal = parsedDate;
                                                        checkedDateType = true;
                                                        break;
                                                    }
                                                }

                                                if (checkedDateType)
                                                {
                                                    if (needToAdd.IsEmpty)
                                                    {
                                                        needToAdd.Add(record);
                                                    }
                                                    else
                                                    {
                                                        if (needToAdd.Any(d => d.DeptNumber == record.DeptNumber))
                                                        {
                                                            var oldDepartment = needToAdd.FirstOrDefault(d => d.DeptNumber == record.DeptNumber);
                                                            needToAdd.TryTake(out oldDepartment);
                                                            needToAdd.Add(record);
                                                        }
                                                        else
                                                        {
                                                            needToAdd.Add(record);
                                                        }
                                                    }
                                                    checkedDateType = false;
                                                }
                                            }
                                        }

                                        // CHECK IF PARSED OBJECTS ARE MORE THAN 50%
                                        int needToAddCount = needToAdd.Count;
                                        int needToUpdateCount = needToUpdate.Count;
                                        int parsedCount = needToAddCount + needToUpdateCount;
                                        if (parsedCount > lineCount/2) 
                                        {
                                            // Adding new departments
                                            if (!needToAdd.IsEmpty)
                                            {
                                                using (var uow2 = unitOfWorkManager.Begin())
                                                {
                                                    unitOfWorkManager.Current.SetTenantId(tenantId);
                                                    departmentAppService.AddRangeDepartment(needToAdd);
                                                    uow2.Complete();
                                                }
                                            }

                                            // Updating repeating departments
                                            if (!needToUpdate.IsEmpty)
                                            {
                                                using (var uow3 = unitOfWorkManager.Begin())
                                                {
                                                    unitOfWorkManager.Current.SetTenantId(tenantId);
                                                    departmentAppService.UpdateRangeDepartment(needToUpdate);
                                                    uow3.Complete();
                                                }
                                            }
                                        }
                                        else
                                            notCopyToCompletePath = true;
                                    }
                                }
                                
                            }
                            else
                                notCopyToCompletePath = true;
                        }

                        if(notCopyToCompletePath)
                        {
                            string badFileName = Path.GetFileName(file);
                            string errorDestFile = Path.Combine(errorsPath, badFileName);
                            File.Move(file, errorDestFile);
                            notCopyToCompletePath = false;
                        }
                        else
                        {
                            string fileName = Path.GetFileName(file);
                            string destFile = Path.Combine(completedPath, fileName);
                            File.Move(file, destFile);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Debug($"Error while reading {file}: {ex.Message}");
                    }
                }
            }
            else
            {
                logger.Debug("Directory doesnt exist.");
            }
        }

        public string checkHeaders(List<string> propertyNames, List<string> lines, out int headerLineIndex)
        {
            headerLineIndex = -1;
            bool hasAllHeaders = false;
            string delimiter = "";

            // CHECK WITH COMMA
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (propertyNames.All(header => line.Split(',').Select(h => h.Trim()).Contains(header)))
                {
                    hasAllHeaders = true;
                    headerLineIndex = i;
                    delimiter = ",";
                    break;
                }
            }

            // CHECK WITH TAB
            if (!hasAllHeaders)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    if (propertyNames.All(header => line.Split('\t').Select(h => h.Trim()).Contains(header)))
                    {
                        hasAllHeaders = true;
                        headerLineIndex = i;
                        delimiter = "\t";
                        break;
                    }
                }
            }

            return delimiter;
        }
    }
}
