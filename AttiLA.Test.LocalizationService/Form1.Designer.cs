namespace AttiLA.Test.LocalizationService
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnLocalizeTrack = new System.Windows.Forms.Button();
            this.btnTrackStop = new System.Windows.Forms.Button();
            this.btnPrediction = new System.Windows.Forms.Button();
            this.lstPreferences = new System.Windows.Forms.ListView();
            this.columnHeaderContextId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPreference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progbarLocalize = new System.Windows.Forms.ProgressBar();
            this.lstRecent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelected = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // btnLocalizeTrack
            // 
            this.btnLocalizeTrack.Location = new System.Drawing.Point(258, 314);
            this.btnLocalizeTrack.Name = "btnLocalizeTrack";
            this.btnLocalizeTrack.Size = new System.Drawing.Size(75, 23);
            this.btnLocalizeTrack.TabIndex = 7;
            this.btnLocalizeTrack.Text = "Track";
            this.btnLocalizeTrack.UseVisualStyleBackColor = true;
            this.btnLocalizeTrack.Click += new System.EventHandler(this.btnTrackStart_Click);
            // 
            // btnTrackStop
            // 
            this.btnTrackStop.Location = new System.Drawing.Point(339, 314);
            this.btnTrackStop.Name = "btnTrackStop";
            this.btnTrackStop.Size = new System.Drawing.Size(75, 23);
            this.btnTrackStop.TabIndex = 8;
            this.btnTrackStop.Text = "Stop";
            this.btnTrackStop.UseVisualStyleBackColor = true;
            this.btnTrackStop.Click += new System.EventHandler(this.btnTrackStop_Click);
            // 
            // btnPrediction
            // 
            this.btnPrediction.Location = new System.Drawing.Point(11, 156);
            this.btnPrediction.Name = "btnPrediction";
            this.btnPrediction.Size = new System.Drawing.Size(94, 23);
            this.btnPrediction.TabIndex = 10;
            this.btnPrediction.Text = "Prediction";
            this.btnPrediction.UseVisualStyleBackColor = true;
            this.btnPrediction.Click += new System.EventHandler(this.btnPrediction_Click);
            // 
            // lstPreferences
            // 
            this.lstPreferences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderContextId,
            this.columnHeaderName,
            this.columnHeaderPreference});
            this.lstPreferences.Location = new System.Drawing.Point(11, 195);
            this.lstPreferences.MultiSelect = false;
            this.lstPreferences.Name = "lstPreferences";
            this.lstPreferences.Size = new System.Drawing.Size(403, 97);
            this.lstPreferences.TabIndex = 11;
            this.lstPreferences.UseCompatibleStateImageBehavior = false;
            this.lstPreferences.View = System.Windows.Forms.View.Details;
            this.lstPreferences.SelectedIndexChanged += new System.EventHandler(this.lstPreferences_SelectedIndexChanged);
            // 
            // columnHeaderContextId
            // 
            this.columnHeaderContextId.Text = "Context ID";
            this.columnHeaderContextId.Width = 166;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderPreference
            // 
            this.columnHeaderPreference.Text = "Preference";
            this.columnHeaderPreference.Width = 113;
            // 
            // progbarLocalize
            // 
            this.progbarLocalize.Location = new System.Drawing.Point(119, 156);
            this.progbarLocalize.Maximum = 10000;
            this.progbarLocalize.Name = "progbarLocalize";
            this.progbarLocalize.Size = new System.Drawing.Size(295, 23);
            this.progbarLocalize.Step = 1;
            this.progbarLocalize.TabIndex = 12;
            // 
            // lstRecent
            // 
            this.lstRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstRecent.Location = new System.Drawing.Point(11, 47);
            this.lstRecent.MultiSelect = false;
            this.lstRecent.Name = "lstRecent";
            this.lstRecent.Size = new System.Drawing.Size(403, 92);
            this.lstRecent.TabIndex = 13;
            this.lstRecent.UseCompatibleStateImageBehavior = false;
            this.lstRecent.View = System.Windows.Forms.View.Details;
            this.lstRecent.SelectedIndexChanged += new System.EventHandler(this.lstRecent_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Context ID";
            this.columnHeader1.Width = 166;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Created";
            this.columnHeader3.Width = 135;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Selected:";
            // 
            // txtSelected
            // 
            this.txtSelected.Location = new System.Drawing.Point(74, 316);
            this.txtSelected.Name = "txtSelected";
            this.txtSelected.ReadOnly = true;
            this.txtSelected.Size = new System.Drawing.Size(178, 20);
            this.txtSelected.TabIndex = 15;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(94, 20);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 387);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtSelected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstRecent);
            this.Controls.Add(this.progbarLocalize);
            this.Controls.Add(this.lstPreferences);
            this.Controls.Add(this.btnPrediction);
            this.Controls.Add(this.btnTrackStop);
            this.Controls.Add(this.btnLocalizeTrack);
            this.Name = "Form1";
            this.Text = "AttiLA test LocalizationService";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   
        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnLocalizeTrack;
        private System.Windows.Forms.Button btnTrackStop;
        private System.Windows.Forms.Button btnPrediction;
        private System.Windows.Forms.ListView lstPreferences;
        private System.Windows.Forms.ColumnHeader columnHeaderContextId;
        private System.Windows.Forms.ColumnHeader columnHeaderPreference;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ProgressBar progbarLocalize;
        private System.Windows.Forms.ListView lstRecent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelected;
        private System.Windows.Forms.Button btnUpdate;
    }
}

