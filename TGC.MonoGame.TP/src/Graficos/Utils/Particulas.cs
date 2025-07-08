using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Graficos.Temporales;


namespace TGC.MonoGame.TP.src.Graficos.Utils
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class Particulas
    {
        private List<Vector3> _posTanquesDestruidos;
        private int _cantidadMaxTanquesDestruidos;

        private List<EmisorParticula> _particulasFuego; // Lista de emisores de particulas para fuego
        private List<EmisorParticula> _particulasHumo; // Lista de emisores de particulas para humo

        public Particulas()
        {
            this._posTanquesDestruidos = new List<Vector3>();
            this._cantidadMaxTanquesDestruidos = 3;
            this._particulasFuego = new List<EmisorParticula>();
            this._particulasHumo = new List<EmisorParticula>();

            /*
            this._particulasDisparo = new EmisorParticula();
            this._particulasDisparo.Initialize(Content, Graphics, 20, this._posicion ,"disparo");
            this._particulasDisparo.SetNuevaPosicion(); // cambia la posicion inicial de las particulas
            this._particulasDisparo.SetPosiciones();
            */
        }

        public void Initialize(ContentManager Content, GraphicsDevice Graphics)
        {
            for (int i = 0; i < _cantidadMaxTanquesDestruidos; i++)
            {
                // Inicializar emisores de particulas para fuego y humo
                var emisorFuego = new EmisorParticula();
                emisorFuego.Initialize(Content, Graphics, 20, Vector3.Zero, "fuego");
                _particulasFuego.Add(emisorFuego);

                var emisorHumo = new EmisorParticula();
                emisorHumo.Initialize(Content, Graphics, 20, Vector3.Zero, "humo");
                _particulasHumo.Add(emisorHumo);
            }
        }

        public void Update(GameTime gameTime)
        {
                // Actualizar las posiciones de los emisores de fuego y humo
                for (int i = 0; i < _posTanquesDestruidos.Count; i++)
                {
                    //_particulasFuego[i].SetNuevaPosicion(_posTanquesDestruidos[i]);
                    //_particulasFuego[i].SetPosiciones(_posTanquesDestruidos[i]);
                    //_particulasHumo[i].SetNuevaPosicion(_posTanquesDestruidos[i]);
                    //_particulasHumo[i].SetPosiciones(_posTanquesDestruidos[i]);
                    _particulasFuego[i].Update(gameTime);
                    _particulasHumo[i].Update(gameTime);
                }
        }

        public void Dibujar()
        {
            foreach (var emisor in _particulasFuego)
            {
                emisor.Dibujar();
            }
            foreach (var emisor in _particulasHumo)
            {
                emisor.Dibujar();
            }
        }

        public void AgregarTanqueDestruido(Vector3 pos){
            if (_posTanquesDestruidos.Count < _cantidadMaxTanquesDestruidos)
            {
                this._posTanquesDestruidos.Add(pos);

                this._particulasFuego[_posTanquesDestruidos.Count - 1].SetPuedeDibujar(true);
                this._particulasFuego[_posTanquesDestruidos.Count - 1].SetNuevaPosicion(pos);
                this._particulasFuego[_posTanquesDestruidos.Count - 1].SetPosiciones(pos);

                this._particulasHumo[_posTanquesDestruidos.Count - 1].SetPuedeDibujar(true);
                this._particulasHumo[_posTanquesDestruidos.Count - 1].SetNuevaPosicion(pos);
                this._particulasHumo[_posTanquesDestruidos.Count - 1].SetPosiciones(pos);

            }
        }


    }
}