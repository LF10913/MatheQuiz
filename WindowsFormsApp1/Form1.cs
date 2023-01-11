using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class MathQuiz : Form
    {
        public MathQuiz()
        {
            InitializeComponent();
        }

        // Create a Random object called randomizer 
        // to generate random numbers.
        Random randomizer = new Random();

        // These integer variables store the numbers 
        // for the addition problem. 
        int addend1;
        int addend2;


        // These integer variables store the numbers 
        // for the subtraction problem. 
        int minuend;
        int subtrahend;

        // These integer variables store the numbers 
        // for the multiplication problem. 
        int multiplicand;
        int multiplier;

        // These integer variables store the numbers 
        // for the division problem. 
        int dividend;
        int divisor;

        // This integer variable keeps track of the
        // remaining time
        int timeLeft;


        bool divisionValueIsChanged = false;
        bool subtractionValueIsChanged = false;
        bool multiplyValueIsChanged = false;
        bool additionValueIsChanged = false; 



        /// <summary>
        /// Start the quiz by filling in all of the problem 
        /// values and starting the timer. 
        /// </summary>
        public void StartTheQuiz()
        {
            // Fill in the addition problem.
            // Generate two random numbers to add.
            // Store the values in the variables 'addend1' and 'addend2'.
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);

            // Convert the two randomly generated numbers
            // into strings so that they can be displayed
            // in the label controls.
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();

            // 'sum' is the name of the NumericUpDown control.
            // This step makes sure its value is zero before
            // adding any values to it.
            sum.Value = 0;

            // Fill in the subtraction problem.
            minuend = randomizer.Next(1, 101);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();
            difference.Value = 0;

            // Fill in the multiplication problem.
            multiplicand = randomizer.Next(2, 11);
            multiplier = randomizer.Next(2, 11);
            timesLeftLabel.Text = multiplicand.ToString();
            timesRightLabel.Text = multiplier.ToString();
            product.Value = 0;

            // Fill in the division problem.
            divisor = randomizer.Next(2, 11);
            int temporaryQuotient = randomizer.Next(2, 11);
            dividend = divisor * temporaryQuotient;
            dividedLeftLabel.Text = dividend.ToString();
            dividedRightLabel.Text = divisor.ToString();
            quotient.Value = 0;

            // Start the timer.
            timeLeft = 1000000000;
            timeLabel.Text = "1000000000 seconds";
            timer1.Start();

            
        }

        private bool CheckAddition()
        {
            if(additionValueIsChanged)
            {
                bool additionIsCorrect = addend1 + addend2 == sum.Value;

                PlaySound(additionIsCorrect);
                additionValueIsChanged = false;

                return additionIsCorrect;

            }

            return false;
        }
        private bool CheckSubtraction()
        {
            if(subtractionValueIsChanged)
            {
                bool subtractionIsCorrect = minuend - subtrahend == difference.Value;

                PlaySound(subtractionIsCorrect);
                subtractionValueIsChanged = false;

                return subtractionIsCorrect;

            }

            return false;
        }
        private bool CheckMultiplication()
        {
            if(multiplyValueIsChanged)
            {
                bool multiplicationIsCorrect = multiplicand * multiplier == product.Value;

                PlaySound(multiplicationIsCorrect);
                multiplyValueIsChanged = false;

                return multiplicationIsCorrect;

            }

            return false;
        }
        private bool CheckDivision()
        {
            if(divisionValueIsChanged)
            {
                 bool disvisionIsCorrect = dividend / divisor == quotient.Value; // TRUE oder FALSE
                
                PlaySound(disvisionIsCorrect);
                divisionValueIsChanged = false;

                return disvisionIsCorrect;
            }
        
            return false;
        }

        // Sound abspielen wenn eigetippte Ergebniss richtig ist oder
        // wenn man den ergebniss geändert hat und es falsh ist
        private void PlaySound(bool playCorrectSound)
        {
            // Default soll das Wrong File abgespielt werden
            var soundFile = @"C:\Windows\Media\Windows Recycle.wav";
            if (playCorrectSound) // wenn playCorrectSound true ist dann soll andere Datei abgespielt werden
                soundFile = @"C:\Windows\Media\Windows Unlock.wav";
;

            var player = new SoundPlayer(soundFile);
            player.Play();
        }


         /// <summary>
        /// Check the answers to see if the user got everything right.
        /// </summary>
        /// <returns>True if the answer's correct, false otherwise.</returns>
        private bool CheckTheAnswer()
        {
            var additionIsCorrect = CheckAddition();
            var subtractionIsCorrect = CheckSubtraction();
            var multiplicationIsCorrect = CheckMultiplication();
            var divisionIsCorrect = CheckDivision();

            if (additionIsCorrect && subtractionIsCorrect &&
                multiplicationIsCorrect && divisionIsCorrect)
                return true;
            else
                return false;
        }



       
        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckTheAnswer())
            {
                // If CheckTheAnswer() returns true, then the user 
                // got the answer right. Stop the timer  
                // and show a MessageBox.
                timer1.Stop();
                MessageBox.Show("You got all the answers right!",
                                "Congratulations!");

                startButton.Enabled = true;
            }
            else if (timeLeft > 0)
            {
                // If CheckTheAnswer() returns false, keep counting
                // down. Decrease the time left by one second and 
                // display the new time left by updating the 
                // Time Left label.
                timeLeft = timeLeft - 1;
                timeLabel.Text = timeLeft + " seconds";
            }
            else
            {
                // If the user ran out of time, stop the timer, show
                // a MessageBox, and fill in the answers.
                timer1.Stop();
                timeLabel.Text = "Time's up!";
                MessageBox.Show("You didn't finish in time.", "Sorry!");
                sum.Value = addend1 + addend2;
                difference.Value = minuend - subtrahend;
                product.Value = multiplicand * multiplier;
                quotient.Value = dividend / divisor;
                startButton.Enabled = true;
            }
        }

        private void answer_Enter(object sender, EventArgs e)
        {

            // Select the whole answer in the NumericUpDown control.
            NumericUpDown answerBox = sender as NumericUpDown;

            if (answerBox != null)
            {
                int lengthOfAnswer = answerBox.Value.ToString().Length;
                answerBox.Select(0, lengthOfAnswer);
            }
        }

        private void divisionValueChanged(object sender, EventArgs e)
        {
            divisionValueIsChanged = true;

        }

        private void subtractionValueChanged(object sender, EventArgs e)
        {
            subtractionValueIsChanged = true;

        }

        private void multiplyValueChanged(object sender, EventArgs e)
        {
            multiplyValueIsChanged = true;

        }

        private void additionValueChanged(object sender, EventArgs e)
        {
            additionValueIsChanged = true;

        }
    }


}
