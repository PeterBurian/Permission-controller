using System;

namespace LicenseTest.Interfaces
{
    //Business license know all methods from pro

    public interface ILicenseBusiness : ILicensePro
    {
        int Do3();
        void Do4();
        void Do5(String someParam);
    }
}