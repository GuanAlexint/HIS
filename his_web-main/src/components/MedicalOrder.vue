<template>
<div class="patient-out-visit">

<div class="list-container-112">
    <h2 class="list-title">
        患者列表 ({{ patients.length }})<i class="el-icon-refresh" @click="getPatiToVist" />
    </h2>
    <ul class="custom-list-112 custom-list-113">
        <li
            v-for="(item, index) in patients"

            @click="patientOrder(item, index)"
            :key="item.pid"
            :class="{
                selected:
                    patient_edit_index === index,
            }"
        >
            <span>{{ item.pname }}</span>
            

        </li>
    </ul>
</div>


<div class="order-box">

    <div class="order-header">
        <el-autocomplete 
            placeholder="医嘱搜索" 
            clearable
            v-model="orderSearchWord" 
            :fetch-suggestions="querySearchAsync"
            @select="onOrderSelected"
            class="order-search-append">
            <el-select class="select-order-type"  v-model="dictOrderInfo.ordertype" slot="prepend" placeholder="请选择医嘱类型">
                <el-option key="药品" label="药品" value="药品"></el-option>
                <el-option key="检查" label="检查" value="检查"></el-option>
                <el-option key="治疗" label="治疗" value="治疗"></el-option>
                <el-option key="检验" label="检验" value="检验"></el-option>
            </el-select>
            <el-button slot="append" icon="el-icon-search"></el-button>
        </el-autocomplete>

        <div> 
            <el-input class="total-order-price" v-model="orderToPay" disabled>
                <template slot="prepend">待支付</template>
                <template slot="append">元</template>
            </el-input>

            <el-button icon="iconfont icon-fukuan" class="order-pay" type="info" @click="toOrderPay"></el-button>

        </div>
        

    </div>

    <!-- 医嘱展示表格 -->
    <div class="data-table-show">
        <el-table
            :data="orderTableShow"
            class="order-table"  
            border
            height="380"
            :row-class-name="tableRowClassName"
        >

            <el-table-column
                v-for="(colInfo,k) in orderShowColumns"
                :label="colInfo.desc"
                :key="k"
                :prop="k"
                :width="colInfo.width"
            ></el-table-column>

        </el-table>
        <div class="page-container">
            <el-pagination
                @size-change="handleSizeChange"
                @current-change="handleCurrentChange"
                :current-page="page_ctr.current_page"
                :page-size="page_ctr.page_size"
                layout="total, sizes, prev, pager, next, jumper"
                :total="orderTable.length">
            </el-pagination>
        </div>
    </div>
            



</div>

<el-dialog :visible.sync="orderAppendCtr.visible" width="70%">
    <div class="dialog-order-body">

        <el-descriptions class="margin-top" title="添加医嘱" :column="3" border>

             <el-descriptions-item label="医嘱名">{{ dictOrderInfo.ordername }}</el-descriptions-item>
            <el-descriptions-item label="医嘱类型">{{ dictOrderInfo.ordertype }}</el-descriptions-item>
            <el-descriptions-item label="首字母">{{ dictOrderInfo.inputstr }}</el-descriptions-item>
            <el-descriptions-item label="药品规格">{{ dictOrderInfo.drugSpecifications }}</el-descriptions-item>
            <el-descriptions-item label="给药途径">{{ dictOrderInfo.takeMedicine }}</el-descriptions-item>
            <el-descriptions-item label="频次">{{ dictOrderInfo.frequency }}</el-descriptions-item>
            <el-descriptions-item label="单价">{{ dictOrderInfo.orderprice }}</el-descriptions-item>
            <el-descriptions-item label="数量">
                <el-input v-model="orderAppendCtr.otherInfo.totalOrder" 
                @blur="orderAppendCtr.otherInfo.orderallprice=parseInt(orderAppendCtr.otherInfo.totalOrder)*dictOrderInfo.orderprice"></el-input>
            </el-descriptions-item>
            <el-descriptions-item label="总价">
                {{orderAppendCtr.otherInfo.orderallprice}}
            </el-descriptions-item>

           
        </el-descriptions>

    </div>
    
    <!-- 底部区域 -->
    <span slot="footer" class="dialog-footer">
        <el-button @click="orderAppendCtr.visible = false">取 消</el-button>
        <el-button type="primary" @click="appendOrder">添 加</el-button>
    </span>
