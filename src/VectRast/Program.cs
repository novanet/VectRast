using System;
using System.Collections.Generic;
using SkiaSharp;
using VectRast.Models;
using VectRast.Models.Numerics;

namespace VectRast
{
    public class Program
    {
        public static int Main(string[] args)
        {
            (var resultCode,
            var load_type,
            var loadBmpFileName,
            var loadLevFileName,
            var save_type,
            var saveBmpFileName,
            var saveLevFileName,
            var playerXY,
            var flowerXY,
            var applesXY,
            var numFlowers,
            var transformMatrix,
            var printProgressOn,
            var printWarningsOn) =
                ParseArguments(args);
            if (resultCode > 0)
                return resultCode;

            (var loadResultCode, var vr) = LoadAndTransform(load_type, loadBmpFileName, loadLevFileName, transformMatrix,
                                                numFlowers, playerXY, flowerXY, applesXY, printProgressOn, printWarningsOn);
            if (loadResultCode > 0)
                return loadResultCode;

            resultCode = Save(vr, save_type, saveBmpFileName, saveLevFileName);
            if (resultCode > 0)
                return resultCode;

            if (vr.someWarning)
                Console.WriteLine("\ndone, but there were some warnings; set '-warnings true' to view them\n");
            else
                Console.WriteLine("\ndone\n");
            return 0;
        }

