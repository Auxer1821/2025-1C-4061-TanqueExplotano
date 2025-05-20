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
    public class BVCuboAABB : BoundingsVolumes.BoundingVolume
    {
        
        // Variables
        public Vector3 _minimo {get; set;}
        public Vector3 _maximo {get; set;}

        
        public BVCuboAABB(Vector3 minimo, Vector3 maximo){
            _minimo=minimo;
            _maximo=maximo;
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------// 

        public float LadoX(){
            return this._maximo.X - this._minimo.X;
        }

        public float LadoY(){
            return this._maximo.Y - this._minimo.Y;
        }

        public float LadoZ(){
            return this._maximo.Z - this._minimo.Z;
        }

        public Vector3 Centro(){
            return (_maximo + _minimo) / 2 ;
        }


    }
}