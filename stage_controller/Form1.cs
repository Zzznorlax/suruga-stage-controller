using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SurugaSeiki.Motion;
using TwinCAT.Ads;

namespace stage_controller
{
    public partial class Form1 : Form
    {

        SurugaSeiki.Motion.System alignSystem;
        string netAddr;
        AxisComponents[] axis = new AxisComponents[12];

        Alignment alignment;
        Alignment.SingleParameter singleParam;
        Alignment.FlatParameter flatParam;
        Alignment.FocusParameter focusParam;

        int axisNumber = 3;

        double relativeDistance = 50;
        public void SetAddress(string addr)
        {
            netAddr = addr;
        }

        public bool TestConnection()
        {
            if (!alignSystem.Connected)
            {
                return false;
            }

            return true;
        }

        public void TestTCConnection()
        {
            TcAdsClient tcClient;

            try
            {
                tcClient = new TcAdsClient();
                tcClient.Connect(AmsAddress.Parse(netAddr));
                Print(String.Format("TwinCat IsConnected: {0}", tcClient.IsConnected));
                Print(String.Format("TwinCat RouterState: {0}", tcClient.RouterState));
                Print(String.Format("TwinCat ConnectionState: {0}", tcClient.ConnectionState));
            }
            catch (Exception err)
            {
                Print(err.ToString());
            }
        }

        public void Print(string text)
        {
            textBoxConsole.AppendText(text);
            textBoxConsole.AppendText(Environment.NewLine);
        }

        public Form1()
        {
            InitializeComponent();

            SetAddress(netAddr);
            
            alignSystem = SurugaSeiki.Motion.System.Instance;
           
            
            alignment = new Alignment();

            Print("Initialized");

        }

        private void setAxis1Speed()
        {
            axis[0].SetMaxSpeed(Double.Parse(textBoxAxis1Speed.Text));
        }

        private void setAxis2Speed()
        {
            axis[1].SetMaxSpeed(Double.Parse(textBoxAxis2Speed.Text));
        }
        private void setAxis3Speed() 
        {
            axis[2].SetMaxSpeed(Double.Parse(textBoxAxis3Speed.Text));
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonAxis1Plus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis1Speed();
                axis[0].MoveRelative(Double.Parse(textBoxAxis1StepLength.Text));
            }
        }

        private void buttonAxis1Minus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis1Speed();
                axis[0].MoveRelative(-Double.Parse(textBoxAxis1StepLength.Text));
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonAMSConnect_Click(object sender, EventArgs e)
        {
            SetAddress(textBoxAMSNetID.Text);

            alignSystem.SetAddress(netAddr);

            for (int i = 0; i < axisNumber; i++)
            {
                axis[i] = new AxisComponents((ushort)(i + 1));
            }

            Print(String.Format("Connecting to address: {0}", textBoxAMSNetID.Text));

            Print("Connecting...");
            while (!alignSystem.Connected)
            {
               
            }

            Print("Connected");

            setFlatAlignmentParam();

            if (!backgroundWorkerPositions.IsBusy)
            {
                backgroundWorkerPositions.RunWorkerAsync();
            }
            
            if (!backgroundWorkerAlignmentStatus.IsBusy)
            {
                backgroundWorkerAlignmentStatus.RunWorkerAsync();
            }
        }

