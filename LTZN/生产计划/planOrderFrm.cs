using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Rcw.Method;
using Rcw.Data;


namespace LTZN
{
    public partial class PlanOrderFrm : Form
    {
        public PlanOrderFrm()
        {
            DbContext.Create<PlanOrder>();

            InitializeComponent();
            comboBoxEdit1.SelectedIndex = 0;
            
        }
        List<PlanOrder> PlanOrderList = new List<PlanOrder>();

        /// <summary>
        /// 获取窗体的名称
        /// </summary>
        /// <returns></returns>
        public string GetFormName()
        {
            return this.Text;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// 
        public void loadData()
        {

            PlanOrderList = PlanOrder.GetList("gaolu=@gaolu order by xuhao ",comboBoxEdit1.Text.Trim());
            gridControl_PlanOrder.DataSource = PlanOrderList;
            DxSetting.SetMultiSelect(gridView_PlanOrder);
        }
      
        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_PlanOrder_Click(object sender, EventArgs e)
        {
            loadData();
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_PlanOrder_Click(object sender, EventArgs e)
        {
            var datalist = PlanOrder.GetSelectedRows(gridView_PlanOrder);

            foreach (var item in datalist)
            {
                if (item.ZDSJ == "")
                {
                    MessageBox.Show("整点时间不能为空！");
                    return;                      
                }
            }
            //修改操作，如果其他值修改，在ForEach中修改
            //datalist.ForEach(o => o.DataState = DataRowState.Deleted);
            datalist.Update();
            loadData();

        }
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_PlanOrder_Click(object sender, EventArgs e)
        {
            PlanOrder newRow = new PlanOrder();
            int xh = 1, lc = 1;
            string yeban = "夜班";
            int timespan = 0;
            DateTime lastZdsj = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            foreach (var item in PlanOrderList)
            {
                DateTime curZdsj = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")+" "+item.ZDSJ+":00");
                timespan =Convert.ToInt16( (curZdsj - lastZdsj).TotalMinutes);
                lastZdsj = curZdsj;
                xh = Convert.ToInt16(item.XUHAO)+1;
                lc = Convert.ToInt16(item.BANLUCI) + 1;
                yeban = item.BANCI;
            }

            newRow.ZDSJ = lastZdsj.AddMinutes(timespan).ToString("HH:mm");
            newRow.GAOLU = comboBoxEdit1.Text.Trim();
            newRow.XUHAO = xh;
            newRow.BANLUCI = lc.ToString();
            newRow.BANCI = yeban;
            PlanOrderList.Add(newRow);
            gridView_PlanOrder.RefreshData();
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_PlanOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要删除所选数据吗") == DialogResult.OK)
            {
                var datalist = PlanOrder.GetSelectedRows(gridView_PlanOrder);
                datalist.ForEach(o => o.DataState = DataRowState.Deleted);
                datalist.Update();
                loadData();
            }                   
        }
        /// <summary>
        /// 炉次预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string strSql = "select max(luci) from ddluci where gaolu='"+comboBoxEdit1.Text.Substring(0,1)+"'";
            var obj = DbContext.ExecuteScalar(strSql);
            int maxLuci = 1;
            if (obj == null || Convert.IsDBNull(obj))
            {
                MessageBox.Show("没有最大炉号");
                return;
            }
            else
            {
                maxLuci = Convert.ToInt32(obj);
            }         

            foreach (var item in PlanOrderList)
            {
                item.LUHAO = (maxLuci + 1).ToString();
                maxLuci++;
            }
            gridView_PlanOrder.RefreshData();


        }
        /// <summary>
        /// 生成计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}