using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Utilities
{
    //       Autor: L.I. Eden Verdugo
    //       Fecha: 2018/08/10    
    // Comentarios: Me base en la libreria de CEncriptar para hacer esta clase, fue una batalla convertir el cochinero de vb al hermoso c#, ni los traductores de codigo le entendian al desmadre aquel.
    //              Se simplifico la función.
    /// <summary>
    /// Se encarga de encriptar textos, contiene los mismos metodos que CEncriptar
    /// </summary>
    public class Encrypter
    {
        private readonly string userKey;

        public enum Mode
        {
            Encrypt,
            Decrypt
        }
        /// <summary>
        /// Instancia de Encrypter para encriptar textos, contiene los mismos metodos que CEncriptar.
        /// </summary>
        /// <param name="userKey">Es la semilla para encriptar y desencriptar el texto.</param>
        public Encrypter(string userKey)
        {
            this.userKey = userKey;
        }

        /// <summary>
        /// Encripta el texto. 
        /// </summary>
        /// <param name="text">Texto a encriptar.</param>
        /// <returns>Devuelve el texto encriptado.</returns>
        public string Encrypt(string text)
        {
            return EncryptString(this.userKey, text, Mode.Encrypt);
        }

        /// <summary>
        /// Desencripta el texto.
        /// </summary>
        /// <param name="text">Texto a desencriptar.</param>
        /// <returns>Devuelve el texto desencriptado.</returns>
        public string Decrypt(string text)
        {
            return EncryptString(this.userKey, text, Mode.Decrypt);
        }

        /// <summary>
        /// Se encarga de encriptar o desencriptar el texto.
        /// </summary>
        /// <param name="userKey">Semilla para la encriptación o desencriptación.</param>
        /// <param name="text">Texto a encriptar o desencriptar.</param>
        /// <param name="mode">Encritar o Desencriptar</param>
        /// <returns>Devuelve el texto encriptado o desencriptado.</returns>
        public static string EncryptString(string userKey, string text, Mode mode)
        {
            int temp = 0;
            int[] userKeyASCIIS = new int[Strings.Len(userKey) + 1];

            var resultado = "";

            for (int i = 1; i < userKeyASCIIS.Length; i++)
            {
                userKeyASCIIS[i] = Strings.Asc(Strings.Mid(userKey, i, 1));
            }

            int[] textASCIIS = new int[Strings.Len(text) + 1];

            for (int i = 1; i < textASCIIS.Length; i++)
            {
                textASCIIS[i] = Strings.Asc(Strings.Mid(text, i, 1));
            }

            var j = 0;
            var lt = Strings.Len(text) + 1;
            var n = Strings.Len(userKey);

            switch (mode)
            {
                case Mode.Encrypt:

                    for (int i = 1; i < lt; i++)
                    {
                        j = (j + 1) >= n ? 1 : j + 1;

                        temp = textASCIIS[i] + userKeyASCIIS[j];

                        if (temp > 255)
                        {
                            temp -= 255;
                        }

                        resultado += Strings.Chr(temp);
                    }

                    break;
                case Mode.Decrypt:

                    for (int i = 1; i < lt; i++)
                    {
                        j = (j + 1) >= n ? 1 : j + 1;

                        temp = textASCIIS[i] - userKeyASCIIS[j];

                        if (temp < 0)
                            temp = temp + 255;

                        resultado += Strings.Chr(temp);
                    }

                    break;
            }

            return resultado;
        }
    }
}
