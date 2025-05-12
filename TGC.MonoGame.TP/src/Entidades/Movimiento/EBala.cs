using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Tanques;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class EBala:EntidadColision
    {
        
        // Variables
        private Vector3 _direccion;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public EBala(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            base.Initialize(Graphics,Mundo,View,Projection,Content, escenario);
        }

        public void ActualizarDatos(Vector3 direccion, Vector3 puntoPartida ){
            this._direccion = direccion;
            this._posicion = puntoPartida;
            this._boundingVolume= new BoundingsVolumes.BVRayo(direccion, puntoPartida);
        }


        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override bool PuedeChocar(){
            return true;
        }
        public override bool PuedeSerChocado(){
            return false;
        }

        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica){
            this._escenario.EliminarEntidad(this);   
        }
    }
}