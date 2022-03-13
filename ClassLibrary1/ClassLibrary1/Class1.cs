using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;




namespace DataLibrary
{
    public enum VMf
    {
        VMSLGamma_VMDLGamma,
        VMSLN_VMDLN,
    };
    [Serializable]
    public class VMGird
    {
        private int length_vector;
        private float left_endpoint;
        private float right_endpoint;
        private float scale;
        public int Length_vector
        {
            get { return length_vector; }
            set { length_vector = value; }
        }
        public float Left_endpoint
        {
            get { return left_endpoint; }
            set { left_endpoint = value; }

        }
        public float Right_endpoint
        {
            get { return right_endpoint; }
            set { right_endpoint = value; }

        }
        public float Scale
        {
            get { return (right_endpoint - left_endpoint) / length_vector; }
            set { scale = value; }
        }

    }

    [Serializable]
    public struct VMtime
    {
        public VMGird vmGird { get; set; }
        public VMf vmf { get; set; }

        public double time_vmsln_HA, time_vmsln_EP, time_vmdln_EP, time_vmdln_HA;
        public double time_vmslGamma_HA, time_vmslGamma_EP, time_vmdlGamma_HA, time_vmdlGamma_EP;
        public float[] time_relation;

        //  time_relation[0]  // vms_ln(VML_HA)/vmdln(VML_HA)
        //  time_relation[1]  // vms_ln(VML_EP)/vmdln(VML_HA)
        //  time_relation[2]  // vmd_ln(VML_EP)/vmdln(VML_HA)
        // or time_relation[0]  // VMSL_Gamma(VML_HA)/VMDL_Gamma(VML_HA)
        //    time_relation[1]  // VMSL_Gamma(VML_EP)/VMDL_Gamma(VML_HA)
        //    time_relation[2]  // VMDL_Gamma(VML_EP)/VMDL_Gamma(VML_HA)
        public override string ToString()
        {
            string str = "";
            /*  str += "Vector's length:";
              str += vmGird.Length_vector + "\n";
              str += "Vector's left endpoint:";
              str += vmGird.Left_endpoint + "\n";
              str += "Vector's right endpoint:";
              str += vmGird.Right_endpoint + "\n";
              str += "Vector's scale:";
              str += vmGird.Scale + "\n";*/
            str += "Used time:" + "\n";

            if (this.vmf.ToString() == "VMSLN_VMDLN")
            {
                str += "vmsln_HA: " + time_vmsln_HA + " " + "vmsln_EP: " + time_vmsln_EP + " " + "vmdln_HA: " + time_vmdln_HA + " " + "vmdln_EP: " + time_vmdln_EP + " " + "\n";
                str += "time relation:";
                str += "vms_ln(VML_HA)/vmd_ln(VML_HA): " + time_relation[0] + "\n";
                str += "vms_ln(VML_EP)/vmd_ln(VML_HA): " + time_relation[1] + "\n";
                str += "vmd_ln(VML_EP)/vmd_ln(VML_HA): " + time_relation[2] + "\n";
            }
            else if (this.vmf.ToString() == "VMSLGamma_VMDLGamma")
            {
                str += "vmslGamma_HA: " + time_vmslGamma_HA + " " + "vmslGamma_EP: " + time_vmslGamma_EP + " " + "vmdlGamma_HA: " + time_vmdlGamma_HA + " " + "vmdlGamma_EP: " + time_vmdlGamma_EP + " " + "\n";
                str += "time relation:";
                str += "VMSL_Gamma(VML_HA)/VMDL_Gamma(VML_HA): " + time_relation[0] + "\n";
                str += "VMSL_Gamma(VML_EP)/VMDL_Gamma(VML_HA): " + time_relation[1] + "\n";
                str += "VMDL_Gamma(VML_EP)/VMDL_Gamma(VML_HA): " + time_relation[2] + "\n";
            }

            return str;
        }

    }
    [Serializable]
    public struct VMAccuracy
    {

        public VMGird vmGird { get; set; }
        public VMf vmf { get; set; }
        public double Max_error_module;
        public double MAX_error;
        public override string ToString()
        {
            string str = "";
            str += "Max_error_module: ";
            str += Max_error_module + "\n";
            str += "MAX_error:  ";
            str += MAX_error + "\n";
            return str;
        }
    }
    [Serializable]
    public class VMBenchmark
    {
        public ObservableCollection<VMtime> vMtimes = new ObservableCollection<VMtime>();
        public ObservableCollection<VMAccuracy> vMAccuracies = new ObservableCollection<VMAccuracy>();

