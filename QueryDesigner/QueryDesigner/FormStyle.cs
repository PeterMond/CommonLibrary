﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SnControl;

namespace QueryDesigner
{
    public partial class FormStyle : Form
    {
        public FormStyle()
        {
            InitializeComponent();
        }

        public List<StyleFormat> StyleFormat
        {
            get;
            set;
        }

        private void SetControlVisible(int index)
        {
            ComboBox comtype = Controls.Find("comType" + index, true)[0] as ComboBox;
            ComboBox comvalue = Controls.Find("comValue" + index, true)[0] as ComboBox;
            TextBox txtvalue = Controls.Find("txtValue" + index, true)[0] as TextBox;
            DateTimePicker datevalue = Controls.Find("dateValue" + index, true)[0] as DateTimePicker;

            switch (comtype.GetItemText(comtype.SelectedItem))
            {
                case "<Null>":
                    comvalue.Visible = txtvalue.Visible = datevalue.Visible = false;
                    comvalue.SelectedIndex = 0;
                    txtvalue.Text = null;
                    datevalue.Value = DateTime.Today;
                    break;
                default:
                case "System.String":
                case "System.Int32":
                case "System.Decimal":
                    comvalue.Visible = datevalue.Visible = false;
                    comvalue.SelectedIndex = 0;
                    datevalue.Value = DateTime.Today;
                    txtvalue.Visible = true;
                    break;
                case "System.Boolean":
                    txtvalue.Visible = datevalue.Visible = false;
                    txtvalue.Text = null;
                    datevalue.Value = DateTime.Today;
                    comvalue.Visible = true;
                    break;
                case "System.DateTime":
                    comvalue.Visible = txtvalue.Visible = false;
                    comvalue.SelectedIndex = 0;
                    txtvalue.Text = null;
                    datevalue.Visible = true;
                    break;
            }
        }

        private object GetValue(int index)
        {
            object value = null;

            ComboBox comtype = Controls.Find("comType" + index, true)[0] as ComboBox;
            ComboBox comvalue = Controls.Find("comValue" + index, true)[0] as ComboBox;
            TextBox txtvalue = Controls.Find("txtValue" + index, true)[0] as TextBox;
            DateTimePicker datevalue = Controls.Find("dateValue" + index, true)[0] as DateTimePicker;

            switch (comtype.GetItemText(comtype.SelectedItem))
            {
                default:
                case "<Null>":
                    break;
                case "System.String":
                    if (string.IsNullOrEmpty(txtvalue.Text))
                    {
                        value = "<Null>";
                        break;
                    }
                    value = txtvalue.Text;
                    break;
                case "System.Int32":
                case "System.Decimal":
                    value = txtvalue.Text;
                    break;
                case "System.Boolean":
                    value = Convert.ToBoolean(comvalue.GetItemText(comvalue.SelectedItem));
                    break;
                case "System.DateTime":
                    value = datevalue.Value;
                    break;
            }

            return value;
        }

        private void SetValue(int index, string type, string value)
        {
            ComboBox comtype = Controls.Find("comType" + index, true)[0] as ComboBox;
            ComboBox comvalue = Controls.Find("comValue" + index, true)[0] as ComboBox;
            TextBox txtvalue = Controls.Find("txtValue" + index, true)[0] as TextBox;
            DateTimePicker datevalue = Controls.Find("dateValue" + index, true)[0] as DateTimePicker;


            switch (type)
            {
                default:
                case "<Null>":
                    comtype.SelectedIndex = 5;
                    break;
                case "System.String":
                    comtype.SelectedIndex = 0;
                    txtvalue.Text = value;
                    break;
                case "System.Int32":
                    comtype.SelectedIndex = 1;
                    txtvalue.Text = value;
                    break;
                case "System.Decimal":
                    comtype.SelectedIndex = 2;
                    txtvalue.Text = value;
                    break;
                case "System.Boolean":
                    comtype.SelectedIndex = 3;
                    comvalue.SelectedIndex = value == "true" ? 0 : 1;
                    break;
                case "System.DateTime":
                    comtype.SelectedIndex = 4;
                    datevalue.Value = Convert.ToDateTime(value);
                    break;
            }
        }

