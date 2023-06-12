using Master.Core.Contract.Infrastructure.Command;
using Master.Core.Contract.Infrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Master.Utilities.Extentions
{
    public static class DataAccessExtentions
    {
        public static IServiceCollection DataAccessWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
        {
            return
            source
                .RepositoryWireup(assemblies)
                .UnitOfWorkWireup(assemblies);
        }

        private static IServiceCollection RepositoryWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
        {
            return source.AddTransient(assemblies, typeof(ICommandRepository<>), typeof(IQueryRepository));
        }

        private static IServiceCollection UnitOfWorkWireup(this IServiceCollection source, IEnumerable<Assembly> assemblies)
        {
            return source.AddTransient(assemblies, typeof(IUnitOfWork));
        }
    }
}
