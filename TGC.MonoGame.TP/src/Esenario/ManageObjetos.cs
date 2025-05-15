using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;



namespace TGC.MonoGame.TP.src.ManagersObjetos
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class ManagerObjetos 
    {
        private List<Entidades.EntidadFull> _eFulls;
        
        private List<Entidades.EntidadGrafica> _eGrafica;
        
        private List<Entidades.EntidadColision> _eColiisiones;

        public ManagerObjetos()
        {
            _eFulls = new List<Entidades.EntidadFull>();
            _eGrafica = new List<Entidades.EntidadGrafica>();
            _eColiisiones = new List<Entidades.EntidadColision>();
        }

        public void AgregarEntidadFull(Entidades.EntidadFull eFull)
        {
            _eFulls.Add(eFull);
        }
        public void AgregarEntidadGrafica(Entidades.EntidadGrafica eGrafica)
        {
            _eGrafica.Add(eGrafica);
        }
        public void AgregarEntidadColicion(Entidades.EntidadColision eColicion)
        {
            _eColiisiones.Add(eColicion);
        }


        public void DibujarObjetos(GraphicsDevice graphicsDevice)
        {
            foreach (var eGrafica in _eGrafica)
            {
                eGrafica.Dibujar(graphicsDevice);
            }
            foreach (var eFull in _eFulls)
            {
                eFull.Dibujar(graphicsDevice);
            }
        }

        public void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            foreach (var eFull in _eFulls)
            {
                eFull.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var eGrafica in _eGrafica)
            {
                eGrafica.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
        }

    }
}