        private bool Validate(int index)
        {
            ComboBox comtype = Controls.Find("comType" + index, true)[0] as ComboBox;
            TextBox txtvalue = Controls.Find("txtValue" + index, true)[0] as TextBox;

            string selectText = comtype.GetItemText(comtype.SelectedItem);

            if ((selectText == "System.Int32" || selectText == "System.Decimal") && string.IsNullOrEmpty(txtvalue.Text))
            {
                MessageBox.Show("值" + (index == 1 ? "一" : "二") + "数据有误，不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (selectText == "System.Int32")
            {
                try
                {
                    Convert.ToInt32(txtvalue.Text);
                }
                catch
                {
                    txtvalue.Text = null;
                    MessageBox.Show("值" + (index == 1 ? "一" : "二") + "数据有误，请输入正确的数值！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (selectText == "System.Decimal")
            {
                try
                {
                    Convert.ToDecimal(txtvalue.Text);
                }
                catch
                {
                    txtvalue.Text = null;
                    MessageBox.Show("值" + (index == 1 ? "一" : "二") + "数据有误，请输入正确的数值！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void GetStyle(int index)
        {
            btnBack.BackColor = Color.FromArgb(StyleFormat[index].BackColorRed, StyleFormat[index].BackColorGreen, StyleFormat[index].BackColorBlue);
            btnFore.BackColor = Color.FromArgb(StyleFormat[index].ForeColorRed, StyleFormat[index].ForeColorGreen, StyleFormat[index].ForeColorBlue);

            checkApplyToRow.Checked = StyleFormat[index].ApplyToRow;

            switch (StyleFormat[index].Condition)
            {
                default:
                case DevExpress.XtraGrid.FormatConditionEnum.Equal:
                    comCondition.SelectedIndex = 0;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.NotEqual:
                    comCondition.SelectedIndex = 1;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.Between:
                    comCondition.SelectedIndex = 2;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.NotBetween:
                    comCondition.SelectedIndex = 3;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.Greater:
                    comCondition.SelectedIndex = 4;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.Less:
                    comCondition.SelectedIndex = 5;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual:
                    comCondition.SelectedIndex = 6;
                    break;
                case DevExpress.XtraGrid.FormatConditionEnum.LessOrEqual:
                    comCondition.SelectedIndex = 7;
                    break;
            }

            if (StyleFormat[index].Value1 != null)
            {
                SetValue(1, StyleFormat[index].Type1, StyleFormat[index].Value1.ToString());
            }
            else
            {
                comType1.SelectedIndex = 5;
            }

            if (StyleFormat[index].Value2 != null)
            {
                SetValue(2, StyleFormat[index].Type2, StyleFormat[index].Value2.ToString());
            }
            else
            {
                comType2.SelectedIndex = 5;
            }

            SetControlVisible(1);
            SetControlVisible(2);
        }

        private void FormStyle_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            for (int i = 0; i < StyleFormat.Count; i++)
            {
                comIndex.Items.Add("样式" + i);
            }

            comIndex.SelectedIndex = 0;

            GetStyle(0);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btn.BackColor = colorDialog.Color;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!Validate(1) || !Validate(2))
            {
                return;
            }

            StyleFormat[comIndex.SelectedIndex].BackColorRed = (int)btnBack.BackColor.R;
            StyleFormat[comIndex.SelectedIndex].BackColorGreen = (int)btnBack.BackColor.G;
            StyleFormat[comIndex.SelectedIndex].BackColorBlue = (int)btnBack.BackColor.B;

            StyleFormat[comIndex.SelectedIndex].ForeColorRed = (int)btnFore.BackColor.R;
            StyleFormat[comIndex.SelectedIndex].ForeColorGreen = (int)btnFore.BackColor.G;
            StyleFormat[comIndex.SelectedIndex].ForeColorBlue = (int)btnFore.BackColor.B;

            StyleFormat[comIndex.SelectedIndex].ApplyToRow = checkApplyToRow.Checked;


            DevExpress.XtraGrid.FormatConditionEnum temp;

            switch (comCondition.GetItemText(comCondition.SelectedItem))
            {
                default:
                case "等于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.Equal;
                    break;
                case "不等于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.NotEqual;
                    break;
                case "在值一与值二之间":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.Between;
                    break;
                case "不在值一与值二之间":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.NotBetween;
                    break;
                case "大于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.Greater;
                    break;
                case "小于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.Less;
                    break;
                case "大于等于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual;
                    break;
                case "小于等于":
                    temp = DevExpress.XtraGrid.FormatConditionEnum.LessOrEqual;
                    break;
            }

            StyleFormat[comIndex.SelectedIndex].Condition = temp;

            object obj = GetValue(1);

            if (obj != null)
            {
                StyleFormat[comIndex.SelectedIndex].Type1 = comType1.GetItemText(comType1.SelectedItem);
                StyleFormat[comIndex.SelectedIndex].Value1 = obj;
            }

            obj = GetValue(2);

            if (obj != null)
            {
                StyleFormat[comIndex.SelectedIndex].Type2 = comType2.GetItemText(comType2.SelectedItem);
                StyleFormat[comIndex.SelectedIndex].Value2 = obj;
            }

            DialogResult = DialogResult.OK;
        }

        private void comType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlVisible(1);
        }

        private void comType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlVisible(2);
        }

        private void comIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStyle(comIndex.SelectedIndex);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.StyleFormat.RemoveAt(this.comIndex.SelectedIndex);
            this.comIndex.Items.RemoveAt(this.comIndex.SelectedIndex);
            if (this.comIndex.Items.Count == 0)
                this.DialogResult = DialogResult.OK;
            else
                this.comIndex.SelectedIndex = 0;
        }
    }
}
