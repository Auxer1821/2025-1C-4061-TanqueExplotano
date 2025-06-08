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
    public class ETanqueIA:Etanque
    {
        
        

        //-------------------------------------------||---Constructores-e-inicializador--------------------------------------------------//
        public ETanqueIA(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario){
            //Tal vez haga algo
            base.Initialize(Graphics,Mundo,Content, escenario);
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

    


        public override void Update(GameTime gameTime)
        {
            //setear los valores de movimiento y disparo
            float mseg = (float) gameTime.ElapsedGameTime.TotalSeconds;

            //1. Setear Velocidad Actual = tipo.velocidad
            //this._velocidadActual = this._tipoTanque.velocidad() * mseg * 0.5f;

            //2. Setear giro = giro.angulo
            //this._anguloActual = this._tipoTanque.anguloRotacionMovimiento() * mseg /8;
            this.Disparar();
            base.Update(gameTime);
        }
        



//---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//


    }
}