        public void AddVMtime(VMf vMf, VMGird vMGird)
        {
            VMtime vMtime = new VMtime();
            vMtime.time_relation = new float[3];

            vMtime.vmGird = vMGird;
            vMtime.vmf = vMf;

            double[] d_x = new double[vMGird.Length_vector];
            float[] f_x = new float[vMGird.Length_vector];
            int ret = -1;
            f_x[0] = vMGird.Left_endpoint;
            for (int i = 1; i < vMGird.Length_vector; i++)
            {
                d_x[i] = d_x[i - 1] + vMGird.Scale;
                f_x[i] = f_x[i - 1] + vMGird.Scale;
            }
            if (vMf.ToString() == "VMSLN_VMDLN")
            {
                double[] timeVMS_LN = new double[2];
                double[] timeVMD_LN = new double[2];
                double[] d_y_HA = new double[vMGird.Length_vector];
                double[] d_y_EP = new double[vMGird.Length_vector];
                float[] f_y_HA = new float[vMGird.Length_vector];
                float[] f_y_EP = new float[vMGird.Length_vector];
                VMS_LN(vMGird.Length_vector, f_x, f_y_HA, f_y_EP, timeVMS_LN, ref ret);
                VMD_LN(vMGird.Length_vector, d_x, d_y_HA, d_y_EP, timeVMD_LN, ref ret);

                vMtime.time_vmsln_HA = timeVMS_LN[0];
                vMtime.time_vmsln_EP = timeVMS_LN[1];
                vMtime.time_vmdln_HA = timeVMD_LN[0];
                vMtime.time_vmdln_EP = timeVMD_LN[1];

                vMtime.time_relation[0] = (float)(timeVMS_LN[0] / timeVMD_LN[0]);
                vMtime.time_relation[1] = (float)(timeVMS_LN[1] / timeVMD_LN[0]);
                vMtime.time_relation[2] = (float)(timeVMD_LN[1] / timeVMD_LN[0]);
            }

            else if (vMf.ToString() == "VMSLGamma_VMDLGamma")
            {
                double[] d_y_HA_Gamma = new double[vMGird.Length_vector];
                double[] d_y_EP_Gamma = new double[vMGird.Length_vector];
                float[] f_y_HA_Gamma = new float[vMGird.Length_vector];
                float[] f_y_EP_Gamma = new float[vMGird.Length_vector];
                double[] timeVMSL_Gamma = new double[2];
                double[] timeVMDL_Gamma = new double[2];
                VMSL_Gamma(vMGird.Length_vector, f_x, f_y_HA_Gamma, f_y_EP_Gamma, timeVMSL_Gamma, ref ret);
                VMDL_Gamma(vMGird.Length_vector, d_x, d_y_HA_Gamma, d_y_EP_Gamma, timeVMDL_Gamma, ref ret);

                vMtime.time_vmslGamma_HA = timeVMSL_Gamma[0];
                vMtime.time_vmslGamma_EP = timeVMSL_Gamma[1];
                vMtime.time_vmdlGamma_HA = timeVMDL_Gamma[0];
                vMtime.time_vmdlGamma_EP = timeVMDL_Gamma[1];

                vMtime.time_relation[0] = (float)(timeVMSL_Gamma[0] / timeVMDL_Gamma[0]);
                vMtime.time_relation[1] = (float)(timeVMSL_Gamma[1] / timeVMDL_Gamma[0]);
                vMtime.time_relation[2] = (float)(timeVMDL_Gamma[1] / timeVMDL_Gamma[0]);

            }
            vMtimes.Add(vMtime);

        }

