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
    public class BVRayo : BoundingsVolumes.BoundingVolume
    {
        
        // Variables
        public Vector3 _Direccion { get; set;}
        public Vector3 _PuntoPartda { get; set;}

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------// 
        public BVRayo(Vector3 Direccion, Vector3 PuntoPartda){

            _Direccion = Vector3.Normalize(Direccion);
            _PuntoPartda = PuntoPartda;
        }

        //----------------------------------------------Funciones-de-Detección--------------------------------------------------// 

        
    }
}