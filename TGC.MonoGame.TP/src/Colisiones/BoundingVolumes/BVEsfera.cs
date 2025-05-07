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
    public class BVEsfera : BoundingsVolumes.BoundingVolume
    {
        
        // Variables
        public float _radio { get; set;}
        public Vector3 _cento { get; set;}

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        public BVEsfera(float radio, Vector3 centro){
            _radio = radio;
            _cento = centro;
        }

        //----------------------------------------------Funciones-de-Detección--------------------------------------------------// 

        
    }
}