namespace MingleView.UI
{
    partial class JoinMeetingView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textMeetingTicket = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textParticipantName = new System.Windows.Forms.TextBox();
            this.buttonJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the Meeting ticket:";
            // 
            // textMeetingTicket
            // 
            this.textMeetingTicket.Location = new System.Drawing.Point(33, 59);
            this.textMeetingTicket.Name = "textMeetingTicket";
            this.textMeetingTicket.Size = new System.Drawing.Size(168, 20);
            this.textMeetingTicket.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter your name:";
            // 
            // textParticipantName
            // 
            this.textParticipantName.Location = new System.Drawing.Point(30, 132);
            this.textParticipantName.Name = "textParticipantName";
            this.textParticipantName.Size = new System.Drawing.Size(170, 20);
            this.textParticipantName.TabIndex = 3;
            // 
            // buttonJoin
            // 
            this.buttonJoin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonJoin.Location = new System.Drawing.Point(152, 195);
            this.buttonJoin.Name = "buttonJoin";
            this.buttonJoin.Size = new System.Drawing.Size(84, 28);
            this.buttonJoin.TabIndex = 4;
            this.buttonJoin.Text = "Join";
            this.buttonJoin.UseVisualStyleBackColor = true;
            this.buttonJoin.Click += new System.EventHandler(this.buttonJoin_Click);
            // 
            // JoinMeeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 270);
            this.Controls.Add(this.buttonJoin);
            this.Controls.Add(this.textParticipantName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textMeetingTicket);
            this.Controls.Add(this.label1);
            this.Name = "JoinMeeting";
            this.Text = "Join Meeting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMeetingTicket;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textParticipantName;
        private System.Windows.Forms.Button buttonJoin;
    }
}