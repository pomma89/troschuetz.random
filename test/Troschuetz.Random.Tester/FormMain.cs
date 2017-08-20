// The MIT License (MIT)
//
// Copyright (c) 2006-2007 Stefan Troschütz <stefan@troschuetz.de>
//
// Copyright (c) 2012-2019 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Troschuetz.Random.Tester
{
    using Distributions.Continuous;
    using Generators;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using ZedGraph;
    using Label = System.Windows.Forms.Label;

    /// <summary>
    ///   Summary description for FormMain.
    /// </summary>
    public class FormMain : Form
    {
        IContainer components;

        #region Fields
        
        Button buttonClear;
        Button buttonDeselect;
        Button buttonDeselectAll;
        Button buttonSelect;
        Button buttonSelectAll;
        Button buttonTest;
        Button buttonTest2;
        Button buttonTestGenerators;
        CheckBox checkBoxBooleans;
        CheckBox checkBoxDoubles;
        CheckBox checkBoxDoublesMax;
        CheckBox checkBoxDoublesMinMax;
        CheckBox checkBoxHistogramBounds;
        CheckBox checkBoxIntegers;
        CheckBox checkBoxIntegersMax;
        CheckBox checkBoxIntegersMinMax;
        CheckBox checkBoxNext;
        CheckBox checkBoxNextBoolean;
        CheckBox checkBoxNextBytes;
        CheckBox checkBoxNextDouble;
        CheckBox checkBoxNextDoubleMax;
        CheckBox checkBoxNextDoubleMinMax;
        CheckBox checkBoxNextMax;
        CheckBox checkBoxNextMinMax;
        CheckBox checkBoxSmooth;
        CheckedListBox checkedListBoxDistributions;
        CheckedListBox checkedListBoxGenerators;
        ComboBox comboBoxDistribution;
        ComboBox comboBoxGenerator;
        ComboBox comboBoxGenerator2;

        /// <summary>
        ///   Stores the currently active inheritor of Distribution type.
        /// </summary>
        object currentDistribution;

        DataGridView dataGridViewGenerators;

        /// <summary>
        ///   Stores <see cref="Type"/> objects of inheritors of Distribution type.
        /// </summary>
        SortedList<string, Type> distributions;

        /// <summary>
        ///   Stores <see cref="Type"/> objects of inheritors of Generator type.
        /// </summary>
        SortedList<string, Type> generators;

        GroupBox groupBoxDistribution1;
        GroupBox groupBoxDistribution2;
        
        Label label1;
        Label label17;
        Label label18;
        Label label2;
        Label label3;
        Label label4;
        Label label5;
        Label label6;
        Label label7;
        Label label8;
        Label label9;
        NumericUpDown numericUpDownGenSamples;
        NumericUpDown numericUpDownMaximum;
        NumericUpDown numericUpDownMinimum;
        NumericUpDown numericUpDownSamples;
        NumericUpDown numericUpDownSamples2;
        NumericUpDown numericUpDownSteps;
        RichTextBox richTextBoxDistributionTest;
        RichTextBox richTextBoxTest;
        TabControl tabControl1;
        TabPage tabPageDistributions1;
        TabPage tabPageDistributions2;
        TabPage tabPageGenerators;

        /// <summary>
        ///   Stores the <see cref="Type"/> object for the Distribution type.
        /// </summary>
        Type typeDistribution;

        /// <summary>
        ///   Stores the <see cref="Type"/> object for the Generator type.
        /// </summary>
        Type typeGenerator;
        
        private DataGridViewTextBoxColumn Generator;
        private DataGridViewTextBoxColumn Next;
        private DataGridViewTextBoxColumn NextMax;
        private DataGridViewTextBoxColumn NextMinMax;
        private DataGridViewTextBoxColumn NextDouble;
        private DataGridViewTextBoxColumn NextDoubleMax;
        private DataGridViewTextBoxColumn NextDoubleMinMax;
        private DataGridViewTextBoxColumn Integers;
        private DataGridViewTextBoxColumn IntegersMax;
        private DataGridViewTextBoxColumn IntegersMinMax;
        private DataGridViewTextBoxColumn Doubles;
        private DataGridViewTextBoxColumn DoublesMax;
        private DataGridViewTextBoxColumn DoublesMinMax;
        private DataGridViewTextBoxColumn NextBoolean;
        private DataGridViewTextBoxColumn Booleans;
        private DataGridViewTextBoxColumn NextBytes;
        private DataGridViewTextBoxColumn Unit;

        ZedGraphControl zedGraphControl;

        #endregion Fields

        #region construction, destruction

        #region Windows Form Designer generated code

        /// <summary>
        ///   Required method for Designer support - do not modify the contents of this method with
        ///   the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabPageGenerators = new System.Windows.Forms.TabPage();
            this.checkBoxBooleans = new System.Windows.Forms.CheckBox();
            this.checkBoxDoublesMinMax = new System.Windows.Forms.CheckBox();
            this.checkBoxDoublesMax = new System.Windows.Forms.CheckBox();
            this.checkBoxDoubles = new System.Windows.Forms.CheckBox();
            this.checkBoxIntegersMinMax = new System.Windows.Forms.CheckBox();
            this.checkBoxIntegersMax = new System.Windows.Forms.CheckBox();
            this.checkBoxIntegers = new System.Windows.Forms.CheckBox();
            this.checkBoxNextBytes = new System.Windows.Forms.CheckBox();
            this.checkBoxNextBoolean = new System.Windows.Forms.CheckBox();
            this.checkBoxNextDoubleMinMax = new System.Windows.Forms.CheckBox();
            this.checkBoxNextDoubleMax = new System.Windows.Forms.CheckBox();
            this.checkBoxNextMinMax = new System.Windows.Forms.CheckBox();
            this.checkBoxNextDouble = new System.Windows.Forms.CheckBox();
            this.checkBoxNextMax = new System.Windows.Forms.CheckBox();
            this.checkBoxNext = new System.Windows.Forms.CheckBox();
            this.dataGridViewGenerators = new System.Windows.Forms.DataGridView();
            this.Generator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Next = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextMinMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextDouble = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextDoubleMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextDoubleMinMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Integers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntegersMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntegersMinMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Doubles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DoublesMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DoublesMinMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextBoolean = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Booleans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextBytes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numericUpDownGenSamples = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonTestGenerators = new System.Windows.Forms.Button();
            this.checkedListBoxGenerators = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonDeselect = new System.Windows.Forms.Button();
            this.tabPageDistributions2 = new System.Windows.Forms.TabPage();
            this.comboBoxGenerator2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.richTextBoxDistributionTest = new System.Windows.Forms.RichTextBox();
            this.numericUpDownSamples2 = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonTest2 = new System.Windows.Forms.Button();
            this.checkedListBoxDistributions = new System.Windows.Forms.CheckedListBox();
            this.label18 = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            this.tabPageDistributions1 = new System.Windows.Forms.TabPage();
            this.comboBoxGenerator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxDistribution = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxDistribution1 = new System.Windows.Forms.GroupBox();
            this.groupBoxDistribution2 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.numericUpDownSamples = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSteps = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSmooth = new System.Windows.Forms.CheckBox();
            this.checkBoxHistogramBounds = new System.Windows.Forms.CheckBox();
            this.numericUpDownMinimum = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMaximum = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBoxTest = new System.Windows.Forms.RichTextBox();
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGenerators.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGenerators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGenSamples)).BeginInit();
            this.tabPageDistributions2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSamples2)).BeginInit();
            this.tabPageDistributions1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximum)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageGenerators
            // 
            this.tabPageGenerators.Controls.Add(this.checkBoxBooleans);
            this.tabPageGenerators.Controls.Add(this.checkBoxDoublesMinMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxDoublesMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxDoubles);
            this.tabPageGenerators.Controls.Add(this.checkBoxIntegersMinMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxIntegersMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxIntegers);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextBytes);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextBoolean);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextDoubleMinMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextDoubleMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextMinMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextDouble);
            this.tabPageGenerators.Controls.Add(this.checkBoxNextMax);
            this.tabPageGenerators.Controls.Add(this.checkBoxNext);
            this.tabPageGenerators.Controls.Add(this.dataGridViewGenerators);
            this.tabPageGenerators.Controls.Add(this.numericUpDownGenSamples);
            this.tabPageGenerators.Controls.Add(this.label7);
            this.tabPageGenerators.Controls.Add(this.buttonTestGenerators);
            this.tabPageGenerators.Controls.Add(this.checkedListBoxGenerators);
            this.tabPageGenerators.Controls.Add(this.label6);
            this.tabPageGenerators.Controls.Add(this.buttonSelect);
            this.tabPageGenerators.Controls.Add(this.buttonDeselect);
            this.tabPageGenerators.Location = new System.Drawing.Point(4, 22);
            this.tabPageGenerators.Name = "tabPageGenerators";
            this.tabPageGenerators.Size = new System.Drawing.Size(1244, 710);
            this.tabPageGenerators.TabIndex = 1;
            this.tabPageGenerators.Text = "Generators";
            this.tabPageGenerators.UseVisualStyleBackColor = true;
            // 
            // checkBoxBooleans
            // 
            this.checkBoxBooleans.AutoSize = true;
            this.checkBoxBooleans.Location = new System.Drawing.Point(946, 47);
            this.checkBoxBooleans.Name = "checkBoxBooleans";
            this.checkBoxBooleans.Size = new System.Drawing.Size(76, 17);
            this.checkBoxBooleans.TabIndex = 20;
            this.checkBoxBooleans.Text = "Booleans()";
            this.checkBoxBooleans.UseVisualStyleBackColor = true;
            this.checkBoxBooleans.CheckedChanged += new System.EventHandler(this.CheckBoxBooleans_CheckedChanged);
            // 
            // checkBoxDoublesMinMax
            // 
            this.checkBoxDoublesMinMax.AutoSize = true;
            this.checkBoxDoublesMinMax.Location = new System.Drawing.Point(785, 72);
            this.checkBoxDoublesMinMax.Name = "checkBoxDoublesMinMax";
            this.checkBoxDoublesMinMax.Size = new System.Drawing.Size(151, 17);
            this.checkBoxDoublesMinMax.TabIndex = 19;
            this.checkBoxDoublesMinMax.Text = "DistributedDoubles(-99,99)";
            this.checkBoxDoublesMinMax.UseVisualStyleBackColor = true;
            this.checkBoxDoublesMinMax.CheckedChanged += new System.EventHandler(this.CheckBoxDoublesMinMax_CheckedChanged);
            // 
            // checkBoxDoublesMax
            // 
            this.checkBoxDoublesMax.AutoSize = true;
            this.checkBoxDoublesMax.Location = new System.Drawing.Point(785, 48);
            this.checkBoxDoublesMax.Name = "checkBoxDoublesMax";
            this.checkBoxDoublesMax.Size = new System.Drawing.Size(133, 17);
            this.checkBoxDoublesMax.TabIndex = 18;
            this.checkBoxDoublesMax.Text = "DistributedDoubles(99)";
            this.checkBoxDoublesMax.UseVisualStyleBackColor = true;
            this.checkBoxDoublesMax.CheckedChanged += new System.EventHandler(this.CheckBoxDoublesMax_CheckedChanged);
            // 
            // checkBoxDoubles
            // 
            this.checkBoxDoubles.AutoSize = true;
            this.checkBoxDoubles.Checked = true;
            this.checkBoxDoubles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDoubles.Location = new System.Drawing.Point(785, 24);
            this.checkBoxDoubles.Name = "checkBoxDoubles";
            this.checkBoxDoubles.Size = new System.Drawing.Size(121, 17);
            this.checkBoxDoubles.TabIndex = 17;
            this.checkBoxDoubles.Text = "DistributedDoubles()";
            this.checkBoxDoubles.UseVisualStyleBackColor = true;
            this.checkBoxDoubles.CheckedChanged += new System.EventHandler(this.CheckBoxDoubles_CheckedChanged);
            // 
            // checkBoxIntegersMinMax
            // 
            this.checkBoxIntegersMinMax.AutoSize = true;
            this.checkBoxIntegersMinMax.Location = new System.Drawing.Point(625, 72);
            this.checkBoxIntegersMinMax.Name = "checkBoxIntegersMinMax";
            this.checkBoxIntegersMinMax.Size = new System.Drawing.Size(150, 17);
            this.checkBoxIntegersMinMax.TabIndex = 16;
            this.checkBoxIntegersMinMax.Text = "DistributedIntegers(-99,99)";
            this.checkBoxIntegersMinMax.UseVisualStyleBackColor = true;
            this.checkBoxIntegersMinMax.CheckedChanged += new System.EventHandler(this.CheckBoxIntegersMinMax_CheckedChanged);
            // 
            // checkBoxIntegersMax
            // 
            this.checkBoxIntegersMax.AutoSize = true;
            this.checkBoxIntegersMax.Location = new System.Drawing.Point(625, 48);
            this.checkBoxIntegersMax.Name = "checkBoxIntegersMax";
            this.checkBoxIntegersMax.Size = new System.Drawing.Size(132, 17);
            this.checkBoxIntegersMax.TabIndex = 15;
            this.checkBoxIntegersMax.Text = "DistributedIntegers(99)";
            this.checkBoxIntegersMax.UseVisualStyleBackColor = true;
            this.checkBoxIntegersMax.CheckedChanged += new System.EventHandler(this.CheckBoxIntegersMax_CheckedChanged);
            // 
            // checkBoxIntegers
            // 
            this.checkBoxIntegers.AutoSize = true;
            this.checkBoxIntegers.Checked = true;
            this.checkBoxIntegers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIntegers.Location = new System.Drawing.Point(625, 24);
            this.checkBoxIntegers.Name = "checkBoxIntegers";
            this.checkBoxIntegers.Size = new System.Drawing.Size(120, 17);
            this.checkBoxIntegers.TabIndex = 13;
            this.checkBoxIntegers.Text = "DistributedIntegers()";
            this.checkBoxIntegers.UseVisualStyleBackColor = true;
            this.checkBoxIntegers.CheckedChanged += new System.EventHandler(this.CheckBoxIntegers_CheckedChanged);
            // 
            // checkBoxNextBytes
            // 
            this.checkBoxNextBytes.AutoSize = true;
            this.checkBoxNextBytes.Location = new System.Drawing.Point(946, 70);
            this.checkBoxNextBytes.Name = "checkBoxNextBytes";
            this.checkBoxNextBytes.Size = new System.Drawing.Size(118, 17);
            this.checkBoxNextBytes.TabIndex = 12;
            this.checkBoxNextBytes.Text = "NextBytes(byte[64])";
            this.checkBoxNextBytes.UseVisualStyleBackColor = true;
            this.checkBoxNextBytes.CheckedChanged += new System.EventHandler(this.CheckBoxNextBytes_CheckedChanged);
            // 
            // checkBoxNextBoolean
            // 
            this.checkBoxNextBoolean.AutoSize = true;
            this.checkBoxNextBoolean.Checked = true;
            this.checkBoxNextBoolean.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNextBoolean.Location = new System.Drawing.Point(946, 24);
            this.checkBoxNextBoolean.Name = "checkBoxNextBoolean";
            this.checkBoxNextBoolean.Size = new System.Drawing.Size(93, 17);
            this.checkBoxNextBoolean.TabIndex = 12;
            this.checkBoxNextBoolean.Text = "NextBoolean()";
            this.checkBoxNextBoolean.UseVisualStyleBackColor = true;
            this.checkBoxNextBoolean.CheckedChanged += new System.EventHandler(this.CheckBoxNextBoolean_CheckedChanged);
            // 
            // checkBoxNextDoubleMinMax
            // 
            this.checkBoxNextDoubleMinMax.AutoSize = true;
            this.checkBoxNextDoubleMinMax.Location = new System.Drawing.Point(500, 72);
            this.checkBoxNextDoubleMinMax.Name = "checkBoxNextDoubleMinMax";
            this.checkBoxNextDoubleMinMax.Size = new System.Drawing.Size(118, 17);
            this.checkBoxNextDoubleMinMax.TabIndex = 12;
            this.checkBoxNextDoubleMinMax.Text = "NextDouble(-99,99)";
            this.checkBoxNextDoubleMinMax.UseVisualStyleBackColor = true;
            this.checkBoxNextDoubleMinMax.CheckedChanged += new System.EventHandler(this.CheckBoxNextDoubleMinMax_CheckedChanged);
            // 
            // checkBoxNextDoubleMax
            // 
            this.checkBoxNextDoubleMax.AutoSize = true;
            this.checkBoxNextDoubleMax.Location = new System.Drawing.Point(500, 48);
            this.checkBoxNextDoubleMax.Name = "checkBoxNextDoubleMax";
            this.checkBoxNextDoubleMax.Size = new System.Drawing.Size(100, 17);
            this.checkBoxNextDoubleMax.TabIndex = 12;
            this.checkBoxNextDoubleMax.Text = "NextDouble(99)";
            this.checkBoxNextDoubleMax.UseVisualStyleBackColor = true;
            this.checkBoxNextDoubleMax.CheckedChanged += new System.EventHandler(this.CheckBoxNextDoubleMax_CheckedChanged);
            // 
            // checkBoxNextMinMax
            // 
            this.checkBoxNextMinMax.AutoSize = true;
            this.checkBoxNextMinMax.Location = new System.Drawing.Point(408, 72);
            this.checkBoxNextMinMax.Name = "checkBoxNextMinMax";
            this.checkBoxNextMinMax.Size = new System.Drawing.Size(84, 17);
            this.checkBoxNextMinMax.TabIndex = 12;
            this.checkBoxNextMinMax.Text = "Next(-99,99)";
            this.checkBoxNextMinMax.UseVisualStyleBackColor = true;
            this.checkBoxNextMinMax.CheckedChanged += new System.EventHandler(this.CheckBoxNextMinMax_CheckedChanged);
            // 
            // checkBoxNextDouble
            // 
            this.checkBoxNextDouble.AutoSize = true;
            this.checkBoxNextDouble.Checked = true;
            this.checkBoxNextDouble.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNextDouble.Location = new System.Drawing.Point(500, 24);
            this.checkBoxNextDouble.Name = "checkBoxNextDouble";
            this.checkBoxNextDouble.Size = new System.Drawing.Size(88, 17);
            this.checkBoxNextDouble.TabIndex = 12;
            this.checkBoxNextDouble.Text = "NextDouble()";
            this.checkBoxNextDouble.UseVisualStyleBackColor = true;
            this.checkBoxNextDouble.CheckedChanged += new System.EventHandler(this.CheckBoxNextDouble_CheckedChanged);
            // 
            // checkBoxNextMax
            // 
            this.checkBoxNextMax.AutoSize = true;
            this.checkBoxNextMax.Location = new System.Drawing.Point(408, 48);
            this.checkBoxNextMax.Name = "checkBoxNextMax";
            this.checkBoxNextMax.Size = new System.Drawing.Size(66, 17);
            this.checkBoxNextMax.TabIndex = 12;
            this.checkBoxNextMax.Text = "Next(99)";
            this.checkBoxNextMax.UseVisualStyleBackColor = true;
            this.checkBoxNextMax.CheckedChanged += new System.EventHandler(this.CheckBoxNextMax_CheckedChanged);
            // 
            // checkBoxNext
            // 
            this.checkBoxNext.AutoSize = true;
            this.checkBoxNext.Checked = true;
            this.checkBoxNext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNext.Location = new System.Drawing.Point(408, 24);
            this.checkBoxNext.Name = "checkBoxNext";
            this.checkBoxNext.Size = new System.Drawing.Size(54, 17);
            this.checkBoxNext.TabIndex = 12;
            this.checkBoxNext.Text = "Next()";
            this.checkBoxNext.UseVisualStyleBackColor = true;
            this.checkBoxNext.CheckedChanged += new System.EventHandler(this.CheckBoxNext_CheckedChanged);
            // 
            // dataGridViewGenerators
            // 
            this.dataGridViewGenerators.AllowUserToAddRows = false;
            this.dataGridViewGenerators.AllowUserToDeleteRows = false;
            this.dataGridViewGenerators.AllowUserToResizeColumns = false;
            this.dataGridViewGenerators.AllowUserToResizeRows = false;
            this.dataGridViewGenerators.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewGenerators.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewGenerators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGenerators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Generator,
            this.Next,
            this.NextMax,
            this.NextMinMax,
            this.NextDouble,
            this.NextDoubleMax,
            this.NextDoubleMinMax,
            this.Integers,
            this.IntegersMax,
            this.IntegersMinMax,
            this.Doubles,
            this.DoublesMax,
            this.DoublesMinMax,
            this.NextBoolean,
            this.Booleans,
            this.NextBytes,
            this.Unit});
            this.dataGridViewGenerators.Location = new System.Drawing.Point(208, 128);
            this.dataGridViewGenerators.Name = "dataGridViewGenerators";
            this.dataGridViewGenerators.ReadOnly = true;
            this.dataGridViewGenerators.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridViewGenerators.ShowEditingIcon = false;
            this.dataGridViewGenerators.Size = new System.Drawing.Size(1033, 545);
            this.dataGridViewGenerators.TabIndex = 11;
            // 
            // Generator
            // 
            this.Generator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Generator.Frozen = true;
            this.Generator.HeaderText = "Generator";
            this.Generator.Name = "Generator";
            this.Generator.ReadOnly = true;
            this.Generator.Width = 79;
            // 
            // Next
            // 
            this.Next.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Next.HeaderText = "Next()";
            this.Next.Name = "Next";
            this.Next.ReadOnly = true;
            this.Next.Width = 60;
            // 
            // NextMax
            // 
            this.NextMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextMax.HeaderText = "Next(99)";
            this.NextMax.Name = "NextMax";
            this.NextMax.ReadOnly = true;
            this.NextMax.Visible = false;
            // 
            // NextMinMax
            // 
            this.NextMinMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextMinMax.HeaderText = "Next(-99,99)";
            this.NextMinMax.Name = "NextMinMax";
            this.NextMinMax.ReadOnly = true;
            this.NextMinMax.Visible = false;
            // 
            // NextDouble
            // 
            this.NextDouble.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextDouble.HeaderText = "NextDouble()";
            this.NextDouble.Name = "NextDouble";
            this.NextDouble.ReadOnly = true;
            this.NextDouble.Width = 94;
            // 
            // NextDoubleMax
            // 
            this.NextDoubleMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextDoubleMax.HeaderText = "NextDouble(99)";
            this.NextDoubleMax.Name = "NextDoubleMax";
            this.NextDoubleMax.ReadOnly = true;
            this.NextDoubleMax.Visible = false;
            // 
            // NextDoubleMinMax
            // 
            this.NextDoubleMinMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextDoubleMinMax.HeaderText = "NextDouble(-99,99)";
            this.NextDoubleMinMax.Name = "NextDoubleMinMax";
            this.NextDoubleMinMax.ReadOnly = true;
            this.NextDoubleMinMax.Visible = false;
            // 
            // Integers
            // 
            this.Integers.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Integers.HeaderText = "DistributedIntegers()";
            this.Integers.Name = "Integers";
            this.Integers.ReadOnly = true;
            this.Integers.Width = 126;
            // 
            // IntegersMax
            // 
            this.IntegersMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IntegersMax.HeaderText = "DistributedIntegers(99)";
            this.IntegersMax.Name = "IntegersMax";
            this.IntegersMax.ReadOnly = true;
            this.IntegersMax.Visible = false;
            // 
            // IntegersMinMax
            // 
            this.IntegersMinMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.IntegersMinMax.HeaderText = "DistributedIntegers(-99,99)";
            this.IntegersMinMax.Name = "IntegersMinMax";
            this.IntegersMinMax.ReadOnly = true;
            this.IntegersMinMax.Visible = false;
            // 
            // Doubles
            // 
            this.Doubles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Doubles.HeaderText = "DistributedDoubles()";
            this.Doubles.Name = "Doubles";
            this.Doubles.ReadOnly = true;
            this.Doubles.Width = 127;
            // 
            // DoublesMax
            // 
            this.DoublesMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DoublesMax.HeaderText = "DistributedDoubles(99)";
            this.DoublesMax.Name = "DoublesMax";
            this.DoublesMax.ReadOnly = true;
            this.DoublesMax.Visible = false;
            // 
            // DoublesMinMax
            // 
            this.DoublesMinMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.DoublesMinMax.HeaderText = "DistributedDoubles(-99,99)";
            this.DoublesMinMax.Name = "DoublesMinMax";
            this.DoublesMinMax.ReadOnly = true;
            this.DoublesMinMax.Visible = false;
            // 
            // NextBoolean
            // 
            this.NextBoolean.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextBoolean.HeaderText = "NextBoolean()";
            this.NextBoolean.Name = "NextBoolean";
            this.NextBoolean.ReadOnly = true;
            this.NextBoolean.Width = 99;
            // 
            // Booleans
            // 
            this.Booleans.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Booleans.HeaderText = "Booleans()";
            this.Booleans.Name = "Booleans";
            this.Booleans.ReadOnly = true;
            this.Booleans.Visible = false;
            // 
            // NextBytes
            // 
            this.NextBytes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.NextBytes.HeaderText = "NextBytes(byte[64])";
            this.NextBytes.Name = "NextBytes";
            this.NextBytes.ReadOnly = true;
            this.NextBytes.Visible = false;
            // 
            // Unit
            // 
            this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Unit.HeaderText = "";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Width = 19;
            // 
            // numericUpDownGenSamples
            // 
            this.numericUpDownGenSamples.Location = new System.Drawing.Point(312, 64);
            this.numericUpDownGenSamples.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownGenSamples.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownGenSamples.Name = "numericUpDownGenSamples";
            this.numericUpDownGenSamples.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownGenSamples.TabIndex = 9;
            this.numericUpDownGenSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownGenSamples.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(208, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "Number of samples:";
            // 
            // buttonTestGenerators
            // 
            this.buttonTestGenerators.Location = new System.Drawing.Point(208, 88);
            this.buttonTestGenerators.Name = "buttonTestGenerators";
            this.buttonTestGenerators.Size = new System.Drawing.Size(184, 24);
            this.buttonTestGenerators.TabIndex = 6;
            this.buttonTestGenerators.Text = "Test selected generators";
            this.buttonTestGenerators.Click += new System.EventHandler(this.ButtonTestGenerators_Click);
            // 
            // checkedListBoxGenerators
            // 
            this.checkedListBoxGenerators.CheckOnClick = true;
            this.checkedListBoxGenerators.Location = new System.Drawing.Point(8, 24);
            this.checkedListBoxGenerators.Name = "checkedListBoxGenerators";
            this.checkedListBoxGenerators.Size = new System.Drawing.Size(184, 649);
            this.checkedListBoxGenerators.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Select generators to test:";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(208, 24);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(88, 24);
            this.buttonSelect.TabIndex = 6;
            this.buttonSelect.Text = "Select all";
            this.buttonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonDeselect
            // 
            this.buttonDeselect.Location = new System.Drawing.Point(304, 24);
            this.buttonDeselect.Name = "buttonDeselect";
            this.buttonDeselect.Size = new System.Drawing.Size(88, 24);
            this.buttonDeselect.TabIndex = 6;
            this.buttonDeselect.Text = "Deselect all";
            this.buttonDeselect.Click += new System.EventHandler(this.ButtonDeselect_Click);
            // 
            // tabPageDistributions2
            // 
            this.tabPageDistributions2.Controls.Add(this.comboBoxGenerator2);
            this.tabPageDistributions2.Controls.Add(this.label9);
            this.tabPageDistributions2.Controls.Add(this.richTextBoxDistributionTest);
            this.tabPageDistributions2.Controls.Add(this.numericUpDownSamples2);
            this.tabPageDistributions2.Controls.Add(this.label17);
            this.tabPageDistributions2.Controls.Add(this.buttonTest2);
            this.tabPageDistributions2.Controls.Add(this.checkedListBoxDistributions);
            this.tabPageDistributions2.Controls.Add(this.label18);
            this.tabPageDistributions2.Controls.Add(this.buttonSelectAll);
            this.tabPageDistributions2.Controls.Add(this.buttonDeselectAll);
            this.tabPageDistributions2.Location = new System.Drawing.Point(4, 22);
            this.tabPageDistributions2.Name = "tabPageDistributions2";
            this.tabPageDistributions2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDistributions2.Size = new System.Drawing.Size(1244, 710);
            this.tabPageDistributions2.TabIndex = 2;
            this.tabPageDistributions2.Text = "Distributions II";
            this.tabPageDistributions2.UseVisualStyleBackColor = true;
            // 
            // comboBoxGenerator2
            // 
            this.comboBoxGenerator2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenerator2.Location = new System.Drawing.Point(208, 24);
            this.comboBoxGenerator2.Name = "comboBoxGenerator2";
            this.comboBoxGenerator2.Size = new System.Drawing.Size(184, 21);
            this.comboBoxGenerator2.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(208, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 16);
            this.label9.TabIndex = 20;
            this.label9.Text = "Select an underlying generator :";
            // 
            // richTextBoxDistributionTest
            // 
            this.richTextBoxDistributionTest.Location = new System.Drawing.Point(208, 88);
            this.richTextBoxDistributionTest.Name = "richTextBoxDistributionTest";
            this.richTextBoxDistributionTest.ReadOnly = true;
            this.richTextBoxDistributionTest.Size = new System.Drawing.Size(1030, 584);
            this.richTextBoxDistributionTest.TabIndex = 18;
            this.richTextBoxDistributionTest.Text = "";
            // 
            // numericUpDownSamples2
            // 
            this.numericUpDownSamples2.Location = new System.Drawing.Point(504, 24);
            this.numericUpDownSamples2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownSamples2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownSamples2.Name = "numericUpDownSamples2";
            this.numericUpDownSamples2.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownSamples2.TabIndex = 17;
            this.numericUpDownSamples2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownSamples2.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(400, 24);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(112, 16);
            this.label17.TabIndex = 16;
            this.label17.Text = "Number of samples:";
            // 
            // buttonTest2
            // 
            this.buttonTest2.Location = new System.Drawing.Point(400, 56);
            this.buttonTest2.Name = "buttonTest2";
            this.buttonTest2.Size = new System.Drawing.Size(184, 24);
            this.buttonTest2.TabIndex = 15;
            this.buttonTest2.Text = "Test selected distributions";
            this.buttonTest2.Click += new System.EventHandler(this.ButtonTest2_Click);
            // 
            // checkedListBoxDistributions
            // 
            this.checkedListBoxDistributions.CheckOnClick = true;
            this.checkedListBoxDistributions.Location = new System.Drawing.Point(8, 23);
            this.checkedListBoxDistributions.Name = "checkedListBoxDistributions";
            this.checkedListBoxDistributions.Size = new System.Drawing.Size(184, 649);
            this.checkedListBoxDistributions.TabIndex = 12;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(136, 16);
            this.label18.TabIndex = 11;
            this.label18.Text = "Select distributions to test:";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(208, 56);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(88, 24);
            this.buttonSelectAll.TabIndex = 14;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.Click += new System.EventHandler(this.ButtonSelectAll_Click);
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(304, 56);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(88, 24);
            this.buttonDeselectAll.TabIndex = 13;
            this.buttonDeselectAll.Text = "Deselect all";
            this.buttonDeselectAll.Click += new System.EventHandler(this.ButtonDeselectAll_Click);
            // 
            // tabPageDistributions1
            // 
            this.tabPageDistributions1.Controls.Add(this.comboBoxGenerator);
            this.tabPageDistributions1.Controls.Add(this.label1);
            this.tabPageDistributions1.Controls.Add(this.comboBoxDistribution);
            this.tabPageDistributions1.Controls.Add(this.label8);
            this.tabPageDistributions1.Controls.Add(this.groupBoxDistribution1);
            this.tabPageDistributions1.Controls.Add(this.groupBoxDistribution2);
            this.tabPageDistributions1.Controls.Add(this.buttonClear);
            this.tabPageDistributions1.Controls.Add(this.label4);
            this.tabPageDistributions1.Controls.Add(this.label2);
            this.tabPageDistributions1.Controls.Add(this.buttonTest);
            this.tabPageDistributions1.Controls.Add(this.numericUpDownSamples);
            this.tabPageDistributions1.Controls.Add(this.numericUpDownSteps);
            this.tabPageDistributions1.Controls.Add(this.checkBoxSmooth);
            this.tabPageDistributions1.Controls.Add(this.checkBoxHistogramBounds);
            this.tabPageDistributions1.Controls.Add(this.numericUpDownMinimum);
            this.tabPageDistributions1.Controls.Add(this.label3);
            this.tabPageDistributions1.Controls.Add(this.numericUpDownMaximum);
            this.tabPageDistributions1.Controls.Add(this.label5);
            this.tabPageDistributions1.Controls.Add(this.richTextBoxTest);
            this.tabPageDistributions1.Controls.Add(this.zedGraphControl);
            this.tabPageDistributions1.Location = new System.Drawing.Point(4, 22);
            this.tabPageDistributions1.Name = "tabPageDistributions1";
            this.tabPageDistributions1.Size = new System.Drawing.Size(1244, 710);
            this.tabPageDistributions1.TabIndex = 0;
            this.tabPageDistributions1.Text = "Distributions I";
            this.tabPageDistributions1.UseVisualStyleBackColor = true;
            // 
            // comboBoxGenerator
            // 
            this.comboBoxGenerator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenerator.Location = new System.Drawing.Point(464, 8);
            this.comboBoxGenerator.Name = "comboBoxGenerator";
            this.comboBoxGenerator.Size = new System.Drawing.Size(184, 21);
            this.comboBoxGenerator.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(304, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Select an underlying generator :";
            // 
            // comboBoxDistribution
            // 
            this.comboBoxDistribution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDistribution.Location = new System.Drawing.Point(112, 8);
            this.comboBoxDistribution.Name = "comboBoxDistribution";
            this.comboBoxDistribution.Size = new System.Drawing.Size(184, 21);
            this.comboBoxDistribution.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "Select a distribution:";
            // 
            // groupBoxDistribution1
            // 
            this.groupBoxDistribution1.Location = new System.Drawing.Point(8, 40);
            this.groupBoxDistribution1.Name = "groupBoxDistribution1";
            this.groupBoxDistribution1.Size = new System.Drawing.Size(200, 24);
            this.groupBoxDistribution1.TabIndex = 3;
            this.groupBoxDistribution1.TabStop = false;
            this.groupBoxDistribution1.Text = "Distribution Characteristics";
            // 
            // groupBoxDistribution2
            // 
            this.groupBoxDistribution2.Location = new System.Drawing.Point(8, 72);
            this.groupBoxDistribution2.Name = "groupBoxDistribution2";
            this.groupBoxDistribution2.Size = new System.Drawing.Size(200, 24);
            this.groupBoxDistribution2.TabIndex = 4;
            this.groupBoxDistribution2.TabStop = false;
            this.groupBoxDistribution2.Text = "Distribution Parameters";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(112, 473);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(96, 24);
            this.buttonClear.TabIndex = 6;
            this.buttonClear.Text = "Clear histogram";
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 425);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Histogram minimum:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 361);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Histogram steps:";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(8, 473);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(96, 24);
            this.buttonTest.TabIndex = 6;
            this.buttonTest.Text = "Test distribution";
            this.buttonTest.Click += new System.EventHandler(this.ButtonTest_Click);
            // 
            // numericUpDownSamples
            // 
            this.numericUpDownSamples.Location = new System.Drawing.Point(112, 337);
            this.numericUpDownSamples.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownSamples.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownSamples.Name = "numericUpDownSamples";
            this.numericUpDownSamples.Size = new System.Drawing.Size(96, 20);
            this.numericUpDownSamples.TabIndex = 7;
            this.numericUpDownSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownSamples.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // numericUpDownSteps
            // 
            this.numericUpDownSteps.Location = new System.Drawing.Point(112, 361);
            this.numericUpDownSteps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSteps.Name = "numericUpDownSteps";
            this.numericUpDownSteps.Size = new System.Drawing.Size(96, 20);
            this.numericUpDownSteps.TabIndex = 7;
            this.numericUpDownSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownSteps.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // checkBoxSmooth
            // 
            this.checkBoxSmooth.Checked = true;
            this.checkBoxSmooth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSmooth.Location = new System.Drawing.Point(8, 385);
            this.checkBoxSmooth.Name = "checkBoxSmooth";
            this.checkBoxSmooth.Size = new System.Drawing.Size(200, 16);
            this.checkBoxSmooth.TabIndex = 10;
            this.checkBoxSmooth.Text = "Smooth histogram curves";
            this.checkBoxSmooth.CheckedChanged += new System.EventHandler(this.CheckBoxSmooth_CheckedChanged);
            // 
            // checkBoxHistogramBounds
            // 
            this.checkBoxHistogramBounds.Location = new System.Drawing.Point(8, 401);
            this.checkBoxHistogramBounds.Name = "checkBoxHistogramBounds";
            this.checkBoxHistogramBounds.Size = new System.Drawing.Size(200, 16);
            this.checkBoxHistogramBounds.TabIndex = 9;
            this.checkBoxHistogramBounds.Text = "Specify fixed histogram bounds";
            this.checkBoxHistogramBounds.CheckedChanged += new System.EventHandler(this.CheckBoxHistogramBounds_CheckedChanged);
            // 
            // numericUpDownMinimum
            // 
            this.numericUpDownMinimum.Enabled = false;
            this.numericUpDownMinimum.Location = new System.Drawing.Point(120, 425);
            this.numericUpDownMinimum.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDownMinimum.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDownMinimum.Name = "numericUpDownMinimum";
            this.numericUpDownMinimum.Size = new System.Drawing.Size(88, 20);
            this.numericUpDownMinimum.TabIndex = 7;
            this.numericUpDownMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownMinimum.Value = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDownMinimum.ValueChanged += new System.EventHandler(this.NumericUpDownMinimum_ValueChanged);
            this.numericUpDownMinimum.Validated += new System.EventHandler(this.NumericUpDownMinimum_Validated);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Number of samples:";
            // 
            // numericUpDownMaximum
            // 
            this.numericUpDownMaximum.Enabled = false;
            this.numericUpDownMaximum.Location = new System.Drawing.Point(120, 449);
            this.numericUpDownMaximum.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDownMaximum.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDownMaximum.Name = "numericUpDownMaximum";
            this.numericUpDownMaximum.Size = new System.Drawing.Size(88, 20);
            this.numericUpDownMaximum.TabIndex = 7;
            this.numericUpDownMaximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownMaximum.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownMaximum.ValueChanged += new System.EventHandler(this.NumericUpDownMaximum_ValueChanged);
            this.numericUpDownMaximum.Validated += new System.EventHandler(this.NumericUpDownMaximum_Validated);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 449);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "Histogram maximum:";
            // 
            // richTextBoxTest
            // 
            this.richTextBoxTest.Location = new System.Drawing.Point(8, 505);
            this.richTextBoxTest.Name = "richTextBoxTest";
            this.richTextBoxTest.ReadOnly = true;
            this.richTextBoxTest.Size = new System.Drawing.Size(200, 165);
            this.richTextBoxTest.TabIndex = 8;
            this.richTextBoxTest.Text = "";
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.Location = new System.Drawing.Point(216, 40);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.ScrollGrace = 0D;
            this.zedGraphControl.ScrollMaxX = 0D;
            this.zedGraphControl.ScrollMaxY = 0D;
            this.zedGraphControl.ScrollMaxY2 = 0D;
            this.zedGraphControl.ScrollMinX = 0D;
            this.zedGraphControl.ScrollMinY = 0D;
            this.zedGraphControl.ScrollMinY2 = 0D;
            this.zedGraphControl.Size = new System.Drawing.Size(1025, 630);
            this.zedGraphControl.TabIndex = 0;
            this.zedGraphControl.UseExtendedPrintDialog = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDistributions1);
            this.tabControl1.Controls.Add(this.tabPageDistributions2);
            this.tabControl1.Controls.Add(this.tabPageGenerators);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1252, 736);
            this.tabControl1.TabIndex = 11;
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1264, 730);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CodeProject Troschuetz.Random Tester";
            this.tabPageGenerators.ResumeLayout(false);
            this.tabPageGenerators.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGenerators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGenSamples)).EndInit();
            this.tabPageDistributions2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSamples2)).EndInit();
            this.tabPageDistributions1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximum)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        /// <summary>
        ///   Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        FormMain()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            zedGraphControl.GraphPane.Title.Text = "";
            zedGraphControl.GraphPane.XAxis.Title.Text = "X";
            zedGraphControl.GraphPane.YAxis.Title.Text = "";
            zedGraphControl.GraphPane.YAxis.Scale.IsVisible = false;

            LoadTroschuetzRandom();
        }

        /// <summary>
        ///   Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.
        /// </summary>
        /// <param name="disposing">
        ///   true to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion construction, destruction

        #region Class methods

        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ops... An error has occurred :-(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Class methods

        #region Instance methods

        /// <summary>
        ///   Loads the available random distributions and generators.
        /// </summary>
        void LoadTroschuetzRandom()
        {
            try
            {
                // Load the assembly and get the defined types.
                var assembly = typeof(TRandom).Assembly;
                var types = assembly.GetTypes();

                distributions = new SortedList<string, Type>(types.Length);
                generators = new SortedList<string, Type>(types.Length);
                foreach (var t in types)
                {
                    switch (t.FullName)
                    {
                        case "Troschuetz.Random.IDistribution":
                            typeDistribution = t;
                            break;

                        case "Troschuetz.Random.IGenerator":
                            typeGenerator = t;
                            break;

                        default:
                            if (t.IsInterface || t.IsAbstract || t.IsGenericType)
                            {
                                continue;
                            }
                            if (typeof(IDistribution).IsAssignableFrom(t))
                            {
                                distributions.Add(t.Name, t); // The type implements the IDistribution type.
                            }
                            else if (typeof(IGenerator).IsAssignableFrom(t))
                            {
                                generators.Add(t.Name, t); // The type implements the IGenerator type.
                            }
                            break;
                    }
                }
                distributions.TrimExcess();
                generators.TrimExcess();

                for (var index = 0; index < distributions.Count; index++)
                {
                    checkedListBoxDistributions.Items.Add(distributions.Values[index].Name);
                    comboBoxDistribution.Items.Add(distributions.Values[index].Name);
                }
                for (var index = 0; index < generators.Count; index++)
                {
                    checkedListBoxGenerators.Items.Add(generators.Values[index].Name);
                    comboBoxGenerator.Items.Add(generators.Values[index].Name);
                    comboBoxGenerator2.Items.Add(generators.Values[index].Name);
                }

                InitializeGroupBoxDistribution1();

                comboBoxGenerator.Items.Insert(0, "Distribution default");
                comboBoxGenerator.SelectedIndex = 0;
                comboBoxGenerator.SelectedValueChanged += ComboBoxGenerator_SelectedValueChanged;
                comboBoxGenerator2.Items.Insert(0, "Distribution default");
                comboBoxGenerator2.SelectedIndex = 0;

                comboBoxDistribution.SelectedValueChanged += ComboBoxDistribution_SelectedValueChanged;
                comboBoxDistribution.SelectedItem = distributions.Keys[0];
            }
            catch (Exception)
            {
                distributions = null;
                typeDistribution = null;

                comboBoxDistribution.Items.Clear();
                comboBoxDistribution.Text = "Error on loading distributions";
                checkedListBoxGenerators.Items.Clear();
                checkedListBoxGenerators.Items.Add("Error on loading distributions");

                generators = null;
                typeGenerator = null;

                comboBoxGenerator.Items.Clear();
                comboBoxGenerator.Text = "Error on loading generators";
                comboBoxGenerator2.Items.Clear();
                comboBoxGenerator2.Text = "Error on loading generators";
                checkedListBoxGenerators.Items.Clear();
                checkedListBoxGenerators.Items.Add("Error on loading generators");

                tabControl1.Enabled = false;
            }
        }

        /// <summary>
        ///   Formats a value of type <see cref="double"/> according to its absolute value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted value.</returns>
        static string FormatDouble(double value)
        {
            if (Math.Abs(value) >= 1000000 || (Math.Abs(value) < 0.001 && !TMath.IsZero(value)))
            {
                return value.ToString("0.###e+0");
            }
            return value.ToString("0.###");
        }

        #region methods regarding tabpage "Distributions I"

        /// <summary>
        ///   Tests the currently selected inheritor of the Distribution type.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonTest_Click(object sender, EventArgs e)
        {
            //Adjust GUI.
            comboBoxDistribution.Enabled = false;
            comboBoxGenerator.Enabled = false;
            groupBoxDistribution2.Enabled = false;
            numericUpDownSamples.Enabled = false;
            numericUpDownSteps.Enabled = false;
            checkBoxHistogramBounds.Enabled = false;
            numericUpDownMinimum.Enabled = false;
            numericUpDownMaximum.Enabled = false;
            checkBoxSmooth.Enabled = false;
            buttonTest.Enabled = false;
            buttonClear.Enabled = false;
            richTextBoxTest.Clear();
            zedGraphControl.Invalidate();
            Update();

            //Generate the samples.
            var distribution = (IDistribution) currentDistribution;
            var samples = new double[(int) numericUpDownSamples.Value];
            var watch = new Stopwatch();
            watch.Start();
            for (var index = 0; index < samples.Length; index++)
            {
                samples[index] = distribution.NextDouble();
            }
            watch.Stop();
            var duration = watch.ElapsedTicks / (double) Stopwatch.Frequency;

            //Determine sum, minimum, maximum and display the last two together with a computed mean value.
            double sum = 0, minimum = double.MaxValue, maximum = double.MinValue;
            foreach (var t in samples)
            {
                sum += t;
                if (t > maximum)
                {
                    maximum = t;
                }
                if (t < minimum)
                {
                    minimum = t;
                }
            }
            var mean = sum / samples.Length;
            var variance = samples.Sum(t => Math.Pow(t - mean, 2));
            variance /= samples.Length;

            richTextBoxTest.AppendText("Time elapsed for creating " + samples.Length + " samples:\n" +
                                       duration.ToString("#0.0000") + " s\n\n");
            richTextBoxTest.AppendText("Minimum: " + FormatDouble(minimum) + "\n\n");
            richTextBoxTest.AppendText("Maximum: " + FormatDouble(maximum) + "\n\n");
            richTextBoxTest.AppendText("Mean: " + FormatDouble(mean) + "\n\n");
            richTextBoxTest.AppendText("Variance: " + FormatDouble(variance));

            //If the user wants to apply its own histogram bounds, assign them.
            if (checkBoxHistogramBounds.Checked)
            {
                minimum = (double) numericUpDownMinimum.Value;
                maximum = (double) numericUpDownMaximum.Value;
            }

            //Compute the range of histogram and generate the histogram values.
            var range = maximum - minimum;
            double[] x, y;
            if (TMath.IsZero(range)) // cannot occur in case of user defined histogram bounds
            {
                //Samples are all the same, so use a fixed histogram.
                x = new[] { minimum, minimum + double.Epsilon };
                y = new double[] { samples.Length, 0 };
            }
            else {
                x = new double[(int) numericUpDownSteps.Value + 1];
                y = new double[(int) numericUpDownSteps.Value + 1];

                // Compute the histogram intervals (minimum bound of each interval is the x-value of
                // graph points). The last graph point represents the maximum bound of the last
                // histogram interval.
                for (var index = 0; index < x.Length - 1; index++)
                {
                    x[index] = minimum + range / (double) numericUpDownSteps.Value * index;
                }
                x[x.Length - 1] = maximum;

                // Iterate over samples and increase the histogram interval they lie inside.
                var samplesUsed = (int) numericUpDownSamples.Value;
                foreach (var s in samples)
                {
                    if (s < minimum || s > maximum)
                    {
                        // If user specified own histogram bounds, ignore samples that lie outside.
                        samplesUsed--;
                    }
                    else if (TMath.AreEqual(s, maximum))
                    {
                        // Maximum is part of last histogram interval
                        y[y.Length - 2]++;
                    }
                    else
                    {
                        var idx = (int) Math.Floor((s - minimum) / range * (double) numericUpDownSteps.Value);
                        idx = (idx < 0) ? 0 : (idx > y.Length - 2) ? y.Length - 2 : idx;
                        y[idx]++;
                    }
                }

                // Relate the number of samples inside each histogram interval to the overall number
                // of samples
                for (var index = 0; index < y.Length - 1; index++)
                {
                    y[index] = y[index] / samplesUsed * (double) numericUpDownSteps.Value;
                }

                // Assign the y-value of the last but one graph point to the last one, so that the
                // minimum and maximum bound of the last histogram interval share the same y-value
                y[y.Length - 1] = y[y.Length - 2];
            }

            // Add the test result to the graph.
            var label = currentDistribution.GetType().Name;
            for (var index = 0; index < groupBoxDistribution2.Controls.Count; index++)
            {
                var down = groupBoxDistribution2.Controls[index] as NumericUpDown;
                if (down != null)
                {
                    label += ("  " + down.Value.ToString("0.00"));
                }
            }
            var curves = 1 + zedGraphControl.GraphPane.CurveList.Count;
            Color color;
            if (curves > 12)
            {
                color = Color.Black;
            }
            else if (curves % 3 == 1)
            {
                color = Color.FromArgb(255 - curves * 10, 0, 0);
            }
            else if (curves % 3 == 2)
            {
                color = Color.FromArgb(0, 255 - curves * 10, 0);
            }
            else if (curves % 3 == 0)
            {
                color = Color.FromArgb(0, 0, 255 - curves * 10);
            }
            else {
                color = Color.Black;
            }
            var lineItem = zedGraphControl.GraphPane.AddCurve(label, x, y, color, SymbolType.None);
            lineItem.Line.StepType = StepType.ForwardStep;
            lineItem.Line.IsSmooth = checkBoxSmooth.Checked;
            lineItem.Line.SmoothTension = 1.0F;
            zedGraphControl.GraphPane.AxisChange(CreateGraphics());
            zedGraphControl.Invalidate();

            //Adjust GUI.
            comboBoxDistribution.Enabled = true;
            comboBoxGenerator.Enabled = true;
            groupBoxDistribution2.Enabled = true;
            numericUpDownSamples.Enabled = true;
            numericUpDownSteps.Enabled = true;
            checkBoxHistogramBounds.Enabled = true;
            numericUpDownMinimum.Enabled = checkBoxHistogramBounds.Checked;
            numericUpDownMaximum.Enabled = checkBoxHistogramBounds.Checked;
            checkBoxSmooth.Enabled = true;
            buttonTest.Enabled = true;
            buttonClear.Enabled = true;
        }

        /// <summary>
        ///   Create labels to display the values of all properties of Distribution type that are of
        ///   type <see cref="double"/> or array of <see cref="double"/>.
        /// </summary>
        void InitializeGroupBoxDistribution1()
        {
            var propertyInfos = typeDistribution.GetProperties();
            var count = 0;
            foreach (var propertyInfo in propertyInfos)
            {
                if ((propertyInfo.PropertyType != typeof(double) && propertyInfo.PropertyType != typeof(double[])) ||
                    !propertyInfo.CanRead)
                {
                    continue;
                }

                var label = new Label
                {
                    Location = new Point(8, 24 + count * 24),
                    Size = new Size(80, 16),
                    Text = propertyInfo.Name + ":"
                };
                groupBoxDistribution1.Controls.Add(label);

                label = new Label
                {
                    Location = new Point(96, 24 + count * 24),
                    Name = propertyInfo.Name,
                    Size = new Size(80, 16)
                };
                groupBoxDistribution1.Controls.Add(label);

                count++;
            }
            groupBoxDistribution1.Size = new Size(groupBoxDistribution1.Size.Width,
                                                  groupBoxDistribution1.Size.Height + count * 24);
            groupBoxDistribution2.Location = new Point(groupBoxDistribution2.Location.X,
                                                       groupBoxDistribution2.Location.Y + count * 24);
        }

        /// <summary>
        ///   Updates the displayed values of Distribution properties of the currently selected
        ///   inheritor of Distribution type.
        /// </summary>
        void UpdateGroupBoxDistribution1()
        {
            for (var index = 0; index < groupBoxDistribution1.Controls.Count; index++)
            {
                var label = (Label) groupBoxDistribution1.Controls[index];
                if (label.Name == "")
                {
                    continue;
                }

                var propertyInfo = currentDistribution.GetType().GetProperty(label.Name);
                if (propertyInfo.PropertyType == typeof(double))
                {
                    try
                    {
                        var value = (double) propertyInfo.GetValue(currentDistribution, null);
                        if (double.IsNaN(value))
                        {
                            label.Text = "Undefined";
                        }
                        else if (double.IsPositiveInfinity(value))
                        {
                            label.Text = "Pos. infinity";
                        }
                        else if (double.IsNegativeInfinity(value))
                        {
                            label.Text = "Neg. infinity";
                        }
                        else {
                            label.Text = FormatDouble(value);
                        }
                    }
                    catch
                    {
                        // Undefined properties should now throw an exception, rather then returning NaN.
                        label.Text = "Undefined";
                    }
                }
                else if (propertyInfo.PropertyType == typeof(double[]))
                {
                    try
                    {
                        var values = (double[]) propertyInfo.GetValue(currentDistribution, null);
                        label.Text = "";
                        for (var index2 = 0; index2 < values.Length; index2++)
                        {
                            label.Text += FormatDouble(values[index2]);
                            if (index2 < values.Length - 1)
                            {
                                label.Text += " | ";
                            }
                        }
                    }
                    catch
                    {
                        // Undefined properties should now throw an exception, rather then returning NaN.
                        label.Text = "Undefined";
                    }
                }
            }
        }

        /// <summary>
        ///   Create <see cref="NumericUpDown"/> controls for all properties of the currently
        ///   selected inheritor of Distribution type that are of type <see cref="double"/> or
        ///   <see cref="int"/> and not defined by the Distribution type.
        /// </summary>
        void UpdateGroupBoxDistribution2()
        {
            groupBoxDistribution2.Controls.Clear();

            // We get IDistribution properties, so we can eliminate them from the specific
            // distribution type.
            var distProps = typeDistribution.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var distPropsSet = new HashSet<string>(distProps.Select(p => p.Name));

            var propertyInfos = currentDistribution.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var count = 0;
            foreach (var propertyInfo in propertyInfos)
            {
                if (distPropsSet.Contains(propertyInfo.Name) || (propertyInfo.PropertyType != typeof(double) && propertyInfo.PropertyType != typeof(int)) ||
                    !propertyInfo.CanRead || !propertyInfo.CanWrite)
                {
                    continue;
                }

                var label = new Label
                {
                    Location = new Point(8, 24 + count * 24),
                    Size = new Size(80, 16),
                    Text = propertyInfo.Name + ":"
                };
                groupBoxDistribution2.Controls.Add(label);

                var num = new NumericUpDown();
                if (propertyInfo.PropertyType == typeof(double))
                {
                    num.DecimalPlaces = 2;
                }
                num.Increment = new decimal(Math.Pow(10, -1 * num.DecimalPlaces));
                num.Location = new Point(96, 24 + count * 24);
                if (propertyInfo.PropertyType == typeof(double))
                {
                    num.Minimum = decimal.MinValue;
                    num.Maximum = decimal.MaxValue;
                }
                else {
                    num.Minimum = new decimal(int.MinValue);
                    num.Maximum = new decimal(int.MaxValue);
                }
                num.Name = propertyInfo.Name;
                num.Size = new Size(96, 16);
                num.TextAlign = HorizontalAlignment.Right;
                num.CausesValidation = true;
                if (propertyInfo.PropertyType == typeof(double))
                {
                    num.Value = new decimal((double) propertyInfo.GetValue(currentDistribution, null));
                }
                else {
                    num.Value = new decimal((int) propertyInfo.GetValue(currentDistribution, null));
                }
                if (propertyInfo.PropertyType == typeof(double))
                {
                    num.Validated += Double_Validated;
                    num.ValueChanged += Double_ValueChanged;
                }
                else {
                    num.Validated += Int_Validated;
                    num.ValueChanged += Int_ValueChanged;
                }
                groupBoxDistribution2.Controls.Add(num);

                count++;
            }
            groupBoxDistribution2.Size = new Size(groupBoxDistribution2.Size.Width, 32 + count * 24);
        }

        /// <summary>
        ///   Selects an inheritor of Distribution type.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ComboBoxDistribution_SelectedValueChanged(object sender, EventArgs e)
        {
            if (distributions[comboBoxDistribution.Text].GetConstructor(new[] { typeGenerator }) != null)
            {
                comboBoxGenerator.Enabled = true;
                if (comboBoxGenerator.SelectedIndex == 0)
                {
                    currentDistribution = Activator.CreateInstance(distributions[comboBoxDistribution.Text]);
                }
                else {
                    currentDistribution = Activator.CreateInstance(distributions[comboBoxDistribution.Text],
                                                                   new[] {
                                                                             Activator.CreateInstance(
                                                                                 generators[comboBoxGenerator.Text])
                                                                         });
                }
            }
            else {
                comboBoxGenerator.Enabled = false;
                currentDistribution = Activator.CreateInstance(distributions[comboBoxDistribution.Text]);
            }
            UpdateGroupBoxDistribution1();
            UpdateGroupBoxDistribution2();
        }

        /// <summary>
        ///   Selects an inheritor of Generator type as underlying random number generator for
        ///   current inheritor of Distribution type.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ComboBoxGenerator_SelectedValueChanged(object sender, EventArgs e)
        {
            object newDistribution;
            if (comboBoxGenerator.SelectedIndex == 0)
            {
                newDistribution = Activator.CreateInstance(distributions[comboBoxDistribution.Text]);
            }
            else {
                newDistribution = Activator.CreateInstance(distributions[comboBoxDistribution.Text],
                                                           new[] {
                                                                     Activator.CreateInstance(generators[comboBoxGenerator.Text])
                                                                 });
            }
            var propertyInfos = currentDistribution.GetType().GetProperties();
            foreach (var propInfo in propertyInfos.Where(pi => pi.CanRead && pi.CanWrite))
            {
                propInfo.SetValue(newDistribution, propInfo.GetValue(currentDistribution, null), null);
            }
            currentDistribution = newDistribution;
            UpdateGroupBoxDistribution1();
            UpdateGroupBoxDistribution2();
        }

        /// <summary>
        ///   Assigns a new value to a property of the currently selected inheritor of the
        ///   Distribution type that is of type int and updates the displayed values of its
        ///   Distribution properties.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void Int_Validated(object sender, EventArgs e)
        {
            Int_ValueChanged(sender, e);
        }

        /// <summary>
        ///   Assigns a new value to a property of the currently selected inheritor of the
        ///   Distribution type that is of type int and updates the displayed values of its
        ///   Distribution properties.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void Int_ValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown) sender;
            var propertyInfo = currentDistribution.GetType().GetProperty(num.Name);
            var methodInfo = currentDistribution.GetType().GetMethod("IsValid" + num.Name);

            if (methodInfo == null || (bool) methodInfo.Invoke(currentDistribution, new object[] { (int) num.Value }))
            {
                // Either there is no method that checks for validity or the new value is valid.
                // Assign the new value to the distribution and update the GroupBox with base class infos.
                propertyInfo.SetValue(currentDistribution, (int) num.Value, null);
                UpdateGroupBoxDistribution1();
            }
            else {
                // The new value isn't valid. Reassign the current value of the distribution to the
                // NumericUpDown control.
                num.Value = new decimal((int) propertyInfo.GetValue(currentDistribution, null));
            }
        }

        /// <summary>
        ///   Assigns a new value to a property of the currently selected inheritor of the
        ///   Distribution type that is of type double and updates the displayed values of its
        ///   Distribution properties.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void Double_Validated(object sender, EventArgs e)
        {
            Double_ValueChanged(sender, e);
        }

        /// <summary>
        ///   Assigns a new value to a property of the currently selected inheritor of the
        ///   Distribution type that is of type double and updates the displayed values of its
        ///   Distribution properties.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void Double_ValueChanged(object sender, EventArgs e)
        {
            var num = (NumericUpDown) sender;
            var propertyInfo = currentDistribution.GetType().GetProperty(num.Name);
            var methodInfo = currentDistribution.GetType().GetMethod("IsValid" + num.Name);

            if (methodInfo == null || (bool) methodInfo.Invoke(currentDistribution, new object[] { (double) num.Value }))
            {
                // Either there is no method that checks for validity or the new value is valid.
                // Assign the new value to the distribution and update the GroupBox with base class infos.
                propertyInfo.SetValue(currentDistribution, (double) num.Value, null);
                UpdateGroupBoxDistribution1();
            }
            else {
                // The new value isn't valid. Reassign the current value of the distribution to the
                // NumericUpDown control.
                num.Value = new decimal((double) propertyInfo.GetValue(currentDistribution, null));
            }
        }

        /// <summary>
        ///   User wants to enable or disable specific histogram bounds.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxHistogramBounds_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownMinimum.Enabled = checkBoxHistogramBounds.Checked;
            numericUpDownMaximum.Enabled = checkBoxHistogramBounds.Checked;
        }

        /// <summary>
        ///   Checks whether the specified value of <see cref="numericUpDownMinimum"/> is smaller
        ///   than the one of <see cref="numericUpDownMaximum"/> and corrects it, if necessary.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void NumericUpDownMinimum_Validated(object sender, EventArgs e)
        {
            NumericUpDownMinimum_ValueChanged(sender, e);
        }

        /// <summary>
        ///   Checks whether the specified value of <see cref="numericUpDownMinimum"/> is smaller
        ///   than the one of <see cref="numericUpDownMaximum"/> and corrects it, if necessary.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void NumericUpDownMinimum_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownMinimum.Value >= numericUpDownMaximum.Value)
            {
                numericUpDownMinimum.Value = numericUpDownMaximum.Value - decimal.One;
            }
        }

        /// <summary>
        ///   Checks whether the specified value of <see cref="numericUpDownMaximum"/> is greater
        ///   than the one of <see cref="numericUpDownMinimum"/> and corrects it, if necessary.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void NumericUpDownMaximum_Validated(object sender, EventArgs e)
        {
            NumericUpDownMaximum_ValueChanged(sender, e);
        }

        /// <summary>
        ///   Checks whether the specified value of <see cref="numericUpDownMaximum"/> is greater
        ///   than the one of <see cref="numericUpDownMinimum"/> and corrects it, if necessary.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void NumericUpDownMaximum_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownMaximum.Value <= numericUpDownMinimum.Value)
            {
                numericUpDownMaximum.Value = numericUpDownMinimum.Value + decimal.One;
            }
        }

        /// <summary>
        ///   Switch Smooth all curves.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxSmooth_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var t in zedGraphControl.GraphPane.CurveList)
            {
                var lineItem = (LineItem) t;
                lineItem.Line.IsSmooth = checkBoxSmooth.Checked;
                lineItem.Line.SmoothTension = 1.0F;
            }
            zedGraphControl.Invalidate();
        }

        /// <summary>
        ///   Deletes all curves from the graph.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonClear_Click(object sender, EventArgs e)
        {
            zedGraphControl.GraphPane.CurveList.Clear();
            zedGraphControl.Invalidate();
        }

        #endregion methods regarding tabpage "Distributions I"

        #region methods regarding tabpage "Distributions II"

        /// <summary>
        ///   Selects all random distributions in the checked listbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < checkedListBoxDistributions.Items.Count; index++)
            {
                checkedListBoxDistributions.SetItemChecked(index, true);
            }
        }

        /// <summary>
        ///   Deselects all random distributions in the checked listbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonDeselectAll_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < checkedListBoxDistributions.Items.Count; index++)
            {
                checkedListBoxDistributions.SetItemChecked(index, false);
            }
        }

        /// <summary>
        ///   Tests the selected random number distributions.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonTest2_Click(object sender, EventArgs e)
        {
            richTextBoxDistributionTest.Clear();
            if (checkedListBoxDistributions.CheckedItems.Count == 0)
            {
                checkedListBoxDistributions.Text = "Choose at least one distribution to test";
                return;
            }

            buttonTest2.Enabled = false;
            buttonSelectAll.Enabled = false;
            buttonDeselectAll.Enabled = false;
            numericUpDownSamples2.Enabled = false;
            checkedListBoxDistributions.Enabled = false;
            comboBoxGenerator2.Enabled = false;
            Update();

            var samples = (int) numericUpDownSamples2.Value;
            var watch = new Stopwatch();
            var results = new List<string>(distributions.Count);

            // Do some computation before testing, cause otherwise the first tested distribution will
            // have worse results. Guess this needs to be done due to scheduling behaviour of the OS.
            IDistribution distribution = new ContinuousUniformDistribution();
            for (var index2 = 0; index2 < 10000000; index2++)
            {
                distribution.NextDouble();
            }

            // Iterate over the list of random number distributions and test all that are checked in
            // the ListBox.
            for (var index = 0; index < distributions.Count; index++)
            {
                if (checkedListBoxDistributions.CheckedItems.Contains(distributions.Values[index].Name))
                {
                    if (comboBoxGenerator2.SelectedIndex == 0)
                    {
                        distribution = (IDistribution) Activator.CreateInstance(distributions.Values[index]);
                    }
                    else if (distributions.Values[index].GetConstructor(new[] { typeGenerator }) != null)
                    {
                        distribution =
                            (IDistribution)
                            Activator.CreateInstance(distributions.Values[index],
                                                     new[]
                                                     {Activator.CreateInstance(generators[comboBoxGenerator2.Text])});
                    }
                    else {
                        distribution = (IDistribution) Activator.CreateInstance(distributions.Values[index]);
                        if (richTextBoxDistributionTest.Text == "")
                        {
                            richTextBoxDistributionTest.Text +=
                                "The following distributions don't support a specific " +
                                "generator (Use distribution default).\n";
                        }
                        richTextBoxDistributionTest.Text += distributions.Values[index].Name + "\n";
                    }

                    //Test the NextDouble method.
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        distribution.NextDouble();
                    }
                    watch.Stop();
                    var duration = watch.ElapsedTicks / (double) Stopwatch.Frequency;
                    results.Add("  " + duration.ToString("00.0000") + " s\t|  " + distributions.Values[index].Name +
                                "\n");
                }
            }
            results.Sort();
            if (richTextBoxDistributionTest.Text != "")
            {
                richTextBoxDistributionTest.Text += "\n";
            }
            richTextBoxDistributionTest.Text += "  NextDouble()\t|  Distribution\n";
            foreach (var t in results)
            {
                richTextBoxDistributionTest.Text += t;
            }

            buttonTest2.Enabled = true;
            buttonSelectAll.Enabled = true;
            buttonDeselectAll.Enabled = true;
            numericUpDownSamples2.Enabled = true;
            checkedListBoxDistributions.Enabled = true;
            comboBoxGenerator2.Enabled = true;
        }

        #endregion methods regarding tabpage "Distributions II"

        #region Methods regarding generators

        /// <summary>
        ///   Tests the selected random number generators.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonTestGenerators_Click(object sender, EventArgs e)
        {
            dataGridViewGenerators.Rows.Clear();
            if (checkedListBoxGenerators.CheckedItems.Count == 0)
            {
                return;
            }

            buttonTestGenerators.Enabled = false;
            buttonSelect.Enabled = false;
            buttonDeselect.Enabled = false;
            numericUpDownGenSamples.Enabled = false;
            checkedListBoxGenerators.Enabled = false;
            checkBoxNext.Enabled = false;
            checkBoxNextMax.Enabled = false;
            checkBoxNextMinMax.Enabled = false;
            checkBoxNextDouble.Enabled = false;
            checkBoxNextDoubleMax.Enabled = false;
            checkBoxNextDoubleMinMax.Enabled = false;
            checkBoxIntegers.Enabled = false;
            checkBoxIntegersMax.Enabled = false;
            checkBoxIntegersMinMax.Enabled = false;
            checkBoxDoubles.Enabled = false;
            checkBoxDoublesMax.Enabled = false;
            checkBoxDoublesMinMax.Enabled = false;
            checkBoxNextBoolean.Enabled = false;
            checkBoxBooleans.Enabled = false;
            checkBoxNextBytes.Enabled = false;
            Update();

            var watch = new Stopwatch();
            var samples = (int) numericUpDownGenSamples.Value;

            // Do some computation before testing, cause otherwise the first tested generator will
            // have worse results. Guess this needs to be done due to scheduling behaviour of the OS.
            IGenerator currGen = new StandardGenerator();
            for (var index2 = 0; index2 < 10000000; index2++)
            {
                currGen.Next();
            }

            // Iterate over the list of random number generators and test all that are checked in the ListBox.
            for (var index = 0; index < generators.Count; index++)
            {
                if (!checkedListBoxGenerators.CheckedItems.Contains(generators.Values[index].Name))
                {
                    continue;
                }

                const int testMethodCount = 15;
                var resultsRow = new string[testMethodCount + 2]; // 1 is for generator name, 1 is for "samples/s" label
                resultsRow[0] = generators.Values[index].Name;
                currGen = (IGenerator) Activator.CreateInstance(generators.Values[index]);

                double samplesPerSec;

                // Tests the Next method.
                if (checkBoxNext.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.Next();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[1] = samplesPerSec.ToString("#,0");
                }

                // Tests the Next method with maxValue.
                if (checkBoxNextMax.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.Next(99);
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[2] = samplesPerSec.ToString("#,0");
                }

                // Tests the Next method with minValue and maxValue.
                if (checkBoxNextMinMax.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.Next(-99, 99);
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[3] = samplesPerSec.ToString("#,0");
                }

                // Tests the NextDouble method.
                if (checkBoxNextDouble.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.NextDouble();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[4] = samplesPerSec.ToString("#,0");
                }

                // Tests the NextDouble method with maxValue.
                if (checkBoxNextDoubleMax.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.NextDouble(99.0);
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[5] = samplesPerSec.ToString("#,0");
                }

                // Tests the NextDouble method with minValue and maxValue.
                if (checkBoxNextDoubleMinMax.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.NextDouble(-99.0, 99.0);
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[6] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedIntegers method.
                if (checkBoxIntegers.Checked)
                {
                    var en = currGen.Integers().GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[7] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedIntegers method with maxValue.
                if (checkBoxIntegersMax.Checked)
                {
                    var en = currGen.Integers(99).GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[8] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedIntegers method with minValue and maxValue.
                if (checkBoxIntegersMinMax.Checked)
                {
                    var en = currGen.Integers(-99, 99).GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[9] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedDoubles method.
                if (checkBoxDoubles.Checked)
                {
                    var en = currGen.Doubles().GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[10] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedDoubles method with maxValue.
                if (checkBoxDoublesMax.Checked)
                {
                    var en = currGen.Doubles(99).GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[11] = samplesPerSec.ToString("#,0");
                }

                // Tests the DistributedDoubles method with minValue and maxValue.
                if (checkBoxDoublesMinMax.Checked)
                {
                    var en = currGen.Doubles(-99, 99).GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[12] = samplesPerSec.ToString("#,0");
                }

                // Tests the NextBoolean method.
                if (checkBoxNextBoolean.Checked)
                {
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.NextBoolean();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[13] = samplesPerSec.ToString("#,0");
                }

                // Tests the Booleans method.
                if (checkBoxBooleans.Checked)
                {
                    var en = currGen.Booleans().GetEnumerator();
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        en.MoveNext();
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[14] = samplesPerSec.ToString("#,0");
                }

                // Tests the NextBytes method.
                if (checkBoxNextBytes.Checked)
                {
                    var bytes = new byte[64];
                    watch.Reset();
                    watch.Start();
                    for (var index2 = 0; index2 < samples; index2++)
                    {
                        currGen.NextBytes(bytes);
                    }
                    watch.Stop();
                    samplesPerSec = Math.Floor(samples / (double) watch.ElapsedTicks * Stopwatch.Frequency);
                    resultsRow[15] = samplesPerSec.ToString("#,0");
                }

                resultsRow[16] = "samples/s";
                dataGridViewGenerators.Rows.Add(resultsRow);
                dataGridViewGenerators.Update();
            }

            buttonTestGenerators.Enabled = true;
            buttonSelect.Enabled = true;
            buttonDeselect.Enabled = true;
            numericUpDownGenSamples.Enabled = true;
            checkedListBoxGenerators.Enabled = true;
            checkBoxNext.Enabled = true;
            checkBoxNextMax.Enabled = true;
            checkBoxNextMinMax.Enabled = true;
            checkBoxNextDouble.Enabled = true;
            checkBoxNextDoubleMax.Enabled = true;
            checkBoxNextDoubleMinMax.Enabled = true;
            checkBoxIntegers.Enabled = true;
            checkBoxIntegersMax.Enabled = true;
            checkBoxIntegersMinMax.Enabled = true;
            checkBoxDoubles.Enabled = true;
            checkBoxDoublesMax.Enabled = true;
            checkBoxDoublesMinMax.Enabled = true;
            checkBoxNextBoolean.Enabled = true;
            checkBoxBooleans.Enabled = true;
            checkBoxNextBytes.Enabled = true;
        }

        /// <summary>
        ///   Selects all random generators in the checked listbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonSelect_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < checkedListBoxGenerators.Items.Count; index++)
            {
                checkedListBoxGenerators.SetItemChecked(index, true);
            }
        }

        /// <summary>
        ///   Deselects all random generators in the checked listbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void ButtonDeselect_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < checkedListBoxGenerators.Items.Count; index++)
            {
                checkedListBoxGenerators.SetItemChecked(index, false);
            }
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNext_CheckedChanged(object sender, EventArgs e)
        {
            Next.Visible = checkBoxNext.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextMax_CheckedChanged(object sender, EventArgs e)
        {
            NextMax.Visible = checkBoxNextMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextMinMax_CheckedChanged(object sender, EventArgs e)
        {
            NextMinMax.Visible = checkBoxNextMinMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextDouble_CheckedChanged(object sender, EventArgs e)
        {
            NextDouble.Visible = checkBoxNextDouble.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextDoubleMax_CheckedChanged(object sender, EventArgs e)
        {
            NextDoubleMax.Visible = checkBoxNextDoubleMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextDoubleMinMax_CheckedChanged(object sender, EventArgs e)
        {
            NextDoubleMinMax.Visible = checkBoxNextDoubleMinMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxIntegers_CheckedChanged(object sender, EventArgs e)
        {
            Integers.Visible = checkBoxIntegers.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxIntegersMax_CheckedChanged(object sender, EventArgs e)
        {
            IntegersMax.Visible = checkBoxIntegersMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxIntegersMinMax_CheckedChanged(object sender, EventArgs e)
        {
            IntegersMinMax.Visible = checkBoxIntegersMinMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxDoubles_CheckedChanged(object sender, EventArgs e)
        {
            Doubles.Visible = checkBoxDoubles.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxDoublesMax_CheckedChanged(object sender, EventArgs e)
        {
            DoublesMax.Visible = checkBoxDoublesMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxDoublesMinMax_CheckedChanged(object sender, EventArgs e)
        {
            DoublesMinMax.Visible = checkBoxDoublesMinMax.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextBoolean_CheckedChanged(object sender, EventArgs e)
        {
            NextBoolean.Visible = checkBoxNextBoolean.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxBooleans_CheckedChanged(object sender, EventArgs e)
        {
            Booleans.Visible = checkBoxBooleans.Checked;
        }

        /// <summary>
        ///   Changes the visibility of column.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        void CheckBoxNextBytes_CheckedChanged(object sender, EventArgs e)
        {
            NextBytes.Visible = checkBoxNextBytes.Checked;
        }

        #endregion Methods regarding generators

        #endregion Instance methods
    }
}
