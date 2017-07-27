using LicenseTest.Interfaces;
using LicenseTest.Licenses;
using LicenseTest.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LicenseTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //RunStatic();
            RunDynamic();
            Console.ReadKey();
        }

        private static void RunStatic()
        {
            Permisson<Business> personBusiness = new Permisson<Business>("Business");
            personBusiness.CallMethod<ILicenseBusiness>((e, d) => e.Do3());

            Permisson<Business> personBusiness2 = new Permisson<Business>("Business2");
            personBusiness2.CallMethod<ILicenseBusiness>((e, d) => e.Do2());

            Permisson<Pro> personPro = new Permisson<Pro>("Pro");
            personPro.CallMethod<ILicensePro>((e, d) => e.Do2());

            Permisson<Pro> personPro2 = new Permisson<Pro>("Pro 2");
            personPro2.CallMethod<ILicenseBusiness>((e, d) => e.Do3());
        }

        private static void RunDynamic()
        {
            ILicense activeLicense = new Pro();
            Executer.Execute(activeLicense, "Do2", new object[] { });
            Executer.Execute(activeLicense, "Do3", new object[] { });

            Console.WriteLine("Upgrade to business");
            activeLicense = new Business();
            Executer.Execute(activeLicense, "Do3", new object[] { });

            Executer.Execute(activeLicense, "Do5", new object[] { "Execute with some param" });

            Executer.Execute(activeLicense, "Do1", new object[] { });
        }
    }
}
