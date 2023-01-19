using Assets._Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Assets._Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public CameraController Camera;

        public override void InstallBindings()
        {
            Container.BindInstance(Camera);
        }
    }
}