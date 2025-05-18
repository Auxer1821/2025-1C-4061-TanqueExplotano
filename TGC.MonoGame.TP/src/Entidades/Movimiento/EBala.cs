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
    public class EBala : EntidadColision
    {

        // Variables
        private float tiempo;
        private Vector3 _direccion;
        public float _danio { get; set; }

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public EBala()
        {
            this.tiempo = 0.2f;
        }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._tipo = TipoEntidad.Bala;
            base.Initialize(Graphics, Mundo, View, Projection, Content, escenario);
        }
        public void Initialize(Escenarios.Escenario escenario, float danio)
        {
            this._danio = danio;
            this._tipo = TipoEntidad.Bala;
            this._escenario = escenario;
        }

        public void ActualizarDatos(Vector3 direccion, Vector3 puntoPartida)
        {
            this._direccion = direccion;
            this._posicion = puntoPartida;
            this._boundingVolume = new BoundingsVolumes.BVRayo(direccion, puntoPartida);
        }


        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override bool PuedeChocar()
        {
            return true;
        }
        public override bool PuedeSerChocado()
        {
            return true;
        }

        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            //this._escenario.AgregarAEliminar(this);
        }


        public override void Update(GameTime gameTime)
        {
            this.tiempo -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.tiempo < 0f)
            {
                this._escenario.AgregarAEliminar(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //debug
            }
        }
    }
}