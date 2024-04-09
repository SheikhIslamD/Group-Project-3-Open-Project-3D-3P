using UnityEngine;

public static class Scenes
{
    public const string mainMenu = "MenuScene";
    public const string credits = "CreditsScene";
    public const string howToPlay = "HowToPlayScene";

}

public static class Layers
{
    public struct Layer
    {
        public Layer(int id) => this.id = id;

        public int id;
        public string name => LayerMask.LayerToName(id);
        public LayerMask mask => (LayerMask)id;
        public LayerMask maskInvert => ~(LayerMask)id;

        public static implicit operator string(Layer value) => value.name;
        public static implicit operator int(Layer value) => value.id;

    }

    public static Layer Defualt = new(0);
    public static Layer TransparentFX = new(1);
    public static Layer IgnoreRaycast = new(2);
    public static Layer NonCollide = new(3);
    public static Layer Water = new(4);
    public static Layer UI = new(5);
    public static Layer Player = new(6);
    public static Layer Enemy = new(7);
    public static Layer PlayerProjectile = new(8);
    public static Layer EnemyProjectile = new(9);
    public static Layer PlayerTriggers = new(10);



}
