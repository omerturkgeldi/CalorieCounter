using Autofac;
using CalorieCounter.Core.Repositories;
using CalorieCounter.Core.Services;
using CalorieCounter.Core.UnitOfWorks;
using CalorieCounter.Repository;
using CalorieCounter.Repository.Repositories;
using CalorieCounter.Repository.UnitOfWorks;
using CalorieCounter.Service.Mapping;
using CalorieCounter.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace CalorieCounter.API.Modules
{
    public class RepoServiceModule:Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();



            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();


           // builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();

        }
    }
}