        private static
        (
            int resultCode,
            IOType load_type,
            string loadBmpFileName,
            string loadLevFileName,
            IOType save_type,
            string saveBmpFileName,
            string saveLevFileName,
            (int x, int y)? playerXY,
            (int x, int y)? flowerXY,
            (int x, int y)[] applesXY,
            int numFlowers,
            Matrix2D transformMatrix,
            bool printProgressOn,
            bool printWarningsOn
            )
            ParseArguments(string[] args)
        {
            const double ONEPIXEL = 1.0 / 47.0;
            bool printProgressOn = true;
            bool printWarningsOn = false;
            IOType load_type = IOType.None;
            IOType save_type = IOType.None;
            String loadLevFileName = null;
            String saveLevFileName = null;
            String loadBmpFileName = null;
            String saveBmpFileName = null;
            (int x, int y)? playerXY = null;
            (int x, int y)? flowerXY = null;
            var applesXY = new List<(int x, int y)>();
            int numFlowers = 1;
            Matrix2D transformMatrix = Matrix2D.identityM();

            int arg_num = 0;
            try
            {
                while (arg_num < args.Length)
                {
                    String arg_now = args[arg_num++];
                    switch (arg_now)
                    {
                        case "-loadbmp":
                            // initialize from bitmap
                            load_type = IOType.Bitmap;
                            loadBmpFileName = args[arg_num++];
                            break;
                        case "-loadlev":
                            // initialize from level
                            load_type = IOType.Level;
                            loadLevFileName = args[arg_num++];
                            break;
                        case "-savebmp":
                            // save to bitmap
                            save_type = IOType.Bitmap;
                            saveBmpFileName = args[arg_num++];
                            break;
                        case "-savelev":
                            // save to level
                            save_type = IOType.Level;
                            saveLevFileName = args[arg_num++];
                            break;
                        case "-loadlevbmp":
                            load_type = IOType.LevelBitmap;
                            loadLevFileName = args[arg_num++];
                            loadBmpFileName = args[arg_num++];
                            break;
                        case "-translate":
                            // offset by tx, ty
                            double tx = Double.Parse(args[arg_num++]);
                            double ty = Double.Parse(args[arg_num++]);
                            transformMatrix = transformMatrix * Matrix2D.translationM(tx, ty);
                            break;
                        case "-rotate":
                            // rotate by angle (in degrees)
                            double ang = Double.Parse(args[arg_num++]);
                            transformMatrix = transformMatrix * Matrix2D.rotationM(ang);
                            break;
                        case "-scale":
                            // scale by factor sx, sy
                            double sx = Double.Parse(args[arg_num++]) * ONEPIXEL;
                            double sy = Double.Parse(args[arg_num++]) * ONEPIXEL;
                            transformMatrix = transformMatrix * Matrix2D.scaleM(sx, sy);
                            break;
                        case "-flowers":
                            // number of flowers
                            numFlowers = Int32.Parse(args[arg_num++]);
                            break;
                        case "-progress":
                            // show progress continually
                            printProgressOn = Boolean.Parse(args[arg_num++]);
                            break;
                        case "-warnings":
                            // print warnings
                            printWarningsOn = Boolean.Parse(args[arg_num++]);
                            break;
                        case "-playerXY":
                            var playerX = int.Parse(args[arg_num++]);
                            var playerY = int.Parse(args[arg_num++]);
                            playerXY = (playerX, playerY);
                            break;
                        case "-flowerXY":
                            var flowerX = int.Parse(args[arg_num++]);
                            var flowerY = int.Parse(args[arg_num++]);
                            flowerXY = (flowerX, flowerY);
                            break;
                        case "-appleXY":
                            var appleX = int.Parse(args[arg_num++]);
                            var appleY = int.Parse(args[arg_num++]);
                            applesXY.Add((appleX, appleY));
                            break;
                        default:
                            throw new Exception("unknown parameter '" + arg_now + "'");
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("\nexpected value(s) after the switch");
                return (7, IOType.None, "", "", IOType.None, "", "", null, null, null, 0, null, false, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nerror parsing command line: " + e.Message);
                return (8, IOType.None, "", "", IOType.None, "", "", null, null, null, 0, null, false, false);
            }
            if (load_type == IOType.LevelBitmap && save_type == IOType.Bitmap)
            {
                Console.WriteLine("savebmp not allowed after loadlevbmp");
                return (3, IOType.None, "", "", IOType.None, "", "", null, null, null, 0, null, false, false);
            }

            return (
                resultCode: 0,
                load_type,
                loadBmpFileName,
                loadLevFileName,
                save_type,
                saveBmpFileName,
                saveLevFileName,
                playerXY,
                flowerXY,
                applesXY.ToArray(),
                numFlowers,
                transformMatrix,
                printProgressOn,
                printWarningsOn
            );
        }

        private static (int resultCode, VectRast) LoadAndTransform(IOType load_type,
                                        string loadBmpFileName,
                                        string loadLevFileName,
                                        Matrix2D transformMatrix,
                                        int numFlowers,
                                        (int x, int y)? playerXY,
                                        (int x, int y)? flowerXY,
                                        (int x, int y)[] applesXY,
                                        bool printProgressOn, bool printWarningsOn)
        {
            VectRast vr = new VectRast(printProgressOn, printWarningsOn);
            if (load_type == IOType.Bitmap || load_type == IOType.LevelBitmap)
            {
                byte[,] pixelOn;
                SKBitmap bmp;
                try
                {
                    Console.Write("\nloading in bitmap {0}", loadBmpFileName);
                    vr.LoadAsBmp(loadBmpFileName, out bmp, out pixelOn,
                            Math.Abs(transformMatrix.elements[0, 0]) + Math.Abs(transformMatrix.elements[1, 1]),
                            load_type == IOType.LevelBitmap ? (byte)0 : (byte)1,
                            numFlowers, playerXY, flowerXY, applesXY);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nerror loading bitmap {0}: " + e.Message, loadBmpFileName);
                    return (1, null);
                }
                try
                {
                    Console.Write("\ncreating polygons");
                    vr.CollapseVectors(vr.CreateVectors(pixelOn, bmp));
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nerror vectorizing the bitmap: " + e.Message);
                    return (2, null);
                }
                transformMatrix = Matrix2D.translationM(-bmp.Width / 2.0, -bmp.Height / 2.0) * transformMatrix;
                bmp.Dispose();
            }
            if (load_type == IOType.LevelBitmap)
                try
                {
                    Console.Write("\ntransforming vectors");
                    vr.TransformVectors(transformMatrix);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nerror transforming vectors: " + e.Message);
                    return (4, null);
                }
            if (load_type == IOType.Level || load_type == IOType.LevelBitmap)
                try
                {
                    Console.Write("\nloading in level {0}", loadLevFileName);
                    vr.LoadAsLev(loadLevFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nerror loading level {0}: " + e.Message, loadLevFileName);
                    return (5, null);
                }
            if (load_type != IOType.LevelBitmap)
                try
                {
                    Console.Write("\ntransforming vectors");
                    vr.TransformVectors(transformMatrix);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nerror transforming vectors: " + e.Message);
                    return (6, null);
                }
            return (0, vr);
        }

        private static int Save(VectRast vr, IOType save_type, string saveBmpFileName, string saveLevFileName)
        {
            switch (save_type)
            {
                case IOType.None:
                    break;
                case IOType.Bitmap:
                    try
                    {
                        Console.Write("\nsaving bitmap {0}", saveBmpFileName);
                        throw new NotImplementedException("Saving Bitmap has been disabled.");
                        //vr.saveAsBmp(saveBmpFileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nerror saving bitmap {0}: " + e.Message, saveBmpFileName);
                        return 5;
                    }
                //break;
                case IOType.Level:
                    try
                    {
                        Console.Write("\nsaving level {0}", saveLevFileName);
                        vr.SaveAsLev(saveLevFileName, "autogenerated on " + DateTime.Now.ToString("g"), "DEFAULT", "ground", "sky");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\nerror saving level {0}: " + e.Message, saveLevFileName);
                        return 6;
                    }
                    break;
            }

            return 0;
        }
    }
}