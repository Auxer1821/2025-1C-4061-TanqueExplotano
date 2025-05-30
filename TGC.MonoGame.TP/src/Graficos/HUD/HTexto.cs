using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.HUD
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class HTexto: IHUD
    {
        private string _texto;
        private Vector2 _coordenadas;
        private int _tamanioLetra;


        //----------------------------------------------Variables--------------------------------------------------//

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public HTexto()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector2 coordenadas, string texto)
        {
            this._texto = texto;
            this._coordenadas = coordenadas;
            this._tamanioLetra = 5;
        }
        public void Initialize(Vector2 coordenadas)
        {
            this._texto = null;
            this._coordenadas = coordenadas;
            this._tamanioLetra = 5;
        }


        //----------------------------------------------Funciones-Principales--------------------------------------------------//


        //----------------------------------------------Dibujado--------------------------------------------------//
        public void Dibujado(GraphicsDevice Graphics, Effect efecto, IndexBuffer indices, VertexBuffer vertices)
        {
            //matix mundo vista y proyeccion lo setea el mana
            int i = 0;

            foreach (char letra in _texto) //La imagen de los caracteres tiene TODAS LAS LETRAS seguido de TODOS LOS NUMEROS seguido de SIMBOLOS
            {   // no Ñ
                i++;
                int min = 0;
                int max = 0;

                if (char.IsLetter(letra))
                {
                    char enMayuscula = char.ToUpper(letra);
                    min = (enMayuscula - 'A') * _tamanioLetra;
                    max = (enMayuscula - 'A' + 1) * _tamanioLetra;
                }

                else if (char.IsNumber(letra))
                {
                    min = (letra - '0' + 26) * _tamanioLetra;
                    max = (letra - '0' + 26 + 1) * _tamanioLetra;
                }

                else  // signo
                {
                    if (letra == '/')
                    {
                        min = (letra - '0' + 26 + 10) * _tamanioLetra;
                        max = (letra - '0' + 26 + 10 + 1) * _tamanioLetra;
                    }
                    else if (letra == ':')
                    {
                        min = (letra - '0' + 26 + 10 + 1) * _tamanioLetra;
                        max = (letra - '0' + 26 + 10 + 2) * _tamanioLetra;
                    }
                    else if (letra == ' ')
                    {
                        min = (letra - '0' + 26 + 10 + 2) * _tamanioLetra;
                        max = (letra - '0' + 26 + 10 + +3) * _tamanioLetra;

                    }
                    else i--;
                }

                efecto.Parameters["Coordenadas"].SetValue(_coordenadas + Vector2.UnitX * i * (_tamanioLetra + 2));//COMO MATRIX MUNDO
                efecto.Parameters["Minimo"].SetValue(min);
                efecto.Parameters["Maximo"].SetValue(max);

                Graphics.SetVertexBuffer(vertices);
                Graphics.Indices = indices;
                foreach (var pass in efecto.CurrentTechnique.Passes){
                    pass.Apply();
                    Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.IndexCount);
                }

            }

        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public void setValor(string valor){
            this._texto = valor;
        }


        
    }
}