</el-dialog>



<el-dialog :visible.sync="orderToPayCtr.visible" width="70%">
    <!-- 患者就诊信息 -->
    <el-descriptions title="患者就诊信息">
        <el-descriptions-item label="姓名">{{orderToPayCtr.patientVisitInfo.pname}}</el-descriptions-item>
        <el-descriptions-item label="身份证">{{orderToPayCtr.patientVisitInfo.pidcard}}</el-descriptions-item>
        <el-descriptions-item label="挂号医生">{{orderToPayCtr.patientVisitInfo.doctName}}</el-descriptions-item>
        <el-descriptions-item label="挂号类别">
            <el-tag size="small">{{orderToPayCtr.patientVisitInfo.rtype}}</el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="挂号科室">{{orderToPayCtr.patientVisitInfo.dpmtnNme}}</el-descriptions-item>
    </el-descriptions>
    <el-divider><i class="iconfont icon-yiguahaoliebiao_24"></i> 待缴费医嘱</el-divider>

    <div class="order-pay-table-container">
        <div class="order-pay-table">
            <el-table
            :data="orderToPayCtr.orderToPayDataTable"
            show-summary
            @select="onPayOrderSelect"
        >
            <el-table-column
                type="selection"
                width="55">
            </el-table-column>

            <el-table-column
                label="医嘱名"
                prop="ordername"
                width="130"
            ></el-table-column>

            <el-table-column
                label="医嘱类型"
                prop="ordertype"
                width="90"
            ></el-table-column>

            <el-table-column
                label="单价"
                prop="orderprice"
                width="60"
            ></el-table-column>

            <el-table-column
                label="数量"
                prop="totalOrder"
                width="60"
            ></el-table-column>

            <el-table-column
                label="总计"
                prop="orderallprice"
                width="80"
            ></el-table-column>

            

        </el-table>
        </div>
        
        <div class="qrcode-container">
            <qrcode-vue :value="orderToPayCtr.payUrl"  :size="160" />
            <span>{{orderToPayCtr.underQrcodeText}}</span>
        </div>

    </div>


</el-dialog>


</div>
</template>

<script>
// import QrcodeVue from '../../node_modules__/qrcode.vue/dist/qrcode.vue';
// import { mapState, mapMutations, mapActions } from '../../node_modules__/vuex/dist/vuex.mjs'
import QrcodeVue from 'qrcode.vue'
import { mapState, mapMutations, mapActions } from 'vuex'


