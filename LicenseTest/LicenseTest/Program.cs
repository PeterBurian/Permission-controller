using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LicenseTest
{
    public interface ILicense { }

    public interface ILicensePro : ILicense
    {
        void Do1();
        void Do2();
    }

    public interface ILicenseBusiness : ILicensePro
    {
        int Do3();
        void Do4();
        void Do5(String someParam);
    }

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

    public class Permisson<T> where T : ILicense, new()
    {
        String name;
        ILicense license;

        public Permisson(String name)
        {
            this.name = name;
            license = new T();
        }

        internal void CallMethod<T>(Action<T, object> param)
        {
            try
            {
                param.Invoke((T)license, param);
            }
            catch (Exception e)
            {
                StringBuilder builder = new StringBuilder("You have no permission to execute: ");
                builder.Append(e.Message);
                Console.WriteLine(builder.ToString());
            }
        }
    }

    public class PermissonDynamic : ILicense
    {
        String name;
        dynamic license;
        int licType;

        public PermissonDynamic(String name, ILicense type)
        {
            this.name = name;
            license = Activator.CreateInstance(type.GetType());

            Type[] types = type.GetType().GetInterfaces();

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == typeof(ILicensePro))
                {
                    licType = 0;
                }
                else
                {
                    licType = 1;
                }
            }
        }

        public void CallExecute<T>(String methodName, String param)
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            try
            {
                MethodInfo[] methods = interfaces[licType == 0 ? 0 : 2].GetMethods();   //Some cheat
                MethodInfo info = methods.Where(m => m.Name == methodName).FirstOrDefault();

                if (info != null)
                {
                    if (String.IsNullOrEmpty(param))
                    {
                        info.Invoke(license, new object[] { });
                    }
                    else
                    {
                        info.Invoke(license, new object[] { param });
                    }
                }
            }
            catch (Exception e)
            {
                StringBuilder builder = new StringBuilder("You have no permission to execute this method");
                Console.WriteLine(builder.ToString());
            }
        }
    }

    public static class Executer
    {
        //object array should chage to params
        public static void Execute(ILicense activeLicense, String method, object[] parameters)
        {
            PermissonDynamic permissionDyn = new PermissonDynamic("Dynamic", activeLicense);

            Type pType = typeof(PermissonDynamic);
            MethodInfo mInfo = pType.GetMethod("CallExecute");

            MethodInfo mInfoGeneric = mInfo.MakeGenericMethod(activeLicense.GetType());

            List<object> param = new List<object>();
            param.Add(method);

            if (parameters.Length == 0)
            {
                param.Add(String.Empty);
            }
            else
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    param.Add(parameters[i]);
                }
            }
            
            mInfoGeneric.Invoke(permissionDyn, param.ToArray());
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
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
            Executer.Execute(activeLicense, "Do3", new object[] { });

            Console.WriteLine("Upgrade to business");
            activeLicense = new Business();
            Executer.Execute(activeLicense, "Do3", new object[] { });

            Executer.Execute(activeLicense, "Do5", new object[] { "Execute with some param" });
        }
    }
}
