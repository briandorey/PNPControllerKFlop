using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMotion_dotNet;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace PNPControllerKFlop
{
    public class kflop
    {
        KM_Controller _Controller; //Object to interface with the Kflop
        KM_Axis _XAxis;
        KM_Axis _YAxis;
        KM_Axis _ZAxis;
        KM_Axis _AAxis;
        KM_Axis _BAxis;
        KM_Axis _CAxis;
        KM_CoordMotion _Motion;

        private double JogSpeed = 200;

        private double currentX = 0.0;
        private double currentY = 0.0;
        private double currentZ = 0.0;
        private double currentA = 0.0;
        private double currentB = 0.0;
        private double currentC = 0.0;

        public event KM_CoordMotionStraightFeedHandler CoordMotionStraightFeed;
        public bool eStopActive = false;

        public void initdevice()
        {
            _Controller = new KMotion_dotNet.KM_Controller();
            _XAxis = new KMotion_dotNet.KM_Axis(_Controller, 0, "x");
            _YAxis = new KMotion_dotNet.KM_Axis(_Controller, 1, "y");
            _ZAxis = new KMotion_dotNet.KM_Axis(_Controller, 2, "z");
            _AAxis = new KMotion_dotNet.KM_Axis(_Controller, 3, "a");
            _BAxis = new KMotion_dotNet.KM_Axis(_Controller, 4, "b");
            _CAxis = new KMotion_dotNet.KM_Axis(_Controller, 5, "c");
            _Motion = new KMotion_dotNet.KM_CoordMotion(_Controller);

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
           // path = Path.GetDirectoryName(path);
           // path = Path.GetDirectoryName(path);
           // path = Path.GetDirectoryName(path);

            AddHandlers();

            String TheCFile = path + @"\InitPickandPlace.c";

            //************NEW program execution model***********
            String result = _Controller.ExecuteProgram(1, TheCFile, false);
            if (result != "") MessageBox.Show(result);

            // _IO.Add("Lamp", new KMotion_dotNet.KM_IO(_Controller, 32, "Lamp", KMotion_dotNet.IO_TYPE.DIGITAL_OUT));  //Point of IO used to control the Lamp (change from bit 32 to whichever you are using)
            _XAxis.CPU = 2000;
            _YAxis.CPU = 2000;
            _ZAxis.CPU = 2000;
            _AAxis.CPU = 2000;
            _BAxis.CPU = 2000;
            _CAxis.CPU = 2000;

            _XAxis.JogVelocity = 200;
            _YAxis.JogVelocity = 200;
            _ZAxis.JogVelocity = 200;
            _AAxis.JogVelocity = 200;
            _BAxis.JogVelocity = 200;
            _CAxis.JogVelocity = 200;

            // setup homing params
            _ZAxis.HomingParams.SourceType = HOMING_ROUTINE_SOURCE_TYPE.AUTO;
            _ZAxis.HomingParams.HomeFastVel = 1000;
            _ZAxis.HomingParams.HomeSlowVel = 100;
            _ZAxis.HomingParams.HomeLimitBit = 16;
            _ZAxis.HomingParams.HomeLimitState = true;
            _ZAxis.HomingParams.RepeatHomeAtSlowerRate = true;
            _ZAxis.HomingParams.SequencePriority = 1;

            _AAxis.HomingParams.SourceType = HOMING_ROUTINE_SOURCE_TYPE.AUTO;
            _AAxis.HomingParams.HomeFastVel = 1000;
            _AAxis.HomingParams.HomeSlowVel = 100;
            _AAxis.HomingParams.HomeLimitBit = 16;
            _AAxis.HomingParams.HomeLimitState = true;
            _AAxis.HomingParams.RepeatHomeAtSlowerRate = true;
            _AAxis.HomingParams.SequencePriority = 2;

            _XAxis.HomingParams.SourceType = HOMING_ROUTINE_SOURCE_TYPE.AUTO;
            _XAxis.HomingParams.HomeFastVel = 1000;
            _XAxis.HomingParams.HomeSlowVel = 100;
            _XAxis.HomingParams.HomeLimitBit = 16;
            _XAxis.HomingParams.HomeLimitState = true;
            _XAxis.HomingParams.RepeatHomeAtSlowerRate = true;
            _XAxis.HomingParams.SequencePriority = 3;

            _YAxis.HomingParams.SourceType = HOMING_ROUTINE_SOURCE_TYPE.AUTO;
            _YAxis.HomingParams.HomeFastVel = 1000;
            _YAxis.HomingParams.HomeSlowVel = 100;
            _YAxis.HomingParams.HomeLimitBit = 16;
            _YAxis.HomingParams.HomeLimitState = true;
            _YAxis.HomingParams.RepeatHomeAtSlowerRate = true;
            _YAxis.HomingParams.SequencePriority = 4;



            // setup motion params
            _Controller.CoordMotion.Abort();
            _Controller.CoordMotion.ClearAbort();
            _Controller.WriteLine(String.Format("DefineCS = {0} {1} {2} {3} {4} {5}", 0, 1, 2, -1, -1, -1));
            _Controller.WriteLine(String.Format("EnableAxis{0}", 0));
            _Controller.WriteLine(String.Format("EnableAxis{0}", 1));
            _Controller.WriteLine(String.Format("EnableAxis{0}", 2));
            _Controller.CoordMotion.MotionParams.BreakAngle = 30;
            _Controller.CoordMotion.MotionParams.RadiusA = 5;
            _Controller.CoordMotion.MotionParams.RadiusB = 5;
            _Controller.CoordMotion.MotionParams.RadiusC = 5;
            _Controller.CoordMotion.MotionParams.MaxAccelX = 30000;
            _Controller.CoordMotion.MotionParams.MaxAccelY = 3000;
            _Controller.CoordMotion.MotionParams.MaxAccelZ = 3000;
            _Controller.CoordMotion.MotionParams.MaxAccelA = 30;
            _Controller.CoordMotion.MotionParams.MaxAccelB = 30;
            _Controller.CoordMotion.MotionParams.MaxAccelC = 30;
            _Controller.CoordMotion.MotionParams.MaxVelX = 3000;
            _Controller.CoordMotion.MotionParams.MaxVelY = 30;
            _Controller.CoordMotion.MotionParams.MaxVelA = 30;
            _Controller.CoordMotion.MotionParams.MaxVelB = 30;
            _Controller.CoordMotion.MotionParams.MaxVelC = 30;
            _Controller.CoordMotion.MotionParams.CountsPerInchX = 500;
            _Controller.CoordMotion.MotionParams.CountsPerInchY = 300;
            _Controller.CoordMotion.MotionParams.CountsPerInchZ = 300;
            _Controller.CoordMotion.MotionParams.CountsPerInchA = 30;
            _Controller.CoordMotion.MotionParams.CountsPerInchB = 30;
            _Controller.CoordMotion.MotionParams.CountsPerInchC = 30;
            _Controller.CoordMotion.MotionParams.DegreesA = false;
            _Controller.CoordMotion.MotionParams.DegreesB = true;
            _Controller.CoordMotion.MotionParams.DegreesC = true;
        }
        // public methods

        public void HomeAll()
        {
            _ZAxis.StartDoHome();
            _AAxis.StartDoHome();
            _XAxis.StartDoHome();
            _YAxis.StartDoHome();
        }
        public bool MoveXAxis(double newpos)
        {
            _XAxis.MoveTo(newpos);
            while (!_XAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveYAxis(double newpos)
        {
            _YAxis.MoveTo(newpos);
            while (!_YAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveZAxis(double newpos)
        {
            _ZAxis.MoveTo(newpos);
            while (!_ZAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveAAxis(double newpos)
        {
            _AAxis.MoveTo(newpos);
            while (!_AAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveBAxis(double newpos)
        {
            _BAxis.MoveTo(newpos);
            while (!_BAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveCAxis(double newpos)
        {
            _CAxis.MoveTo(newpos);
            while (!_CAxis.MotionComplete())
            {
                Thread.Sleep(10);
            }
            return true;
        }
        public bool MoveSingleFeed(double speed, double x, double y, double z, double a, double b, double c)
        {
            if (!x.Equals(currentX))
            {
                currentX = x;
            }
            if (!y.Equals(currentY))
            {
                currentY = y;
            }
            if (!z.Equals(currentZ))
            {
                currentZ = z;
            }
            if (!a.Equals(currentA))
            {
                currentA = a;
            }
            if (!b.Equals(currentB))
            {
                currentB = b;
            }
            if (!c.Equals(currentC))
            {
                currentC = c;
            }

            _Controller.CoordMotion.StraightFeed(speed, currentX, currentY, currentZ, currentA, currentB, currentC, 0, 0);
           
            _Controller.CoordMotion.WaitForSegmentsFinished(true);
            _Controller.CoordMotion.FlushSegments();
            return true;
        }

        public bool MoveArrayFeed(double[,] array)
        {
            double speed = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                speed = array[i, 0];
                if (!array[i,1].Equals(currentX))
                {
                    currentX = array[i, 1];
                }
                if (!array[i, 2].Equals(currentY))
                {
                    currentY = array[i, 2];
                }
                if (!array[i, 3].Equals(currentZ))
                {
                    currentZ = array[i, 3];
                }
                if (!array[i, 4].Equals(currentA))
                {
                    currentA = array[i, 4];
                }
                if (!array[i, 5].Equals(currentB))
                {
                    currentB = array[i, 5];
                }
                if (!array[i, 6].Equals(currentC))
                {
                    currentC = array[i, 6];
                }

            _Controller.CoordMotion.StraightFeed(speed, currentX, currentY, currentZ, currentA, currentB, currentC, 0, 0);


            }


            

            _Controller.CoordMotion.WaitForSegmentsFinished(true);
            _Controller.CoordMotion.FlushSegments();
            return true;
        }

        public double GetDROX()
        {
           return _XAxis.GetActualPosition();
        }
        public double GetDROY()
        {
            return _YAxis.GetActualPosition();
        }
        public double GetDROZ()
        {
            return _ZAxis.GetActualPosition();
        }
        public double GetDROA()
        {
            return _AAxis.GetActualPosition();
        }
        public double GetDROB()
        {
            return _BAxis.GetActualPosition();
        }
        public double GetDROC()
        {
            return _CAxis.GetActualPosition();
        }
        public void EStop()
        {
            if (!eStopActive)
            {

                _XAxis.Disable();
                _YAxis.Disable();
                _ZAxis.Disable();
                _AAxis.Disable();
                _BAxis.Disable();
                _CAxis.Disable();
            }
            else
            {
                _XAxis.Enable();
                _YAxis.Enable();
                _ZAxis.Enable();
                _AAxis.Enable();
                _BAxis.Enable();
                _CAxis.Enable();
            }
        }
        public void JogAxis(string axis, bool direction)
        {
           // to do
            if (axis.Equals("X"))
            {
                if (direction)
                {
                    _XAxis.Jog(JogSpeed);
                }
                else
                {
                    _XAxis.Jog(-JogSpeed);
                }
            }
            if (axis.Equals("Y"))
            {
                if (direction)
                {
                    _YAxis.Jog(JogSpeed);
                }
                else
                {
                    _YAxis.Jog(-JogSpeed);
                }
            }
            if (axis.Equals("Z"))
            {
                if (direction)
                {
                    _ZAxis.Jog(JogSpeed);
                }
                else
                {
                    _ZAxis.Jog(-JogSpeed);
                }
            }
            if (axis.Equals("A"))
            {
                if (direction)
                {
                    _AAxis.Jog(JogSpeed);
                }
                else
                {
                    _AAxis.Jog(-JogSpeed);
                }
            }
            if (axis.Equals("B"))
            {
                if (direction)
                {
                    _BAxis.Jog(JogSpeed);
                }
                else
                {
                    _BAxis.Jog(-JogSpeed);
                }
            }
            if (axis.Equals("C"))
            {
                if (direction)
                {
                    _CAxis.Jog(JogSpeed);
                }
                else
                {
                    _CAxis.Jog(-JogSpeed);
                }
            }

        }
        public void JogAxisStop()
        {
            _XAxis.Stop();
            _XAxis.Stop();
            _YAxis.Stop();
            _ZAxis.Stop();
            _AAxis.Stop();
            _BAxis.Stop();
            _CAxis.Stop();
        }

        // event handlers for motion controller
        void Interpreter_Interpreter_CoordMotionStraightTranverse(double x, double y, double z, int sequence_number)
        {
            Console.WriteLine("Interpreter CoordMotion Straight Tranverse::  {0} | {1} | {2} | {3}", x, y, z, sequence_number);
        }

        void Interpreter_Interpreter_CoordMotionStraightFeed(double DesiredFeedRate_in_per_sec, double x, double y, double z, int sequence_number, int ID)
        {
            Console.WriteLine("Interpreter CoordMotion Straight Feed::  {0} | {1} | {2} | {3} | {4} | {5}", DesiredFeedRate_in_per_sec, x, y, z, sequence_number, ID);
        }

        void Interpreter_Interpreter_CoordMotionArcFeed(bool ZeroLenAsFullCircles, double DesiredFeedRate_in_per_sec, int plane, double first_end, double second_end, double first_axis, double second_axis, int rotation, double axis_end_point, double first_start, double second_start, double axis_start_point, int sequence_number, int ID)
        {
            Console.WriteLine("Interpreter CoordMotion Arc Feed::  {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9} | {10} | {11} | {12}",
                ZeroLenAsFullCircles,
                DesiredFeedRate_in_per_sec,
                plane, first_end,
                second_end,
                first_axis,
                second_axis,
                rotation,
                axis_end_point,
                first_start,
                second_start,
                axis_start_point,
                sequence_number,
                ID);
        }

        void Interpreter_InterpreterCompleted(int status, int lineno, int sequence_number, string err)
        {
            Console.WriteLine(String.Format("Interpreter Completed::  {0} | {1} | {2} | {3}", status, lineno, sequence_number, err));
            //            complete = true;
        }

        void Interpreter_InterpreterStatusUpdated(int lineno, string msg)
        {
            Console.WriteLine("Interpreter Status Update:");
            Console.WriteLine(lineno);
            Console.WriteLine(msg);
        }

        void Interpreter_InterpreterUserCallbackRequested(string msg)
        {
            Console.WriteLine("Interpreter User Callback:");
            Console.WriteLine(msg);
        }

        int Interpreter_InterpreterUserMCodeCallbackRequested(int code)
        {
            throw new NotImplementedException();
        }

        void AddHandlers()
        {


            //Set the callback for general messages
            _Controller.MessageReceived += new KMotion_dotNet.KMConsoleHandler(_Controller_MessageUpdated);
            //And Errors
            _Controller.ErrorReceived += new KMotion_dotNet.KMErrorHandler(_Controller_ErrorUpdated);


            //CoordMotion Callbacks 
            _Controller.CoordMotion.CoordMotionStraightTraverse += new KMotion_dotNet.KM_CoordMotionStraightTraverseHandler(CoordMotion_CoordMotionStraightTranverse);
            _Controller.CoordMotion.CoordMotionArcFeed += new KMotion_dotNet.KM_CoordMotionArcFeedHandler(CoordMotion_CoordMotionArcFeed);
            _Controller.CoordMotion.CoordMotionStraightFeed += new KMotion_dotNet.KM_CoordMotionStraightFeedHandler(CoordMotion_CoordMotionStraightFeed);


            //Set the Interpreter's callbacks
            _Controller.CoordMotion.Interpreter.InterpreterStatusUpdated += new KMotion_dotNet.KM_Interpreter.KM_GCodeInterpreterStatusHandler(Interpreter_InterpreterStatusUpdated);
            _Controller.CoordMotion.Interpreter.InterpreterCompleted += new KMotion_dotNet.KM_Interpreter.KM_GCodeInterpreterCompleteHandler(Interpreter_InterpreterCompleted);
            _Controller.CoordMotion.Interpreter.InterpreterUserCallbackRequested += new KMotion_dotNet.KM_Interpreter.KM_GCodeInterpreterUserCallbackHandler(Interpreter_InterpreterUserCallbackRequested);
            _Controller.CoordMotion.Interpreter.InterpreterUserMCodeCallbackRequested += new KMotion_dotNet.KM_Interpreter.KM_GCodeInterpreterUserMcodeCallbackHandler(Interpreter_InterpreterUserMCodeCallbackRequested);
        }

        static int _Controller_MessageUpdated(string message)
        {
            Console.WriteLine(message);
            return 0;
        }

        /// <summary>
        /// Handler for the error message pump
        /// </summary>
        /// <param name="message">error string</param>
        static void _Controller_ErrorUpdated(string message)
        {
            Console.WriteLine("#########################  ERROR  #########################");
            Console.WriteLine(message);
            Console.WriteLine("#########################  ERROR  #########################");
        }

        static void CoordMotion_CoordMotionStraightTranverse(double x, double y, double z, int sequence_number)
        {
            Console.WriteLine("CoordMotion Straight Tranverse::  {0} | {1} | {2} | {3}", x, y, z, sequence_number);
        }

        static void CoordMotion_CoordMotionStraightFeed(double DesiredFeedRate_in_per_sec, double x, double y, double z, int sequence_number, int ID)
        {
            Console.WriteLine("CoordMotion Straight Feed::  {0} | {1} | {2} | {3} | {4} | {5}", DesiredFeedRate_in_per_sec, x, y, z, sequence_number, ID);
        }

        static void CoordMotion_CoordMotionArcFeed(bool ZeroLenAsFullCircles, double DesiredFeedRate_in_per_sec, int plane, double first_end, double second_end, double first_axis,
            double second_axis, int rotation, double axis_end_point, double first_start, double second_start, double axis_start_point, int sequence_number, int ID)
        {
            Console.WriteLine("CoordMotion Arc Feed::  {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9} | {10} | {11} | {12}",
                ZeroLenAsFullCircles,
                DesiredFeedRate_in_per_sec,
                plane, first_end,
                second_end,
                first_axis,
                second_axis,
                rotation,
                axis_end_point,
                first_start,
                second_start,
                axis_start_point,
                sequence_number,
                ID);
        }
    }
}
