using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.BoundingsVolumes;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es el manajer que hace el flujo de colisiones
    /// </summary>
    public class ManagerColisiones 
    {
        private List<Entidades.Entidad> _movimientos;
        private List<Entidades.Entidad> _estaticas;
        public ManagerColisiones()
        {
            _movimientos = new List<Entidades.Entidad>();
            _estaticas=  new List<Entidades.Entidad>();
        }

        public void AgregarEntidad(Entidades.Entidad entidad)
        {
            if (entidad.PuedeChocar())
                _movimientos.Add(entidad);
            else if (entidad.PuedeSerChocado())
                _estaticas.Add(entidad);
        }

        public void RemoverEntidad(Entidades.Entidad entidad) //TODO - Revisar si intenta borrar algo que no existe ROMPE o no.
        {
            _movimientos.Remove(entidad);
            _estaticas.Remove(entidad);
        }

        public void VerificarColisiones(){
            
            foreach (Entidades.Entidad EntidadMovimeinto in _movimientos)
            {
                foreach (Entidades.Entidad EntidadEstatica in _estaticas)
                {
                    if(CalculadorasChoque.DetectarColisiones(EntidadMovimeinto._boundingVolume, EntidadEstatica._boundingVolume))
                    {
                        EntidadMovimeinto.Chocar(CalculadorasChoque.ParametrosChoque(EntidadMovimeinto._boundingVolume, EntidadEstatica._boundingVolume), EntidadEstatica);
                        EntidadEstatica.Chocar(CalculadorasChoque.ParametrosChoque(EntidadEstatica._boundingVolume, EntidadMovimeinto._boundingVolume), EntidadMovimeinto);
                    }   
                }
                foreach (Entidades.Entidad EntidadMovimeinto2 in _movimientos)
                {
                    if(CalculadorasChoque.DetectarColisiones(EntidadMovimeinto._boundingVolume, EntidadMovimeinto2._boundingVolume) && EntidadMovimeinto2 != EntidadMovimeinto)
                    {
                        EntidadMovimeinto.Chocar(CalculadorasChoque.ParametrosChoque(EntidadMovimeinto._boundingVolume, EntidadMovimeinto2._boundingVolume), EntidadMovimeinto2);
                    }   
                }

            }
            
        }

    }
}