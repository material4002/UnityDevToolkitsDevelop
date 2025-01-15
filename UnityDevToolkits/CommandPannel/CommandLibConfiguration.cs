using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Material.UnityDevToolkits.CommandPannel.Interfaces;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.PlayerStateMachine;

namespace Material.UnityDevToolkits.CommandPanel
{
    /// <summary>
    /// 指令面板配置类。
    /// 用于创建一些调试功能，
    /// 可以注册指令，并在运行时调用指令
    /// </summary>
    [RegisterConfig(typeof(RegisterCommand))]
    public class CommandLibConfiguration : IConfig,IConfigInit,IExecuteCommand
    {
        private Dictionary<string , ICommand> _commandDic;
        private Type _commandType;
        
        private string pattern = @"""([^""]*)""|([^\s""]+)";
        private Regex regex; 
        
        public void OnConfigInit()
        {
            _commandDic = new Dictionary<string , ICommand>();
            _commandType = typeof(ICommand);
            regex= new Regex(pattern);
        }
        
        public void BeforeConfig()
        {
            
        }

        public void Config(Assembly assembly, Type classType, Type attributeType)
        {
            if(!classType.IsSubclassOf(_commandType))return;
            
            ICommand command = (ICommand)assembly.CreateInstance(classType.FullName);
            command.InitCommand();

            RegisterCommand registerCommand = classType.GetCustomAttribute<RegisterCommand>();
            command.Description = registerCommand.Description;
            _commandDic.Add(registerCommand.Command,command);
        }

        public void AfterConfig()
        {
            
        }

        public void ExecuteCommand(string input)
        {
            MatchCollection matches = regex.Matches(input);
            List<string> args = new List<string>();

            if (matches.Any())
            {
                string commandName = matches[0].Value;
                if(_commandDic.ContainsKey(commandName)) foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1 && match.Groups[1].Success)
                    {
                        args.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        args.Add(match.Value);
                    }
                }

                try
                {
                    _commandDic[commandName].ExecuteCommand(args);
                }
                catch (Exception )
                {
                    //TODO: 在此使用Error指令输出
                }
                
            }
                
            
        }
    }
}