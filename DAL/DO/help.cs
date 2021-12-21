using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DO
{
    public  class help
        {
            /// <summary>
            /// function that returning the latitude in base 60
            /// </summary>
            /// <param name="lat"></param>
            /// <returns></returns> 
            /// //
            public static string getBase60Lat(double lat)
            {
                string ch;
                if (lat < 0)
                {
                    ch = "W";
                    lat = lat * -1;
                }
                else
                    ch = "E";
                int latSec = (int)Math.Round(lat * 60 * 60);
                double x = (lat - Math.Truncate(lat)) * 60;
                int deg = ((latSec / 60) / 60);//the integer part
                int min = ((latSec / 60) % 60);//the decimal number*60 (i took only the integer out of it)
                float sec = (float)(x - Math.Truncate(x)) * 60;//the decimal part that was in the min times 60
                return $"{deg}° {min}' {sec}'' {ch}";
            }
            /// <summary>
            /// function that returns the lngitude in base 60
            /// </summary>
            /// <param name="lng"></param>
            /// <returns></returns>
            public static string getBase60Lng(double lng)
            {
                string ch;
                if (lng < 0)
                {
                    ch = "S";
                    lng *= -1;
                }
                else
                    ch = "N";
                int lngSec = (int)Math.Round(lng * 60 * 60);
                double x = (lng - Math.Truncate(lng)) * 60;
                float sec = (float)(x - Math.Truncate(x)) * 60;
                int min = ((lngSec / 60) % 60);
                int deg = ((lngSec / 60) / 60);
                return $"{deg}° {min}' {sec}'' {ch}";
            }
        }

    }
