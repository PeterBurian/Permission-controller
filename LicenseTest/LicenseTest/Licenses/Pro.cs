using LicenseTest.Interfaces;
using System;

namespace LicenseTest.Licenses
{
    public class Pro : ILicensePro
    {
        public Pro() { }

        public virtual void Do1()
        {
            Console.WriteLine("Do1");
        }

        public virtual void Do2()
        {
            Console.WriteLine("Do2");
        }
    }
}