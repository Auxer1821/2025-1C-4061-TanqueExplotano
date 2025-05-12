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
    public class Etanque:EntidadFull
    {
        
        // Variables
        private bool _activo;
        private TipoTanque _tipoTanque;
        private Vector2 _dirMovimiento = Vector2.UnitX;
        private Vector3 _dirApuntado = Vector3.UnitX;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Etanque(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this._tipoTanque = new TanquePanzer(); 
            this._modelo = new Tanques.MTanque(_tipoTanque);
            this._tipo = TipoEntidad.Tanque;
            this._activo = true;
            //Crear Bounding Volume
            base.Initialize(Graphics,Mundo,View,Projection,Content, escenario);
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override bool PuedeChocar(){
            return _activo;
        }
        public override bool PuedeSerChocado(){
            return true;
        }

        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica){
            switch (entidadEstatica._tipo)
            {
                case TipoEntidad.Bala:
                    this.RecibirDaño(dataChoque);
                    break;

                case TipoEntidad.Tanque:
                case TipoEntidad.Obstaculo:
                    this.AplicarColisionEstructural(dataChoque);
                    break;

                default:
                    // Quizás no hacer nada, o loguear el caso
                    break;
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            this.ActualizarMatrizMundo();
        }
        

        //Metodos propios
        private void AplicarColisionEstructural(DataChoque choque)
        {
            // Empuja al tanque hacia afuera para que no quede dentro del obstáculo
            Vector3 correccion = choque._normal * choque._penetracion;

            // Corregir posición
            //this.ubicacion += correccion;

            // Cancelar velocidad en dirección del impacto (variable de velocidad / movimiento)
            // this.Velocidad -= Vector3.Dot(this.Velocidad, choque._normal) * choque._normal;

            // También podrías anular el movimiento completamente, si es más simple
            // this.Velocidad = Vector3.Zero;
        }

        private void RecibirDaño(DataChoque choque)
        {
            // Restar vida (suponiendo que existe una propiedad 'Vida')
            //this.Vida -= CalcularDaño(choque);

            // Efectos visuales
            // MostrarChispa(choque._puntoContacto);
            // ReproducirSonidoImpacto();
            // Modificar la mesh del modelo para simular el impacto (Entrega 4)

            // Chequear destrucción
            //if (this.Vida <= 0)
            //  Destruir();
        }
        

        private void Destruir()
        {
            // Lógica de destrucción:  explosionar, etc.
            this._activo = false;
            // CrearEfectoExplosion(this._modelo.Posicion);
            this._escenario.EliminarEntidad(this);
            this._escenario.AgregarEntidad(this);
        }

//---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//

        // Función que actualiza los valores de posición y ángulo
        // Para luego usarlos y crear la matriz mundo
        public void Mover(){
            this._posicion += new Vector3(_dirMovimiento.X, 0, _dirMovimiento.Y);
            this._angulo += new Vector3(0f, (float) Math.Atan2(_dirMovimiento.Y, _dirMovimiento.X), 0f);
            this.ActualizarMatrizMundo();
        }

        //Función llamada gameplay para que actualice los valores de la matriz dirección.
        // Valores de 1, -1, 0
        // Luego será multiplicado por su respectiva velocidad
        public void ActualizarMovimiento(float x, float y){
            this._dirMovimiento.X = x;
            this._dirMovimiento.Y = y;
            this._dirMovimiento *= this._tipoTanque.velocidad();
        }
        
        
        // 
        public void Disparar(Vector3 apuntado){
            EBala bala = new EBala();
            bala.ActualizarDatos(this._dirApuntado,this._posicion); //TODO - Cambiar lugar de disparo para que no se autodestruya
            this._escenario.AgregarEntidad(bala);//temporal
        }

        //
        public void ActualizarApuntado(Vector3 apuntado){
            this._dirApuntado = apuntado;
        }

    }
}