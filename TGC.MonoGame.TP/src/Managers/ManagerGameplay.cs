using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Entidades;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es el manajer que hace el flujo de colisiones
    /// </summary>
    public class ManagerGameplay
    {
        private List<Etanque> _enemigos;
        private Etanque _player;
         private List<EBala> _balas;
        public ManagerGameplay()
        {
            _enemigos = new List<Etanque>();
            _player = new Etanque();
        }

        public void AgregarEnemigo(Etanque entidad)
        {
           _enemigos.Add( entidad );
        }

        public void RemoverEnemigo(Entidad entidad) //TODO - Revisar si intenta borrar algo que no existe ROMPE o no.
        {
            if(entidad._tipo == TipoEntidad.Tanque)
            _enemigos.Remove((Etanque)entidad);
        }

        public void AgregarJugador(Etanque entidad)
        {
           _player = entidad;
        }

        public void RemoverJugador(Entidad entidad) //TODO - Revisar si intenta borrar algo que no existe ROMPE o no.
        {
            //TODO: no se, algo  hará
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            foreach (Etanque tanque in _enemigos)
            {
                tanque.Update(gameTime);
            }
        }

    }
}