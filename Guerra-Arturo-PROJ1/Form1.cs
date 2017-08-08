/*	Developer:Guerra,Arturo
	Course: MIS 4321 – Spring 2017
	Assignment: Project #1 - Vitals Dashboard
	Description: An interactive dashboard for a patients vitals.
                 User inputs data and gets results about his/her health
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Guerra_Arturo_PROJ1
{
    public partial class VitalsForm : Form
    {
        //Variables and constants
        const double BMI_MULT = 703;
        const int TOTAL_WIDTH = 400;
        const int MAX_SYSTOLIC = 200;
        const int MAX_DIASTOLIC = 133;
        const int HEART_RATE_MODIFIER = 220;
        const double LOWER_TARGET_RATE = .5;
        const double HIGHER_TARGET_RATE = .85;

        bool btnPressed;
        int validatedTextBoxes;

        Color white = Color.White;
        Color gray = Color.Gray;
        Color gold = Color.Gold;
        Color red = Color.Firebrick;
        Color orange = Color.Orange;

        public VitalsForm()
        {
            InitializeComponent();
        }

        private void VitalsForm_Load(object sender, EventArgs e)
        {
            //loading the test data
            LoadTestData();
            DisplayDashboard(false);
        }

        private void btnSubmitVitals_Click(object sender, EventArgs e)
        {
            //REQUIRES ALL TEXTBOXES TO BE FILLED WITH VALID DATA                   
            validatedTextBoxes = 0;
            DataValidation(txtAge, "Age");
            DataValidation(txtHeightFt, "Height in feet");
            DataValidation(txtHeightIn, "Height in inches");
            DataValidation(txtWeight, "Weight in pounds");
            DataValidation(txtPressureSys, "Systolic pressure");
            DataValidation(txtPressureDia, "Diastolic pressure");
            DataValidation(txtGlucose, "Glucose");

            //If all the textboxes are valid then its allowed to proceed
            if (validatedTextBoxes==7)
            {
                CalcHeartRate();
                CalcBMI();
                CalcGlucose();

                // prevents the button being "clicked again" so that the indicators dont fly off
                if (btnPressed)
                {
                    ResetIndicators();
                    CalcBloodPressureSystolic();
                    CalcBloodPressureDiastolic();
                }

                SetOverallHypertensionLevel();
                DisplayDashboard(true);
                btnPressed = false;
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //Clears the text, colours, and indicators
            btnPressed = true;

            txtAge.Text = "";
            txtHeightFt.Text = "";
            txtHeightIn.Text = "";
            txtWeight.Text = "";
            txtPressureSys.Text = "";
            txtPressureDia.Text = "";
            txtGlucose.Text = "";

            DisplayDashboard(false);

            ResetIndicators();
            ResetColors();
        }


        private void btnLoadTestData_Click(object sender, EventArgs e)
        {
            //loads the test data
            LoadTestData();
            txtAge.Focus();
        }


        private void LoadTestData()
        {
            //Basic test data from the project file
            txtAge.Text = 20.ToString();
            txtHeightFt.Text = 5.ToString();
            txtHeightIn.Text = 6.ToString();
            txtWeight.Text = (170.5).ToString();
            txtPressureSys.Text = 110.ToString();
            txtPressureDia.Text = 95.ToString();
            txtGlucose.Text = 90.ToString();

            DisplayDashboard(false);
            txtAge.Focus();
        }

        private void DisplayDashboard(bool makeVisible)
        {
            //Makes the panels visible
            pnlBloodPressure.Visible = makeVisible;
            pnlBMI.Visible = makeVisible;
            pnlGlucose.Visible = makeVisible;
            pnlHeartRate1.Visible = makeVisible;
            pnlHeartRate2.Visible = makeVisible;            
        }

        private void CalcHeartRate()
        {
            
            double dblMHR;
            double dblTargetMaxHeartRate;
            double dblTargetMinHeartRate;

            //calculates the ranges and max heart rate
            dblMHR = HEART_RATE_MODIFIER - Convert.ToDouble(txtAge.Text);
            dblTargetMinHeartRate = dblMHR * LOWER_TARGET_RATE;
            dblTargetMaxHeartRate = dblMHR * HIGHER_TARGET_RATE;

            //10 sec rule

            //Write to labels
            lblMaxBPM.Text = dblMHR.ToString();
            lblTargetBPMMin.Text = dblTargetMinHeartRate.ToString("N0");
            lblTargetBPMMax.Text = dblTargetMaxHeartRate.ToString("N0");

            lbl10SecMin.Text = ((dblTargetMinHeartRate / 60) * 10).ToString("N0");
            lbl10SecMax.Text = ((dblTargetMaxHeartRate / 60) * 10).ToString("N0");
            lbl10SecExceed.Text = ((dblMHR / 60) * 10).ToString("N0");
        }

        private void CalcBMI()
        {
            double dblBMI;
            double dblHeight;
            double dblWeight;

            int anIntX;
            int anIntY;

            //calculates the BMI and moves the indicators
            dblWeight = Convert.ToDouble(txtWeight.Text);
            dblHeight = (Convert.ToDouble(txtHeightFt.Text) * 12) + Convert.ToDouble(txtHeightIn.Text);

            dblBMI = ((dblWeight * BMI_MULT) / dblHeight) /dblHeight;
            lblBMIIndicator.Text = dblBMI.ToString("N1");
            

            if (dblBMI < 18.5)
            {
                anIntX = picBmiUnder.Location.X + 5;
                anIntY = picBmiUnder.Location.Y + 5;
                picIndicatorBmi.Location = new Point(anIntX, anIntY);
                picIndicatorBmi.BackColor = gold;

            }
            else if(dblBMI>=18.5 && dblBMI <= 24.9)
            {
                anIntX = picBmiHealthy.Location.X + 5;
                anIntY = picBmiHealthy.Location.Y + 5;
                picIndicatorBmi.Location = new Point(anIntX, anIntY);
                picIndicatorBmi.BackColor = gold;

            }
            else if (dblBMI >= 25 && dblBMI <= 29.9)
            {
                anIntX = picBmiOver.Location.X + 5;
                anIntY = picBmiOver.Location.Y + 5;
                picIndicatorBmi.Location = new Point(anIntX, anIntY);
                picIndicatorBmi.BackColor = red;


            }
            else if (dblBMI >= 30 && dblBMI <= 39.9)
            {
                anIntX = picBmiObese.Location.X + 5;
                anIntY = picBmiObese.Location.Y + 5;
                picIndicatorBmi.Location = new Point(anIntX, anIntY);
                picIndicatorBmi.BackColor = red;

            }
            else if (dblBMI >= 40)
            {
                anIntX = picBmiRisk.Location.X + 5;
                anIntY = picBmiRisk.Location.Y + 5;
                picIndicatorBmi.Location = new Point(anIntX, anIntY);
                picIndicatorBmi.BackColor = red;

            }

        }

        private void CalcGlucose()
        {
            double dblGlucose;

            int anIntX;
            int anIntY;

            //calculates the glucose levels and moves the indicators
            dblGlucose = Convert.ToDouble(txtGlucose.Text);
            lblGlucoseIndicator.Text = dblGlucose.ToString("N0");


            if (dblGlucose <= 69)
            {
                anIntX = picGlucoseLow.Location.X + 5;
                anIntY = picGlucoseLow.Location.Y + 5;
                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
                picIndicatorGlucose.BackColor = gold;

            }
            else if (dblGlucose >= 70 && dblGlucose <= 99)
            {
                anIntX = picGlucoseNormal.Location.X + 5;
                anIntY = picGlucoseNormal.Location.Y + 5;
                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
                picIndicatorGlucose.BackColor = gold;

            }
            else if (dblGlucose >= 100 && dblGlucose <= 125)
            {
                anIntX = picGlucosePre.Location.X + 5;
                anIntY = picGlucosePre.Location.Y + 5;
                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
                picIndicatorGlucose.BackColor = red;

            }

            else if (dblGlucose >= 126)
            {
                anIntX = picGlucoseDiab.Location.X + 5;
                anIntY = picGlucoseDiab.Location.Y + 5;
                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
                picIndicatorGlucose.BackColor = red;

            }
        }

        private void CalcBloodPressureSystolic()
        {
            int intSystolic;
            int anIntX;
            int anIntY;
            int shiftAmount;

            //calculates the blood pressure and moves the slider
            intSystolic = Convert.ToInt32(txtPressureSys.Text);
            lblSystolic.Text = intSystolic.ToString();

            anIntX = picIndicatorBPsystolic.Location.X;
            anIntY = picIndicatorBPsystolic.Location.Y;

            shiftAmount = TOTAL_WIDTH * intSystolic / MAX_SYSTOLIC;
            picIndicatorBPsystolic.Location = new Point(anIntX + shiftAmount, anIntY);

                anIntX = picIndicatorBPsystolic.Location.X;

            //the slider cant go past the bar
                if (anIntX >= 430)
                {
                    picIndicatorBPsystolic.Location = new Point(432, anIntY);

                }
                if (anIntX <= 30)
                {
                    picIndicatorBPsystolic.Location = new Point(36, anIntY);

                }

                //changes the colour of the bar depending on the pressure levels
            if (intSystolic < 120)
                {
                    picIndicatorBPsystolic.BackColor = gray;
                }
                if (intSystolic >= 120)
                {
                    picIndicatorBPsystolic.BackColor = gray;

                }
                if (intSystolic >= 140)
                {
                    picIndicatorBPsystolic.BackColor = orange;

                }
                 if (intSystolic >= 160)
                {
                    picIndicatorBPsystolic.BackColor = red;

                }
        }

        private void CalcBloodPressureDiastolic()
        {
            //same as the sys function except for diastolic
            int intDiastolic;
            int anIntX;
            int anIntY;
            int shiftAmount;

            intDiastolic = Convert.ToInt32(txtPressureDia.Text);
            lblDiastolic.Text = intDiastolic.ToString();

            anIntX = picIndicatorBPdiastolic.Location.X;
            anIntY = picIndicatorBPdiastolic.Location.Y;

            shiftAmount = TOTAL_WIDTH * intDiastolic / MAX_DIASTOLIC;
            picIndicatorBPdiastolic.Location = new Point(anIntX + shiftAmount, anIntY);

                anIntX = picIndicatorBPdiastolic.Location.X;

                if (anIntX >= 430)
                {
                    picIndicatorBPdiastolic.Location = new Point(432, anIntY);

                }
                if (anIntX <= 30)
                {
                    picIndicatorBPdiastolic.Location = new Point(36, anIntY);

                }
                    if (intDiastolic < 80)
                    {
                        picIndicatorBPdiastolic.BackColor = gray;
                    }
                     if (intDiastolic >= 80)
                    {
                        picIndicatorBPdiastolic.BackColor = gray;

                    }
                     if (intDiastolic >= 90)
                    {
                        picIndicatorBPdiastolic.BackColor = orange;

                    }
                     if (intDiastolic >= 100)
                    {
                        picIndicatorBPdiastolic.BackColor = red;

                    }
        }
        private void SetOverallHypertensionLevel()
        {
        
            int intSystolic;
            int intDiastolic;

            intSystolic = Convert.ToInt32(txtPressureSys.Text);
            intDiastolic = Convert.ToInt32(txtPressureDia.Text);

            //calculates the level of hyperdimension with the highest value determining the level, also changes the colour
            if (intSystolic < 120 && intDiastolic < 80)
            {
                ResetColors();
                lblHyperNormal.BackColor = gray;
                lblHyperNormal.ForeColor = white;

            }

            if ((intSystolic >= 120 && intSystolic < 140) || (intDiastolic >= 80 && intDiastolic < 90))
            {
                ResetColors();
                lblHyperPre.BackColor = Color.Orange;
                lblHyperPre.ForeColor = white;

            }

             if ((intSystolic >= 140 && intSystolic < 160)||(intDiastolic >= 90 && intDiastolic < 100))
            {
                ResetColors();
                lblHyperStage1.BackColor = red;
                lblHyperStage1.ForeColor = white;      

            }

             if (intSystolic >= 160 || intDiastolic >= 100)
            {
                ResetColors();
                lblHyperStage2.BackColor = red;
                lblHyperStage2.ForeColor = white;

                
            }
                      
        }

        //resets the colours to the defaults
        private void ResetColors()
        {
            lblHyperNormal.BackColor = white;
            lblHyperNormal.ForeColor = gray;

            lblHyperPre.BackColor = white;
            lblHyperPre.ForeColor = gray;

            lblHyperStage1.BackColor = white;
            lblHyperStage1.ForeColor = gray;

            lblHyperStage2.BackColor = white;
            lblHyperStage2.ForeColor = gray;

            

        }

        //resets the indicators to the starting positions
        private void ResetIndicators()
        {
            picIndicatorBPsystolic.Location = new Point(36, 66);

            picIndicatorBPdiastolic.Location = new Point(36, 124);

        }

        //Validates the textboxes so that each has a number, if it doesnt it doesnt validate it doesnt increase the counter, for the calculations to run it needs to add up to 7
        //7 being the number of textboxes
        private void DataValidation(TextBox txtBox, string name)
        {
            
            double val;

            if (!double.TryParse(txtBox.Text, out val))
            {
                MessageBox.Show(name + " is invalid. Must contain a number!");
                txtBox.BackColor = Color.PaleVioletRed;
                txtBox.Focus();
                validatedTextBoxes--;
                return;
            }
            else {
                validatedTextBoxes++;
            }


        }
              
        //when the text is changed it changes the colour of the background back to white so that it doesnt stay as red
        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            txtAge.BackColor = white;
        }

        private void txtPressureSys_TextChanged(object sender, EventArgs e)
        {
            btnPressed = true;
            txtPressureSys.BackColor = white;
        }

        private void txtPressureDia_TextChanged(object sender, EventArgs e)
        {
            btnPressed = true;
            txtPressureDia.BackColor = white;
        }

        private void txtHeightFt_TextChanged(object sender, EventArgs e)
        {
            txtHeightFt.BackColor = white;

        }

        private void txtHeightIn_TextChanged(object sender, EventArgs e)
        {
            txtHeightIn.BackColor = white;

        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            txtWeight.BackColor = white;

        }

        private void txtGlucose_TextChanged(object sender, EventArgs e)
        {
            txtGlucose.BackColor = white;

        }
    }
}
