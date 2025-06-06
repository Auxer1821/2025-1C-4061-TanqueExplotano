using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.HUD
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class BotonMenuJugar : IBotonMenu
    {
        private Escenarios.DirectorEscenarios _escenarioDirector;
        public BotonMenuJugar(){}
        
        internal void Inicializar(Escenarios.DirectorEscenarios escenarioDirector )
        {
            _escenarioDirector = escenarioDirector;
        }
        public override void Enter()
        {
            this._escenarioDirector.GetGame().IsMouseVisible = false;
            this._escenarioDirector.CambiarEsenarioActivo(Escenarios.TipoEsenario.Gameplay);
        }


    }
}