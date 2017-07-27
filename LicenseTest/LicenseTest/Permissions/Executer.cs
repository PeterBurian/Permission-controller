using LicenseTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LicenseTest.Permissions
{
    public static class Executer
    {
        //object array should chage to params
        public static void Execute(ILicense activeLicense, String method, object[] parameters)
        {
            PermissonDynamic permissionDyn = new PermissonDynamic(activeLicense);

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
}
