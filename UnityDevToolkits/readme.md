# UnityDevToolkits
## MaterialUnity开发脚手架

### 目录
1. 概述
2. Core
3. Manager
4. SceneFilter
5. PlayerStateMachine

### 概述
为了简化开发成本，开发了这套Unity的开发脚手架。  
脚手架本质上是基于反射实现的一套IOC容器。
在Unity5以上的版本，框架可在加载场景之前自动启动，并注入注册的内容。
框架使用接口和特性作为注册依据，在编写代码期间即可实现模块的注入，无需在游戏场景中配置或者编写配置文件。  
框架保留了拓展接口，可开发更多的模块以实现功能的拓展。
大量的反射操作可能导致过多的资源消耗，使用时请考虑由于资源引起的隐藏卡顿问题。  
相关开发尚未完成大规模的测试，只进行了基础的测试，请谨慎使用。

注：Command模块尚处于开发过程中，尚未进行测试以及UI的开发，请忽略相关内容。

### Core

Core模块是核心的框架，包含了框架的入口以及拓展开发的接口和特性。

#### 框架入口

1.在Unity5以上版本，框架会在加载场景之前自动启动。

2.在Unity5以下版本，需要在场景加载之前手动调用框架的启动方法。  
已有的模块中，手动模块的工厂方法获取模块都可作为启动的入口
```csharp
public class Example : MonoBehaviour : IGetContainer{//实现该接口即可直接调用工厂中的获取方法
    void Start(){
        var container = this.GetFrameContainer();//只要获取方法被初次调用，框架就会启动
    }
}

```

#### 拓展开发

拓展的开发主要分为以下几步：
1. 为类添加RegisterConfig特性，用以注册模块以及需要接收的特性。
2. 实现ICofig接口，即可实现基本的配置功能；实现IConfigInit,IAfterConfig,ISceneChange接口，丰富模块的生命周期
3. 建立工厂类，创立拓展方法和对应接口，简化获取过程。

快速案例：
```csharp
//创建一个特性，用来作为模块监听的目标标签
public class MyAttribute : DirectRetrieveAttribute{}//使用的特性需要继承DirectRetrieveAttribute才能被框架监听到

//创建一个模块
[RegisterConfig(typeof(MyAttribute))]//特性用来向框架注册模块，同时传入的内容为监听的类型，合法的类型会被传入Config方法
public class MyModule : IConfig,IConfigInit,IAfterConfig,ISceneChange
{
    //IConfigInit接口方法，会在开始配置前执行
    //可用于模块的初始化
    public void void OnConfigInit(){}
    
    //在每次配置之前执行，可用于配置前的准备工作
    public void BeforeConfig(){}
    
    //在每次配置时执行，可用于模块的配置工作
    //assembly为当前的程序集，可用来生成实例
    //classType为当前监听的特性所属的类型，可用于生成实例
    //attributeType为当前监听的特性类型，可用提取信息，监听的目标以此为依据
    public void Config(Assembly assembly,Type classType,Type attributeType){}
    
    //在每次配置之后执行，可用于配置后的清理工作
    public void AfterConfig(){}
    
    //在所有的模块配置完成后执行，此时所有的合法的类型已经被模块接收
    //可用于初始化等工作
    public void AfterConfigAll(){}
}
```

### Manager

管理器实际上是一个脱离场景存在的全局类，提供一些全局的服务。  
由于全局的类往往需要先于场景加载，为场景中的脚本提供服务。
而全局的类又缺少合适的程序入口，或需要维系一个庞大的加载链，操作复杂且容易出错。  
因此，框架提供了一个全局的容器用以存放此类。并提供了自动激活和工厂方法快速获取管理器。

注册管理器主要分为以下几步：
1. 为类添加RegisterManager特性，用以注册管理器
2. 如果需要依托于场景的组件，可对组件字段添加GetComponent特性，请求注入
3. 实现IManagerAfterConstruct,IManagerCoreInit,IManagerInit,IManagerLastInit,IManagerChangeScene拓展管理器生命周期。
4. 在需要获取管理器的类上实现IGetManager接口，即可通过工厂方法获取管理器。

快速案例：
```csharp

//创建一个管理器
[RegisterManager]
public class MyManager : IManagerAfterConstruct,IManagerCoreInit,IManagerInit,IManagerLastInit,IManagerChangeScene{
    
    //使用此特性会在场景中生成一个对应的组件并注入
    [GetComponent]
    public BoxCollider collider;
    
    //如果该物体比较特殊，也可使用工厂方法获取
    //自定义方法会忽略创建对应组件，在切换场景时可能会被销毁，需要注意
    [GetComponent(factoryType = typeof(MyFactory),String methodName="GetMainCamera)]
    public Camera mainCamera;
    
    //IManagerAfterConstruct接口方法
    //在管理器构造后执行，可用于初始化，此时组件并未完成注入
    void InitAfterConstruct(){}
    
    //IManagerCoreInit接口方法
    //在框架启动时执行，此方法会最先执行
    //可用于一些核心管理器的初始化，以便于为其他管理器提供服务
    //若不是其他管理器依赖的管理器，不建议实现此接口
    void ManagerInit(){}
    
    //IManagerInit接口方法
    //在场景加载前执行，可用于初始化
    //会先于Start执行
    void Awake();
    
    //在场景加载前执行，可用于初始化
    void Start();
    
    //IManagerLastInit接口方法
    //在所有的管理器都初始化完成后执行，可用于初始化
    void LastInit(){}
    
    //IManagerChangeScene接口方法
    //此接口会在每次场景切换执行，而其他接口只会在进入程序时执行一次
    //在每次进入场景时执行
    void OnEnterScene(){}
    //在每次进入场景时执行
    void OnExitScene(){}
}

//工厂方法，使用静态方法，可实现自定义注入方式
public class MyFactory
{
    public static Camera GetMainCamera()
    {
        return Camera.main;
    }
}
```  

