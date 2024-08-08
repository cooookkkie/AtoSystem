namespace AdoNetWindow.Config
{
    partial class CountryConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CountryConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nupDelivery = new System.Windows.Forms.NumericUpDown();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.countryList = new System.Windows.Forms.ListView();
            this.Country = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Delivery = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.nupDelivery)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "국가명";
            // 
            // txtCountry
            // 
            this.txtCountry.Location = new System.Drawing.Point(59, 12);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(117, 21);
            this.txtCountry.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "배송기간";
            // 
            // nupDelivery
            // 
            this.nupDelivery.Location = new System.Drawing.Point(244, 13);
            this.nupDelivery.Name = "nupDelivery";
            this.nupDelivery.Size = new System.Drawing.Size(46, 21);
            this.nupDelivery.TabIndex = 3;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(306, 10);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(42, 23);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "등록";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(354, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(42, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // countryList
            // 
            this.countryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Country,
            this.Delivery});
            this.countryList.FullRowSelect = true;
            this.countryList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.countryList.HideSelection = false;
            this.countryList.Location = new System.Drawing.Point(12, 40);
            this.countryList.Name = "countryList";
            this.countryList.Size = new System.Drawing.Size(382, 262);
            this.countryList.TabIndex = 7;
            this.countryList.UseCompatibleStateImageBehavior = false;
            this.countryList.View = System.Windows.Forms.View.Details;
            this.countryList.SelectedIndexChanged += new System.EventHandler(this.countryList_SelectedIndexChanged);
            // 
            // Country
            // 
            this.Country.Text = "국가명";
            this.Country.Width = 260;
            // 
            // Delivery
            // 
            this.Delivery.Text = "배송기간";
            this.Delivery.Width = 90;
            // 
            // CountryConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(408, 314);
            this.Controls.Add(this.countryList);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.nupDelivery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CountryConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "국가별 설정";
            this.Load += new System.EventHandler(this.CountryConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nupDelivery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nupDelivery;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView countryList;
        private System.Windows.Forms.ColumnHeader Country;
        private System.Windows.Forms.ColumnHeader Delivery;
    }
}