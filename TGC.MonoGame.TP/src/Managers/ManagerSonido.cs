using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es el manager que hace el flujo de sonido
    /// </summary>
    public class ManagerSonido
    {
        // Musica de fondo
        private SoundEffectInstance _sonidoFondoAccionInstance;
        private SoundEffectInstance _sonidoFondoSuspensoInstance;
        private SoundEffectInstance _sonidoFondoMenuInstance;

        // Sonidos menu
        private SoundEffectInstance _sonidoMenuSeleccionInstance;
        private SoundEffectInstance _sonidoMenuCambioBotonInstance;
        private SoundEffectInstance _sonidoMenuCambioTanqueInstance;
        //Sonidos de Victoria
        private SoundEffectInstance _sonidoVictoriaInstance;
        //Sonidos de Derrota
        private SoundEffectInstance _sonidoDerrotaInstance;

        // Sonido de tanque
        private SoundEffectInstance _sonidoDisparoInstance;
        private SoundEffectInstance _sonidoMovimientoInstance;
        private SoundEffectInstance _sonidoDetenidoInstance;
        private SoundEffectInstance _sonidoImpactoBalaInstance;
        private SoundEffectInstance _sonidoColicionInstance;
        private SoundEffectInstance _sonidoMuerteInstance;
        // Volumen maestro para todos los sonidos TODO: Separar en volumen musica y efectos de sonido
        float _volumenMaestro = 0.1f;

        ContentManager _contentManager;
        public ManagerSonido(ContentManager Content)
        {
            this._contentManager = Content;
        }

        public void InstanciarSonidosTanque()
        {
            SoundEffect sonidoDetenido = _contentManager.Load<SoundEffect>(@"Sounds/tankStop");
            this._sonidoDetenidoInstance = sonidoDetenido.CreateInstance();
            _sonidoDetenidoInstance.IsLooped = true;
            _sonidoDetenidoInstance.Volume = _volumenMaestro / 6;

            SoundEffect sonidoMovimiento = _contentManager.Load<SoundEffect>(@"Sounds/tankMove");
            this._sonidoMovimientoInstance = sonidoMovimiento.CreateInstance();
            _sonidoMovimientoInstance.IsLooped = true;
            _sonidoMovimientoInstance.Volume = _volumenMaestro / 4;

            SoundEffect sonidoDisparo = _contentManager.Load<SoundEffect>(@"Sounds/disparo2");
            this._sonidoDisparoInstance = sonidoDisparo.CreateInstance();
            _sonidoDisparoInstance.IsLooped = false;
            _sonidoDisparoInstance.Volume = _volumenMaestro * 2;
            _sonidoDisparoInstance.Pitch = 0.0f;

            SoundEffect sonidoImpactoBala = _contentManager.Load<SoundEffect>(@"Sounds/explocion2");
            this._sonidoImpactoBalaInstance = sonidoImpactoBala.CreateInstance();
            _sonidoImpactoBalaInstance.IsLooped = false;
            _sonidoImpactoBalaInstance.Volume = _volumenMaestro * 3;
            _sonidoImpactoBalaInstance.Pitch = 0.0f;

            SoundEffect sonidoColision = _contentManager.Load<SoundEffect>(@"Sounds/explocion");//explosion es lo suficientemente parecido
            this._sonidoColicionInstance = sonidoColision.CreateInstance();
            _sonidoColicionInstance.IsLooped = false;
            _sonidoColicionInstance.Volume = _volumenMaestro;

            SoundEffect sonidoMuerte = _contentManager.Load<SoundEffect>(@"Sounds/explocion3");
            this._sonidoMuerteInstance = sonidoMuerte.CreateInstance();
            _sonidoMuerteInstance.IsLooped = false;
            _sonidoMuerteInstance.Volume = _volumenMaestro * 4;

        }

        public void reproducirSonido(string sonido)
        {
            switch (sonido)
            {
                case "disparo":
                    if (_sonidoDisparoInstance.State == SoundState.Playing)
                    {
                        _sonidoDisparoInstance.Stop();
                    }
                    _sonidoDisparoInstance.Pitch = new Random().Next(-80, 0) / 100.0f; // Randomiza el pitch del sonido de disparo
                    _sonidoDisparoInstance.Play();
                    break;
                case "movimiento":
                    _sonidoMovimientoInstance.Play();
                    break;
                case "detenido":
                    _sonidoDetenidoInstance.Play();
                    break;
                case "impactoBala":
                    if (_sonidoImpactoBalaInstance.State != SoundState.Playing)
                    {
                        _sonidoImpactoBalaInstance.Pitch = new Random().Next(-90, 100) / 100.0f; // Randomiza el pitch del sonido de impacto de bala
                        _sonidoImpactoBalaInstance.Play();
                    }
                    break;
                case "colision":
                    if (_sonidoColicionInstance.State == SoundState.Playing)
                    {
                        _sonidoColicionInstance.Stop();
                    }
                    else
                    {
                        _sonidoColicionInstance.Play();
                    }
                    break;
                case "muerte":
                    _sonidoMuerteInstance.Play();
                    break;
            }
        }

        public void sonidoMovimiento(bool moviendose)
        {
            if (moviendose)
            {
                if (_sonidoMovimientoInstance.State != SoundState.Playing)
                {
                    _sonidoMovimientoInstance.Play();
                }
            }
            else
            {
                if (_sonidoMovimientoInstance.State == SoundState.Playing)
                {
                    _sonidoMovimientoInstance.Stop();
                }
            }
        }

        public void sonidoDetenido(bool moviendose)
        {
            if (!moviendose)
            {
                if (_sonidoDetenidoInstance.State != SoundState.Playing)
                {
                    _sonidoDetenidoInstance.Play();
                }
            }
            else
            {
                if (_sonidoDetenidoInstance.State == SoundState.Playing)
                {
                    _sonidoDetenidoInstance.Stop();
                }
            }
        }

        public void InstanciarMusica()
        {
            SoundEffect sonidoFondoAccion = _contentManager.Load<SoundEffect>(@"Music/epico");
            this._sonidoFondoAccionInstance = sonidoFondoAccion.CreateInstance();
            _sonidoFondoAccionInstance.IsLooped = true;
            _sonidoFondoAccionInstance.Volume = _volumenMaestro * 2;

            SoundEffect sonidoFondoSuspenso = _contentManager.Load<SoundEffect>(@"Music/musicaEpicaTranquila");
            this._sonidoFondoSuspensoInstance = sonidoFondoSuspenso.CreateInstance();
            _sonidoFondoSuspensoInstance.IsLooped = true;
            _sonidoFondoSuspensoInstance.Volume = _volumenMaestro * 2;

            SoundEffect sonidoFondoMenu = _contentManager.Load<SoundEffect>(@"Music/musicaTranquila");
            this._sonidoFondoMenuInstance = sonidoFondoMenu.CreateInstance();
            _sonidoFondoMenuInstance.IsLooped = true;
            _sonidoFondoMenuInstance.Volume = _volumenMaestro;
        }

        public void reproducirMusica(string musica)
        {
            switch (musica)
            {
                case "accion":
                    reproducirMusica(_sonidoFondoAccionInstance);
                    break;
                case "suspenso":
                    reproducirMusica(_sonidoFondoSuspensoInstance);
                    break;
                case "menu":
                    reproducirMusica(_sonidoFondoMenuInstance);
                    break;
            }
        }

        public void InstanciarSonidosMenu()
        {
            SoundEffect sonidoMenuSeleccion = _contentManager.Load<SoundEffect>(@"Sounds/sonidoArcade");
            this._sonidoMenuSeleccionInstance = sonidoMenuSeleccion.CreateInstance();
            _sonidoMenuSeleccionInstance.IsLooped = false;
            _sonidoMenuSeleccionInstance.Volume = _volumenMaestro * 3f;

            SoundEffect sonidoMenuCambioBoton = _contentManager.Load<SoundEffect>(@"Sounds/clickModerno");
            this._sonidoMenuCambioBotonInstance = sonidoMenuCambioBoton.CreateInstance();
            _sonidoMenuCambioBotonInstance.IsLooped = false;
            _sonidoMenuCambioBotonInstance.Volume = _volumenMaestro * 3f;

            SoundEffect sonidoMenuCambioTanque = _contentManager.Load<SoundEffect>(@"Sounds/otroClickModerno");
            this._sonidoMenuCambioTanqueInstance = sonidoMenuCambioTanque.CreateInstance();
            _sonidoMenuCambioTanqueInstance.IsLooped = false;
            _sonidoMenuCambioTanqueInstance.Volume = _volumenMaestro * 3f;

            SoundEffect sonidoVictoria = _contentManager.Load<SoundEffect>(@"Sounds/victoria");
            this._sonidoVictoriaInstance = sonidoVictoria.CreateInstance();
            _sonidoVictoriaInstance.IsLooped = false;
            _sonidoVictoriaInstance.Volume = _volumenMaestro * 3f;

            SoundEffect sonidoDerrota = _contentManager.Load<SoundEffect>(@"Sounds/sonidoInquietante");
            this._sonidoDerrotaInstance = sonidoDerrota.CreateInstance();
            _sonidoDerrotaInstance.IsLooped = false;
            _sonidoDerrotaInstance.Volume = _volumenMaestro * 3f;
        }

        public void ReproducirSonidoMenu(string sonido)
        {
            switch (sonido)
            {
                case "seleccion":
                    this.reproducirSonido(_sonidoMenuSeleccionInstance);
                    break;
                case "cambioBoton":
                    this.reproducirSonido(_sonidoMenuCambioBotonInstance);
                    break;
                case "cambioTanque":
                    this.reproducirSonido(_sonidoMenuCambioTanqueInstance);
                    break;

                case "victoria":
                    this.reproducirSonido(_sonidoVictoriaInstance);
                    break;
                case "derrota":
                    this.reproducirSonido(_sonidoDerrotaInstance);
                    break;
            }
        }
        private void stopMusica(){
            _sonidoFondoAccionInstance.Stop();
            _sonidoFondoSuspensoInstance.Stop();
            _sonidoFondoMenuInstance.Stop();
        }
        private void reproducirMusica(SoundEffectInstance musica){
            if (musica.State != SoundState.Playing)
            {
                this.stopMusica();
                musica.Play();
            }
        }

        private void reproducirSonido(SoundEffectInstance instanciaSonido) {
            if (instanciaSonido.State == SoundState.Playing) instanciaSonido.Stop();
            instanciaSonido.Play();
        }

    }
}