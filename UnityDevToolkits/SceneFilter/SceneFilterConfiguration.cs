using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.SceneFilter.Structures;
using Unity.VisualScripting;
using UnityEngine;

namespace Material.UnityDevToolkits.SceneFilter
{
    /// <summary>
    /// 场景过滤器，
    /// 在场景加载完成后进行加载，
    /// 会遍历场景中的所有物体，
    /// 并传入过滤树，可根据条件进行筛选。
    /// 在筛选完成后可进行一次回调
    /// </summary>
    [RegisterConfig(typeof(RegisterFilter))]
    public class SceneFilterConfiguration: IAfterConfig,IConfig,IConfigInit
    {
        //根节点
        private FilterNode _root = null;
        
        
        private readonly Type _baseFilterType = typeof(BaseFilter);
        
        public void OnConfigInit()
        {
            _root = new FilterNode("");
            
        }
        

        public void BeforeConfig()
        {
            
        }
        
        /// <summary>
        /// 将合法的过滤器注册到过滤树对应的位置
        /// </summary>
        public void Config(Assembly assembly, Type classType, Type attributeType,Attribute attribute)
        {
            //双验证，必须继承BaseFilter，并且有RegisterFilter属性标记
            if (classType.IsSubclassOf(_baseFilterType))
            {
                //使用单次注册替换注册全部的内容
                //储存路径
                RegisterFilter filterAttr = attribute as RegisterFilter;
                
                string path = filterAttr.Path;
                        
                //如果没有路径，则使用类名作为默认的路径，并储存于根目录上
                if (string.IsNullOrEmpty(path))
                {
                    path = classType.Name;
                }
                        
                //分割路径，并逐个遍历创建节点
                string[] paths = path.Split('/');
                        
                //当前节点，默认从根目录开始遍历
                FilterNode node = _root;
                        
                //遍历路径
                if (paths.Any())
                {
                    for (int i = 0; i < paths.Length; i++)
                    {
                        //如果当前节点已经存在，则直接进入子节点
                        if (node.children.ContainsKey(paths[i]))
                        {
                            node = node.children[paths[i]];
                        }
                        else //如果不存在，则创建新节点并加入到当前节点的子节点中
                        {
                            FilterNode newNode = new FilterNode(paths[i]);
                            node.children.Add(paths[i], newNode);
                            node = newNode;
                        }

                        //如果到达最后一个节点，说明到达过滤器注册位置，添加过滤器
                        if (i == paths.Length - 1)
                        {
                            BaseFilter filter = assembly.CreateInstance(classType.FullName) as BaseFilter;
                            node.AddFilter(filter);
                            node.type = filterAttr.Type;
                            filter.Init(); //初始化过滤器
                        }
                    }
                }

                //获取全部的注册
                /*List<RegisterFilter> attributes = classType.GetCustomAttributes<RegisterFilter>().ToList();
                
                //遍历所有的注册，进行注册操作
                if (attributes.Any())
                {
                    foreach (RegisterFilter filterAttr in attributes)
                    {
                        //储存路径
                        string path = filterAttr.Path;
                        
                        //如果没有路径，则使用类名作为默认的路径，并储存于根目录上
                        if (string.IsNullOrEmpty(path))
                        {
                            path = classType.Name;
                        }
                        
                        //分割路径，并逐个遍历创建节点
                        string[] paths = path.Split('/');
                        
                        //当前节点，默认从根目录开始遍历
                        FilterNode node = _root;
                        
                        //遍历路径
                        if (paths.Any())
                        {
                            for (int i = 0; i < paths.Length; i++)
                            {
                                //如果当前节点已经存在，则直接进入子节点
                                if(node.children.ContainsKey(paths[i]))
                                {
                                    node = node.children[paths[i]];
                                }
                                else//如果不存在，则创建新节点并加入到当前节点的子节点中
                                {
                                    FilterNode newNode = new FilterNode(paths[i]);
                                    node.children.Add(paths[i], newNode);
                                    node = newNode;
                                }
                                
                                //如果到达最后一个节点，说明到达过滤器注册位置，添加过滤器
                                if (i == paths.Length - 1)
                                {
                                    BaseFilter filter = assembly.CreateInstance(classType.FullName) as BaseFilter;
                                    node.AddFilter(filter);
                                    node.type = filterAttr.Type;
                                    filter.Init();//初始化过滤器
                                }
                            }
                        }
                    }
                }*/
            }
        }

        public void AfterConfig()
        {
            
        }
        
        /// <summary>
        /// 在准备完所有模块后，再进行过滤，防止过滤器回调不能获取各模块
        /// </summary>
        public void AfterConfigAll()
        {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();//获取全部的物体
            
            DoFilter(_root,gameObjects);//递归调用
            
            //过滤完成后，由于场景中的数据是动态的，没有储存的意义，可以直接删除
            _root.children.Clear();
            _root = null;
        }
        
        /// <summary>
        /// 基于递归的过滤操作
        /// 会将输入的物体组依次传入过滤器进行过滤，并向下传递
        /// </summary>
        /// <param name="node">过滤器</param>
        /// <param name="gameObjects">要过滤的数据</param>
        private void DoFilter(FilterNode node, GameObject[] gameObjects)
        {
            BaseFilter filter = node.filter;
            
            //如果过滤器不为空，且有物体需要过滤
            if (filter != null && gameObjects.Any())
            {
                //根据过滤器的类型进行不同的操作
                if (node.type == FilterType.Normal)//普通过滤，全部传入过滤器
                {
                    foreach (GameObject gameObject in gameObjects)
                    {
                        if (filter.Where(gameObject))
                        {
                            filter.Add(gameObject.GetInstanceID(),gameObject);//合法的数据集添加到过滤器中
                        }
                    }
                }else if (node.type == FilterType.First)//首个过滤器，找到第一个符合条件的物体后不再继续向下传递
                {
                    foreach (GameObject gameObject in gameObjects)
                    {
                        if (filter.Where(gameObject))
                        {
                            filter.Add(gameObject.GetInstanceID(),gameObject);//合法的数据集添加到过滤器中
                            break;
                        }
                    }
                }
                
            }
            
            //如果当前节点进行了过滤，则将过滤后的数据集传入子节点进行下一次的过滤
            if (filter != null)
            {
                gameObjects = filter.Values.ToArray();
            }
            
            //如果有子节点，则继续向下传递数据集进行过滤
            if (node.children.Any())
            {
                foreach (FilterNode filterNode in node.children.Values)
                {
                    DoFilter(filterNode,gameObjects);
                }
            }
            
            //过滤完成后，进行回调操作
            if (filter != null)
            {
                filter.Register();
            }
        }
    }
}