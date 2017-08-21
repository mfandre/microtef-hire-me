using Stone.BusinessEntities;
using Stone.BusinessEntities.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Stone.WebApi.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CaptureRequestsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var result = actionExecutedContext.Response.Content as ObjectContent;
            if (result != null && result.Value != null)
            {
                var type = result.Value.GetType();

                if (type.GetGenericTypeDefinition() == typeof(ResultData<>).GetGenericTypeDefinition())
                {
                    var genArgs = type.GetGenericArguments();
                    var typedVariant = typeof(ResultData<>).GetGenericTypeDefinition().MakeGenericType(genArgs);
                    var items = typedVariant.GetProperty("Data").GetValue(result.Value, null);

                    //PropertyInfo propInfo = result.Value.GetType().GetProperty("Data"); //this returns null
                    //object itemValue = propInfo.GetValue(propInfo, null);
                    foreach (var data in ((IEnumerable)items))
                    {
                        CheckSensitive(data);
                    }
                    
                }
                
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private static void CheckSensitive(object data)
        {
            var typeToCheck = data.GetType();
            var properties = typeToCheck.GetProperties();

            var SensitiveProperties = properties.Where(IsSensitiveData);

            foreach (var p in SensitiveProperties)
            {
                if(p.PropertyType == typeof(string))
                    p.SetValue(data, null);
                else if(p.PropertyType == typeof(int))
                    p.SetValue(data, 0);
                else if(p.PropertyType == typeof(float))
                    p.SetValue(data, 0);
                else if(p.PropertyType == typeof(double))
                    p.SetValue(data, 0);
                else if (p.PropertyType.IsSubclassOf(typeof(object)))
                    CheckSensitive(p.GetValue(data));
            }
        }

        private static bool IsSensitiveData(PropertyInfo property)
        {
            return property.GetCustomAttributes<SensitiveDataAttribute>().Count() > 0;
        }
    }
}