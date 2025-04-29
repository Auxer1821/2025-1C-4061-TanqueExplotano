using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;



namespace TGC.MonoGame.TP.src.Esenario
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class ManageObjetos 
    {
        private List<Objetos.Objetos> _objetos; // genérica
        private List<Casa.Arbol1> _arboles;
        private List<Casa.Caja> _cajas;
        private List<Casa.Casa> _casas;
        private List<Casa.Roca> _rocas;
        private List<Montana.Montana> _montanas;
        private List<Entidad.Entidad> _tanks;

        public ManageObjetos()
        {
            _objetos = new List<Objetos.Objetos>();
            _arboles = new List<Casa.Arbol1>();
            _cajas = new List<Casa.Caja>();
            _casas = new List<Casa.Casa>();
            _rocas = new List<Casa.Roca>();
            _montanas = new List<Montana.Montana>();
            _tanks = new List<Entidad.Entidad>();
        }

        public void AgregarObjeto(Objetos.Objetos objeto)
        {
            _objetos.Add(objeto);
        }

        public void AgregarArbol(Casa.Arbol1 arbol)
        {
            _arboles.Add(arbol);
        }

        public void AgregarCaja(Casa.Caja caja)
        {
            _cajas.Add(caja);
        }

        public void AgregarCasa(Casa.Casa casa)
        {
            _casas.Add(casa);
        }

        public void AgregarRoca(Casa.Roca roca)
        {
            _rocas.Add(roca);
        }

        public void AgregarMontana(Montana.Montana montana)
        {
            _montanas.Add(montana);
        }

        public void AgregarTanke(Tanke.Tanke Tanke)
        {
            _tanks.Add(Tanke);
        }


        public void DibujarObjetos(GraphicsDevice graphicsDevice)
        {
            foreach (var objeto in _objetos)
            {
                objeto.Dibujar(graphicsDevice);
            }

            foreach (var casa in _casas)
            {
                casa.Dibujar(graphicsDevice);
            }
            foreach (var caja in _cajas)
            {
                caja.Dibujar(graphicsDevice);
            }
            foreach (var arbol in _arboles)
            {
                arbol.Dibujar(graphicsDevice);
            }
            foreach (var roca in _rocas)
            {
                roca.Dibujar(graphicsDevice);
            }
            foreach (var montana in _montanas)
            {
                montana.Dibujar(graphicsDevice);
            }
            foreach (var tank in _tanks)
            {
                tank.Dibujar(graphicsDevice);
            }
        }

        public void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            foreach (var objeto in _objetos)
            {
                objeto.ActualizarVistaProyeccion(Vista, Proyeccion);
            }

            foreach (var casa in _casas)
            {
                casa.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var caja in _cajas)
            {
                caja.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var arbol in _arboles)
            {
                arbol.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var roca in _rocas)
            {
                roca.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var montana in _montanas)
            {
                montana.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
            foreach (var tank in _tanks)
            {
                tank.ActualizarVistaProyeccion(Vista, Proyeccion);
            }
        }

    }
}