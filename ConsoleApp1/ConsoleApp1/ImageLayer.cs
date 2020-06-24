using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApp1
{
    public class ImageLayer
    {
        /// <summary>
        /// the Red channel of the image data
        /// </summary>
        readonly double[,] R;
        /// <summary>
        /// the Green channel of the image data
        /// </summary>
        readonly double[,] G;
        /// <summary>
        /// the Blue channel of the image data
        /// </summary>
        readonly double[,] B;
        /// <summary>
        /// image layer is grey scale by default unless initialised otherwise
        /// </summary>
        readonly bool isGreyScale =true;
        /// <summary>
        /// instantiates the imageLayer as greyScale, with the input copied into the R-channel
        /// </summary>
        /// <param name="input">the R-channel input</param>
        public ImageLayer (double[,] input)
        {
            isGreyScale = true;
            R = input;
        }
        /// <summary>
        /// checks if the image layer is a greyscale+
        /// </summary>
        public bool IsGrey
        {
            get { return this.isGreyScale; }
        }
        /// <summary>
        /// the Red-channel array
        /// </summary>
        public double[,] Rcn
        {
            get { return this.R; }
        }
        /// <summary>
        /// the Green channel array
        /// </summary>
        public double[,] Gcn
        {
            get { return this.G; }
        }
        /// <summary>
        /// the Blue-channel array
        /// </summary>
        /// <value> gets the value of the  B channel</value>
        public double[,] Bcn
        {
            get { return this.B; }
        }
        /// <summary>
        /// initialise all three color channels of the imaga layer. isGreyScale is set to false
        /// </summary>
        /// <param name="inputR">the R-channel input</param>
        /// <param name="inputG">the G-channel input</param>
        /// <param name="inputB">the B-channel input</param>
        public ImageLayer(double[,] inputR, double[,] inputG, double[,] inputB)
        {
            isGreyScale = false;
            R = inputR;
            G = inputG;
            B = inputB;
        }
        /// <summary>
        /// loads image layer from a Bitmap; isGreyscale is set to false as it loads all
        /// three color channels
        /// </summary>
        /// <param name="bitmap">the bitmap to iinitialise the imageLayer with</param>
        public ImageLayer(Bitmap bitmap):this(bitmap,false)
        {
                       
        }
        /// <summary>
        /// loads the specified channel of the bitmap data into the R-channel of the imageLayer
        /// isGreyScale is set to true
        /// </summary>
        /// <param name="bitmap">the bitmap to be loaded</param>
        /// <param name="channel">the channel of the bitmap to be loaded</param>
        public ImageLayer(Bitmap bitmap, int channel)
        {
            isGreyScale = true;
            R = LoadBitmap(bitmap, channel);
        }

        /// <summary>
        /// load an imageLayer from an image file specified by Path
        /// </summary>
        /// <param name="path"></param>
        public ImageLayer(string path):this(new Bitmap(path))
        {

        }

        public ImageLayer(Bitmap bitmap, bool greyScale)
        {
            if (greyScale)
            {
                R = LoadBitmapAvg(bitmap);
                isGreyScale = true;
            }
            else
            {
                isGreyScale = false;
                R = LoadBitmap(bitmap, 0);
                G = LoadBitmap(bitmap, 1);
                B = LoadBitmap(bitmap, 2);
            }
        }
        /// <summary>
        /// returns a greycale array which is the avergae of the three color channels
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public double[,] LoadBitmapAvg(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            double[,] output = new double[width, height];

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    int sum = color.R + color.G + color.B;
                    output[i, j] = (double)sum / 3;
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the bitmap data from the specified channel and outputs an array
        /// </summary>
        /// <param name="bitmap">the bitmap from which data is to be extracted</param>
        /// <param name="channel">the required channel; 0 for red, 1 for green, 2 for blue</param>
        /// <returns></returns>
        public double[,] LoadBitmap(Bitmap bitmap, int channel)
        {
            int W = bitmap.Width;
            int H = bitmap.Height;
            double[,] output=new double[W,H];
            //channel can only have values 0,1 and 2
            if (channel > 2||channel<0) {
                throw new Exception("channel value is out of bounds; Cannot load bitmap");
            }

            for(int i = 0; i < W; i++)
            {
                for(int j = 0; j < H; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    switch (channel)
                    {
                        case 0:
                            output[i, j] = (double)color.R;
                            break;
                        case 1:
                            output[i, j] = (double)color.G;
                            break;
                        case 2:
                            output[i, j] = (double)color.B;
                            break;


                    }
                   
                }
            }

            return output;

        }

        //the Plus operator
        /// <summary>
        /// overloaded plus operator
        /// </summary>
        /// <param name="inputA"></param>
        /// <param name="inputB"></param>
        /// <returns></returns>
        public static ImageLayer operator +(ImageLayer inputA, ImageLayer inputB)
        {
            //if the dimesions and greyscale properties of the two inputs dont match, throw
            //an exception
            if (!CheckDimension(inputA, inputB)) { throw new Exception("objectproperties do not match :plus"); }

            if (!inputA.IsGrey)//if both inputs are not greyscale
            {//note at this point, if one input is a greyscale, so is the other, and vice versa
                double[,] outR = AddMap(inputA.Rcn, inputB.Rcn);
                double[,] outG = AddMap(inputA.Gcn, inputB.Gcn);
                double[,] outB = AddMap(inputA.Bcn, inputB.Bcn);

                return new ImageLayer(outR, outG, outB);
            }
            else
            {//if both imagelayer inputs are greyScale
                double[,] outR = AddMap(inputA.Rcn, inputB.Rcn);
                return new ImageLayer(outR);
            }

        }

        /// <summary>
        /// adds an integer to the relevant image channels of the imageLayer object.
        /// if the object is a greyscale, it adds the integer to
        /// <para> only the R-channel, else, it adds the integer to all the color channels</para>
        /// </summary>
        /// <param name="num">the integer to be added to the imageLayer object</param>
        /// <param name="imLayer">the ImageLayer object</param>
        /// <returns></returns>
        public static ImageLayer operator +(int num, ImageLayer imLayer)
        {
            if (imLayer.IsGrey)
            {
                double[,] outArray = AddNumber(num, imLayer.Rcn);
                return new ImageLayer(outArray);
            }
            else
            {//if the imageLayer is not a greyScale
                double[,] outR = AddNumber(num, imLayer.Rcn);
                double[,] outG = AddNumber(num, imLayer.Gcn);
                double[,] outB = AddNumber(num, imLayer.Bcn);

                return new ImageLayer(outR, outG, outB);
            }
        }

        /// <summary>
        /// adds an integer to the relevant image channels of the imageLayer object. if the object
        /// <para>is a greyscale, it adds the integer to only the R-channel, else, it adds the integer to all the color channels</para>
        /// </summary>
        /// <param name="num">the integer to be added to the imageLayer object</param>
        /// <param name="imLayer">the ImageLayer object</param>
        /// <returns></returns>
        public static ImageLayer operator +(ImageLayer imLayer ,int num)
        {
            return(num + imLayer);           
            
        }

        //the minus operator
        /// <summary>
        /// subtracts the relevant image array channels in the ImageLayer object from an integer
        /// </summary>
        /// <param name="num">an integer</param>
        /// <param name="imLayer">an ImageLayer Object</param>
        /// <returns></returns>
        public static ImageLayer operator - (int num, ImageLayer imLayer)
        {
            if (imLayer.IsGrey)
            {
                double[,] outArray = Subtract(num, imLayer.Rcn);
                return new ImageLayer(outArray);
            }
            else
            {//if the imageLayer is not a greyScale
                double[,] outR = Subtract(num, imLayer.Rcn);
                double[,] outG = Subtract(num, imLayer.Gcn);
                double[,] outB = Subtract(num, imLayer.Bcn);

                return new ImageLayer(outR, outG, outB);
            }
        }

        /// <summary>
        /// overloaded  minus operator. it performs element-wise operation: inputA - inputB
        /// </summary>
        /// <param name="inputA"></param>
        /// <param name="inputB"></param>
        /// <returns></returns>
        public static ImageLayer operator -(ImageLayer inputA, ImageLayer inputB)
        {
            //if the dimesions and greyscale properties of the two inputs dont match, throw
            //an exception
            if (!CheckDimension(inputA, inputB)) { throw new Exception("objectproperties do not match :plus"); }

            if (!inputA.IsGrey)//if both inputs are not greyscale
            {//note at this point, if one input is a greyscale, so is the other, and vice versa
                double[,] outR = SubtractMap(inputA.Rcn, inputB.Rcn);
                double[,] outG = SubtractMap(inputA.Gcn, inputB.Gcn);
                double[,] outB = SubtractMap(inputA.Bcn, inputB.Bcn);

                return new ImageLayer(outR, outG, outB);
            }
            else
            {//if both imagelayer inputs are greyScale
                double[,] outR = SubtractMap(inputA.Rcn, inputB.Rcn);
                return new ImageLayer(outR);
            }
        }

        /// <summary>
        /// subtracts an integer from the elements in the relevant channels of the ImageLayer object
        /// </summary>
        /// <param name="imLayer">the ImageLayer object</param>
        /// <param name="num">the integer</param>
        /// <returns></returns>
        public static ImageLayer operator -(ImageLayer imLayer, int num)
        {
            if (imLayer.IsGrey)
            {
                double[,] outArray = Subtract(imLayer.Rcn, num);
                return new ImageLayer(outArray);
            }
            else
            {//if the imageLayer is not a greyScale
                double[,] outR = Subtract(imLayer.Rcn, num);
                double[,] outG = Subtract(imLayer.Gcn ,num);
                double[,] outB = Subtract(imLayer.Bcn, num);

                return new ImageLayer(outR, outG, outB);
            }
        }

        //the multiply operator
        /// <summary>
        /// multiplies the relevant image channels of the ImageLayer obect with a number value, 
        /// <para>depending on whether its is a greyscale</para>
        /// </summary>
        /// <remarks> let me see the remark</remarks>
        /// <param name="imLayer"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static ImageLayer operator *(ImageLayer imLayer, double num)
        {
            if (imLayer.IsGrey)
            {
                double[,] outArray = Multiply(imLayer.Rcn, num);
                return new ImageLayer(outArray);
            }
            else
            {//if the imageLayer is not a greyScale
                double[,] outR = Multiply(imLayer.Rcn, num);
                double[,] outG = Multiply(imLayer.Gcn, num);
                double[,] outB = Multiply(imLayer.Bcn, num);

                return new ImageLayer(outR, outG, outB);
            }
        }

        /// <summary>
        /// multiplies the relevant image channels of the ImageLayer obect with a number value, 
        /// <para>depending on whether its is a greyscale</para>
        /// </summary>
        /// <param name="num"></param>
        /// <param name="imLayer"></param>
        /// <returns></returns>
        public static ImageLayer operator *(double num, ImageLayer imLayer)
        {
            ImageLayer output =imLayer * num;
            return output;
        }
        /// <summary>
        /// performs element-wise multiplication on corresponding channels in bothe imagelayers
        /// <para>i.e imA.Rcn * imB.Rcn is the final output R-channel, and same for other channels</para>
        /// </summary>
        /// <param name="imA"></param>
        /// <param name="imB"></param>
        /// <returns></returns>
        public static ImageLayer operator *(ImageLayer imA, ImageLayer imB)
        {
            //throw an error if the R,G,B or isGrey properties of both imageLayers dont match
            if(!CheckDimension(imA, imB)) {
                throw new Exception("ImageLayer objects must have similar properties and dimensions : Multiply"); }

            //if both imageLayer objects are greyscales, multiply only their R-channels
            if (imA.IsGrey)
            {
                double[,] outR = Multiply(imA.Rcn, imB.Rcn);
                return new ImageLayer(outR);
            }
            else
            {
                double[,] outR = Multiply(imA.Rcn, imB.Rcn);
                double[,] outG = Multiply(imA.Gcn, imB.Gcn);
                double[,] outB = Multiply(imA.Bcn, imB.Bcn);

                return new ImageLayer(outR, outG, outB);
            }
            
        }


        /// <summary>
        /// performs the element-wise multiplication of two arrays
        /// </summary>
        /// <param name="inpA">the first array</param>
        /// <param name="inpB">the second array</param>
        /// <returns></returns>
        public static double[,] Multiply(double[,] inpA, double[,] inpB)
        {
            //if the diumensions of the two arrays don't match, throw an error
            if (!CheckDimension(inpA, inpB)) { throw new Exception("arrays mustbe of similar sizes to be multiplied"); }

            int n = inpA.GetLength(0);
            int m = inpA.GetLength(1);

            double[,] output = new double[n, m];

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    output[i, j] = inpA[i, j] * inpB[i, j];
                }
            }
            return output;
        }

        /// <summary>
        /// multiplies all the elements of a data array with a number 
        /// </summary>
        /// <param name="num">the number to multiply with. must be a Double</param>
        /// <param name="data">the data array</param>
        /// <returns></returns>
        static double[,] Multiply(double num, double[,] data)
        {
            int n = data.GetLength(0);
            int m = data.GetLength(1);
            double[,] output = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output[i, j] = num * data[i, j];
                }
            }
            return output;
        }


        
        /// <summary>
        /// multiplies all the elements of a data array with a number 
        /// </summary>
        /// <param name="num">the number to multiply with. must be a Double</param>
        /// <param name="data">the data array</param>
        /// <returns></returns>
        static double[,] Multiply(double[,] data, double num)
        {
            return Multiply(num, data);

        }


        
        /// <summary>
        /// subtracts all the elements of the data array from an integer. 
        /// the result is an array whose element position correspond to
        /// the position of the elemt n the data array that produced them:
        /// num - data
        /// </summary>
        /// <param name="num"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static double[,] Subtract(int num, double[,] data)
        {
            int n = data.GetLength(0);
            int m = data.GetLength(1);
            double[,] output = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output[i, j] = (double)num - data[i, j];
                }
            }
            return output;
        }

        /// <summary>
        /// subtracts an integer from all the corresponding elements of a data array
        /// : Date[] - num
        /// </summary>
        /// <param name="data"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        static double[,] Subtract(double[,] data, int num)
        {
            int n = data.GetLength(0);
            int m = data.GetLength(1);
            double[,] output = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output[i, j] = data[i, j] - num;
                }
            }
            return output;
        }

        /// <summary>
        /// adds an integer to all the elements of a data array
        /// </summary>
        /// <param name="num">the integer to  be added</param>
        /// <param name="data">the data array</param>
        /// <returns></returns>
        static double[,] AddNumber(int num, double[,] data)
        {
            int n = data.GetLength(0);
            int m = data.GetLength(1);
            double[,] output = new double[n, m];

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    output[i, j] = num + data[i, j];
                }
            }
            return output;
        }

        

       

        /// <summary>
        /// returns the element-wise operation: input1 - input2
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        static double[,] SubtractMap(double[,] input1, double[,] input2) {
            if (!CheckDimension(input1, input2)) { throw new Exception("input arrays are of dissimilar dimensions :AddMap"); }

            int n = input1.GetLength(0);
            int m = input2.GetLength(1);

            double[,] output = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    output[i, j] = input1[i, j] - input2[i, j];
                }
            }
            return output;
        }

        /// <summary>
        /// performs the element-wise addition of the two input arrays
        /// </summary>
        /// <param name="input1">the first input array</param>
        /// <param name="input2">the second input array</param>
        /// <returns></returns>
        static double[,] AddMap(double[,] input1, double[,] input2)
        {
            //throw an exception if the dimensions of the input arrays do not match
            if(!CheckDimension(input1, input2)) { throw new Exception("input arrays are of dissimilar dimensions :AddMap"); }

            int n = input1.GetLength(0);
            int m = input2.GetLength(1);

            double[,] output = new double[n, m];

            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    output[i, j] = input1[i, j] + input2[i, j];
                }
            }
            return output;

        }

        /// <summary>
        /// returns true if the R, G and B channels of the two image layers together all have the same dimensions
        /// and also that the isGrey properties of both inputs are equal
        /// </summary>
        /// <param name="imA"></param>
        /// <param name="imB"></param>
        /// <returns></returns>
        static bool CheckDimension(ImageLayer imA, ImageLayer imB)
        {
            //if one imageLayer is a greyscale, while the other isnt, return false
            if(imA.IsGrey != imB.IsGrey) { return false; }

            //if both imageLayers are not greyscale
            if (!imA.IsGrey && imA.IsGrey==imB.IsGrey)
            {
                //check if the channels of imA have uniform dimensions
                bool checkA = CheckDimension(imA.Rcn, imA.Gcn) && CheckDimension(imA.Gcn, imA.Bcn);

                //check if the channels of imB have uniform dimensions
                bool checkB = CheckDimension(imB.Rcn, imB.Gcn) && CheckDimension(imB.Gcn, imB.Bcn);

                //if the channel dimenions in imA are the same and the channel dimensions in imB are thesame
                // check if the dimensions of imA match those of imB
                if(checkA && checkB) { return CheckDimension(imA.Rcn, imB.Rcn); }
                else { return false; }
            }
            else if(imA.IsGrey && imA.IsGrey == imB.IsGrey)
            {//if both imageLayers are greyscale, compare only their R-channels
                return CheckDimension(imA.Rcn, imB.Rcn);
            }

            //program execution shouldnt possibly get here
            throw new Exception("dimensions of imageLayers to be checked have unknown states");
           
        }

        /// <summary>
        /// returns true if the dimensions of the two input arrays match exactly, else, it returns 
        /// false
        /// </summary>
        /// <param name="A">the first array</param>
        /// <param name="B">the second array</param>
        /// <returns></returns>
        static bool CheckDimension(double[,] A, double[,] B)
        {
            int a0 = A.GetLength(0);
            int a1 = A.GetLength(1);

            int b0 = B.GetLength(0);
            int b1 = B.GetLength(1);

            if(a0==b0 && a1 == b1) { return true; }
            else { return false; }
        }





        //convolution taking Imlayer object as input still needs a lot of clarity
        /// <summary>
        /// Computes the convolution of filter Wf on input layet I.
        /// for now, Wf, I and the output array must be square arrays
        /// </summary>
        /// <param name="Wf"> The filter matrix</param>
        /// <param name="I">the input array</param>
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

            for (int i = 0; i < outSize; i++)
            {
                for (int j = 0; j < outSize; j++)
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
        /// <param name="inputArray">the input array</param>
        /// <param name="arrayDim">dimension of the square output array</param>
        /// <param name="xPos">the x-position reference in the input array</param>
        /// <param name="yPos">the y-position reference in the input array</param>
        /// <returns></returns>
        Double[,] MapCopy(double[,] inputArray, int arrayDim, int xPos, int yPos)
        {
            double[,] output = new double[arrayDim, arrayDim];

            for (int i = 0; i < arrayDim; i++)
            {
                for (int j = 0; j < arrayDim; j++)
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
            if (data.GetLength(0) != data.GetLength(1))
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
            if (X.GetLength(0) != Y.GetLength(0) || X.GetLength(1) != Y.GetLength(1))
            {
                throw new Exception("Arrays must be of similar sizes to perform DotProduct");
            }

            int N = X.GetLength(0);
            int M = X.GetLength(1);

            double output = 0;// the output to be computed

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    output += X[i, j] * Y[i, j];
                }
            }
            return output;
        }



        /// <summary>
        /// on each pixel, it selects the channel with the minimum value 
        /// </summary>
        /// <returns> a double[] array representing the minimum value of all three channels for each pixel</returns>
        public double[,] SelectMinChannel()
        {
            if (IsGrey) { return Rcn; }
            else
            {
                int n = Rcn.GetLength(0);
                int m = Rcn.GetLength(1);
                double[,] output = new double[n, m];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        //get the minimum between the red and green channel values for a given pixel
                        double min1 = Math.Min(Rcn[i, j], Gcn[i, j]);

                        //get the minimum between the Blue channel and the Red and Green channels for the given pixel
                        output[i, j] = Math.Min(min1, Bcn[i, j]);
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// on each pixel, it selects the channel with the maximum value 
        /// </summary>
        /// <returns>a double[] array representing the maximum value of all three channels for each pixel</returns>
        public double[,] SelectMaxChannel()
        {
            if (IsGrey) { return Rcn; }
            else
            {
                int n = Rcn.GetLength(0);
                int m = Rcn.GetLength(1);
                double[,] output = new double[n, m];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        //get the maximum between the red and green channel values for a given pixel
                        double min1 = Math.Max(Rcn[i, j], Gcn[i, j]);

                        //get the maximum between the Blue channel and the Red and Green channels for the given pixel
                        output[i, j] = Math.Max(min1, Bcn[i, j]);
                    }
                }
                return output;
            }
        }




    }
}
