﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ConsoleApp1
{
    class Program
    {
         static Bitmap image;
        //Image im;
        static void Main(string[] args)
        {
            image = new Bitmap("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert.jpg");
            Console.WriteLine("hello world");

            //test editing image pixel
            for(int i = 0; i < image.Width; i++)
            {
                for(int j = 0; j < image.Height; j++)
                {
                    Color pixelColor = image.GetPixel(i,j);
                    int greyscale = (pixelColor.R) / 3 + (pixelColor.G) / 3 + (pixelColor.B) / 3;
                    Color newColor = Color.FromArgb(greyscale,greyscale,0);
                    image.SetPixel(i, j, newColor);
                }
            }
            image.Save("C:\\Users\\Public\\Pictures\\Sample Pictures\\Desert3.jpg");

            Console.WriteLine("Modified image saved");
            Console.WriteLine("Testing DotProduct");
            Console.ReadKey(true);

            Program myProgram = new Program();
            double[,] X = { { 1, 2 }, 
                            { 3, 4 } };

            double[,] Y = { { 1, 4 },
                            { 1, 2 } };

            double dotP = myProgram.DotProduct(X, Y);
            Console.WriteLine("Dot product is " + dotP);

           // X = new double[2, 3];
            //myProgram.CheckSquare(X);

            X = new double[2, 3];
            //Console.WriteLine("CHeckSquare works");
            Console.WriteLine("X is a " + X.GetLength(0) + " X " + X.GetLength(1) + " array");
            Console.ReadKey(true);
             
            double[,] cpTest = new double[4, 4];
            cpTest[0, 0] = 1;  cpTest[1, 0] = 2;  cpTest[2, 0] = 3;  cpTest[3, 0] = 4;
            cpTest[0, 1] = 5;  cpTest[1, 1] = 6;  cpTest[2, 1] = 7;  cpTest[3, 1] = 8;
            cpTest[0, 2] = 9;  cpTest[1, 2] = 10; cpTest[2, 2] = 11; cpTest[3, 2] = 12;
            cpTest[0, 3] = 13; cpTest[1, 3] = 14; cpTest[2, 3] = 15; cpTest[3, 3] = 16;

            double[,] copyResult = myProgram.MapCopy(cpTest, 2, 1, 1);
            Console.WriteLine("MapCopy result is a " + copyResult.GetLength(0) + " X " + copyResult.GetLength(1) + " array");
            Console.WriteLine(copyResult[0, 0] + " " + copyResult[1, 0]);
            Console.WriteLine(copyResult[0, 1] + " " + copyResult[1, 1]);

            double[,] tFtr = new double[2, 2];
            tFtr[0, 0] = 1; tFtr[1, 0] = 1;
            tFtr[0, 1] = 1; tFtr[1, 1] = 1;

            double[,] testConv = myProgram.ComputeConvolution(tFtr, cpTest);
            Console.ReadKey(true);
            //double[,] nn;
             

        }
        /// <summary>
        /// Computes the convolution of filter Wf on input layet I.
        /// for now, Wf, I and the output array must be square arrays
        /// </summary>
        /// <param filter matrix="Wf"></param>
        /// <param Input array="I"></param>
        /// <returns></returns>
        double[,] ComputeConvolution(double[,] Wf, Double[,] I)
        {
            //ensure the filter matrix and input arrays are square matrices
            CheckSquare(Wf);
            CheckSquare(I);

            int M = Wf.GetLength(0);
            int N = I.GetLength(0);
            int outSize = N - M + 1;// the size of the output array

            double[,] output = new double[outSize, outSize];

            for(int i=0; i < outSize; i++)
            {
                for(int j=0; j < outSize; j++)
                {
                    double[,] Ic = MapCopy(I, M, i, j);
                    output[i, j] = DotProduct(Ic, Wf);
                }
            }
            return output;
        }

        /// <summary>
        /// copies out an (arrayDim x arrayDim) sized array from the input array, with a top left
        /// reference defined by (xPos, yPos)
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="arrayDim"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        Double[,] MapCopy(double[,] inputArray, int arrayDim, int xPos, int yPos)
        {
            double[,] output = new double[arrayDim, arrayDim];

            for(int i = 0; i < arrayDim; i++)
            {
                for(int j = 0; j < arrayDim; j++)
                {
                    output[i, j] = inputArray[xPos + i, yPos + j];
                }
            }
            return output;
        }

        /// <summary>
        /// checks if the data array is a square matrix. if it is not, an error is thrown
        /// </summary>
        /// <param name="data"></param>
        void CheckSquare(Double[,] data)
        {
            if(data.GetLength(0) != data.GetLength(1))
            {
                throw new Exception("array supplied is not a square");
            }
            
        }

        /// <summary>
        /// Computes the sum of the element-wise multiplication of X[] and Y[]
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        Double DotProduct(double[,] X, double[,] Y)
        {
            if(X.GetLength(0)!= Y.GetLength(0) || X.GetLength(1) != Y.GetLength(1))
            {
                throw new Exception("Arrays must be of similar sizes to perform DotProduct");
            }

            int N = X.GetLength(0);
            int M = X.GetLength(1);

            double output = 0;// the output to be computed

            for(int i=0; i<N; i++)
            {
                for(int j=0; j<M; j++)
                {
                    output += X[i, j] * Y[i, j];
                }
            }
            return output;
        }

        //double[,]
    }
}