export default {
    components: {
        'qrcode-vue':QrcodeVue
    },
    data() {
        return {
            // 患者信息列表
            patients: [],
            patient_edit_index: -1,


            orderSearchWord: '',

            // 下医嘱弹框控制
            orderAppendCtr: {
                visible: false,
                otherInfo: {
                    rid: '',
                    patiid: '',
                    prescriptiontime: '', //开立时间
                    totalOrder: null,
                    orderallprice: null,
                    ispaid: 0
                }
            },
            dictOrderInfo: {
                ordid: '',              // 医嘱编码
                ordername: '',          // 医嘱名称
                ordertype: '药品',      // 医嘱类型
                inputstr: '',           // 首字母
                drugSpecifications: '', // 药品规格
                takeMedicine: '',       // 给药途径
                frequency: '',          // 频次
                orderprice: null        // 单价
            },

            orderShowColumns: {
                // pname: { desc: '姓名', width: 80 },
                rid: { desc: '挂号编号', width: 130 },
                ordername: { desc: '医嘱名称', width: 130 },
                ordertype: { desc: '医嘱类型', width: 60 },
                prescriptiontime: { desc: '医嘱开立时间', width: 160 },
                orderprice: { desc: '单价', width: 60 },
                totalOrder: { desc: '总量', width: 60 },
                orderallprice: { desc: '总价', width: 80 },
                ispaid: { desc: '是否付费', width: 100 },
                drugSpecifications: { desc: '药品规格', width: 150 },
                takeMedicine: { desc: '给药途径', width: 150 },
                frequency: { desc: '频次', width: 100 },
                // patiid: { desc: '患者就诊卡号', width: 150 },
            },
            doctorOrders: [], // 数据库中查询的结果
            orderTable: [], // 需要展示数据，分页前
            orderTableShow: [],  // 展示的数据
            // 分页控制
            page_ctr:{
                page_size:10,
                current_page:1
            },

            // 医嘱缴费弹出控制
            orderToPayCtr: {
                visible: false,
                patientVisitInfo: {

                },
                orderToPayDataTable: [],
                selectedOrder: [],
                totalTOPay: 0,
                payUrl: 'https://baidu.com',
                underQrcodeText: ''
            }

        }
    },
    created() {
        this.getPatiToVist()
        window.medicalOrderV = this
    },
    methods: {
        async getPatiToVist() {
            const {data:res} = await this.$http.get('/outvisit/visited/'+this.userInfo.userName)
            console.log(res)
            this.patients = res.data
            if (this.$route.params.pid){
                this.patient_edit_index = _.map(this.patients,'pid').indexOf(this.$route.params.pid)
            }
        },

        async querySearchAsync(queryString,cb) {
            console.log(queryString,cb)
            if (!queryString) return
            const {data:res} = await this.$http.get('/dictorder/keyword/'+this.dictOrderInfo.ordertype+'/'+queryString)
            console.log(res)
            cb(res.data.map(item=>{
                return{
                    ...item,
                    value: item.ordername
                }
            }))
        },

        // 获取患者医嘱信息
        async patientOrder(patientInfo, index) {
            console.log('患者信息：',patientInfo)
            this.patient_edit_index = index
            // 对医嘱表中患者信息进行赋值
            this.orderAppendCtr.otherInfo.rid = patientInfo.rid
            this.orderAppendCtr.otherInfo.patiid = patientInfo.pid
            
            const {data:res} = await this.$http.get('/doctororder/rid/'+patientInfo.rid)
            console.log('患者医嘱：',res.data)

            this.doctorOrders = res.data
            this.orderTable = []
            for (let o of this.doctorOrders) {
                this.orderTable.push({
                    pname: patientInfo.pname,
                    
                    ...o
                })
            }

            this.handleCurrentChange(1)

        },

        onOrderSelected(item) {
            //清空上一次填写的表单信息
            this.orderAppendCtr.otherInfo.totalOrder = null;
            this.orderAppendCtr.otherInfo.orderallprice = null;

            for (let key in this.dictOrderInfo) {
                if (item.hasOwnProperty(key)) {
                    this.dictOrderInfo[key] = item[key];
                }
            }
            this.dictOrderInfo.orderprice = item.price
            this.orderAppendCtr.visible = true

        },

        async appendOrder() {

            this.orderAppendCtr.otherInfo.prescriptiontime = formatDateTimeToNorm()
            const orderInfo = {
                ...this.orderAppendCtr.otherInfo,
                ...this.dictOrderInfo
            }
            console.log('post doctorOrder: ', orderInfo)
            const {data:res} = await this.$http.post('/doctororder',orderInfo)
            if(res.code===200) {
                this.$message.success('医嘱下达成功！')
                this.patientOrder(this.patients[this.patient_edit_index],this.patient_edit_index)
            }else{
                this.$message.error('医嘱下达失败！'+res.message)
            }

            this.orderAppendCtr.visible = false
        },


        getSummaries(param){
            const { columns, data } = param;
            const sums = [];
            columns.forEach((column, index) => {
                if (index === 0) {
                    sums[index] = '总价';
                    return;
                }
                if (column.property !== "totalOrder"){
                    sums[index] = '';
                    return ;
                }
                const values = data.map(item => Number(item[column.property]));
                if (!values.every(value => isNaN(value))) {
                    sums[index] = values.reduce((prev, curr) => {
                    const value = Number(curr);
                    if (!isNaN(value)) {
                        return prev + curr;
                    } else {
                        return prev;
                    }
                    }, 0);
                    sums[index] += ' 元';
                } else {
                    sums[index] = '';
                }
            });

            return sums;
        },


        // 分页操作后重新刷新Table
        handleSizeChange(page_size){
            this.page_ctr.page_size=page_size
            this.orderTableShow = this.orderTable.slice((this.page_ctr.current_page-1)*this.page_ctr.page_size,this.page_ctr.current_page*this.page_ctr.page_size)
        },
        handleCurrentChange(current_page){
            this.page_ctr.current_page = current_page
            this.orderTableShow = this.orderTable.slice((this.page_ctr.current_page-1)*this.page_ctr.page_size,this.page_ctr.current_page*this.page_ctr.page_size)
        },

        // 新增方法：根据医嘱类型动态设置行样式
        tableRowClassName({ row }) {
            switch (row.ordertype) {
                case '药品': return 'row-type-drug';
                case '检查': return 'row-type-check';
                case '治疗': return 'row-type-treat';
                case '检验': return 'row-type-test';
                default: return '';
            }
        },
        // tableRowClassName() {
        //     return 'row-type-drug';  // 所有行都变成浅蓝
        // },


        // 收费
        toOrderPay(){
            const patiInfo = this.patients[this.patient_edit_index]
            this.orderToPayCtr.patientVisitInfo = patiInfo

            this.orderToPayCtr.orderToPayDataTable = _.filter(this.doctorOrders,o=>{return !o.ispaid})

            this.orderToPayCtr.visible = true
        },

        // 付费选项改变
        onPayOrderSelect(orderRows) {
            console.log(orderRows)
            this.orderToPayCtr.selectedOrder=_.map(orderRows,'orderid')
            this.orderToPayCtr.totalTOPay = _.map(orderRows,'orderallprice').reduce((acc, cur) => acc + cur, 0);
            this.orderToPayCtr.underQrcodeText = '需要支付医嘱 '+orderRows.length+'/'+this.orderToPayCtr.orderToPayDataTable.length+' 总计：'+ this.orderToPayCtr.totalTOPay+'元'
            this.orderToPayCtr.payUrl = 'http://172.18.64.140:8080/#/outbillpay?rid='+this.orderToPayCtr.patientVisitInfo.rid+'&selectedOrders='+this.orderToPayCtr.selectedOrder.join(',')
        },
        
    },
    computed: {
        ...mapState(['userInfo']),
        orderToPay() {
            return _.filter(this.doctorOrders,o=>{return !o.ispaid}).reduce((sum, item) => {
                return sum + (item.orderallprice || 0);
            }, 0);
        }
    }
}
</script>