        private void buttonAxis1Org_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                axis[0].ReturnOrigin();
            }
        }

        private void buttonAxis3Org_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                axis[2].ReturnOrigin();
            }
        }

        private void buttonAxis2Org_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                axis[1].ReturnOrigin();
            }
        }

        private void buttonAxis3Plus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis3Speed();
                axis[2].MoveRelative(Double.Parse(textBoxAxis3StepLength.Text));
            }
        }

        private void buttonAxis3Minus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis3Speed();
                axis[2].MoveRelative(-Double.Parse(textBoxAxis3StepLength.Text));
            }
        }

        private void buttonAxis2Plus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis2Speed();
                axis[1].MoveRelative(Double.Parse(textBoxAxis2StepLength.Text));
            }
        }

        private void buttonAxis2Minus_Click(object sender, EventArgs e)
        {
            if (TestConnection())
            {
                setAxis2Speed();
                axis[1].MoveRelative(-Double.Parse(textBoxAxis2StepLength.Text));
            }
        }

        private void setFlatAlignmentParam()
        {
            flatParam.mainStageNumberX = (uint) numericUpDownX.Value;
            flatParam.mainStageNumberY = (uint) numericUpDownY.Value;
            flatParam.subStageNumberXY = 0;
            flatParam.subAngleX = 0.0;
            flatParam.subAngleY = 0.0;
            flatParam.pmCh = (uint) numericUpDownPMCH.Value;
            flatParam.analogCh = (uint) numericUpDownAnalogCH.Value;
            flatParam.pmAutoRangeUpOn = false;
            flatParam.pmInitRangeSettingOn = false;
            flatParam.pmInitRange = -20;
            flatParam.fieldSearchThreshold = (double) numericUpDownFieldSearch.Value;
            flatParam.peakSearchThreshold = (double) numericUpDownPeakSearch.Value;
            flatParam.searchRangeX = (double) numericUpDownSearchRangeX.Value;
            flatParam.searchRangeY = (double) numericUpDownSearchRangeY.Value;
            flatParam.fieldSearchPitchX = (double) numericUpDownPitchX.Value;
            flatParam.fieldSearchPitchY = (double) numericUpDownPitchY.Value;
            flatParam.fieldSearchFirstPitchX = 0;
            flatParam.fieldSearchSpeedX = (double) numericUpDownFieldSearchSpeedX.Value;
            flatParam.fieldSearchSpeedY = (double) numericUpDownFieldSearchSpeedY.Value;
            flatParam.peakSearchSpeedX = (double) numericUpDownPeakSearchSpeedX.Value;
            flatParam.peakSearchSpeedY = (double) numericUpDownPeakSearchSpeedY.Value;
            flatParam.smoothingRangeX = 50;
            flatParam.smoothingRangeY = 50;
            flatParam.centroidThresholdX = 0;
            flatParam.centroidThresholdY = 0;
            flatParam.convergentRangeX = 0.5;
            flatParam.convergentRangeY = 0.5;
            flatParam.comparisonCount = 2;
            flatParam.maxRepeatCount = 10;
        }

        private void buttonStartAlignment_Click(object sender, EventArgs e)
        {
            setFlatAlignmentParam();
            alignment.SetFlat(flatParam);

            Print(String.Format("Start aligning on stage {0} and {1}", flatParam.mainStageNumberX.ToString(), flatParam.mainStageNumberY.ToString()));
            alignment.StartFlat();
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonAxis1Stop_Click(object sender, EventArgs e)
        {
            axis[0].Stop();
        }

        private void buttonAxis2Stop_Click(object sender, EventArgs e)
        {
            axis[1].Stop();
        }

        private void buttonAxis3Stop_Click(object sender, EventArgs e)
        {
            axis[2].Stop();
        }

        private void buttonAligmnetStop_Click(object sender, EventArgs e)
        {
            alignment.Stop();
        }

        private void buttonAlignmentStatus_Click(object sender, EventArgs e)
        {
            Print(String.Format("Alignment status: {0}", alignment.GetStatus()));
        }

        private void backgroundWorkerPositions_DoWork(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                textBoxAxis1CP.Invoke((MethodInvoker)delegate
                {
                    textBoxAxis1CP.Text = axis[0].GetActualPosition().ToString();
                });

                textBoxAxis2CP.Invoke((MethodInvoker)delegate
                {
                    textBoxAxis2CP.Text = axis[1].GetActualPosition().ToString();
                });

                textBoxAxis3CP.Invoke((MethodInvoker)delegate
                {
                    textBoxAxis3CP.Text = axis[2].GetActualPosition().ToString();
                });

            }
        }

        private void backgroundWorkerAlignmentStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                labelAlignmentStatus.Invoke((MethodInvoker)delegate
                {
                    labelAlignmentStatus.Text = alignment.GetStatus().ToString();
                });

                labelPower.Invoke((MethodInvoker)delegate
                {
                    labelPower.Text = alignment.GetPower(flatParam.analogCh).ToString();
                });

                labelVoltage.Invoke((MethodInvoker)delegate
                {
                    labelVoltage.Text = alignment.GetVoltage(flatParam.analogCh).ToString();
                });

                System.Threading.Thread.Sleep(500);
            }
        }


        private void labelAlignmentStatus_Click(object sender, EventArgs e)
        {

        }

        private void CheckBoxServo_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxServo.Checked)
            {
                
                for (int i = 0; i < axisNumber; i++)
                {
                    axis[i].TurnOnServo();
                }
                CheckBoxServo.BackColor = Color.LightGreen;

                Print("Servo turned on");

            } else
            {
                for (int i = 0; i < axisNumber; i++)
                {
                    axis[i].TurnOffServo();
                }
                CheckBoxServo.BackColor = Color.LightCoral;

                Print("Servo turned off");

            }
        }

        private void buttonAxis1Go_Click(object sender, EventArgs e)
        {
            setAxis1Speed();
            axis[0].MoveAbsolute(Double.Parse(textBoxAxis1TargetPosition.Text));
        }

        private void buttonAxis2Go_Click(object sender, EventArgs e)
        {
            setAxis2Speed();
            axis[1].MoveAbsolute(Double.Parse(textBoxAxis2TargetPosition.Text));
        }

        private void buttonAxis3Go_Click(object sender, EventArgs e)
        {
            setAxis3Speed();
            axis[2].MoveAbsolute(Double.Parse(textBoxAxis3TargetPosition.Text));
        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }
    }
}