        public void AddVMAccuracy(VMf vMf, VMGird vMGird)
        {
            VMAccuracy vMAccuracy = new VMAccuracy();
            vMAccuracy.vmGird = vMGird;
            vMAccuracy.vmf = vMf;


            double max_error_moudle = -1;
            double max_error = -1;
            double[] d_x = new double[vMGird.Length_vector];
            float[] f_x = new float[vMGird.Length_vector];
            int ret = -1;

            f_x[0] = vMGird.Left_endpoint;
            for (int i = 1; i < vMGird.Length_vector; i++)
            {
                d_x[i] = d_x[i - 1] + vMGird.Scale;
                f_x[i] = f_x[i - 1] + vMGird.Scale;
            }


            if (vMf.ToString() == "VMSLN_VMDLN")
            {

                double[] timeVMS_LN = new double[2];
                double[] timeVMD_LN = new double[2];
                double[] d_y_HA = new double[vMGird.Length_vector];
                double[] d_y_EP = new double[vMGird.Length_vector];
                float[] f_y_HA = new float[vMGird.Length_vector];
                float[] f_y_EP = new float[vMGird.Length_vector];


                VMS_LN(vMGird.Length_vector, f_x, f_y_HA, f_y_EP, timeVMS_LN, ref ret);
                VMD_LN(vMGird.Length_vector, d_x, d_y_HA, d_y_EP, timeVMD_LN, ref ret);

                for (int i = 0; i < vMGird.Length_vector; i++)
                {
                    double module1 = Math.Abs(f_y_HA[i] - f_y_EP[i]) / Math.Abs(f_y_HA[i]);
                    double module2 = Math.Abs(d_y_HA[i] - d_y_EP[i]) / Math.Abs(d_y_HA[i]);
                    max_error_moudle = max_error_moudle > module1 ? max_error_moudle : module1;
                    max_error_moudle = max_error_moudle > module2 ? max_error_moudle : module2;

                    double error1 = Math.Abs(f_y_HA[i] - f_y_EP[i]);
                    double error2 = Math.Abs(d_y_HA[i] - d_y_EP[i]);
                    max_error = max_error > error1 ? max_error : error1;
                    max_error = max_error > error2 ? max_error : error2;
                }
            }
            else if (vMf.ToString() == "VMSLGamma_VMDLGamma")
            {
                double[] timeVMSL_Gamma = new double[2];
                double[] timeVMDL_Gamma = new double[2];
                double[] d_y_HA_Gamma = new double[vMGird.Length_vector];
                double[] d_y_EP_Gamma = new double[vMGird.Length_vector];
                float[] f_y_HA_Gamma = new float[vMGird.Length_vector];
                float[] f_y_EP_Gamma = new float[vMGird.Length_vector];

                VMSL_Gamma(vMGird.Length_vector, f_x, f_y_HA_Gamma, f_y_EP_Gamma, timeVMSL_Gamma, ref ret);
                VMDL_Gamma(vMGird.Length_vector, d_x, d_y_HA_Gamma, d_y_EP_Gamma, timeVMDL_Gamma, ref ret);

                for (int i = 0; i < vMGird.Length_vector; i++)
                {
                    double module3 = Math.Abs(f_y_HA_Gamma[i] - f_y_EP_Gamma[i]) / Math.Abs(f_y_HA_Gamma[i]);
                    double module4 = Math.Abs(d_y_HA_Gamma[i] - d_y_EP_Gamma[i]) / Math.Abs(d_y_HA_Gamma[i]);

                    max_error_moudle = max_error_moudle > module3 ? max_error_moudle : module3;
                    max_error_moudle = max_error_moudle > module4 ? max_error_moudle : module4;

                    double error3 = Math.Abs(f_y_HA_Gamma[i] - f_y_EP_Gamma[i]);
                    double error4 = Math.Abs(d_y_HA_Gamma[i] - d_y_EP_Gamma[i]);

                    max_error = max_error > error3 ? max_error : error3;
                    max_error = max_error > error4 ? max_error : error4;
                }
            }
            vMAccuracy.Max_error_module = max_error_moudle;
            vMAccuracy.MAX_error = max_error;
            vMAccuracies.Add(vMAccuracy);
        }
        public double max_time_relation
        {
            get
            {
                double Max_time_relation = 0;
                foreach (VMtime vMtime in vMtimes)
                {
                    double Max = vMtime.time_relation.Max();
                    Max_time_relation = Max_time_relation > Max ? Max_time_relation : Max;
                }
                return Max_time_relation;
            }

        }
        public double min_time_relation
        {
            get
            {
                double Min_time_relation = 0;
                foreach (VMtime vMtime in vMtimes)
                {
                    double Min = vMtime.time_relation.Min();
                    Min_time_relation = Min_time_relation > Min ? Min_time_relation : Min;
                }
                return Min_time_relation;
            }

        }

        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
          void VMS_LN(int n, float[] a, float[] y_HA, float[] y_EP, double[] time, ref int ret);
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
           void VMD_LN(int n, double[] a, double[] y_HA, double[] y_EP, double[] time, ref int ret);


        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        void VMSL_Gamma(int n, float[] a, float[] y_HA, float[] y_EP, double[] time, ref int ret);


        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
         void VMDL_Gamma(int n, double[] a, double[] y_HA, double[] y_EP, double[] time, ref int ret);


    }
}