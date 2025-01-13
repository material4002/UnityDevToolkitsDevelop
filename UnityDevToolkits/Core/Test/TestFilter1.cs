using System.Linq;
using Material.UnityDevToolkits.SceneFilter;
using Material.UnityDevToolkits.SceneFilter.Structures;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterFilter("T3/T2/T1")]
    public class TestFilter1 :BaseFilter
    {
        public override void Init()
        {
            Debug.Log("TestFilter1 Init");
        }

        public override bool Where(GameObject gameObject)
        {
            if (gameObject.name.Equals("T1")) return true;
            return false;
        }

        public override void Register()
        {
            Debug.Log($"TestFilter1 :{Count},{Values.ToArray()[0].name}");
        }
    }
}