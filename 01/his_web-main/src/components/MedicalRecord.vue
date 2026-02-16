<template>
<div class="patient-out-visit">

<div class="list-container-112">
    <h2 class="list-title">
        患者列表 ({{ patients.length }}) <i class="el-icon-refresh" @click="getPatiToVist" />
    </h2>
    <ul class="custom-list-112 custom-list-113">
        <li
            v-for="(item, index) in patients"

            @click="patientRecord(item, index)"
            :key="item.pid"
            :class="{
                selected:
                    patient_edit_index === index,
                submitted: isSubmitted(item.pid)
            }"
        >
            <span 
            >{{ item.pname }}</span>
            

        </li>
    </ul>
</div>


<el-form label-width="100px" class="medical-form">
    <!-- 主诉 -->
    <el-form-item label="主诉">
      <el-input type="textarea" v-model="form.chiefComplaint" :rows="2" />
    </el-form-item>

    <!-- 现病史 -->
    <el-form-item label="现病史">
      <el-input type="textarea" v-model="form.presentIllness" :rows="2" />
    </el-form-item>

    <!-- 既往史 -->
    <el-form-item label="既往史">
      <el-checkbox-group v-model="form.pastHistory">
        <el-checkbox label="否认慢性病史" />
        <el-checkbox label="高血压" />
        <el-checkbox label="糖尿病" />
        <el-checkbox label="冠心病" />
        <el-checkbox label="脑卒中" />
        <el-checkbox label="慢阻肺" />
        <el-checkbox label="肺结核" />
        <el-checkbox label="肺癌" />
        <el-checkbox label="心梗" />
        <el-checkbox label="肾病" />
        <el-checkbox label="肝炎" />
        <el-checkbox label="精神病" />
        <el-checkbox label="职业病" />
        <el-checkbox label="其他" />
      </el-checkbox-group>
      <el-input type="textarea" placeholder="其他病史：外伤史、手术史、个人史、家族史、婚育史、输血史等" v-model="form.pastOther" :rows="2" />
    </el-form-item>

    <!-- 过敏史 -->
    <el-form-item label="过敏史">
      <el-input v-model="form.allergy" />
    </el-form-item>

    <!-- 体格检查 -->
    <el-form-item label="体格检查">
      <el-radio-group v-model="form.physical">
        <el-radio label="无特殊" />
        <el-radio label="是外伤" />
      </el-radio-group>
    </el-form-item>

    <!-- 辅助检查 -->
    <el-form-item label="辅助检查">
      <el-input v-model="form.assist" />
    </el-form-item>
  
    <!-- <el-form-item class="diagnose-icd" label="门诊诊断">
        <el-select v-model="medicalContent.diagnoseICD" filterable > TODO filter-method=""
            <el-option v-for="item in icdCode" 
                :key=item.icd
                :label=item.icdname
                :value=item.icd></el-option>
        </el-select>
        <el-button type="primary" @click="onSaveMedicalContent" v-if="!Boolean(docId)" :disabled="patient_edit_index===-1">提交</el-button>
        <el-button type="primary" v-if="Boolean(docId)" disabled>已提交</el-button>
    </el-form-item> -->

    <el-form-item class="diagnose-icd" label="门诊诊断">
        <!-- 修改1: 添加filter-method属性实现自定义搜索功能 -->
        <el-select 
            v-model="medicalContent.diagnoseICD" 
            filterable 
            :filter-method="filterIcdOptions"
            placeholder="请输入诊断名称或首字母搜索"
        >
            <!-- 修改2: 使用过滤后的选项列表 -->
            <el-option 
                v-for="item in filteredIcdCode" 
                :key="item.icd"
                :label="item.icdname"
                :value="item.icd">
            </el-option>
        </el-select>
        <el-button type="primary" @click="onSaveMedicalContent" v-if="!Boolean(docId)" :disabled="patient_edit_index===-1">提交</el-button>
        <el-button type="primary" v-if="Boolean(docId)" disabled>已提交</el-button>
    </el-form-item>

    <el-form-item>
        <el-button type="success" @click="printMedicalRecord" :disabled="patient_edit_index===-1">
            打印病历
        </el-button>
    </el-form-item>
  
</el-form>


</div>
</template>

<script>
// import { mapState, mapMutations, mapActions } from '../../node_modules__/vuex/dist/vuex.mjs'
import { mapState, mapMutations, mapActions } from 'vuex'


