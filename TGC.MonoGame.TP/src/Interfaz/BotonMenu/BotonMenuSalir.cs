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
    public class BotonMenuSalir : IBotonMenu
    {
        TGCGame _game;
        public BotonMenuSalir(){}
        public override void Enter()
        {
            _game.Exit();
        }

        internal void Inicializar(TGCGame TGCGame)
        {
            _game = TGCGame;
        }
    }
}