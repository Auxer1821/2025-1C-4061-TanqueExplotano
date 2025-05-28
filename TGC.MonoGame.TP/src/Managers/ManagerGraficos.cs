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
        private List<Entidades.Entidad> _entidades; // dif listas [arboles,casa,caja,roca,pasto,montaña]
                                                    //effecto particular [arbol,casa,caja,roca,pasto,montaña]
        private Camaras.Camera _camera;
        
                                                    //skybox , pasto
                                                    //decidir terreno (separar el dibujado del alturamapa)
        public ManagerGrafico()
        {
            _entidades = new List<Entidades.Entidad>();

        }
        public void inicializarCamara(Camaras.Camera camera){
            _camera = camera;
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
                    //recorrer por listas separadas
            foreach (var entidad in _entidades)
            {
                entidad.EfectCamera(_camera.Vista,_camera.Proyeccion);
                entidad.Dibujar(graphicsDevice);
            }
        }


    }
}