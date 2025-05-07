using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.BoundingsVolumes
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class DataChoque
    {
        
        // Variables
        private Vector3 _puntoContacto {get; set;}
        private float _penetracion{get; set;}
        private Vector3 _normal{get; set;}

 //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public DataChoque (Vector3 puntoContacto, float penetracion, Vector3 normal){
            this._puntoContacto =  puntoContacto;
            this._penetracion =  penetracion;
            this._normal =  normal;
        } 

    }
}