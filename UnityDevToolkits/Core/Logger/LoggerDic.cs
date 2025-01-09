using System.Collections.Generic;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Logger
{
    /// <summary>
    /// 日志字典，用于管理多个日志实现。
    /// </summary>
    public class LoggerDic : Dictionary<string,ILog> , ILog
    {
        public void Add(ILog logger)
        {
            TryAdd(nameof(logger), logger);
        }
        
        public void Print(string message)
        {
            foreach (ILog logger in Values)
            {
                logger.Print(message);
            }
        }

        public void Warning(string message)
        {
            foreach (ILog logger in Values)
            {
                logger.Warning(message);
            }
        }

        public void Error(string message)
        {
            foreach (ILog logger in Values)
            {
                logger.Error(message);
            }
        }
    }
}