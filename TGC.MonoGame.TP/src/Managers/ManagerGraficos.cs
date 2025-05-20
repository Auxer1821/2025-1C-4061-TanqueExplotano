using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ManagerGrafico 
    {
        private List<Entidades.Entidad> _entidades;
        
        public ManagerGrafico()
        {
            _entidades = new List<Entidades.Entidad>();

        }

        public void AgregarEntidad(Entidades.Entidad entidad)
        {
            if (entidad.PuedeDibujar())
                _entidades.Add(entidad);
        }
        public void RemoverEntidad(Entidades.Entidad entidad)
        {
            _entidades.Remove(entidad);
        }

        public void DibujarObjetos(GraphicsDevice graphicsDevice)
        {
            foreach (var entidad in _entidades)
            {
                entidad.Dibujar(graphicsDevice);
            }
        }

        public void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){

            foreach (var entidad in _entidades)
            {
                entidad.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
        }

    }
}