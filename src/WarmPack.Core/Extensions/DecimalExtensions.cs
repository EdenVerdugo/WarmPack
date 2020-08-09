namespace WarmPack.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToLetras(this decimal val, bool asMoney = false)
        {
            long numeroEntero = (long)val / 1;
            int decimales = int.Parse(val.ToString("0.00").Split('.')[1]);

            string resultado = ToLetras(numeroEntero) + (asMoney ? string.Format(" pesos {0}/100 M.N.", decimales.ToString("00")) : "");

            if (asMoney)
            {
                if (numeroEntero > 1)
                {
                    resultado = resultado.Replace("uno pesos", "un pesos");
                }
                else
                {
                    resultado = resultado.Replace("uno pesos", "un peso");
                }
            }

            return resultado;
        }

        public static string ToLetras(this long val)
        {
            string Resultado = "";
            if (val >= 0 && val < 16)
            {
                switch (val)
                {
                    case 0: Resultado = "cero"; break;
                    case 1: Resultado = "uno"; break;
                    case 2: Resultado = "dos"; break;
                    case 3: Resultado = "tres"; break;
                    case 4: Resultado = "cuatro"; break;
                    case 5: Resultado = "cinco"; break;
                    case 6: Resultado = "seis"; break;
                    case 7: Resultado = "siete"; break;
                    case 8: Resultado = "ocho"; break;
                    case 9: Resultado = "nueve"; break;
                    case 10: Resultado = "diez"; break;
                    case 11: Resultado = "once"; break;
                    case 12: Resultado = "doce"; break;
                    case 13: Resultado = "trece"; break;
                    case 14: Resultado = "catorce"; break;
                    case 15: Resultado = "quince"; break;
                }
            }
            else if (val > 15 && val < 20)
            {
                Resultado = "dieci" + ToLetras(val - 10);
            }
            else if (val > 19 && val < 30)
            {
                if (val == 20)
                {
                    Resultado = "veinte";
                }
                else
                {
                    Resultado = "veinti" + ToLetras(val - 20);
                }
            }
            else if (val > 29 && val < 40)
            {
                if (val == 30)
                {
                    Resultado = "treinta";
                }
                else
                {
                    Resultado = "treinta y " + ToLetras(val - 30);
                }
            }
            else if (val > 39 && val < 50)
            {
                if (val == 40)
                {
                    Resultado = "cuarenta";
                }
                else
                {
                    Resultado = "cuarenta y " + ToLetras(val - 40);
                }
            }
            else if (val > 49 && val < 60)
            {
                if (val == 50)
                {
                    Resultado = "cincuenta";
                }
                else
                {
                    Resultado = "cincuenta y " + ToLetras(val - 50);
                }
            }
            else if (val > 59 && val < 70)
            {
                if (val == 60)
                {
                    Resultado = "sesenta";
                }
                else
                {
                    Resultado = "sesenta y " + ToLetras(val - 60);
                }
            }
            else if (val > 69 && val < 80)
            {
                if (val == 70)
                {
                    Resultado = "setenta";
                }
                else
                {
                    Resultado = "setenta y " + ToLetras(val - 70);
                }
            }
            else if (val > 79 && val < 90)
            {
                if (val == 80)
                {
                    Resultado = "ochenta";
                }
                else
                {
                    Resultado = "ochenta y " + ToLetras(val - 80);
                }
            }
            else if (val > 89 && val < 100)
            {
                if (val == 90)
                {
                    Resultado = "noventa";
                }
                else
                {
                    Resultado = "noventa y " + ToLetras(val - 90);
                }
            }
            else if (val > 99 && val < 200)//centenas
            {
                if (val == 100)
                {
                    Resultado = "cien";
                }
                else
                {
                    Resultado = "ciento " + ToLetras(val - 100);
                }
            }
            else if (val > 199 && val < 1000)//cientos
            {
                int centenas = (int)val / 100;
                int decenas = (int)val - (centenas * 100);
                string aux;
                switch (centenas)
                {
                    case 5: aux = "quinientos "; break;
                    case 7: aux = "setecientos "; break;
                    case 9: aux = "novecientos "; break;
                    default: aux = ToLetras(centenas) + "cientos "; break;
                }
                Resultado = aux + ((decenas > 0) ? ToLetras(decenas) : "");
            }
            else if (val > 999 && val < 1000000)//miles
            {
                int miles = (int)val / 1000;
                int cientos = (int)val - (miles * 1000);
                Resultado = ((miles > 1) ? ToLetras(miles) : "") + " mil " + ((cientos > 0) ? ToLetras(cientos) : "");
            }
            else if (val > 999999 && val < 1000000000000)//millones
            {
                long millones = (long)(val / 1000000M);
                long miles = (long)(val - (millones * 1000000M));
                Resultado = ((millones > 1) ? ToLetras(millones) + " millones " : " un millon ") + ((miles > 0) ? ToLetras(miles) : "");
            }
            return Resultado.Trim();
        }
    }
}
