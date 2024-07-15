using MyCompanyName.AbpZeroTemplate.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCompanyName.AbpZeroTemplate.PhoneBook;

namespace MyCompanyName.AbpZeroTemplate.Migrations.Seed.Host
{
    public class InitialPeopleAndPhoneCreator
    {
        private readonly AbpZeroTemplateDbContext _context;

        public InitialPeopleAndPhoneCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var douglas = _context.Persons.FirstOrDefault(p => p.EmailAddress == "douglas.adams@fortytwo.com");
            if (douglas == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "Douglas",
                        Surname = "Adams",
                        EmailAddress = "douglas.adams@fortytwo.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "1112242"},
                                    new Phone {Type = PhoneType.Mobile, Number = "2223342"}
                                    }
                    });
            }

            var asimov = _context.Persons.FirstOrDefault(p => p.EmailAddress == "isaac.asimov@foundation.org");
            if (asimov == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "Isaac",
                        Surname = "Asimov",
                        EmailAddress = "isaac.asimov@foundation.org",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "8889977"}
                                    }
                    });
            }
            var user1 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user1@user1.com");
            if (user1 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User1",
                        Surname = "Surname1",
                        EmailAddress = "user1@user1.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "8559933"}
                                    }
                    });
            }
            var user2 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user2@user2.com");
            if (user2 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User2",
                        Surname = "Surname2",
                        EmailAddress = "user2@user2.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "7878722"}
                                    }
                    });
            }
            var user3 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user3@user3.com");
            if (user3 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User3",
                        Surname = "Surname3",
                        EmailAddress = "user3@user3.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "81242447"}
                                    }
                    });
            }
            var user4 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user4@user4.com");
            if (user4 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User4",
                        Surname = "Surname4",
                        EmailAddress = "user4@user4.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "1111977"}
                                    }
                    });
            }
            var user5 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user5@user5.com");
            if (user5 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User5",
                        Surname = "Surname5",
                        EmailAddress = "user5@user5.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "8889977"}
                                    }
                    });
            }
            var user6 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user6@user6.com");
            if (user6 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User6",
                        Surname = "Surname6",
                        EmailAddress = "user6@user6.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "82323232"}
                                    }
                    });
            }
            var user7 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user7@user7.com");
            if (user7 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User7",
                        Surname = "Surname7",
                        EmailAddress = "user7@user7.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "8889977"}
                                    }
                    });
            }
            var user8 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user8@user8.com");
            if (user8 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User8",
                        Surname = "Surname8",
                        EmailAddress = "user8@user8.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "7779977"}
                                    }
                    });
            }
            var user9 = _context.Persons.FirstOrDefault(p => p.EmailAddress == "user9@user9.com");
            if (user9 == null)
            {
                _context.Persons.Add(
                    new Person
                    {
                        Name = "User9",
                        Surname = "Surname9",
                        EmailAddress = "user9@user9.com",
                        Phones = new List<Phone>
                                    {
                                    new Phone {Type = PhoneType.Home, Number = "82222111"}
                                    }
                    });
            }
        }
    }
}