### SceneFilter

场景过滤器，由于在场景中检索物体耗时耗力，如果有多个又检索脚本必然会造成卡顿。
场景过滤器会将场景中的物体在进入场景前进行遍历，将场景物体传入过滤器。
过滤器会判断物体是否合法并储存。
当场景完全遍历完成后，过滤器会调用回调，可对过滤到的物体进行操作。  
过多的场景元素和过多的过滤器都会导致该模块需要更多的时间运算，请谨慎使用。

注册场景过滤器主要分为以下几步：
1. 为类添加RegisterFilter特性，用以注册场景过滤器,以及配置过滤器信息
2. 场景过滤器需要继承BaseFilter类，用以实现过滤器功能

快速案例：
```csharp

//创建一个场景过滤器
//path代表过滤器链地址，可实现层层过滤
//不填写名称默认以类名作为过滤器名称，注册在根路径，接收整个场景的物体
//type代表过滤器类型，默认为Normal,使用First会在找到第一个合法物体后停止查找，继续向下一层过滤器传递
[RegisterFilter(string path="",type = FilterType.Normal)]
public class MySceneFilter : BaseFilter{//实现BaseFilter即可实现过滤器功能
    public void Init();//在开始过滤之前执行，可作为初始化方法
    //过滤时会将上一层过滤的结果一次传入，用以判断物体是否合法
    //返回true代表物体合法，会被保存
    //返回false代表物体不合法，不会被保存
    public bool Where(GameObject gameObject);
    //回调函数，会在全部完成过滤后执行
    //过滤器本身为字典，合法的数据储存在其中，可使用字典的方法获取
    public void Register();
}

```

### PlayerStateMachine
角色状态机，使用特性注册的方式解耦状态业务与状态机的组装。  
使用  编写状态业务并注册 -> 框架扫描并生成组装状态机 -> 场景中状态机组件从框架获取状态机并使用 的模式
该状态机是直接切换状态的有限状态机，而非状态转换图。

注册状态机主要分为以下几步：
1. 为类添加RegisterState特性，用以注册状态(可添加多个特性，注册到多个状态或者状态机)
2. 状态类继承AbstractState类，用以实现状态的业务
3. 在场景中物体上挂载PlayerStateMachineComp组件，并填写状态机名称和初始状态。
4. 场景启动，状态机进入生命周期

快速案例：
```csharp
//注册一个状态，可将一个类注册为多个状态
[IRegisterState(stateName="", stateMachineName="")]//不填写状态机名称默认使用类名作为状态名，不填写状态机名称默认注册到全部状态机
[IRegisterState(stateName="MyState1", stateMachineName="MyMachine")]
public class MyState : AbstractState
{
    //构造器调用后执行
    public  void InitAfterConstruct(){}
    
    //最初执行，用于初始化，在Comp组件的Awake中调用
    public  void OnAwake(){}
    
    //最初执行，用于初始化，在Comp组件的Start中调用
    public  void OnStart(){}
    
    //在每次Update中执行，在Comp组件的Update中调用
    public  void OnUpdate(){}
    //在每次FixedUpdate中执行，在Comp组件的FixedUpdate中调用
    public  void OnFixedUpdate(){}
    //在每次LateUpdate中执行，在Comp组件的LateUpdate中调用
    public  void OnLateUpdate(){}
    
    //在进入状态时执行
    public  void OnEnterState(){}
    //在离开状态时执行
    public  void OnExitState(){}
    
    //在Comp执行OnEnable时执行
    public  void OnMachineEnabled(){}
    
    //在Comp执行OnDisable时执行
    public  void OnMachineDisabled(){}
}
```

在状态，状态机Comp组件中，都可调用bool ChangeState(string stateName)函数实现切换状态。  
不合法的状态名会返回false，合法的状态名会切换到对应的状态并返回true。  
切换状态为延迟执行，不会立刻执行，防止在一帧中大量切换状态。

在状态中transform,gameobject,component(PlayerStateMachineComp:MonoBehavior),body(StateMachineBody)都可直接作为属性获取。  
stateName,stateMachineName也都可直接获取

状态机组件PlayerStateMachineComp在继承后通过重新一系列函数可向其MonoBehavior的生命周期中添加方法，拓展功能