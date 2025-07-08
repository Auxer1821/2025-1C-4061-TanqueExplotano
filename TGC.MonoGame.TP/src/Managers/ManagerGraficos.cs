using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.Moldes;
using TGC.MonoGame.TP.src.Graficos.Utils;
using System.IO;
using TGC.MonoGame.TP.src.Entidades;




namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ManagerGrafico
    {
        //deberia estar el frostum aca o en coliciones?
        private BoundingsVolumes.BVTrufas _boundingFrustum;

        private List<Entidades.Entidad> _entidades; // dif listas [arboles,casa,caja,roca,pasto,montaña]
                                                    //effecto particular [arbol,casa,caja,roca,pasto,montaña]
        private List<Entidades.EPasto> _pastos;
        private Entidades.ESkyBox _skyBox;
        private Camaras.Camera _camera;
        private Terrenos.Terreno _terreno;
        private List<Moldes.IMolde> _moldes;
        private Vector3 _posSOL;
        public ShadowMapping _shadowMapper;
        private Particulas _particulas;
        private EJugador _jugador;
        private List<Entidades.Entidad> _entidadesShadowMap = new List<Entidades.Entidad>();


        //skybox , pasto
        //decidir terreno (separar el dibujado del alturamapa)
        public ManagerGrafico()
        {
            _entidades = new List<Entidades.Entidad>();
            _pastos = new List<Entidades.EPasto>();
            _posSOL = new Vector3(900.0f, 400.0f, -1000.0f);
            //_posSOL = new Vector3(500.0f, 400.0f, -600.0f);
            _shadowMapper = new ShadowMapping();
            _particulas = new Particulas();
        }

        public void inicializarCamara(Camaras.Camera camera)
        {
            _camera = camera;
            _boundingFrustum = new BoundingsVolumes.BVTrufas(camera);
        }
        public void inicializarSkyBox(Entidades.ESkyBox skyBox)
        {
            _skyBox = skyBox;
        }

        public void inicializarTerreno(Terrenos.Terreno terreno)
        {
            _terreno = terreno;
        }
        public void InicializarMoldes(List<IMolde> moldes)
        {
            _moldes = moldes;
        }

        public void InicializarShadowMapping(GraphicsDevice graphicsDevice){
            _shadowMapper.Initialize(_posSOL, graphicsDevice);
        }
        public void InicializarParticulas(ContentManager content, GraphicsDevice graphicsDevice){
            _particulas.Initialize(content, graphicsDevice);
        }
        

        public void AgregarEntidad(Entidades.Entidad entidad)
        {
            if (entidad.PuedeDibujar()){
                _entidades.Add(entidad);
                this.AgregarAShadowMap(entidad);
            }
        }
        public void InicializarJugador(EJugador jugador){
            _jugador = jugador;
        }

        private void AgregarAShadowMap(Entidad entidad)
        {
            if (entidad._posicion.X > 140 && entidad._posicion.Z > 200 && entidad._tipo == TipoEntidad.Obstaculo){
                return;
            }
            else{
                _entidadesShadowMap.Add(entidad);
            }
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
            //--------------1er Recorrida: ShadowMapping--------------//
            _shadowMapper.ActualizarShadowMap(graphicsDevice, _entidadesShadowMap, _terreno);

            //--------------2da Recorrida: Dibujado comun--------------//

            //recorrer por listas separadas
            _skyBox.EfectCamera(_camera.Vista, _camera.Proyeccion);
            _skyBox.Dibujar(graphicsDevice);//se dibuja primero

            _terreno.EfectCamera(_camera.Vista, _camera.Proyeccion);
            _terreno.setCamara(_camera.Position);
            _terreno.Dibujar(graphicsDevice, _shadowMapper);

            //solamente los tanques pasan aqui
            /*
            foreach (var entidad in _entidades)
            {
                if (entidad.GetMolde() != null)
                    continue;
                entidad.EfectCamera(_camera.Vista,_camera.Proyeccion);
                entidad.Dibujar(graphicsDevice);
            }
            */

            //setear la vista y proyeccion una vez por frame
            foreach (var molde in _moldes)
            {
                setVistaProjection(molde, _camera.Vista, _camera.Proyeccion);
            }


            //dibuja todas las instancias con sus matrices mundo
            /*
            foreach (var entidad in _entidades)//TODO - Filtrar solo para obstaculos
            {
                if (entidad.GetMolde() == null)
                    continue;
                entidad.GetMolde().Draw(entidad.GetMundo(), graphicsDevice);
            }
            */

            //TODO de los chunks de minegraft

            foreach (var entidad in _entidades)
            {
//                if (_boundingFrustum.colisiona(entidad._boundingVolume) || entidad.ExcluidoDelFrustumCulling())
                if (entidad.FrustumCulling(_boundingFrustum) || entidad.ExcluidoDelFrustumCulling())
                {
                    if (entidad._tipo == Entidades.TipoEntidad.Obstaculo)
                    {
                        entidad.GetMolde().Draw(entidad.GetMundo(), graphicsDevice, _shadowMapper);
                    }
                    else    //tanque
                    {
                        entidad._modelo.SetPosSOL(_posSOL);
                        entidad._modelo.setCamara(_camera.Position);
                        entidad.EfectCamera(_camera.Vista, _camera.Proyeccion);
                        entidad.Dibujar(graphicsDevice, _shadowMapper);
                    }
                }
            }

            //--------------5da Sección: Dibujado Opcional--------------//

            foreach (var pasto in _pastos) 
            {
                if (_boundingFrustum.colisiona(pasto._posicion))
                {
                    //pasto.EfectCamera(_camera.Vista, _camera.Proyeccion);
                    //pasto.Dibujar(graphicsDevice);
                    //pasto.GetMolde().Draw(pasto.GetMundo(), graphicsDevice);

                    pasto.GetMolde().Draw(pasto.GetMundo(), graphicsDevice, _shadowMapper);
                }
            }

            //--------------4ta Sección: Dibujado de Particulas--------------//
            _particulas.Dibujar();

        }

        private void ActualizarEntidadesShadowMap(GameTime gameTime)
        {
                _entidadesShadowMap.Clear();
                foreach (var entidad in _entidades)
                {
                    float distancia = Vector3.Distance(entidad._posicion, _jugador._posicion);
                    if ( entidad.FrustumCulling(_boundingFrustum) || distancia < 100.0f || entidad.ExcluidoDelFrustumCulling()){
                        if (distancia < 300.0f || entidad.ExcluidoDelFrustumCulling()){
                            _entidadesShadowMap.Add(entidad);
                        }
                    }    
                }
            
        }

        private void setVistaProjection(Moldes.IMolde molde, Matrix vista, Matrix projection)
        {
            molde.setProjection(projection);
            molde.setVista(vista);
            molde.setCamara(_camera.Position);
            molde.SetPosSOL(_posSOL); //sol NO AFECTA AL TERRENO
        }

        public void ActualizarAnimacion(GameTime gameTime)
        {
            foreach (var molde in _moldes)
            {
                molde.setTime(gameTime);
            }
            this._particulas.Update(gameTime);
            this.ActualizarEntidadesShadowMap(gameTime);
        }

        public void ActualizarCamera()
        {
            _boundingFrustum.UpdateFrustum(_camera);
        }

        public void AgregarTanqueDestruido(Entidad entidad){
            this._particulas.AgregarTanqueDestruido(entidad._posicion);
        }

    }
}