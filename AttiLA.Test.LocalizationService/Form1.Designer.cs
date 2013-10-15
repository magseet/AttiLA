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
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxContextId = new System.Windows.Forms.TextBox();
            this.buttonChangeContextId = new System.Windows.Forms.Button();
            this.buttonTrackStart = new System.Windows.Forms.Button();
            this.buttonTrackStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonLocalize = new System.Windows.Forms.Button();
            this.listViewContexts = new System.Windows.Forms.ListView();
            this.columnHeaderContextId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSimilarity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Localization interval";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(339, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(86, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Auto localize";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Context ID";
            // 
            // textBoxContextId
            // 
            this.textBoxContextId.Location = new System.Drawing.Point(137, 79);
            this.textBoxContextId.Name = "textBoxContextId";
            this.textBoxContextId.Size = new System.Drawing.Size(172, 20);
            this.textBoxContextId.TabIndex = 5;
            // 
            // buttonChangeContextId
            // 
            this.buttonChangeContextId.Location = new System.Drawing.Point(339, 79);
            this.buttonChangeContextId.Name = "buttonChangeContextId";
            this.buttonChangeContextId.Size = new System.Drawing.Size(94, 20);
            this.buttonChangeContextId.TabIndex = 6;
            this.buttonChangeContextId.Text = "Change";
            this.buttonChangeContextId.UseVisualStyleBackColor = true;
            this.buttonChangeContextId.Click += new System.EventHandler(this.buttonChangeContextId_Click);
            // 
            // buttonTrackStart
            // 
            this.buttonTrackStart.Location = new System.Drawing.Point(137, 132);
            this.buttonTrackStart.Name = "buttonTrackStart";
            this.buttonTrackStart.Size = new System.Drawing.Size(75, 23);
            this.buttonTrackStart.TabIndex = 7;
            this.buttonTrackStart.Text = "Start";
            this.buttonTrackStart.UseVisualStyleBackColor = true;
            this.buttonTrackStart.Click += new System.EventHandler(this.buttonTrackStart_Click);
            // 
            // buttonTrackStop
            // 
            this.buttonTrackStop.Location = new System.Drawing.Point(234, 132);
            this.buttonTrackStop.Name = "buttonTrackStop";
            this.buttonTrackStop.Size = new System.Drawing.Size(75, 23);
            this.buttonTrackStop.TabIndex = 8;
            this.buttonTrackStop.Text = "Stop";
            this.buttonTrackStop.UseVisualStyleBackColor = true;
            this.buttonTrackStop.Click += new System.EventHandler(this.buttonTrackStop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Track mode";
            // 
            // buttonLocalize
            // 
            this.buttonLocalize.Location = new System.Drawing.Point(29, 184);
            this.buttonLocalize.Name = "buttonLocalize";
            this.buttonLocalize.Size = new System.Drawing.Size(94, 23);
            this.buttonLocalize.TabIndex = 10;
            this.buttonLocalize.Text = "Localize";
            this.buttonLocalize.UseVisualStyleBackColor = true;
            this.buttonLocalize.Click += new System.EventHandler(this.buttonLocalize_Click);
            // 
            // listViewContexts
            // 
            this.listViewContexts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderContextId,
            this.columnHeaderName,
            this.columnHeaderSimilarity});
            this.listViewContexts.Location = new System.Drawing.Point(34, 219);
            this.listViewContexts.Name = "listViewContexts";
            this.listViewContexts.Size = new System.Drawing.Size(398, 119);
            this.listViewContexts.TabIndex = 11;
            this.listViewContexts.UseCompatibleStateImageBehavior = false;
            this.listViewContexts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderContextId
            // 
            this.columnHeaderContextId.Text = "Context ID";
            this.columnHeaderContextId.Width = 166;
            // 
            // columnHeaderSimilarity
            // 
            this.columnHeaderSimilarity.Text = "Similarity";
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 376);
            this.Controls.Add(this.listViewContexts);
            this.Controls.Add(this.buttonLocalize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonTrackStop);
            this.Controls.Add(this.buttonTrackStart);
            this.Controls.Add(this.buttonChangeContextId);
            this.Controls.Add(this.textBoxContextId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += Form1_FormClosing;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   
        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxContextId;
        private System.Windows.Forms.Button buttonChangeContextId;
        private System.Windows.Forms.Button buttonTrackStart;
        private System.Windows.Forms.Button buttonTrackStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonLocalize;
        private System.Windows.Forms.ListView listViewContexts;
        private System.Windows.Forms.ColumnHeader columnHeaderContextId;
        private System.Windows.Forms.ColumnHeader columnHeaderSimilarity;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
    }
}

