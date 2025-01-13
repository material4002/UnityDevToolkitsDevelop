using System.Linq;
using Material.UnityDevToolkits.SceneFilter;
using Material.UnityDevToolkits.SceneFilter.Structures;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterFilter("T3")]
    public class TestFilter3 :BaseFilter
    {
        public override void Init()
        {
            Debug.Log("TestFilter3 Init");
        }

        public override bool Where(GameObject gameObject)
        {
            if (gameObject.layer==4) return true;
            return false;
        }

        public override void Register()
        {
            Debug.Log($"TestFilter3 :{Count},{Values.ToArray()[0].name}");
        }
    }
}