using com.bbbirder;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    /// <summary>
    /// 状态配置信息
    /// 储存注册的状态名称，状态机名称
    ///
    /// 状态名称:如果不进行填写/为空字符串则以类名作为状态名称
    /// 状态机名称：如果不进行填写/为空字符串则为所有的状态机进行注册
    /// </summary>
    public class RegisterState:DirectRetrieveAttribute
    {
        private readonly string _stateName;
        public string stateName => _stateName;
        private readonly string _stateMachineName;
        public string stateMachineName => _stateMachineName;

        public RegisterState(string stateName="", string stateMachineName="")
        {
            _stateName = stateName;
            _stateMachineName = stateMachineName;
        }
    }
}