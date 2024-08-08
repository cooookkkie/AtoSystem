namespace AdoNetWindow.Product
{
    partial class AddProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProduct));
            this.label1 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSIzes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInExpense = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTax = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCostUnit = new System.Windows.Forms.TextBox();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.rbWeightCalculate = new System.Windows.Forms.RadioButton();
            this.rbTrayCalculate = new System.Windows.Forms.RadioButton();
            this.txtProductionDays = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPurchaseMargin = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBaseArounndMonth = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "품명";
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(85, 38);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(183, 21);
            this.txtProduct.TabIndex = 1;
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(85, 65);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(183, 21);
            this.txtOrigin.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "원산지";
            // 
            // txtSIzes
            // 
            this.txtSIzes.Location = new System.Drawing.Point(85, 92);
            this.txtSIzes.Name = "txtSIzes";
            this.txtSIzes.Size = new System.Drawing.Size(183, 21);
            this.txtSIzes.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "규격";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(177, 119);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(91, 21);
            this.txtUnit.TabIndex = 7;
            this.txtUnit.Text = "0";
            this.txtUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "단위";
            // 
            // txtCustom
            // 
            this.txtCustom.Location = new System.Drawing.Point(177, 210);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(91, 21);
            this.txtCustom.TabIndex = 10;
            this.txtCustom.Text = "0";
            this.txtCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(12, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "관세 (%)";
            // 
            // txtInExpense
            // 
            this.txtInExpense.Location = new System.Drawing.Point(177, 237);
            this.txtInExpense.Name = "txtInExpense";
            this.txtInExpense.Size = new System.Drawing.Size(91, 21);
            this.txtInExpense.TabIndex = 11;
            this.txtInExpense.Text = "0";
            this.txtInExpense.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInExpense.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(12, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 16);
            this.label6.TabIndex = 10;
            this.label6.Text = "부대비용 (%)";
            // 
            // txtTax
            // 
            this.txtTax.Location = new System.Drawing.Point(177, 264);
            this.txtTax.Name = "txtTax";
            this.txtTax.Size = new System.Drawing.Size(91, 21);
            this.txtTax.TabIndex = 13;
            this.txtTax.Text = "0";
            this.txtTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(12, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "과세 (%)";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(204, 501);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(62, 32);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫기(X)";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(12, 501);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(62, 32);
            this.btnInsert.TabIndex = 19;
            this.btnInsert.Text = "등록(A)";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(11, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 123;
            this.label8.Text = "트레이";
            // 
            // txtCostUnit
            // 
            this.txtCostUnit.Location = new System.Drawing.Point(177, 146);
            this.txtCostUnit.Name = "txtCostUnit";
            this.txtCostUnit.Size = new System.Drawing.Size(91, 21);
            this.txtCostUnit.TabIndex = 8;
            this.txtCostUnit.Text = "0";
            this.txtCostUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCostUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // txtGroup
            // 
            this.txtGroup.Location = new System.Drawing.Point(85, 11);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(183, 21);
            this.txtGroup.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(11, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 16);
            this.label9.TabIndex = 124;
            this.label9.Text = "그룹";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(176, 385);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(91, 21);
            this.txtManager.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(10, 386);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 125;
            this.label10.Text = "담당자";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(11, 174);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 16);
            this.label11.TabIndex = 127;
            this.label11.Text = "계산방식";
            // 
            // rbWeightCalculate
            // 
            this.rbWeightCalculate.AutoSize = true;
            this.rbWeightCalculate.Checked = true;
            this.rbWeightCalculate.Location = new System.Drawing.Point(156, 174);
            this.rbWeightCalculate.Name = "rbWeightCalculate";
            this.rbWeightCalculate.Size = new System.Drawing.Size(47, 16);
            this.rbWeightCalculate.TabIndex = 9;
            this.rbWeightCalculate.TabStop = true;
            this.rbWeightCalculate.Text = "중량";
            this.rbWeightCalculate.UseVisualStyleBackColor = true;
            // 
            // rbTrayCalculate
            // 
            this.rbTrayCalculate.AutoSize = true;
            this.rbTrayCalculate.Location = new System.Drawing.Point(209, 174);
            this.rbTrayCalculate.Name = "rbTrayCalculate";
            this.rbTrayCalculate.Size = new System.Drawing.Size(59, 16);
            this.rbTrayCalculate.TabIndex = 9;
            this.rbTrayCalculate.Text = "트레이";
            this.rbTrayCalculate.UseVisualStyleBackColor = true;
            // 
            // txtProductionDays
            // 
            this.txtProductionDays.Location = new System.Drawing.Point(177, 291);
            this.txtProductionDays.Name = "txtProductionDays";
            this.txtProductionDays.Size = new System.Drawing.Size(91, 21);
            this.txtProductionDays.TabIndex = 14;
            this.txtProductionDays.Text = "20";
            this.txtProductionDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProductionDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustom_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(12, 292);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 16);
            this.label12.TabIndex = 128;
            this.label12.Text = "생산일";
            // 
            // txtPurchaseMargin
            // 
            this.txtPurchaseMargin.Location = new System.Drawing.Point(176, 318);
            this.txtPurchaseMargin.Name = "txtPurchaseMargin";
            this.txtPurchaseMargin.Size = new System.Drawing.Size(91, 21);
            this.txtPurchaseMargin.TabIndex = 15;
            this.txtPurchaseMargin.Text = "5";
            this.txtPurchaseMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(11, 319);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 16);
            this.label13.TabIndex = 130;
            this.label13.Text = "국내수입 마진 (%)";
            // 
            // txtBaseArounndMonth
            // 
            this.txtBaseArounndMonth.Location = new System.Drawing.Point(176, 345);
            this.txtBaseArounndMonth.Name = "txtBaseArounndMonth";
            this.txtBaseArounndMonth.Size = new System.Drawing.Size(91, 21);
            this.txtBaseArounndMonth.TabIndex = 16;
            this.txtBaseArounndMonth.Text = "2";
            this.txtBaseArounndMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(11, 346);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 16);
            this.label15.TabIndex = 134;
            this.label15.Text = "기준회전율(월)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("한컴 고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(11, 416);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 16);
            this.label14.TabIndex = 135;
            this.label14.Text = "비고";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(85, 416);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(181, 79);
            this.txtRemark.TabIndex = 18;
            this.txtRemark.Text = "";
            // 
            // AddProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 545);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtBaseArounndMonth);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtPurchaseMargin);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtProductionDays);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.rbTrayCalculate);
            this.Controls.Add(this.rbWeightCalculate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtManager);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCostUnit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.txtTax);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtInExpense);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCustom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSIzes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOrigin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProduct);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddProduct";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "임의 품목추가";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddProduct_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSIzes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInExpense;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCostUnit;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rbWeightCalculate;
        private System.Windows.Forms.RadioButton rbTrayCalculate;
        private System.Windows.Forms.TextBox txtProductionDays;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPurchaseMargin;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBaseArounndMonth;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RichTextBox txtRemark;
    }
}