<style scoped>
.patient-out-visit {
    width: 870px;
    display: flex;
    justify-content: space-between;
}

.medical-form {
    width: 600px;
    /* margin-left: 10px; */
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.list-container-112 {
    min-width: 150px;
    padding: 15px;
    padding-top: 8px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    height: 600px;
}

.list-container-112 .list-title {
    color: #333;
    text-align: center;
    font-size: 13px;
}


.list-title i {
    font-weight: bold;
    cursor: pointer;
}

.custom-list-112 {
    list-style: none;
    padding: 0;
    overflow-y: auto;
}


.custom-list-112 .selected {
    background-color: #A3D9FF;
}


.custom-list-112 li {
    background-color: #f5f5f5;
    border: 1px solid #ddd;
    margin: 3px 0;
    padding: 7px 14px;
    border-radius: 4px;
    cursor: pointer;
}
ul::-webkit-scrollbar {
    width: 0; /* 隐藏垂直滚动条 */
}

.custom-list-112 li:hover {
    background-color: #5999FF;
}

.diagnose-icd .el-button{
    margin-left: 60px;
}



/* 医嘱添加对话框 */
.dialog-footer {
    display: flex;
}

.select-order-type {
    width: 90px;
    padding-left: 8px;
}

/* 右边主体区域 */
.order-box {
    width: 900px;
    padding-left: 10px;
}


/* 患者医嘱表 */
.order-box .data-table-show {
    height: 300px;
}


.order-header {
    display: flex;
    justify-content: space-between;
}

.order-search-append {
    width: 400px;
}
.total-order-price {
    width: 180px;
}

.order-pay {
    margin-left: 10px;
    padding: 8px;
    cursor: pointer;
}

.order-pay-table {
    width: 480px;
}

.order-pay-table-container {
    display: flex;
    /* justify-content: space-around; */
}
.qrcode-container {
    padding-left: 50px;
}

/* 新增样式：按类型设置背景色 */
::v-deep(.row-type-drug) {
  background-color: #e6f7ff !important;
}
::v-deep(.row-type-check) {
  background-color: #fff7e6 !important;
}
::v-deep(.row-type-treat) {
  background-color: #f6ffed !important;
}
::v-deep(.row-type-test) {
  background-color: #fff0f6 !important;
}



</style>
