using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Filter
    {
        /// <summary>
        /// the X-direction Sobel edge filter
        /// </summary>
        public double[,] Sobel_x;

        /// <summary>
        /// the y-direction Sobel edge filter
        /// </summary>
        public double[,] Sobel_y;

        /// <summary>
        /// initialises the Sobel filters for now
        /// </summary>
        public Filter()
        {
            double[,] Sx = new double[3, 3];
            Sx[0, 0] = -1; Sx[1, 0] = 0; Sx[2, 0] = 1;
            Sx[0, 1] = -1; Sx[1, 1] = 0; Sx[2, 1] = 1;
            Sx[0, 2] = -1; Sx[1, 2] = 0; Sx[2, 2] = 1;

            Sobel_x = Sx;

            double[,] Sy = new double[3, 3];
            Sy[0, 0] = -1; Sy[1, 0] = -2; Sy[2, 0] = -1;
            Sy[0, 1] = 0; Sy[1, 1] = 0; Sy[2, 1] = 0;
            Sy[0, 2] = 1; Sy[1, 2] = 2; Sy[2, 2] = 1;

            Sobel_y = Sy;
        }

    }

}
