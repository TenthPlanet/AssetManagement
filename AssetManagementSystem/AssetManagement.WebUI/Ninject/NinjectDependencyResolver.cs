using AssetManagement.Domain.Abstract;
using AssetManagement.Domain.Concrete;
using AssetManagement.WebUI.QuickResolvers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AssetManagement.WebUI.Ninject
{

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam) { kernel = kernelParam; AddBindings(); }
        public object GetService(Type serviceType) { return kernel.TryGet(serviceType); }
        public IEnumerable<object> GetServices(Type serviceType) { return kernel.GetAll(serviceType); }
        private void AddBindings()
        {
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IStockRepository>().To<StockRepository>();

            kernel.Bind<ICatergoryRepository>().To<CatergoryRepository>();
            kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
            kernel.Bind<IKeyboardRepository>().To<KeyboardRepository>();
            kernel.Bind<ILaptopRepository>().To<LaptopRepository>();
            kernel.Bind<IMonitorRepository>().To<MonitorRepository>();
            kernel.Bind<IMouseRepository>().To<MouseRepository>();
            kernel.Bind<IPCRepository>().To<PCRepository>();
            kernel.Bind<IPrinterRepository>().To<PrinterRepository>();
        }
    }
}
