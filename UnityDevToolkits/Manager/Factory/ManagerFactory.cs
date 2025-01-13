using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork;

namespace Material.UnityDevToolkits.Manager.Factory
{
    public static class ManagerFactory 
    {
        private static ManagerConfiguration _managerConfiguration;
        
        static ManagerFactory()
        {
            FrameContainer frameContainer = new Obj().GetFrameContainer();
            _managerConfiguration = frameContainer.GetConfigure<ManagerConfiguration>();
        }
        
        public static T GetManager<T>(this IGetManager target) where T : class
        {
            return _managerConfiguration.GetManager<T>();

        }
        
        private class Obj : IGetContainer{}
    }
}