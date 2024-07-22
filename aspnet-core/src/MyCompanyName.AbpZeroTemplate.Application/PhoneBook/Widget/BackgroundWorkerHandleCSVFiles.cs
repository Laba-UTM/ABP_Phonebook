using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using AutoMapper;
using Castle.Core.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Widget
{
    public class BackgroundWorkerHandleCSVFiles(ILogger logger, IDepartmentAppService departmentAppService,
        IUnitOfWorkManager unitOfWorkManager)
    {
        public void Execute()
        {
            string folderPath = @"C:\ProjFiles";
            string completedPath = @"C:\ProjFiles\Completed";

            if (!Directory.Exists(completedPath))
            {
                Directory.CreateDirectory(completedPath);
            }

            if (Directory.Exists(folderPath))
            {
                string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");               

                var sortedCsvFiles = csvFiles.OrderBy(f => File.GetCreationTime(f)).ToArray();

                foreach (string file in sortedCsvFiles)
                {
                    try
                    {
                        using (var reader = new StreamReader(file))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = true,
                        }))
                        {
                            var records = csv.GetRecords<DepartmentDto>().ToList();
                            foreach (var record in records)
                            {
                                DepartmentDto existingDepartment;
                                // Check existing department
                                using (var uow1 = unitOfWorkManager.Begin())
                                {
                                    existingDepartment = departmentAppService.GetDepartmentByNumber(record.DeptNumber);
                                    uow1.Complete();
                                }

                                if (existingDepartment != null)
                                {
                                    // UPDATE current department
                                    using (var uow2 = unitOfWorkManager.Begin())
                                    {
                                        departmentAppService.UpdateDepartment(record);
                                        uow2.Complete();
                                    }
                                }
                                else
                                {
                                    // ADD new department
                                    using (var uow3 = unitOfWorkManager.Begin())
                                    {
                                        departmentAppService.AddDepartment(record);
                                        uow3.Complete();
                                    }
                                }

                            }
                        }

                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(completedPath, fileName);
                        File.Move(file, destFile);         
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
    }
}
