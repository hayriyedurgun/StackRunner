using Assets._Scripts.Factories;
using Assets._Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Assets._Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public CameraController Camera;
        public AudioController Audio;

        public TileController TilePrefab;
        public FinishTileController FinishTilePrefab;

        public GameController GameController;
        public CharacterController Character;

        public override void InstallBindings()
        {
            Container.Bind<LevelFactory>().AsSingle();
            Container.BindInstance(Camera);
            Container.BindFactory<FinishTileController, FinishTileFactory>().FromComponentInNewPrefab(FinishTilePrefab);
            Container.BindFactory<TileController, TileFactory>().FromComponentInNewPrefab(TilePrefab);
            Container.BindInstance(Audio);
            Container.BindInstance(GameController);
            Container.BindInstance(Character);
        }
    }
}