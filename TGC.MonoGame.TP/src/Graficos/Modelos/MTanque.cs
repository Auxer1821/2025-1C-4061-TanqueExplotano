using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Tanques
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class MTanque : Modelos.Modelo
    {
        
        //----------------------------------------------Variables--------------------------------------------------//
        private TipoTanque _tipoTanque;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public MTanque(TipoTanque tipoTanque)
        {
            _tipoTanque = tipoTanque;
        }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.Gray.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//

        protected override void ConfigurarModelo(ContentManager Content){
            this._modelo = Content.Load<Model>("Models/tgc-tanks"+ this._tipoTanque.directorioModelo());
        }
        
        protected override void AjustarModelo(){
            this._matixBase = Matrix.CreateScale(this._tipoTanque.escala()) * Matrix.CreateRotationX(this._tipoTanque.angulo()) ;//TODO - SOLO ROTA EN X. Si queres hacerlo hermoso, cambiar. Escala de modelo no amerita.
        }

        //----------------------------------------------Dibujado--------------------------------------------------//
        

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        
    }
}