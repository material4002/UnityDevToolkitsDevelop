namespace Material.UnityDevToolkits.Core.Config
{
    public interface ISceneChange
    {
        void OnSceneEnter();
        void OnSceneExit();
    }
}