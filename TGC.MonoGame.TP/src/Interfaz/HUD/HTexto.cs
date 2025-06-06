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
    public class HTexto
    {
        /// <summary>
        /// Usás _tamanioLetra + 2 como espaciado. Esto puede dejar texto desalineado. Considerá usar un float Espaciado = 1f; o escalar con resolución para adaptarlo a distintos tamaños de pantalla.
        /// </summary
        private string _texto;
        private Vector2 _coordenadas;
        private float _tamanioLetra;


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
            this._tamanioLetra = 63f;
        }
        public void Initialize(Vector2 coordenadas)
        {
            this._texto = null;
            this._coordenadas = coordenadas;
            this._tamanioLetra = 63f;
        }


        //----------------------------------------------Funciones-Principales--------------------------------------------------//


        //----------------------------------------------Dibujado--------------------------------------------------//
        public void Dibujado(GraphicsDevice Graphics, Effect efecto, IndexBuffer indices, VertexBuffer vertices)
        {

            //modificar cordenadas
            int i = 0;

            foreach (char letra in _texto)
            {
                if (!TryGetCharIndex(letra, out int indice))
                {
                    i++;
                    continue; // saltea caracteres no dibujables
                }

                float min = indice * _tamanioLetra;
                float max = (indice + 1) * _tamanioLetra;

                efecto.Parameters["Coordenadas"].SetValue(_coordenadas + Vector2.UnitX * ( i * 0.05f ) );
                efecto.Parameters["Minimo"].SetValue(min);
                efecto.Parameters["Maximo"].SetValue(max);

                Graphics.SetVertexBuffer(vertices);
                Graphics.Indices = indices;

                foreach (var pass in efecto.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.IndexCount);
                }

                i++;
            }


        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public void setValor(string valor){
            this._texto = valor;
        }





        private bool TryGetCharIndex(char c, out int index)
        {
            index = 0;
            
            if (char.IsLetter(c))
            {
                index = char.ToUpper(c) - 'A';
                return true;
            }

            if (char.IsDigit(c))
            {
                index = (c - '0') + 26;
                return true;
            }

            switch (c)
            {
                case '/':
                    index = 36; // después de letras (26) + números (10)
                    return true;
                case ':':
                    index = 37;
                    return true;
                case ' ':
                    index = 38;
                    return true;
                default:
                    return false; // no se puede dibujar
            }
        }


        



        
    }
}