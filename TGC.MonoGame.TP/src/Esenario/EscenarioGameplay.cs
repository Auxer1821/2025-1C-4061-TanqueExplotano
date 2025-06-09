using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Entidades;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.Managers;
using TGC.MonoGame.TP.src.Moldes;


namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla los managers
    /// </summary>
    public class Escenario : IEscenario
    {

        // Variables
        private Terrenos.Terreno _terreno;
        private ManagerGrafico _managerGrafico;
        private ManagerColisiones _managerColision;
        private ManagerGameplay _managerGameplay;
        private ManagerInterfaz _managerInterfaz;
        private List<Entidad> _entidadesEliminar;
        private bool _faltaEliminar;
        private List<Entidad> _entidadesCrear;
        private bool _faltaCrear;
        private List<IMolde> _moldes;
        private DirectorEscenarios _director;


        private Cameras.FreeCamera _camara;
        private EJugador jugador;
        //TODO: Que sea el primero en ser dibujado
        //private Entidades.ESkyBox _skyBox; //TODO -> Actualizar en el manager Graficos para que se dibujen primeros / ultimos
        //private Entidades.EPasto[] pastos = new Entidades.EPasto[1000];

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Escenario()
        {
            _managerGrafico = new ManagerGrafico();
            _managerColision = new ManagerColisiones();
            _managerGameplay = new ManagerGameplay();
            _managerInterfaz = new ManagerInterfaz();
            _entidadesEliminar = new List<Entidad>();
            _entidadesCrear = new List<Entidad>();
            _moldes = new List<IMolde>();
            _faltaEliminar = false;
            _faltaCrear = false;
        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, ContentManager content, DirectorEscenarios directorEscenarios)
        {
            this._director = directorEscenarios;
            var screenSize = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            _camara = new Cameras.FreeCamera(graphicsDevice.Viewport.AspectRatio, Vector3.UnitZ * 150, screenSize);
            Matrix view = _camara.Vista;
            Matrix projection = _camara.Proyeccion;
            _managerGrafico.inicializarCamara(_camara);

            //-----------------Inicializar el skybox-------------------//
            Entidades.ESkyBox skyBox = new ESkyBox();
            skyBox.Initialize(graphicsDevice, world, content, this);
            //this.AgregarACrear(skyBox);
            _managerGrafico.inicializarSkyBox(skyBox);

            //-----------------Inicializar terreno--------------------//
            _terreno = new Terrenos.Terreno();
            _terreno.Initialize(graphicsDevice, world, content);
            _managerGrafico.inicializarTerreno(_terreno);

            //-----------------Inicializar moldes---------------------//
            MoldeCasa moldeCasa = new MoldeCasa(content);
            MoldeCaja moldeCaja = new MoldeCaja(content, graphicsDevice);
            MoldeArbol moldeArbol = new MoldeArbol(content);
            MoldeRoca moldeRoca = new MoldeRoca(content);
            MoldeMontana moldeMontana = new MoldeMontana(content, graphicsDevice);
            MoldePasto moldePasto = new MoldePasto(content, graphicsDevice);
            this._moldes.Add(moldeCasa);
            this._moldes.Add(moldeCaja);
            this._moldes.Add(moldeArbol);
            this._moldes.Add(moldeRoca);
            this._moldes.Add(moldeMontana);
            this._moldes.Add(moldePasto);
            _managerGrafico.InicializarMoldes(_moldes);

            //-----------------Posiciones usadas----------------------//
            List<Vector3> posicionesUsadas = new List<Vector3>();

            //-------Crear un pequeño pueblo (casas y cajas)-----------//

            for (int x = -100; x <= 100; x += 40)
            {
                for (int z = -100; z <= 100; z += 30)
                {
                    var casa = new ECasa();
                    casa.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x, z), z), content, this, new Vector3(x, _terreno.GetHeightAt(x, z), z));
                    casa.SetMolde(moldeCasa);/*SEGUIR DESDE AQUI*/
                    this.AgregarACrear(casa);
                    posicionesUsadas.Add(new Vector3(x, z, 4));

                    var caja = new ECaja();
                    caja.Initialize(graphicsDevice, world * Matrix.CreateTranslation(x + 8, _terreno.GetHeightAt(x, z), z + 8), content, this);
                    caja.SetMolde(moldeCaja);
                    this.AgregarACrear(caja);
                    posicionesUsadas.Add(new Vector3(x + 8, z + 8, 2));
                }
            }

            //--------Crear un bosque (árboles)---------------//
            Random random = new Random(0);
            for (int i = 0; i < 500; i++)
            {
                var arbol = new EArbol();
                float x = random.Next(-300, 300);
                float z;
                if (random.Next(2) == 0) // 50% de probabilidad para cada zona
                    z = random.Next(-500, -200); // Zona sur (z negativa)
                else
                    z = random.Next(200, 500); // Zona norte (z positiva)
                float rotacion = random.Next(0, 360);
                float tamano = random.Next(10, 20) / 10;
                var pos = new Vector2(x, z);
                if (PosicionesLibre(pos, posicionesUsadas, 1))
                {
                    arbol.Initialize(graphicsDevice, Matrix.CreateScale(tamano) * Matrix.CreateRotationY(MathHelper.ToRadians(rotacion)) * world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x, z), z), content, this);
                    arbol.SetMolde(moldeArbol);
                    this.AgregarACrear(arbol);
                    posicionesUsadas.Add(new Vector3(x, z, 1));
                }
                else
                {
                    i--;
                }
            }

            //-------Crear algunas rocas dispersas----------//
            for (int i = 0; i < 120; i++)
            {
                var roca = new ERoca();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                float rotacion = random.Next(0, 360);
                float tamano = random.Next(10, 20) / 10;
                var pos = new Vector2(x, z);
                if (PosicionesLibre(pos, posicionesUsadas, 1))
                {
                    roca.Initialize(graphicsDevice, Matrix.CreateScale(tamano) * Matrix.CreateRotationY(MathHelper.ToRadians(rotacion)) * world * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x, z), z), content, this);
                    roca.SetMolde(moldeRoca);
                    this.AgregarACrear(roca);
                    posicionesUsadas.Add(new Vector3(x, z, 1));
                }
                else
                {
                    i--;
                }
            }


            //---------Crear Coordillera--------//
            for (int i = 0; i < 5; i++)
            {
                //IZQUIERDA
                var montana = new EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(-400, 0, -400 + 200 * i), content, this);
                montana.SetMolde(moldeMontana);
                this.AgregarACrear(montana);
                //DERECHA
                montana = new EMontana();
                montana.Initialize(graphicsDevice, world * Matrix.CreateTranslation(400, 0, -400 + 200 * i), content, this);
                montana.SetMolde(moldeMontana);
                this.AgregarACrear(montana);
            }

            //-------------------Crear tanks--------------------//
            //---Jugador---//
            jugador = new EJugador();
            float Jx = random.Next(-100, 100);
            float Jz = random.Next(-100, 100);
            var Jpos = new Vector2(Jx, Jz);

            while (!PosicionesLibre(Jpos, posicionesUsadas, 10))    //ENCONTRAR UNA POS LIBRE
            {
                Jx = random.Next(-100, 100);
                Jz = random.Next(-100, 100);
                Jpos = new Vector2(Jx, Jz);
            }

            jugador.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Jx, 0f, Jz), content, this);
            jugador.setCamara(_camara);
            this.AgregarACrear(jugador);
            this._managerGameplay.AgregarJugador(jugador);
            this._managerInterfaz.Inicializar(graphicsDevice, content, jugador);
            posicionesUsadas.Add(new Vector3(Jx, Jz, 10));

            //----IA---//

            for (int i = 0; i < 5; i++)
            {
                var tank = new ETanqueIA();
                tank.SetTipoTanque("Panzer");
                float Ax = random.Next(-150, 150);
                //float Az = random.Next(-150, 150);
                float Az;
                if (random.Next(2) == 0) // 50% de probabilidad para cada zona
                    Az = random.Next(-500, -150); // Zona sur (z negativa)
                else
                    Az = random.Next(150, 500); // Zona norte (z positiva)

                var pos = new Vector2(Ax, Az);
                if (PosicionesLibre(pos, posicionesUsadas, 10))
                {
                    tank.Initialize(graphicsDevice, world * Matrix.CreateTranslation(Ax, _terreno.GetHeightAt(Ax, Az), Az), content, this);
                    this.AgregarACrear(tank);
                    this._managerGameplay.AgregarEnemigo(tank);
                    posicionesUsadas.Add(new Vector3(Ax, Az, 10));
                }
                else
                {
                    i--;
                }
            }
            //------Crear pasto--------------------//
            for (int i = 0; i < 1000; i++)
            {
                var pasto = new Entidades.EPasto();
                float x = random.Next(-300, 300);
                float z = random.Next(-300, 300);
                float rotacion = random.Next(0, 360);
                float tamano = random.Next(10, 20) / 10;
                var pos = new Vector2(x, z);
                if (PosicionesLibre(pos, posicionesUsadas, 1))
                {
                    pasto.Initialize(graphicsDevice, Matrix.CreateScale(tamano) * Matrix.CreateRotationY(MathHelper.ToRadians(rotacion)) * world * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(x, _terreno.GetHeightAt(x, z) + 1f, z), content, this);
                    pasto.SetMolde(moldePasto);
                    this._managerGrafico.AgregarPasto(pasto);
                    posicionesUsadas.Add(new Vector3(x, z, 1));
                }
                else
                {
                    i--;
                }

            }

        }
        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            _managerGrafico.DibujarObjetos(graphicsDevice);


            _managerInterfaz.Dibujar();
        }

        public void ActualizarCamara(GameTime gameTime)
        {
            _camara.Update(gameTime);
        }

        public void AgregarAEliminar(Entidad entidad)
        {
            this._entidadesEliminar.Add(entidad);
            this._faltaEliminar = true;
        }

        public void AgregarACrear(Entidad entidad)
        {
            this._entidadesCrear.Add(entidad);
            this._faltaCrear = true;
        }

        public void Update(GameTime gameTime)
        {

            _managerGameplay.Update(gameTime);
            _managerColision.VerificarColisiones();
            _managerInterfaz.Update();
            this.ActualizarCamara(gameTime);
            _managerGrafico.ActualizarCamera();
            _managerGrafico.ActualizarAnimacion(gameTime);//actualiza todos los moldes
            if (_faltaEliminar) this.EliminarEntidades();
            if (_faltaCrear) this.CrearEntidades();

        }


        //----------------------------------Funciones-auxiliares--------------------------------------------//
        private bool PosicionesLibre(Vector2 newPos, List<Vector3> usadas, float distancia)
        {
            foreach (var pos in usadas)
            {
                if (Vector2.Distance(new Vector2(pos.X, pos.Y), newPos) < (distancia + pos.Z))
                    return false;
            }
            return true;
        }

        internal void EliminarEntidades()
        {
            foreach (var entidad in _entidadesEliminar)
            {
                this._managerGrafico.RemoverEntidad(entidad);
                this._managerColision.RemoverEntidad(entidad);
                this._managerGameplay.RemoverEntidad(entidad);
            }
            this._entidadesEliminar.Clear();
            this._faltaEliminar = false;
        }

        internal void CrearEntidades()
        {
            foreach (var entidad in _entidadesCrear)
            {
                this._managerGrafico.AgregarEntidad(entidad);
                this._managerColision.AgregarEntidad(entidad);
                this._managerGameplay.AgregarEntidad(entidad);
            }
            this._entidadesCrear.Clear();
            this._faltaCrear = false;
        }

        internal float getAltura(Vector3 pos)
        {
            return _terreno.GetHeightAt(pos.X, pos.Z);
        }


        public Vector3 getNormal(Vector2 pos1, Vector2 pos2, Vector2 pos3)
        {
            return this._terreno.getNormal(pos1, pos2, pos3);
        }
        public float getAltura(Vector2 pos1, Vector2 pos2, Vector2 pos3)
        {
            return this._terreno.getAltura(pos1, pos2, pos3);
        }

        public void SetSkinTanque(string skin)
        {
            this.jugador.SetSkinTanque(skin);
        }

        public void FinJuegoGanar()
        {
            this._director.CambiarEsenarioActivo(TipoEsenario.Victoria);
        }
        public void FinJuegoPerder()
        {
            this._director.CambiarEsenarioActivo(TipoEsenario.Derrota);
        }
    }

}
