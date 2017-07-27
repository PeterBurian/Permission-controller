using LicenseTest.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LicenseTest.Permissions
{
    public class PermissonDynamic : ILicense
    {
        dynamic license;

        public PermissonDynamic(ILicense type)
        {
            Type licenseType = type.GetType();
            license = Activator.CreateInstance(licenseType);
        }

        public void CallExecute<T>(String methodName, String param)
        {
            Type[] interfaces = typeof(T).GetInterfaces();

            try
            {
                MethodInfo info = GetMethodFromInterface(interfaces, methodName);

                //If is missing purposely, if info is null you have no permission to run the method

                if (String.IsNullOrEmpty(param))
                {
                    info.Invoke(license, new object[] { });
                }
                else
                {
                    info.Invoke(license, new object[] { param });
                }
            }
            catch (Exception e)
            {
                StringBuilder builder = new StringBuilder("You have no permission to execute this method");
                Console.WriteLine(builder.ToString());
            }
        }

        private MethodInfo GetMethodFromInterface(Type[] interfaces, String methodName)
        {
            for (int i = 0; i < interfaces.Length; i++)
            {
                MethodInfo[] methods = interfaces[i].GetMethods();
                MethodInfo info = methods.Where(m => m.Name == methodName).FirstOrDefault();
                if (info != null)
                {
                    return info;
                }
            }
            return null;
        }
    }
}