export default {
    data() {
        return {
            // 患者信息列表
            patients: [],
            patient_edit_index: -1,

            // 病历待保存信息
            medicalContent: {
                // docId: '',
                rid: '',
                pid: '',
                pname: '',
                diagnoseICD: '',
                diagnoseICDName: '',
                medicalContent: '',
                visitTime: ''
            },
            docId: '',
            // 病历结构化信息
            form: {
                chiefComplaint: '',
                presentIllness: '',
                pastHistory: [],
                pastOther: '',
                allergy: '无',  // 过敏史
                physical: '无特殊',
                assist: '无'
            },
            icdCode: [],

            // 修改3: 添加过滤后的ICD代码列表
            filteredIcdCode: [],

        }
    },
    created() {
        this.getPatiToVist()
        this.getAllICDCode()
        window.medicalRecordV = this
    },
    methods: {
        async getPatiToVist() {
            const {data:res} = await this.$http.get('/outvisit/visit/'+this.userInfo.userName)
            console.log(res)
            this.patients = res.data
        },
        async getAllICDCode() {
            const {data:res} = await this.$http.get('/medicalrecord/icd/all')
            this.icdCode = res.data

            // 修改4: 初始化过滤列表
            this.filteredIcdCode = this.icdCode

            console.log(
                _.keyBy(this.icdCode,'icd')
            )
        },

        // 修改6: 添加ICD选项过滤方法（使用数据库InputStr字段）
        filterIcdOptions(query) {
            if (!query) {
                this.filteredIcdCode = this.icdCode;
                return;
            }
            
            const searchQuery = query.toLowerCase();
            
            this.filteredIcdCode = this.icdCode.filter(item => {
                // 按诊断名称搜索
                const nameMatch = item.icdname.toLowerCase().includes(searchQuery);
                
                // 按拼音首字母搜索（使用数据库中的InputStr字段）
                const pinyinMatch = item.inputStr && item.inputStr.toLowerCase().includes(searchQuery);
                
                // 按ICD编码搜索
                const codeMatch = item.icd.toLowerCase().includes(searchQuery);
                
                return nameMatch || pinyinMatch || codeMatch;
            });
        },

        //判断病历是否提交
        isSubmitted(pid) {
            return this.patients.find(p => p.pid === pid)?.submitted;
        },
        
        //病历打印
        printMedicalRecord() {
            const patient = this.patients[this.patient_edit_index];
            const form = this.form;
            const icdName = this.medicalContent.diagnoseICDName;

            const content = `
            ================ 病历信息 =================
            【患者姓名】：${patient.pname}
            【主诉】：${form.chiefComplaint}
            【现病史】：${form.presentIllness}
            【既往史】：${form.pastHistory.join('、')}
            【其他病史】：${form.pastOther}
            【过敏史】：${form.allergy}
            【体格检查】：${form.physical}
            【辅助检查】：${form.assist}
            【门诊诊断】：${icdName}
            签名医生：${this.userInfo.userName}
            日期：${new Date().toLocaleDateString()}
            =========================================`;

            const win = window.open('', '_blank');
            win.document.write(`<pre style="font-size:16px">${content}</pre>`);
            win.document.close();
            win.print();
        },


        async patientRecord(patientInfo,index){
            this.patient_edit_index = index
            this.medicalContent.pid = patientInfo.pid
            this.medicalContent.rid = patientInfo.rid
            this.medicalContent.pname = patientInfo.pname
            const {data:res} = await this.$http.get('/medicalrecord/rid/'+patientInfo.rid)
            console.log(res)
            if (res.data.length>0) { // 病历已写
                const mr = res.data[0]
                this.docId = mr.docId
                this.medicalContent.diagnoseICD = mr.diagnoseICD
                this.medicalContent.diagnoseICDName = mr.diagnoseICDName
                this.medicalContent.medicalContent = mr.medicalContent
                this.medicalContent.visitTime = mr.visitTime
                this.form = JSON.parse(mr.medicalContent)
            }else {
                this.docId = ''
                this.medicalContent = {
                    rid: '',
                    pid: '',
                    pname: '',
                    diagnoseICD: '',
                    diagnoseICDName: '',
                    medicalContent: '',
                    visitTime: ''
                }
            
                this.form = {
                    chiefComplaint: '',
                    presentIllness: '',
                    pastHistory: [],
                    pastOther: '',
                    allergy: '无',  // 过敏史
                    physical: '无特殊',
                    assist: '无'
                }
            }




        },
        async onSaveMedicalContent () {
            const patientInfo = this.patients[this.patient_edit_index]
            this.medicalContent.pid = patientInfo.pid
            this.medicalContent.rid = patientInfo.rid
            this.medicalContent.pname = patientInfo.pname
            this.medicalContent.diagnoseICDName = _.keyBy(this.icdCode,'icd')[this.medicalContent.diagnoseICD].icdname
            this.medicalContent.medicalContent = JSON.stringify(this.form)
            this.medicalContent.visitTime = formatDateTimeToNorm()

            const {data:res} = await this.$http.post('/medicalrecord',this.medicalContent)
            if(res.code===200){
                this.docId = res.data.docId
                this.$message.success('病历保存成功!')
                // 标记为已提交
                this.$set(this.patients[this.patient_edit_index], 'submitted', true)
            }else{
                this.$message.error(res.message)
            }

        }
    },
    computed: {
        ...mapState(['userInfo'])
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
    width: 200px;
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
    padding: 7px;
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

.custom-list-112 .submitted {
  background-color: #e0ffe0;
  font-weight: bold;
  position: relative;
}

.custom-list-112 .submitted::after {
  content: '✔';
  color: green;
  position: absolute;
  right: 10px;
}

</style>
