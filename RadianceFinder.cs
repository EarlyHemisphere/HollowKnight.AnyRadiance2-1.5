using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnyRadiance {
    internal class RadianceFinder : MonoBehaviour {
        private GameObject _radiance;
        private bool _foundRadiance;

        private void Update()
        {
            if (_radiance == null)
            {
                _foundRadiance = false;
                _radiance = GameObject.Find("Absolute Radiance");
            }
            else if (!_foundRadiance)
            {
                _foundRadiance = true;
                Log("[Any Radiance]: Found Absolute Radiance.");
                _radiance.AddComponent<Radiance>();
            }
        }

        private static IEnumerator AddComponent()
        {
            yield return null;
            GameObject.Find("Absolute Radiance").AddComponent<Radiance>();
        }

        private void Log(object message)
        {
            Modding.Logger.Log("[Radiance Finder]: " + message);
        }
    }
}
