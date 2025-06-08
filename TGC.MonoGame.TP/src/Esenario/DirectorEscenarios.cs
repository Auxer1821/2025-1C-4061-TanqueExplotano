using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.src.Managers;


namespace TGC.MonoGame.TP.src.Escenarios
{
    public enum TipoEsenario
    {
        Gameplay,
        Menu,
        Victoria,
        Derrota
    }
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public class DirectorEscenarios
    {
        ///-----------Variables-------------//
        private Escenario _escenarioGameplay;
        private EscenarioMenu _escenarioMenu;
        private EscenarioVictoria _escenarioVictoria;
        private EscenarioDerrota _escenarioDerrota;

        private IEscenario _esenarioActivo;
        private GraphicsDevice _graphicsDevice;
        private Matrix _world;
        private ContentManager _content;
        private ManagerSonido _managerSonido;
        private string tipoMusica = "menu";

        private TGCGame _game;

        public DirectorEscenarios()
        {

        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, ContentManager content, TGCGame game)
        {
            _graphicsDevice = graphicsDevice;
            _world = world;
            _content = content;
            _game = game;
            _escenarioGameplay = new Escenario();
            _escenarioGameplay.Initialize(_graphicsDevice, _world, _content);

            _escenarioMenu = new EscenarioMenu();
            _escenarioMenu.Initialize(graphicsDevice, content, this);


            _escenarioVictoria = new EscenarioVictoria();
            _escenarioVictoria.Initialize(graphicsDevice, content, this);

            _escenarioDerrota = new EscenarioDerrota();
            _escenarioDerrota.Initialize(graphicsDevice, content, this);

            _esenarioActivo = _escenarioMenu; //TODO: cambiar para que sea el menu

            _managerSonido = new ManagerSonido(content);
            _managerSonido.InstanciarMusica();
            _managerSonido.InstanciarSonidosMenu();
        }

        public void Dibujar()
        {

            //logica de dibujado del escenario
            _esenarioActivo.Dibujar(this._graphicsDevice);

        }

        public void Update(GameTime gameTime)
        {
            _esenarioActivo.Update(gameTime);
            this._managerSonido.reproducirMusica(tipoMusica);

        }
        public void CambiarEsenarioActivo(TipoEsenario esenario)
        {
            switch (esenario)
            {
                case TipoEsenario.Gameplay:
                    _esenarioActivo = this._escenarioGameplay;
                    
                    this.tipoMusica = "accion";
                    break;
                case TipoEsenario.Victoria:
                    _esenarioActivo = this._escenarioVictoria;
                    this._managerSonido.ReproducirSonidoMenu("victoria");
                    this.tipoMusica = "accion";
                    break;
                case TipoEsenario.Derrota:
                    _esenarioActivo = this._escenarioDerrota;
                    this._managerSonido.ReproducirSonidoMenu("derrota");
                    this.tipoMusica = "suspenso";
                    break;
                case TipoEsenario.Menu:
                    _esenarioActivo = this._escenarioMenu;
                    this.tipoMusica = "menu";
                    break;
                default:
                    _esenarioActivo = this._escenarioMenu;
                    this.tipoMusica = "menu";
                    break;

            }
        }

        public void SetSkinTanque(string skin)
        {
            _escenarioGameplay.SetSkinTanque(skin);
        }
        
        public TGCGame GetGame()
        {
            return _game;
        }

    }
}
