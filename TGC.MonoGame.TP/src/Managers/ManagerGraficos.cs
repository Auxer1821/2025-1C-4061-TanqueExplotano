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
        //deberia estar el frostum aca o en coliciones?
        private BoundingFrustum _boundingFrustum;
        //ejemplo de uso
        //_boundingFrustum = new BoundingFrustum(_testCamera.View * _testCamera.Projection);
        //_boudingFrustum.intersects(_entidad.BoundingBox);
        private List<Entidades.Entidad> _entidades; // dif listas [arboles,casa,caja,roca,pasto,montaña]
                                                    //effecto particular [arbol,casa,caja,roca,pasto,montaña]

        private List<Entidades.EPasto> _pastos;
        private Entidades.ESkyBox _skyBox;
        private Camaras.Camera _camera;
        private Terrenos.Terreno _terreno;

        //skybox , pasto
        //decidir terreno (separar el dibujado del alturamapa)
        public ManagerGrafico()
        {
            _entidades = new List<Entidades.Entidad>();
            _pastos = new List<Entidades.EPasto>();

        }
        public void inicializarCamara(Camaras.Camera camera){
            _camera = camera;
        }

        public void inicializarSkyBox(Entidades.ESkyBox skyBox)
        {
            _skyBox = skyBox;
        }

        public void inicializarTerreno(Terrenos.Terreno terreno)
        {
            _terreno = terreno;
        }

        public void AgregarEntidad(Entidades.Entidad entidad)
        {
            if (entidad.PuedeDibujar())
                _entidades.Add(entidad);
        }

        public void AgregarPasto(Entidades.EPasto pasto)
        {
                _pastos.Add(pasto);
        }
        public void RemoverEntidad(Entidades.Entidad entidad)
        {
            _entidades.Remove(entidad);
        }

        public void DibujarObjetos(GraphicsDevice graphicsDevice)
        {
                    //recorrer por listas separadas
            _skyBox.EfectCamera(_camera.Vista,_camera.Proyeccion);
            _skyBox.Dibujar(graphicsDevice);

            _terreno.EfectCamera(_camera.Vista,_camera.Proyeccion);
            _terreno.Dibujar(graphicsDevice);

            foreach (var entidad in _entidades)
            {
                entidad.EfectCamera(_camera.Vista,_camera.Proyeccion);
                entidad.Dibujar(graphicsDevice);
            }

            foreach (var pasto in _pastos)
            {
                pasto.EfectCamera(_camera.Vista,_camera.Proyeccion);
                pasto.Dibujar(graphicsDevice);
            }

        }

        public void ActualizarPasto(GameTime gameTime)
        {
            foreach (var pasto in _pastos)
            {
                pasto.ActualizarTime((float)gameTime.TotalGameTime.TotalSeconds);
            }
        }


    }
}