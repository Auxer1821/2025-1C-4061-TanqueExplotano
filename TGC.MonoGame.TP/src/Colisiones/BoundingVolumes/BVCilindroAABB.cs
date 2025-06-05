using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase de esfera para los bounding Volume.
    /// </summary>
    public class BVCilindroAABB : BoundingsVolumes.BoundingVolume
    {
        
        // Variables
        public float _radio { get; set;}
        public Vector3 _centro { get; set;}//El centro es el centro de la figura (no la base)
        public float _alto { get; set; }

        
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
    public BVCilindroAABB(Vector3 centro, float radio, float alto)
    {
        _centro = centro;
        _radio = radio;
        _alto = alto;
    }

    public override Vector3 GetCentro()
    {
        return _centro;
    }
    }
}