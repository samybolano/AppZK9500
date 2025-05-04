using AppZK9500.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libzkfpcsharp;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Text.RegularExpressions;
using AppZK9500.Models;
using System.Security.Cryptography;
using Microsoft.VisualBasic;


namespace AppZK9500
{
    public partial class Form1 : Form
    {
        IntPtr mDevHandle = IntPtr.Zero;
        IntPtr mDBHandle = IntPtr.Zero;
        IntPtr FormHandle = IntPtr.Zero;
        bool bIsTimeToDie = false;
        bool IsRegister = false;
        bool bIdentify = true;
        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 3;

        byte[][] RegTmps = new byte[3][];
        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];
        int cbCapTmp = 2048;
        int cbRegTmp = 0;
        int iFid = 1;

        private int mfpWidth = 0;
        private int mfpHeight = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public List<PersonModel>? Personas;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormHandle = this.Handle;
        }




        private void bnInit_Click()
        {
            cmbIdx.Items.Clear();
            int ret = zkfperrdef.ZKFP_ERR_OK;
            if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
            {
                int nCount = zkfp2.GetDeviceCount();
                if (nCount > 0)
                {
                    for (int i = 0; i < nCount; i++)
                    {
                        cmbIdx.Items.Add(i.ToString());
                    }
                    cmbIdx.SelectedIndex = 0;

                }
                else
                {
                    zkfp2.Terminate();
                    MessageBox.Show("No device connected!");
                }
            }
            else
            {
                MessageBox.Show("Initialize fail, ret=" + ret + " !");
            }
        }

        private void bnFree_Click(object sender, EventArgs e)
        {
            zkfp2.Terminate();
            cbRegTmp = 0;

        }

        private void bnOpen_Click()
        {
            int ret = zkfp.ZKFP_ERR_OK;
            if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(cmbIdx.SelectedIndex)))
            {
                MessageBox.Show("OpenDevice fail");
                return;
            }
            if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
            {
                MessageBox.Show("Init DB fail");
                zkfp2.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
                return;
            }
            GetAllPersons();







            RegisterCount = 0;
            cbRegTmp = 0;
            iFid = 1;
            for (int i = 0; i < 3; i++)
            {
                RegTmps[i] = new byte[2048];
            }
            byte[] paramValue = new byte[4];
            int size = 4;
            zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

            size = 4;
            zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

            FPBuffer = new byte[mfpWidth * mfpHeight];

            Thread captureThread = new Thread(new ThreadStart(DoCapture));
            captureThread.IsBackground = true;
            captureThread.Start();
            bIsTimeToDie = false;
            MessageBox.Show("Listo");


        }

        private void GetAllPersons()
        {
            using (var db = new MiDbContext())
            {
                Personas = db.Person.ToList();

                foreach (var reg in Personas)
                {

                    zkfp2.DBAdd(mDBHandle, reg.Id, reg.Huella);
                }
            }
        }

        private void DoCapture()
        {
            while (!bIsTimeToDie)
            {
                cbCapTmp = 2048;
                int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                }
                Thread.Sleep(200);
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        MemoryStream ms = new MemoryStream();
                        BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                        Bitmap bmp = new Bitmap(ms);
                        this.picFPImg.Image = bmp;
                        if (IsRegister)
                        {
                            int ret = zkfp.ZKFP_ERR_OK;
                            int fid = 0, score = 0;
                            ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                                RegistrarAccion registrarAccion = new RegistrarAccion(fid);
                                registrarAccion.Show();

                                //MessageBox.Show("This finger was already register by " + fid + "!");
                                return;
                            }
                            if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                MessageBox.Show("Presione el mismo dedo 3 veces para el registro");

                                return;
                            }
                            Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                            String strBase64 = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                            byte[] blob = zkfp2.Base64ToBlob(strBase64);
                            RegisterCount++;
                            if (RegisterCount >= REGISTER_FINGER_COUNT)
                            {
                                RegisterCount = 0;
                                if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)))
                                {
                                    string Name = Interaction.InputBox("Ingrese nombre a registrar", "", "");

                                    using (var context = new MiDbContext())
                                    {
                                        var persona = new PersonModel
                                        {
                                            Nombre = Name,
                                            Huella = RegTmp  // RegTmp es byte[]
                                        };

                                        context.Person.Add(persona);
                                        context.SaveChanges();
                                    }


                                    GetAllPersons();

                                    ///*iFid*/++;
                                    MessageBox.Show("Registro Satisfactorio");

                                }
                                else
                                {
                                    MessageBox.Show("enroll fail, error code=" + ret);

                                }
                                IsRegister = false;
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Presiona tu dedo " + (REGISTER_FINGER_COUNT - RegisterCount));
                            }
                        }
                        else
                        {
                            if (cbRegTmp <= 0)
                            {
                                MessageBox.Show("Primero registre su huella");
                                return;
                            }
                            if (bIdentify)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                if (zkfp.ZKFP_ERR_OK == ret)
                                {
                                    RegistrarAccion registrarAccion = new RegistrarAccion(fid);
                                    registrarAccion.Show();
                                    //MessageBox.Show("Identify succ, fid= " + fid + ",score=" + score + "!");

                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Identify fail, ret= " + ret);

                                    return;
                                }
                            }
                            else
                            {
                                int ret = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
                                if (0 < ret)
                                {
                                    MessageBox.Show("Match finger succ, score=" + ret + "!");

                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Match finger fail, ret= " + ret);
                                    return;
                                }
                            }
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }


        private void bnClose_Click(object sender, EventArgs e)
        {
            bIsTimeToDie = true;
            RegisterCount = 0;
            Thread.Sleep(1000);
            zkfp2.CloseDevice(mDevHandle);
        }

        private void bnEnroll_Click()
        {
            if (!IsRegister)
            {
                IsRegister = true;
                RegisterCount = 0;
                cbRegTmp = 0;
                //MessageBox.Show("Presione el dedo 3 veces");

            }
        }

        private void bnIdentify_Click(object sender, EventArgs e)
        {
            if (!bIdentify)
            {
                bIdentify = true;
                MessageBox.Show("Presione el dedo");

            }
        }

        private void bnVerify_Click(object sender, EventArgs e)
        {
            if (bIdentify)
            {
                bIdentify = false;
                MessageBox.Show("Presione el dedo");

            }
        }

        private void btnInicializarAll_Click(object sender, EventArgs e)
        {

            if (btnInicializarAll.Text == "Iniciar")
            {
                bnInit_Click();
                bnOpen_Click();
                bnEnroll_Click();
                btnInicializarAll.Text = "Terminar";
                btnInicializarAll.BackColor = Color.IndianRed;
            }
            else
            {
                zkfp2.Terminate();
                bIsTimeToDie = true;
                RegisterCount = 0;
                Thread.Sleep(1000);
                zkfp2.CloseDevice(mDevHandle);
                btnInicializarAll.Text = "Iniciar";
                btnInicializarAll.BackColor = Color.RoyalBlue;
            }

        }
    }

}

