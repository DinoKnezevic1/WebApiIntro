using Autofac;
using Autofac.Integration.WebApi;
using Example.Repository;
using Example.Repository.Common;
using Example.Service;
using Example.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApi.App_Start
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Controller"));

            return builder.Build();
        }
    }
}