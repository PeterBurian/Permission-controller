using LicenseTest.Interfaces;
using System;
using System.Text;

namespace LicenseTest.Licenses
{
    public class Business : Pro, ILicenseBusiness
    {
        public Business() { }

        public override void Do2()
        {
            base.Do2();
            Console.WriteLine("Doing from Business, you can modify the original method.");
        }

        public virtual int Do3()
        {
            Console.WriteLine("Do3");
            return 0;
        }

        public virtual void Do4()
        {
            Console.WriteLine("Do4");
        }

        public virtual void Do5(String someParam)
        {
            StringBuilder builder = new StringBuilder("Do5: ");
            builder.Append(someParam);
            Console.WriteLine(builder.ToString());
        }
    }
}