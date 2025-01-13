using System.Linq;
using Material.UnityDevToolkits.SceneFilter;
using Material.UnityDevToolkits.SceneFilter.Structures;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterFilter("T3/T2")]
    public class TestFilter2 :BaseFilter
    {
        public override void Init()
        {
            Debug.Log("TestFilter1 Init");
        }

        public override bool Where(GameObject gameObject)
        {
            if (gameObject.tag.Equals("Player")) return true;
            return false;
        }

        public override void Register()
        {
            Debug.Log($"TestFilter2 :{Count},{Values.ToArray()[0].name}");
        }
    }
}