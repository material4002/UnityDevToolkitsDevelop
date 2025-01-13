using System.Collections.Generic;

namespace Material.UnityDevToolkits.SceneFilter.Structures
{
    /// <summary>
    /// 数据结构
    /// 用于储存过滤器并构造一个树
    /// 树枝使用字典进行索引
    /// 每注册一个过滤器，都是树的一条树枝
    /// </summary>
    public class FilterNode
    {
        public string name;//路径名称
        public BaseFilter filter;//储存的过滤器，可以为空
        public bool isActive = true;//储存过滤器是否激活状态
        public Dictionary<string,FilterNode> children;//子节点的容器，使用路径名进行注册
        public FilterType type = FilterType.First;//储存过滤器状态，如果是First会在Where执行为true时跳出

        public FilterNode(string name,BaseFilter filter)
        {
            this.name = name;
            this.filter = filter;
            this.children = new Dictionary<string, FilterNode>();
        }

        public FilterNode(string name)
        {
            this.name = name;
            this.filter = null;
            this.children = new Dictionary<string, FilterNode>();
        }
        
        //不会重写已有的节点
        //不会添加null
        public void AddChild(FilterNode child)
        {
            this.children.TryAdd(child.name, child);
        }
        
        //会发生覆盖
        public void AddFilter(BaseFilter filter)
        {
            this.filter = filter;
        }
    }
}