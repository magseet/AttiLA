namespace BleDA.Test.TaskEditor
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
            this._createButton = new System.Windows.Forms.Button();
            this._tasksCommonBox = new System.Windows.Forms.ComboBox();
            this._actionListView = new System.Windows.Forms.ListView();
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // _createButton
            // 
            this._createButton.Location = new System.Drawing.Point(27, 19);
            this._createButton.Name = "_createButton";
            this._createButton.Size = new System.Drawing.Size(61, 22);
            this._createButton.TabIndex = 0;
            this._createButton.Text = "Create";
            this._createButton.UseVisualStyleBackColor = true;
            this._createButton.Click += new System.EventHandler(this._createButton_Click);
            // 
            // _tasksCommonBox
            // 
            this._tasksCommonBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._tasksCommonBox.FormattingEnabled = true;
            this._tasksCommonBox.Location = new System.Drawing.Point(27, 59);
            this._tasksCommonBox.Name = "_tasksCommonBox";
            this._tasksCommonBox.Size = new System.Drawing.Size(173, 21);
            this._tasksCommonBox.TabIndex = 1;
            this._tasksCommonBox.SelectedIndexChanged += new System.EventHandler(this._tasksCommonBox_SelectedIndexChanged);
            // 
            // _actionListView
            // 
            this._actionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.typeColumn,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this._actionListView.Location = new System.Drawing.Point(32, 104);
            this._actionListView.Name = "_actionListView";
            this._actionListView.Size = new System.Drawing.Size(484, 135);
            this._actionListView.TabIndex = 3;
            this._actionListView.UseCompatibleStateImageBehavior = false;
            this._actionListView.View = System.Windows.Forms.View.Details;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 261);
            this.Controls.Add(this._actionListView);
            this.Controls.Add(this._tasksCommonBox);
            this.Controls.Add(this._createButton);
            this.Name = "Form1";
            this.Text = "BleaDA test - Task Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _createButton;
        private System.Windows.Forms.ComboBox _tasksCommonBox;
        private System.Windows.Forms.ListView _actionListView;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

